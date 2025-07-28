using HalvesOfTria.Classes;
using System.Collections.Generic;

namespace HalvesOfTria.Interfaces
{
    public interface IStaticCollideable
    {
        protected void HandleIncomingCollisions(List<EntityNode> entityNodes);
    }
}
