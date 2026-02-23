using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halves_of_Tria.Caches
{
    internal sealed class CollisionsCache
    {
        public readonly List<CollisionData> ConfirmedCollisions = new();
    }
}
