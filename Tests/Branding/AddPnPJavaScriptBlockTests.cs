using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace SharePointPnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class AddJavaScriptBlockTests
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
        public void AddPnPJavaScriptBlockTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the script block. Can be used to identify the script with other cmdlets or coded solutions
				var name = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The javascript block to add to the specified scope
				var script = "";
				// From Cmdlet Help: A sequence number that defines the order on the page
				var sequence = "";
				var siteScoped = "";
				// From Cmdlet Help: The scope of the script to add to. Either Web or Site, defaults to Web. 'All' is not valid for this command.
				var scopeVar = "";

                var results = scope.ExecuteCommand("Add-PnPJavaScriptBlock",
					new CommandParameter("Name", name),
					new CommandParameter("Script", script),
					new CommandParameter("Sequence", sequence),
					new CommandParameter("SiteScoped", siteScoped),
					new CommandParameter("Scope", scopeVar));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            