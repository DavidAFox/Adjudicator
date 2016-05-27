using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{

    public class BoardStatus
    {
        public int Id { get; set; }
        public List<Unit> Units { get; set; }
        public Dictionary<string, Nations> Control { get; set; }
        public List<Order> LastOrders  { get; set; }
        public List<Retreat> LastRetreats { get; set; }
        public DislodgedStatus Dislodged { get; set; }
        public Board MyBoard { get; set; }
        public BoardStatus(Board board)
        {
            MyBoard = board;
            Control = new Dictionary<string, Nations>();
            Units = new List<Unit>();
            LastOrders = new List<Order>();
        }
        protected struct dislodgedUnit
        {
            public Unit Unit;
            public string AttackedFrom;
        }
        public BoardStatus Starting()
        {
            var bs = new BoardStatus(new Board());
            bs.Units.Add(new Army(Nations.Austria, new LocationName("vie")));
            bs.Units.Add(new Army(Nations.Austria, new LocationName("bud")));
            bs.Units.Add(new Fleet(Nations.Austria, new LocationName("tri")));
            bs.Units.Add(new Fleet(Nations.England, new LocationName("lon")));
            bs.Units.Add(new Fleet(Nations.England, new LocationName("edi")));
            bs.Units.Add(new Army(Nations.England, new LocationName("liv")));
            bs.Units.Add(new Army(Nations.France, new LocationName("par")));
            bs.Units.Add(new Army(Nations.France, new LocationName("mar")));
            bs.Units.Add(new Fleet(Nations.France, new LocationName("bre")));
            bs.Units.Add(new Army(Nations.Germany, new LocationName("ber")));
            bs.Units.Add(new Army(Nations.Germany, new LocationName("mun")));
            bs.Units.Add(new Fleet(Nations.Germany, new LocationName("kie")));
            bs.Units.Add(new Army(Nations.Italy, new LocationName("rom")));
            bs.Units.Add(new Army(Nations.Italy, new LocationName("ven")));
            bs.Units.Add(new Fleet(Nations.Italy, new LocationName("nap")));
            bs.Units.Add(new Army(Nations.Russia, new LocationName("mos")));
            bs.Units.Add(new Fleet(Nations.Russia, new LocationName("sev")));
            bs.Units.Add(new Army(Nations.Russia, new LocationName("war")));
            bs.Units.Add(new Fleet(Nations.Russia, new LocationName("stp", "south")));
            bs.Units.Add(new Fleet(Nations.Turkey, new LocationName("ank")));
            bs.Units.Add(new Army(Nations.Turkey, new LocationName("con")));
            bs.Units.Add(new Army(Nations.Turkey, new LocationName("smy")));
            foreach (var unit in bs.Units)
            {
                bs.Control[unit.LocName.Name] = unit.Nation;
            }
            return bs;
        }
        public BoardStatus Next(List<Order> orders)
        {
            //resolve orders
            var resolved = false;
            while(!resolved)
            {
                var unOrders = orders.Where(order => order.Status == OrderStatus.Unresolved).ToArray();
                var starting = unOrders.Length;
                foreach (var order in unOrders)
                {
                    order.Resolve(orders, this);
                }
                if(orders.Where(order => order.Status == OrderStatus.Unresolved).ToArray().Length >= starting)
                {
                    var convoyOrders = orders.Where(order => order.Type == OrderType.Convoy && order.Status == OrderStatus.Unresolved).ToArray();
                    if(convoyOrders.Length > 0)
                    {
                        foreach (var order in convoyOrders)
                        {
                            order.Status = OrderStatus.Failed;
                        }
                    } else
                    {
                        //should be only move orders left unresolved
                        if (orders.Where(order => order.Type != OrderType.Move || order.Type != OrderType.MoveByConvoy).Where(order => order.Status == OrderStatus.Unresolved).Any())
                        {
                            //unresolved non-move orders this isn't good
                            throw new Exception("Non move order not resovled");
                        }
                        var moveOrders = orders.Where(order => order.Type == OrderType.Move || order.Type == OrderType.MoveByConvoy).Where(order => order.Status == OrderStatus.Unresolved).ToArray();
                        foreach (var order in moveOrders)
                        {
                            order.Status = OrderStatus.Succeded;
                        }
                    }
                }
            }
            //update the board
            var nextStatus = new BoardStatus(MyBoard);
            var dUnits = new List<dislodgedUnit>();
            foreach (var Unit in Units)
            {
                var moveOrder = orders.Where(order => order.Status == OrderStatus.Succeded && order.Unit.LocName.Name == Unit.LocName.Name && (order.Type == OrderType.Move || order.Type == OrderType.MoveByConvoy)).FirstOrDefault();
                if (moveOrder == null)
                {
                    var moveOrdersIntoSpace = orders.Where(order => order.Status == OrderStatus.Succeded).Where(order => order.Type == OrderType.Move || order.Type == OrderType.MoveByConvoy).Where(order => order.TargetLocation.Name == Unit.LocName.Name).ToArray();
                    if (moveOrdersIntoSpace.Length > 0)
                    {
                        var dUnit = new dislodgedUnit();
                        dUnit.Unit = Unit;
                        dUnit.AttackedFrom = moveOrdersIntoSpace[0].TargetLocation.Name;
                        dUnits.Add(dUnit);
                        //dislodged
                    }
                    else
                    {
                        nextStatus.Units.Add(Unit);
                    }
                } else
                {
                    Unit.LocName = moveOrder.TargetLocation;
                    nextStatus.Units.Add(Unit);
                }
            }

            //create the dislodged status
            var invalidRetreatLocations = orders.Where(order => order.Type == OrderType.Move || order.Type == OrderType.MoveByConvoy).Where(order => order.Status == OrderStatus.Succeded).Select(order => order.TargetLocation.Name).ToList();
            foreach (var Unit in Units)
            {
                invalidRetreatLocations.Add(Unit.LocName.Name);
            }
            foreach (var Unit in dUnits)
            {
                var Locations = Unit.Unit.GetAdjacent(MyBoard);
                foreach (var invalid in invalidRetreatLocations)
                {
                    Locations.RemoveAll(location => location.Name == invalid);
                }
                nextStatus.Dislodged.Units.Add(new DislodgedUnit(Unit.Unit, Locations));
            }
            nextStatus.LastOrders = orders;
            return nextStatus;

        }
        public void UpdateControl(List<string> centers)
        {
            foreach (var unit in Units)
            {
                if(centers.Contains(unit.LocName.Name))
                {
                    Control[unit.LocName.Name] = unit.Nation;
                }
            }
        }
        public void UpdateDislodged (List<Retreat> retreats)
        {
            List<Retreat> toRemove = new List<Retreat>();
            foreach (var retreat in retreats)
            {
                if(Dislodged.Units.Where(dUnit => dUnit.Unit.Equals(retreat.Unit)).Count() != 1)
                {
                    toRemove.Add(retreat);
                }
            }
            foreach (var retreat in toRemove)
            {
                retreats.Remove(retreat);
            }
            foreach (var retreat in retreats)
            {
                if (retreats.Where(retreat2 => retreat2.Target.Name == retreat.Target.Name).Count() > 1)
                {
                    retreat.Status = OrderStatus.Failed;
                }
            }
            foreach (var retreat in retreats)
            {
                if(retreat.Status != OrderStatus.Failed)
                {
                    retreat.Unit.LocName = retreat.Target;
                    Units.Add(retreat.Unit);
                    retreat.Status = OrderStatus.Succeded;
                }
            }
            LastRetreats = retreats;
        }
    }
}
