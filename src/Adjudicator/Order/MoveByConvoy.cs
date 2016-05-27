using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class MoveByConvoy : Move
    {
        public override void Resolve(List<Order> orders, BoardStatus status)
        {
            var board = new Board();
            //check path
            var convoyOrders = orders.Where(order => order.Type == OrderType.Convoy).
                Where(order => order.Status != OrderStatus.Failed).
                Where(order => order.TargetStartingLocation.Name == Unit.LocName.Name && order.TargetLocation.Name == TargetLocation.Name).
                Where(order => board.Map[order.Unit.LocName.Name].Type == LocationType.Water).ToList();
            var res = findPath(Unit.LocName.Name, TargetLocation.Name, convoyOrders, board, true);
            if (res == OrderStatus.Succeded)
            {
                ResolveAfterAdj(orders, status);
            } else
            {
                Status = res;
            }            
        }
        protected OrderStatus findPath(string current, string dest, List<Order> convoyOrders, Board board, bool complete)
        {
            var adj = board.Map[current].AdjacentLocations;
            foreach (var loc in adj)
            {
                if(loc.Name == dest)
                {
                    if (complete)
                    {
                        return OrderStatus.Succeded;
                    } else
                    {
                        return OrderStatus.Unresolved;
                    }
                }
                if(convoyOrders.Count == 0)
                {
                    return OrderStatus.Failed;
                }
                //if we're on a succeded chain look for the rest of it as succeeded first
                var newOrders = new List<Order>(convoyOrders);                
                if (complete)
                {
                    do
                    {
                        var conv = newOrders.Where(order => order.Unit.LocName.Name == loc.Name).First();
                        newOrders.Remove(conv);
                        var res = findPath(conv.Unit.LocName.Name, dest, newOrders, board, complete);
                        if (res == OrderStatus.Succeded)
                        {
                            return OrderStatus.Succeded;
                        }
                    } while (newOrders.Count > 0);
                }
                newOrders = new List<Order>(convoyOrders);
                do
                {
                    var conv = newOrders.Where(order => order.Unit.LocName.Name == loc.Name).First();
                    newOrders.Remove(conv);
                    var res = findPath(conv.Unit.LocName.Name, dest, newOrders, board, false);
                    if (res == OrderStatus.Unresolved || res == OrderStatus.Succeded)
                    {
                        return OrderStatus.Unresolved;
                    }

                } while (newOrders.Count > 0);
            }
            return OrderStatus.Failed;
        }
    }
}
