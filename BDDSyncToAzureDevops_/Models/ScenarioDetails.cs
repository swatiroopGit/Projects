using Gherkin.Ast;

namespace BDDSyncToAzureDevops_.Models
{
    class ScenarioDetails
    {
        public string Title { get; set; }
        public List<Step> Steps { get; set; }
        public List<string> Tags { get; set; }
        public IReadOnlyCollection<Examples> Examples { get; set; }

        public Tag existingTestcaseId { get; set; }
    }
}
