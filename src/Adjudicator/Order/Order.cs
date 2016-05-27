using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public struct Strength
    {
        public int Max;
        public int Min;
        public Strength(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
    public class Order
    {
        public Unit Unit { get; set; }
        public LocationName TargetLocation { get; set; }
        public LocationName TargetStartingLocation { get; set; }
        public OrderType Type { get; set; }
        public OrderStatus Status { get; set; }
        public virtual void Resolve(List<Order> orders, BoardStatus status)
        {
            var holdStr = GetStrength(orders);
            var moveOrders = orders.Where(order => order.Status != OrderStatus.Failed).Where(order => (order.Type == OrderType.Move || order.Type == OrderType.MoveByConvoy)).
                Where(order => order.TargetLocation.Name == Unit.LocName.Name).ToArray();
            Strength moveStr = new Strength(0,0);
            if(moveOrders.Length > 0)
            {
                moveStr = moveOrders[0].GetStrength(orders);
            }
            foreach (var order in moveOrders)
            {
                var str = order.GetStrength(orders);
                if(moveStr.Max < str.Max)
                {
                    moveStr.Max = str.Max;
                }
                //the maximum min of the move orders ( a move order exists with at least this str)
                if (moveStr.Min < str.Min)
                {
                    moveStr.Min = str.Min;
                }
            }
            //no move orders can beat the min hold str
            if(holdStr.Min > moveStr.Max)
            {
                Status = OrderStatus.Succeded;
                foreach (var moveOrder in moveOrders)
                {
                    moveOrder.Status = OrderStatus.Failed;
                }
                return;
            }
        }
        public virtual Strength GetStrength(List<Order> orders)
        {
            int max = 1;
            int min = 1;
            var supports = orders.Where(order => order.Status != OrderStatus.Failed).Where(order => order.Type == OrderType.Support).Where(order => order.TargetLocation.Name == Unit.LocName.Name && order.TargetStartingLocation.Name == Unit.LocName.Name).ToArray();
            foreach (var support in supports)
            {
                if(support.Status == OrderStatus.Succeded)
                {
                    min += 1;
                }
                max += 1;
            }
            return new Strength(min, max);
        }
    }
}
