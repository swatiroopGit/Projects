using Gherkin.Ast;

namespace BDDSyncToAzureDevops_.Models
{
    // Helper classes for feature and scenarios
    internal class Feature
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public List<ScenarioDetails> Scenarios { get; set; }
        public Tag existingSuiteId { get; set; }
    }
}
