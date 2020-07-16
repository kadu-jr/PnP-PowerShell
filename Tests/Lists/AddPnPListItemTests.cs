using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace SharePointPnP.PowerShell.Tests.Lists
{
    [TestClass]
    public class AddListItemTests
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
        public void AddPnPListItemTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The ID, Title or Url of the list.
				var list = "";
				// From Cmdlet Help: Specify either the name, ID or an actual content type.
				var contentType = "";
				// From Cmdlet Help: Use the internal names of the fields when specifying field names.
				// Single line of text: -Values @{"Title" = "Title New"}
				// Multiple lines of text: -Values @{"MultiText" = "New text\n\nMore text"}
				// Rich text: -Values @{"MultiText" = "<strong>New</strong> text"}
				// Choice: -Values @{"Choice" = "Value 1"}
				// Number: -Values @{"Number" = "10"}
				// Currency: -Values @{"Number" = "10"}
				// Currency: -Values @{"Currency" = "10"}
				// Date and Time: -Values @{"DateAndTime" = "03/13/2015 14:16"}
				// Lookup (id of lookup value): -Values @{"Lookup" = "2"}
				// Multi value lookup (id of lookup values as array 1): -Values @{"MultiLookupField" = "1","2"}
				// Multi value lookup (id of lookup values as array 2): -Values @{"MultiLookupField" = 1,2}
				// Multi value lookup (id of lookup values as string): -Values @{"MultiLookupField" = "1,2"}
				// Yes/No: -Values @{"YesNo" = $false}
				// Person/Group (id of user/group in Site User Info List or email of the user, separate multiple values with a comma): -Values @{"Person" = "user1@domain.com","21"}
				// Managed Metadata (single value with path to term): -Values @{"MetadataField" = "CORPORATE|DEPARTMENTS|FINANCE"}
				// Managed Metadata (single value with id of term): -Values @{"MetadataField" = "fe40a95b-2144-4fa2-b82a-0b3d0299d818"} with Id of term
				// Managed Metadata (multiple values with paths to terms): -Values @{"MetadataField" = "CORPORATE|DEPARTMENTS|FINANCE","CORPORATE|DEPARTMENTS|HR"}
				// Managed Metadata (multiple values with ids of terms): -Values @{"MetadataField" = "fe40a95b-2144-4fa2-b82a-0b3d0299d818","52d88107-c2a8-4bf0-adfa-04bc2305b593"}
				// Hyperlink or Picture: -Values @{"Hyperlink" = "https://github.com/OfficeDev/, OfficePnp"}
				var values = "";
				// From Cmdlet Help: The list relative URL of a folder. E.g. "MyFolder" for a folder located in the root of the list, or "MyFolder/SubFolder" for a folder located in the MyFolder folder which is located in the root of the list.
				var folder = "";
				// From Cmdlet Help: The name of the retention label.
				var label = "";

                var results = scope.ExecuteCommand("Add-PnPListItem",
					new CommandParameter("List", list),
					new CommandParameter("ContentType", contentType),
					new CommandParameter("Values", values),
					new CommandParameter("Folder", folder),
					new CommandParameter("Label", label));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            