using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MS.OSM.Querys.DAL;

namespace MS.OSM.Querys.Tests
{
    [TestClass]
    public class AccessWayDataTests
    {

        [TestMethod]
        public void TestGetClosest()
        {
            using(OSMDataAccessLayer dataAccessLayer = new OSMDataAccessLayer())
            {
                var way = dataAccessLayer.Ways.GetClosestWay(38.88688, -9.05550);

                Assert.AreEqual(way.Ref, "A1");
            }
        }

        [TestMethod]
        public void TestGetClosestWithInvalidPoint()
        {
            using(OSMDataAccessLayer dataAccessLayer = new OSMDataAccessLayer())
            {
                dataAccessLayer.Ways.GetClosestWay(0, 0);
            }
        }
    }
}
