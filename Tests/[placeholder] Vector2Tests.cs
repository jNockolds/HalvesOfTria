using Microsoft.Xna.Framework;

namespace Tests
{
    [TestClass]
    public sealed class Vector2Tests
    {
        [TestMethod]
        public void Length_ReturnsCorrectValue()
        {
            Vector2 vector = new Vector2(3, 4);
            float length = vector.Length();
            Assert.AreEqual(5, length);
        }


    }
}
