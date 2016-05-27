using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class Fleet : Unit
    {
        public override List<LocationName> GetAdjacent(Board board)
        {
            var loc = board.GetLocation(LocName.Name);
            if(LocName.Coast=="")
            {
                return loc.WaterAdjacent();
            } else
            {
                return loc.WaterAdjacent(LocName.Coast);
            }
        }
        public Fleet(Nations nation, LocationName loc)
        {
            Type = UnitType.Fleet;
            LocName = loc;
            Nation = nation;
        }
        public override bool IsAdjacent(LocationName dest, Board board)
        {
            if (board.Map[dest.Name].Type == LocationType.Land)
            {
                return false;
            }
            var adj = GetAdjacent(board);
            var locDetail = board.Map[dest.Name];
            if (locDetail.MultiCoastal)
            {
                return adj.Exists(loc => loc.Name == dest.Name && loc.Coast == dest.Coast);
            } else
            {
                return adj.Exists(loc => loc.Name == dest.Name);
            }
        }
    }
}
