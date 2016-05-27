using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class LocationName
    {
        public string Name { get; set; }
        public string Coast { get; set; }
        public LocationName(string name, string coast = "")
        {
            Name = name;
            Coast = coast;
        }
        public override string ToString()
        {
            if (Coast != "")
            {
                return Name;
            }
            return System.String.Format("{0} {1} coast", Name, Coast);
        }
    }
}
