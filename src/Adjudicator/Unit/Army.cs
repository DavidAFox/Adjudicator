using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class Army : Unit
    {
        public override List<LocationName> GetAdjacent(Board board)
        {
            var loc = board.GetLocation(LocName.Name);
            return loc.Adjacent();
        }
        public Army(Nations nation, LocationName loc)
        {
            Type = UnitType.Army;
            LocName = loc;
            Nation = nation;
        }
        //will return false for water spaces so it is not useful for checking convoys
        public override bool IsAdjacent(LocationName dest, Board board)
        {
            if (board.Map[dest.Name].Type == LocationType.Water)
            {
                return false;
            }
            var adj = GetAdjacent(board);
            return adj.Exists(loc => loc.Name == dest.Name);
        }
    }
}
