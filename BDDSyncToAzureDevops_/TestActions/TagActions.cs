

namespace BDDSyncToAzureDevops_.TestActions
{
    internal class TagActions
    {
        internal static void AddTestCaseTagToFeatureFile(string filePath, string scenarioTitle, string testCaseId)
        {
            var lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().StartsWith("Scenario") && lines[i].Contains(scenarioTitle))
                {
                    // Add the tag @tc:{testCaseId} one line before the scenario title
                    lines[i] = $"{Program.tcTagFormat}{testCaseId}\n{lines[i]}";
                    break;
                }
            }
            // Write the updated content back to the feature file
            File.WriteAllLines(filePath, lines);
        }

        internal static void AddSuiteIdTagToFeatureFile(string filePath, string featureTitle, string suiteId)
        {
            var lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().StartsWith("Feature") && lines[i].Contains(featureTitle))
                {
                    // Add the tag @tc:{testCaseId} one line before the scenario title
                    lines[i] = $"@suiteId:{suiteId}\n{lines[i]}";
                    break;
                }
            }
            // Write the updated content back to the feature file
            File.WriteAllLines(filePath, lines);
        }
    }
}
