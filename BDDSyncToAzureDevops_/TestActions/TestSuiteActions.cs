using BDDSyncToAzureDevops_.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace BDDSyncToAzureDevops_.TestActions
{
    internal class TestSuiteActions
    {
        // Create test suite in Azure DevOps
        internal static string CreateTestSuite(string suiteName)
        {
            //var client = new RestClient($"https://dev.azure.com/{organization}/{project}/_apis/testplan/Plans/{planId}/suites/{parentSuiteId}?api-version=7.1-preview.1");
            var client = new RestClient($"https://dev.azure.com/{Program.organization}/{Program.project}/_apis/test/Plans/{Program.planId}/suites/{Program.parentSuiteId}?api-version=5.0");
            var request = new RestRequest("", Method.Post);
            request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($":{Program.pat}"))}");
            request.AddJsonBody(new { name = suiteName, suiteType = "StaticTestSuite" });

            var response = client.Execute(request);
            var jsonResponse = JsonConvert.DeserializeObject<CreateSuiteModal>(response.Content);
            return jsonResponse.value.ElementAt(0).id.ToString();
        }

        // Update test suite in Azure DevOps
        internal static string UpdateTestSuite(string suiteId, string suiteName)
        {
            var client = new RestClient($"https://dev.azure.com/{Program.organization}/{Program.project}/_apis/test/Plans/{Program.planId}/suites/{suiteId}?api-version=5.0");
            var request = new RestRequest("", Method.Patch);
            request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($":{Program.pat}"))}");
            request.AddJsonBody(new { name = suiteName, suiteType = "StaticTestSuite" });

            var response = client.Execute(request);
            var jsonResponse = JsonConvert.DeserializeObject<UpdateSuiteResponse>(response.Content);
            return jsonResponse.id.ToString();
        }
    }
}
