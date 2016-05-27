using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public abstract class Unit
    {
        public UnitType Type { get; set; }
        public LocationName LocName { get; set; }
        public Nations Nation { get; set; }
        public abstract List<LocationName> GetAdjacent(Board board);
        public abstract bool IsAdjacent(LocationName dest, Board board);
        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherUnit = obj as Unit;
            return otherUnit.Type == Type && otherUnit.LocName == LocName && otherUnit.Nation == Nation;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
