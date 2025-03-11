namespace BDDSyncToAzureDevops_.Models
{
    public class CreateSuiteModal
    {
        public Value[] value { get; set; }
        public int count { get; set; }
    }

    public class Value
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

    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Plan
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Parent
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Lastupdatedby
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public _Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class _Links
    {
        public Avatar avatar { get; set; }
    }

    public class Avatar
    {
        public string href { get; set; }
    }

}