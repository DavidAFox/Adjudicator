using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class DislodgedUnit
    {
        public Unit Unit { get; set; }
        public List<LocationName> OptionalLocations { get; set; }
        public DislodgedUnit(Unit u, List<LocationName> oLocations)
        {
            Unit = u;
            OptionalLocations = oLocations;
        }
    }
}
