namespace Halves_of_Tria.Components
{
    // Defined by half of the length along one of the straight edges of the capsule
    // and the radius of the capsule semi-cicles at each end.
    // Transform.Position should be at its centre.
    internal class CapsuleCollider
    {
        /// <summary>
        /// Half of the length along one of the straight edges of the capsule.
        /// </summary>
        public float HalfLength { get; set; }
        public float Radius { get; set; }
    }
}
