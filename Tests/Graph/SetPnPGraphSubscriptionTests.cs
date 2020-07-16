using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace SharePointPnP.PowerShell.Tests.Graph
{
    [TestClass]
    public class SetGraphSubscriptionTests
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
        public void SetPnPGraphSubscriptionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The unique id or an instance of a Microsoft Graph Subscription
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Date and time to set the expiration to. Take notice of the maximum allowed lifetime of the subscription endponts as documented at https://docs.microsoft.com/graph/api/resources/subscription#maximum-length-of-subscription-per-resource-type
				var expirationDate = "";

                var results = scope.ExecuteCommand("Set-PnPGraphSubscription",
					new CommandParameter("Identity", identity),
					new CommandParameter("ExpirationDate", expirationDate));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            