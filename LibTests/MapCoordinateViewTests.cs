using kRPCLib.Viewmodels;
using NUnit.Framework;
using System;

namespace LibTests
{
    public class MapCoordinateViewTests
    {
        [Test]
        public void TestUpdateMap_PlanetHasMap_ReturnsPath()
        {
            MapCoordinateView view = new MapCoordinateView();
            view.Planet = "kerbin";

            string path = view.MapImagePath;

            Assert.AreEqual("/Maps/kerbin.jpg", path);
        }

        [Test]
        public void TestUpdateMap_PlanetHasNoMap_ReturnsPathToEmpty()
        {
            MapCoordinateView view = new MapCoordinateView();
            view.Planet = "thisisnomoon";

            string path = view.MapImagePath;

            Assert.AreEqual("/Maps/Empty.jpg", path);
        }
    }
}
