

namespace BDDSyncToAzureDevops_.Models
{

    public class UpdateSuiteResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Project project { get; set; }
        public Plan plan { get; set; }
        public Parent parent { get; set; }
        public int revision { get; set; }
        public int testCaseCount { get; set; }
        public string suiteType { get; set; }
        public string testCasesUrl { get; set; }
        public bool inheritDefaultConfigurations { get; set; }
        public string state { get; set; }
        public Lastupdatedby lastUpdatedBy { get; set; }
        public DateTime lastUpdatedDate { get; set; }
    }





}
