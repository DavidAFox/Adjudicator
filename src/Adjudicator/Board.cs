using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public class Board
    {
        public Dictionary<string, Location> Map { get; set; }
        public List<string> SupplyCenters { get; set; }
        public Location GetLocation(string loc)
        {
            if(Map.ContainsKey(loc))
            {
                return Map[loc];
            } else
            {
                throw new ArgumentOutOfRangeException("Location Not Found");
            }
        }
        //default board hard coded for now
        //mutli coast locations can be included twice so it is important to calculate str based on orders not adjacent locations
        public Board()
        {
            Map = new Dictionary<string, Location>();
            //water  
            Map["nat"] = new Location("nat", LocationType.Water, LWrap("mid", "iri", "lvp", "cly", "nrg"));
            Map["nrg"] = new Location("nrg", LocationType.Water, LWrap("nat", "cly", "edi", "nth", "nwy", "bar"));
            var adj = new LocationName[] { new LocationName("nrg"), new LocationName("nwy"), new LocationName("stp", "north") };
            Map["bar"] = new Location("bar", LocationType.Water, adj);
            Map["nth"] = new Location("nth", LocationType.Water, LWrap("eng", "lon", "yor", "edi", "nrg", "nwy", "ska", "den", "hel", "hol", "bel"));
            Map["iri"] = new Location("iri", LocationType.Water, LWrap("nat", "lvp", "wal", "eng", "mid"));
            Map["eng"] = new Location("eng", LocationType.Water, LWrap("mid", "iri", "wal", "lon", "nth", "bel", "pic", "bre"));
            adj = new LocationName[] { new LocationName("nat"), new LocationName("iri"), new LocationName("eng"), new LocationName("bre"), new LocationName("gas"), new LocationName("spa", "north"), new LocationName("por"), new LocationName("spa", "south"), new LocationName("wes"), new LocationName("naf") };
            Map["mid"] = new Location("mid", LocationType.Water, adj);
            Map["hel"] = new Location("hel", LocationType.Water, LWrap("nth", "den", "bal", "kie", "hol", "ber"));
            Map["ska"] = new Location("ska", LocationType.Water, LWrap("nth", "nwy", "swe", "den"));
            Map["bal"] = new Location("bal", LocationType.Water, LWrap("den", "swe", "bot", "lvn", "pru", "ber", "hel"));
            adj = new LocationName[] { new LocationName("swe"), new LocationName("fin"), new LocationName("stp", "south"), new LocationName("lvn"), new LocationName("bal") };
            Map["bot"] = new Location("bal", LocationType.Water, adj);
            adj = new LocationName[] { new LocationName("mid"), new LocationName("spa", "south"), new LocationName("gol"), new LocationName("ty"), new LocationName("tun"), new LocationName("naf") };
            Map["wes"] = new Location("wes", LocationType.Water, adj);
            adj = new LocationName[] { new LocationName("spa", "south"), new LocationName("mar"), new LocationName("pie"), new LocationName("tus"), new LocationName("ty"), new LocationName("wes") };
            Map["gol"] = new Location("gol", LocationType.Water, adj);
            Map["ty"] = new Location("ty", LocationType.Water, LWrap("wes", "gol", "tus", "rom", "nap", "ion", "tun"));
            Map["ion"] = new Location("ion", LocationType.Water, LWrap("tun", "ty", "nap", "apu", "adr", "alb", "gre", "aeg", "eas"));
            Map["adr"] = new Location("adr", LocationType.Water, LWrap("apu", "ven", "tri", "alb", "ion"));
            adj = new LocationName[] { new LocationName("ion"), new LocationName("gre"), new LocationName("bul", "south"), new LocationName("con"), new LocationName("smy"), new LocationName("eas"), new LocationName("bla") };
            Map["aeg"] = new Location("aeg", LocationType.Water, adj);
            Map["eas"] = new Location("eas", LocationType.Water, LWrap("ion", "aeg", "smy", "syr"));
            adj = new LocationName[] { new LocationName("bul", "east"), new LocationName("rum"), new LocationName("sev"), new LocationName("arm"), new LocationName("ank"), new LocationName("con"), new LocationName("aeg") };
            Map["bla"] = new Location("bla", LocationType.Water, adj);

            //coast
            Map["cly"] = new Location("cly", LocationType.Coast, LWrap("nat", "nrg", "edi", "lvp"));
            Map["edi"] = new Location("edi", LocationType.Coast, LWrap("cly", "nrg", "nth", "yor", "lvp"), LWrap("cly", "nrg", "yor", "nth"));
            Map["lvp"] = new Location("lvp", LocationType.Coast, LWrap("iri", "nat", "cly", "edi", "yor", "wal"), LWrap("wal", "iri", "nat", "cly"));
            Map["wal"] = new Location("wal", LocationType.Coast, LWrap("iri", "lvp", "yor", "lon", "eng"), LWrap("lon", "eng", "iri", "lvp"));
            Map["lon"] = new Location("lon", LocationType.Coast, LWrap("wal", "yor", "nth", "eng"));
            Map["yor"] = new Location("yor", LocationType.Coast, LWrap("lvp", "edi", "nth", "lon", "wal"), LWrap("edi", "nth", "lon"));

            adj = new LocationName[] { new LocationName("nth"), new LocationName("nrg"), new LocationName("bar"), new LocationName("stp", "north"), new LocationName("fin"), new LocationName("swe"), new LocationName("ska") };
            var adjw = new LocationName[] { new LocationName("nth"), new LocationName("nrg"), new LocationName("bar"), new LocationName("stp", "north"), new LocationName("swe"), new LocationName("ska") };
            Map["nwy"] = new Location("nwy", LocationType.Coast, adj, adjw);
            Map["swe"] = new Location("swe", LocationType.Coast, LWrap("nwy", "fin", "bot", "bal", "den", "ska"));
            adj = new LocationName[] { new LocationName("swe"), new LocationName("nwy"), new LocationName("stp", "south"), new LocationName("bot") };
            adjw = new LocationName[] { new LocationName("swe"), new LocationName("stp", "south"), new LocationName("bot") };
            Map["fin"] = new Location("fin", LocationType.Coast, adj, adjw);
            Map["stp"] = new Location("stp", LocationType.Coast, LWrap("bot", "fin", "nwy", "bar", "mos", "lvn"), LWrap("bot", "fin", "nwy", "bar", "lvn"));
            Map["stp"].CoastAdjacentLocations = new Dictionary<string, LocationName[]>();
            Map["stp"].CoastAdjacentLocations["north"] = new LocationName[] { new LocationName("nwy"), new LocationName("bar") };
            Map["stp"].CoastAdjacentLocations["south"] = new LocationName[] { new LocationName("lvn"), new LocationName("bot"), new LocationName("fin") };
            Map["stp"].MultiCoastal = true;
            adj = new LocationName[] { new LocationName("bal"), new LocationName("bot"), new LocationName("stp", "south"), new LocationName("mos"), new LocationName("wa"), new LocationName("pru") };
            adjw = new LocationName[] { new LocationName("bal"), new LocationName("bot"), new LocationName("stp", "south"), new LocationName("pru") };
            Map["lvn"] = new Location("lvn", LocationType.Coast, adj, adjw);
            Map["pru"] = new Location("pru", LocationType.Coast, LWrap("ber", "bal", "lvn", "wa", "sil"), LWrap("ber", "bal", "lvn"));
            Map["ber"] = new Location("ber", LocationType.Coast, LWrap("kie", "hel", "bal", "pru", "sil", "mun"), LWrap("kie", "hel", "bal", "pru"));
            Map["kie"] = new Location("kie", LocationType.Coast, LWrap("hol", "hel", "den", "bal", "ber", "mun", "ruh"), LWrap("hol", "hel", "den", "bal", "ber"));
            Map["den"] = new Location("den", LocationType.Coast, LWrap("nth", "ska", "swe", "bal", "kie", "hel"));
            Map["hol"] = new Location("hol", LocationType.Coast, LWrap("bel", "nth", "hel", "kie", "ruh"), LWrap("bel", "nth", "hel", "kie"));
            Map["bel"] = new Location("bel", LocationType.Coast, LWrap("pic", "eng", "nth", "hol", "ruh", "bur"), LWrap("pic", "eng", "nth", "hol"));
            Map["pic"] = new Location("pic", LocationType.Coast, LWrap("bre", "eng", "bel", "bur", "par"), LWrap("bre", "eng", "bel"));
            Map["bre"] = new Location("bre", LocationType.Coast, LWrap("mid", "eng", "pic", "par", "gas"), LWrap("gas", "mid", "eng", "pic"));
            adj = new LocationName[] { new LocationName("mid"), new LocationName("bre"), new LocationName("par"), new LocationName("bur"), new LocationName("mar"), new LocationName("spa", "north") };
            adjw = new LocationName[] { new LocationName("mid"), new LocationName("bre"), new LocationName("spa", "north") };
            Map["gas"] = new Location("gas", LocationType.Coast, adj, adjw);


            SupplyCenters = new List<string> { "vie", "bud", "tri", "lon", "edi", "liv", "par", "mar",
                "bre", "ber", "mun", "kie", "rom", "ven", "nap", "mos", "sev", "war", "stp", "ank", "con", "smy",
                "nwy", "swe", "den", "bel", "hol", "por", "spa", "ser", "rum", "bul", "tun", "gre" };

        }
        private LocationName[] LWrap(params string[] names)
        {
            var lname = new LocationName[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                lname[i] = new LocationName(names[i]);
            }
            return lname;
        }
    }
}
