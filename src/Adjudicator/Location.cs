using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class Location
    {
        public string Id { get; set; }
        public bool MultiCoastal { get; set; }
        public LocationType Type { get; set; }
        public LocationName[] AdjacentLocations { get; set; }
        //WaterAdjacent is because not all coastal provences that are adjacent by land are adjacent by sea
        public LocationName[] WaterAdjacentLocations { get; set; }
        public Dictionary<string, LocationName[]> CoastAdjacentLocations { get; set; }
        public Location(string id, LocationType type, LocationName[] adjLoc, LocationName[] waterAdjLoc)
        {
            Id = id;
            Type = type;
            AdjacentLocations = adjLoc;
            WaterAdjacentLocations = waterAdjLoc;
            CoastAdjacentLocations = new Dictionary<string, LocationName[]>();
            MultiCoastal = false;
        }
        public Location(string id, LocationType type, LocationName[] adjLoc)
        {
            Id = id;
            Type = type;
            AdjacentLocations = adjLoc;
            WaterAdjacentLocations = adjLoc;
            CoastAdjacentLocations = new Dictionary<string, LocationName[]>();
            MultiCoastal = false;
        }
        public Location()
        {

        }
        public List<LocationName> Adjacent()
        {
            var names = new List<LocationName>();
            foreach (var Loc in AdjacentLocations)
            {
                names.Add(Loc);
            }
            return names;
        }
        public List<LocationName> WaterAdjacent()
        {
            var names = new List<LocationName>();
            foreach (var Loc in WaterAdjacentLocations)
            {
                names.Add(Loc);
            }
            return names;
        }
        public List<LocationName> WaterAdjacent(string coast)
        {
            var names = new List<LocationName>();
            if (this.CoastAdjacentLocations.ContainsKey(coast))
            {
                foreach (var Loc in this.CoastAdjacentLocations[coast])
                {
                        names.Add(Loc);
                }
                return names;
            }
            else
            {
                return this.WaterAdjacent();
            }
        }
    }
}