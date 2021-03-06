﻿using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.Interfaces;
using Xbim.IO.Memory;

namespace Xbim.Geometry.Engine.Interop.Tests.TestFiles
{
    [TestClass ]
    // [DeploymentItem("TestFiles")]
    public class IfcExtrudedAreaSolidTests
    {
        static private IXbimGeometryEngine geomEngine;
        static private ILoggerFactory loggerFactory;
        static private ILogger logger;

        [ClassInitialize]
        static public void Initialise(TestContext context)
        {
            loggerFactory = new LoggerFactory().AddConsole(LogLevel.Trace);
            geomEngine = new XbimGeometryEngine();
            logger = loggerFactory.CreateLogger<IfcAdvancedBrepTests>();
        }
        [ClassCleanup]
        static public void Cleanup()
        {
            loggerFactory = null;
            geomEngine = null;
            logger = null;
        }
        [TestMethod]
        public void arbritary_closed_profile_with_intersecting_voids_test()
        {
            using (var er = new EntityRepository<IIfcBooleanClippingResult>(nameof(arbritary_closed_profile_with_intersecting_voids_test)))
            {
                Assert.IsTrue(er.Entity != null, "No IIfcBooleanClippingResult found");
                var solidSet = geomEngine.CreateSolidSet(er.Entity, logger);
                Assert.IsTrue(solidSet.Count == 1, "This solid set should have 1 solid");
                Assert.IsTrue(solidSet.First().Faces.Count == 28, "This solid should have 28 faces");
            }

        }

        [TestMethod]
        public void IfcExtrudedAreaSolidInvalidPlacementTest()
        {
            using (var er = new EntityRepository<IIfcExtrudedAreaSolid>(nameof(IfcExtrudedAreaSolidInvalidPlacementTest)))
            {
                Assert.IsTrue(er.Entity != null, "No IIfcExtrudedAreaSolid found");
                var solid = geomEngine.CreateSolid(er.Entity, logger);
                Assert.IsTrue(solid.Faces.Count == 6, "This solid should have 6 faces");
            }

        }

        [TestMethod]
        public void IfcSweptDiskSolidWithPolylineTest()
        {
            using (var er = new EntityRepository<IIfcSweptDiskSolid>(nameof(IfcSweptDiskSolidWithPolylineTest)))
            {
                Assert.IsTrue(er.Entity != null, "No IIfcSweptDiskSolid found");
                
                var solid = geomEngine.CreateSolid(er.Entity, logger);
                Assert.AreEqual(39, solid.Faces.Count , "This solid has the wrong number of faces");
            }
        }
        [TestMethod]
        public void swept_disk_solid_with_trim_params_test()
        {
            using (var model = MemoryModel.OpenRead(@"TestFiles\swept_disk_solid_with_trim_params.ifc"))
            {
                var disk = model.Instances.OfType<IIfcSweptDiskSolid>().FirstOrDefault();
                Assert.IsNotNull(disk, "No IIfcSweptDiskSolid found");

                var solid = geomEngine.CreateSolid(disk, logger);
                Assert.AreEqual(5, solid.Faces.Count, "This solid has the wrong number of faces");
            }
        }
    }
}
