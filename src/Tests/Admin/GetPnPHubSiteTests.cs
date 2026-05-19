using System;
using System.Linq;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class GetPnPHubSiteTests
    {
        #region Get all hub sites

        [TestMethod]
        public void GetPnPHubSite_NoIdentity_ReturnsAllHubSites()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPHubSite");

                Assert.IsNotNull(results);
                Assert.IsTrue(results.Count > 0, "Expected at least one hub site to be returned.");
            }
        }

        #endregion

        #region Get hub site by URL

        [TestMethod]
        public void GetPnPHubSite_ByValidHubUrl_ReturnsHubSiteProperties()
        {
            using (var scope = new PSTestScope(true))
            {
                var allResults = scope.ExecuteCommand("Get-PnPHubSite");
                Assert.IsNotNull(allResults);
                Assert.IsTrue(allResults.Count > 0, "No hub sites available to test with.");

                var firstHub = allResults[0].BaseObject as HubSiteProperties;
                Assert.IsNotNull(firstHub, "Could not cast result to HubSiteProperties.");
                var hubUrl = firstHub.SiteUrl;

                var results = scope.ExecuteCommand("Get-PnPHubSite",
                    new CommandParameter("Identity", hubUrl));

                Assert.IsNotNull(results);
                Assert.AreEqual(1, results.Count, "Expected exactly one hub site to be returned for a specific URL.");

                var hubSite = results[0].BaseObject as HubSiteProperties;
                Assert.IsNotNull(hubSite);
                Assert.AreEqual(hubUrl, hubSite.SiteUrl);
            }
        }

        #endregion

        #region Get hub site by ID

        [TestMethod]
        public void GetPnPHubSite_ByValidHubId_ReturnsHubSiteProperties()
        {
            using (var scope = new PSTestScope(true))
            {
                var allResults = scope.ExecuteCommand("Get-PnPHubSite");
                Assert.IsNotNull(allResults);
                Assert.IsTrue(allResults.Count > 0, "No hub sites available to test with.");

                var firstHub = allResults[0].BaseObject as HubSiteProperties;
                Assert.IsNotNull(firstHub);
                var hubId = firstHub.ID.ToString();

                var results = scope.ExecuteCommand("Get-PnPHubSite",
                    new CommandParameter("Identity", hubId));

                Assert.IsNotNull(results);
                Assert.AreEqual(1, results.Count, "Expected exactly one hub site to be returned for a specific ID.");

                var hubSite = results[0].BaseObject as HubSiteProperties;
                Assert.IsNotNull(hubSite);
                Assert.AreEqual(firstHub.ID, hubSite.ID);
            }
        }

        #endregion

        #region Non-hub site URL returns null

        [TestMethod]
        public void GetPnPHubSite_ByNonHubSiteUrl_ReturnsNull()
        {
            using (var scope = new PSTestScope(true))
            {
                var siteUrl = TestCommon.Configuration.SiteUrl;

                var allResults = scope.ExecuteCommand("Get-PnPHubSite");
                var hubUrls = allResults
                    .Select(r => (r.BaseObject as HubSiteProperties)?.SiteUrl)
                    .Where(u => u != null)
                    .ToList();

                if (hubUrls.Contains(siteUrl, StringComparer.OrdinalIgnoreCase))
                {
                    Assert.Inconclusive("The test site is itself a hub site. Use a non-hub site URL to test this scenario.");
                }

                var results = scope.ExecuteCommand("Get-PnPHubSite",
                    new CommandParameter("Identity", siteUrl));

                Assert.IsTrue(results == null || results.Count == 0,
                    "Expected null or empty results when querying a non-hub site URL, but got a result.");
            }
        }

        #endregion

        #region Non-hub site ID returns null

        [TestMethod]
        public void GetPnPHubSite_ByNonHubSiteId_ReturnsNull()
        {
            using (var scope = new PSTestScope(true))
            {
                var nonHubId = Guid.NewGuid().ToString();

                var results = scope.ExecuteCommand("Get-PnPHubSite",
                    new CommandParameter("Identity", nonHubId));

                Assert.IsTrue(results == null || results.Count == 0,
                    "Expected null or empty results when querying a non-existent hub site ID, but got a result.");
            }
        }

        #endregion

        #region Return type validation

        [TestMethod]
        public void GetPnPHubSite_ReturnsCorrectType()
        {
            using (var scope = new PSTestScope(true))
            {
                var results = scope.ExecuteCommand("Get-PnPHubSite");

                Assert.IsNotNull(results);
                Assert.IsTrue(results.Count > 0);

                foreach (var result in results)
                {
                    Assert.IsTrue(result.BaseObject is HubSiteProperties,
                        $"Expected HubSiteProperties but got {result.BaseObject.GetType().Name}");
                }
            }
        }

        #endregion

        #region Hub site properties contain expected fields

        [TestMethod]
        public void GetPnPHubSite_ByUrl_HasNonEmptySiteUrl()
        {
            using (var scope = new PSTestScope(true))
            {
                var allResults = scope.ExecuteCommand("Get-PnPHubSite");
                Assert.IsNotNull(allResults);
                Assert.IsTrue(allResults.Count > 0);

                var firstHub = allResults[0].BaseObject as HubSiteProperties;
                Assert.IsNotNull(firstHub);

                var results = scope.ExecuteCommand("Get-PnPHubSite",
                    new CommandParameter("Identity", firstHub.SiteUrl));

                Assert.IsNotNull(results);
                Assert.AreEqual(1, results.Count);

                var hubSite = results[0].BaseObject as HubSiteProperties;
                Assert.IsNotNull(hubSite);
                Assert.IsFalse(string.IsNullOrEmpty(hubSite.SiteUrl), "SiteUrl should not be empty.");
                Assert.AreNotEqual(Guid.Empty, hubSite.ID, "ID should not be an empty GUID.");
            }
        }

        #endregion

        #region Invalid URL throws exception

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPnPHubSite_ByInvalidUrl_ThrowsException()
        {
            using (var scope = new PSTestScope(true))
            {
                scope.ExecuteCommand("Get-PnPHubSite",
                    new CommandParameter("Identity", "not-a-valid-url"));
            }
        }

        #endregion

        #region Empty string Identity throws exception

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPnPHubSite_ByEmptyString_ThrowsException()
        {
            using (var scope = new PSTestScope(true))
            {
                scope.ExecuteCommand("Get-PnPHubSite",
                    new CommandParameter("Identity", ""));
            }
        }

        #endregion
    }
}
