using Gherkin.Ast;
using Gherkin;
using BDDSyncToAzureDevops_.Models;
using Feature = BDDSyncToAzureDevops_.Models.Feature;

namespace BDDSyncToAzureDevops_.ParseFeature
{
    internal class FeatureAction
    {
        // Parse feature file and return feature object
        internal static Feature ParseFeatureFile(string filePath)
        {
            var parser = new Parser();
            using (var reader = new StreamReader(filePath))
            {
                var document = parser.Parse(reader);
                var feature = document.Feature;

                //Extract feature tags
                var featureTags = feature.Tags.Where(x => !x.Name.StartsWith(Program.tcTagFormat) && !x.Name.StartsWith(Program.suiteIdTagFormat)).Select(t => t.Name).ToList();
                var _existingSuiteId = feature.Tags.FirstOrDefault(x => x.Name.StartsWith(Program.suiteIdTagFormat));

                // Collect scenarios
                var scenarios = new List<ScenarioDetails>();
                foreach (var scenarioDefinition in feature.Children)
                {
                    if (scenarioDefinition is Scenario scenario)
                    {
                        scenarios.Add(new ScenarioDetails
                        {
                            Title = scenario.Name,
                            Steps = scenario.Steps.ToList(),
                            Tags = scenario.Tags.Where(x => !x.Name.StartsWith(Program.tcTagFormat)).Select(t => t.Name).ToList(),
                            Examples = scenario.Examples.ToList(),
                            existingTestcaseId = scenario.Tags.FirstOrDefault(x => x.Name.StartsWith(Program.tcTagFormat))
                        });
                    }
                    //else if (scenarioDefinition is ScenarioOutline outline)
                    //{
                    //    scenarios.Add(new ScenarioDetails
                    //    {
                    //        Title = outline.Name,
                    //        Steps = outline.Steps,
                    //        Examples = outline.Examples
                    //    });
                    //}
                }

                return new Feature
                {
                    Name = feature.Name,
                    Tags = featureTags,
                    Scenarios = scenarios,
                    existingSuiteId = _existingSuiteId
                };
            }
        }
    }
}
