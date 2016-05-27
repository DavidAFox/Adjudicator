using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class Support : Order
    {
        public override void Resolve(List<Order> orders, BoardStatus status)
        {
            var board = new Board();
            var adj = Unit.GetAdjacent(board);
            if(!adj.Where(loc => loc.Name == TargetLocation.Name).Any())
            {
                Status = OrderStatus.Failed;
                return;
            }
            if(board.Map[TargetLocation.Name].Type == LocationType.Water && Unit.Type == UnitType.Army)
            {
                Status = OrderStatus.Failed;
                return;
            }
            var moveOrdersTargetingUnit = orders.Where(order => (order.Type == OrderType.Move || order.Type == OrderType.MoveByConvoy) && order.TargetLocation.Name == Unit.LocName.Name).ToArray();
            if (!moveOrdersTargetingUnit.Any())
            {
                Status = OrderStatus.Succeded;
                return;
            }
            //attack from somewhere other than where its supporting
            if (moveOrdersTargetingUnit.Where(order => order.Type == OrderType.Move && order.Unit.IsAdjacent(Unit.LocName, board) && order.Unit.LocName.Name != TargetLocation.Name).Any())                
            {
                Status = OrderStatus.Failed;
                return;
            }
            if (moveOrdersTargetingUnit.Where(order => order.Status == OrderStatus.Succeded).Any())
            {
                Status = OrderStatus.Failed;
            }
        }
    }
}
