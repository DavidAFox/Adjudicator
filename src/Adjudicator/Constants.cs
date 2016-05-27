using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adjudicator
{
    public enum Nations { Austria, England, France, Germany, Italy, Russia, Turkey }
    public enum Phases { Order, Retreat, Build}
    public enum Season { Spring, Fall }
    public enum OrderType { Hold, Move, Support, Convoy, MoveByConvoy}
    public enum UnitType { Fleet, Army }
    public enum LocationType { Water, Land, Coast}
    public enum OrderStatus { Unresolved, Succeded, Failed }
}
