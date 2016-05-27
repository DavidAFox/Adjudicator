using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class Retreat
    {
        public LocationName Target { get; set; }
        public LocationName Origin { get; set; }
        public bool Disband { get; set; }
        public Unit Unit { get; set; }
        public OrderStatus Status { get; set; }
        public Retreat(Unit unit, bool disband)
        {
            Unit = unit;
            Disband = disband;
            Target = new LocationName("");
            Status = OrderStatus.Unresolved;
            Origin = unit.LocName;
        }
        public Retreat(Unit unit, bool disband, LocationName tar)
        {
            Unit = unit;
            Disband = disband;
            Target = tar;
            Status = OrderStatus.Unresolved;
            Origin = unit.LocName;
        }
        public override string ToString()
        {
            return System.String.Format("{0} {1} R {2} Status: {3}", Unit.Type.ToString(), Origin.ToString(), Target.ToString(), Status.ToString());
        }
    }
}
