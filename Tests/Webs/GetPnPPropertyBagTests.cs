using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace SharePointPnP.PowerShell.Tests.Webs
{
    [TestClass]
    public class GetPropertyBagTests
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
        public void GetPnPPropertyBagTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Key that should be looked up
				var key = "";
				// From Cmdlet Help: Site relative url of the folder. See examples for use.
				var folder = "";

                var results = scope.ExecuteCommand("Get-PnPPropertyBag",
					new CommandParameter("Key", key),
					new CommandParameter("Folder", folder));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            