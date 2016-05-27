using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class DislodgedStatus
    {
        public List<DislodgedUnit> Units { get; set; }
        public DislodgedStatus()
        {
            Units = new List<DislodgedUnit>();
        }
    }
}
