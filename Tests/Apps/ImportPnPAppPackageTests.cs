using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace SharePointPnP.PowerShell.Tests.Apps
{
    [TestClass]
    public class ImportAppPackageTests
    {
        #region Test Setup/CleanUp
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            // This runs on class level once before all tests run
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [ClassCleanup]
        public static void Cleanup(TestContext testContext)
        {
            // This runs on class level once
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [TestInitialize]
        public void Initialize()
        {
            using (var scope = new PSTestScope())
            {
                // Example
                // scope.ExecuteCommand("cmdlet", new CommandParameter("param1", prop));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var scope = new PSTestScope())
            {
                try
                {
                    // Do Test Setup - Note, this runs PER test
                }
                catch (Exception)
                {
                    // Describe Exception
                }
            }
        }
        #endregion

        #region Scaffolded Cmdlet Tests
        //TODO: This is a scaffold of the cmdlet - complete the unit test
        //[TestMethod]
        public void ImportPnPAppPackageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Path pointing to the .app file
				var path = "";
				// From Cmdlet Help: Will forcibly install the app by activating the addin sideloading feature, installing the addin, and deactivating the sideloading feature
				var force = "";
				// From Cmdlet Help: Will only upload the addin, but not install it
				var loadOnly = "";
				// From Cmdlet Help: Will install the addin for the specified locale
				var locale = "";

                var results = scope.ExecuteCommand("Import-PnPAppPackage",
					new CommandParameter("Path", path),
					new CommandParameter("Force", force),
					new CommandParameter("LoadOnly", loadOnly),
					new CommandParameter("Locale", locale));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            