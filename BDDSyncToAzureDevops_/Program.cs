using BDDSyncToAzureDevops_.ParseFeature;
using BDDSyncToAzureDevops_.TestActions;

class Program
{
    internal static string organization = "Swatiroop1";
    internal static string project = "Coding";
    internal static string planId = "1";
    internal static string parentSuiteId = "2";
    internal static string pat = "FyimVTsiHk5hJymb9lwn39BVxnrtEWFLFrK7BaacGZ7vojf8wLnIJQQJ99BBACAAAAAAAAAAAAASAZDO3B7q";
    internal static string folderPath = @"C:\Users\swati\source\repos\AmazonTestAutomation\AmazonTestAutomation\Features";

    internal static string tcTagFormat = "@tc:";
    internal static string suiteIdTagFormat = "@suiteId:";
   

    static void Main(string[] args)
    {
        var featureFiles = Directory.GetFiles(folderPath, "*.feature");

        foreach (var file in featureFiles)
        {
            var feature = FeatureAction.ParseFeatureFile(file);
            if (feature != null)
            {
                string suiteId = "";
                if (feature.existingSuiteId == null)
                {
                    suiteId = TestSuiteActions.CreateTestSuite(feature.Name); // Create test suite for feature
                    TagActions.AddSuiteIdTagToFeatureFile(file, feature.Name, suiteId);
                }
                else
                {
                    suiteId = TestSuiteActions.UpdateTestSuite(feature.existingSuiteId.Name.Substring(suiteIdTagFormat.Length), feature.Name);
                }

                foreach (var scenario in feature.Scenarios)
                {
                    var allTags = new List<string>(feature.Tags);
                    allTags.AddRange(scenario.Tags);
                    if (scenario.existingTestcaseId != null)
                    {
                        string testCaseId = TestcaseActions.CreateOrUpdateTestCase(suiteId, scenario.Title, scenario.Steps, scenario.Examples, allTags, "update", scenario.existingTestcaseId.Name.Substring(tcTagFormat.Length)); // Update test case
                        TestcaseActions.AddTestCaseToSuite(suiteId, testCaseId); // Add test case to suite
                    }
                    else
                    {
                        string testCaseId = TestcaseActions.CreateOrUpdateTestCase(suiteId, scenario.Title, scenario.Steps, scenario.Examples, allTags, "create", "0"); // Create test case
                        TestcaseActions.AddTestCaseToSuite(suiteId, testCaseId); // Add test case to suite
                        TagActions.AddTestCaseTagToFeatureFile(file, scenario.Title, testCaseId);
                    }

                }
            }
        }
    }

 }



