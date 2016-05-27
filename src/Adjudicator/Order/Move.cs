using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class Move : Order
    {
        public override Strength GetStrength(List<Order> orders)
        {
            int min = 1;
            int max = 1;
            var supportOrders = orders.Where(order => order.Type == OrderType.Support && order.TargetStartingLocation.Name == Unit.LocName.Name && order.TargetLocation.Name == TargetLocation.Name).Where(order=> order.Status != OrderStatus.Failed).ToArray();
            foreach (var support in supportOrders)
            {
                if (!orders.Where(order => order.Unit.Nation == support.Unit.Nation && order.Unit.LocName.Name == TargetLocation.Name && ((order.Type != OrderType.Move && order.Type != OrderType.MoveByConvoy) || order.Status == OrderStatus.Failed)).Any())
                {
                    if (support.Status == OrderStatus.Succeded && !orders.Where(order => order.Unit.Nation == support.Unit.Nation && order.Unit.LocName.Name == TargetLocation.Name && order.Status == OrderStatus.Unresolved).Any())
                    {
                        min += 1;
                    }
                    max += 1;
                }
            }
            return base.GetStrength(orders);
        }
        public override void Resolve(List<Order> orders, BoardStatus status)
        {
            var board = new Board();
            if(!Unit.IsAdjacent(TargetLocation, board))
            {
                Status = OrderStatus.Failed;
                return;
            }
            ResolveAfterAdj(orders, status);
        }
        protected void ResolveAfterAdj(List<Order> orders, BoardStatus status)
        {
            var str = GetStrength(orders);
            Strength holdStr;
            if (status.Units.Where(unit => unit.LocName.Name == TargetLocation.Name).Any())
            {
                var ExistingUnitOrder = orders.Where(order => order.Unit.LocName.Name == TargetLocation.Name).FirstOrDefault();
                if (ExistingUnitOrder == null)
                {
                    holdStr = new Strength(0, 0);
                }
                if (ExistingUnitOrder.Type != OrderType.Move && ExistingUnitOrder.Type != OrderType.MoveByConvoy)
                {
                    if (ExistingUnitOrder.Unit.Nation == Unit.Nation)
                    {
                        Status = OrderStatus.Failed;
                        return;
                    }
                    holdStr = ExistingUnitOrder.GetStrength(orders);
                }
                else
                {
                    //There is a unit there but its trying to move. Support wont help but it may fail the move and end up resisting.
                    switch (ExistingUnitOrder.Status)
                    {
                        case OrderStatus.Unresolved:
                            if (ExistingUnitOrder.Unit.Nation == Unit.Nation)
                            {
                                Status = OrderStatus.Unresolved;
                                return;
                            }
                            holdStr = new Strength(0, 1);
                            break;
                        case OrderStatus.Succeded:
                            holdStr = new Strength(0, 0);
                            break;
                        case OrderStatus.Failed:
                            if (ExistingUnitOrder.Unit.Nation == Unit.Nation)
                            {
                                Status = OrderStatus.Failed;
                                return;
                            }
                            holdStr = new Strength(1, 1);
                            break;
                        default:
                            //Something went wrong
                            throw new Exception("Invalid order status");
                    }
                }
            }
            else
            {
                holdStr = new Strength(0, 0);
            }
            Strength preventStr = new Strength(0, 0);
            var preventOrders = orders.Where(order => order.TargetLocation.Name == TargetLocation.Name).ToArray();
            foreach (var order in preventOrders)
            {
                var pStr = order.GetStrength(orders);
                if (pStr.Min > preventStr.Min)
                {
                    preventStr.Min = pStr.Min;
                }
                if (pStr.Max > preventStr.Max)
                {
                    preventStr.Max = pStr.Max;
                }
            }
            //check for head to head
            var headToHeadOrder = orders.Where(order => order.TargetLocation.Name == Unit.LocName.Name && order.Unit.LocName.Name == TargetLocation.Name && order.Type == OrderType.Move).FirstOrDefault();
            if (headToHeadOrder != null)
            {
                var hStr = headToHeadOrder.GetStrength(orders);
                if (hStr.Min > preventStr.Min)
                {
                    preventStr.Min = hStr.Min;
                }
                if (hStr.Max > preventStr.Max)
                {
                    preventStr.Max = hStr.Max;
                }
            }

            if (str.Min > holdStr.Max && str.Min > preventStr.Max)
            {
                Status = OrderStatus.Succeded;
                return;
            }
            if (str.Max <= holdStr.Min || str.Max <= preventStr.Min)
            {
                Status = OrderStatus.Failed;
                return;
            }
        }
    }
}
