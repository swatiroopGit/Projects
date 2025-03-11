using BDDSyncToAzureDevops_.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using Gherkin.Ast;

namespace BDDSyncToAzureDevops_.TestActions
{
    internal class TestcaseActions
    {
        // Create test case in Azure DevOps
        internal static string CreateOrUpdateTestCase(string suiteId, string title, List<Step> steps, IReadOnlyCollection<Examples> examples, List<string> tags, string createOrUpdate, string existingId)
        {
            string stepBody = "";
            var stepsJson = new JArray();
            int i = 0;
            foreach (var step in steps)
            {
                if (IsStepValid($"{step.Keyword} {step.Text}"))
                {
                    var paramStep = step.Text.Replace("\"", "");
                    paramStep = System.Text.RegularExpressions.Regex.Replace(step.Text, @"['""]<(\w+)>['""]", "@$1");
                    i++;
                    stepBody = stepBody + $"<step id='{i}' type='Action'><parameterizedString isformatted='true'>{step.Keyword} {paramStep}</parameterizedString><parameterizedString isformatted='true'>{""}</parameterizedString></step>";

                }
            }
            stepBody = $"<steps>{stepBody}</steps>";
            var parameters = new JArray();
            string parametersString = "";
            string localdataString = "";
            string localDataParams = "";
            if (examples != null)
            {
                foreach (var example in examples)
                {
                    List<string> headers = new List<string>();
                    foreach (var cell in example.TableHeader.Cells)
                    {
                        parametersString = parametersString + $"<param name=\'{cell.Value}\' bind=\'default\' />";
                        localDataParams = localDataParams + $"<xs:element name='{cell.Value}' type='xs:string' minOccurs='0' />";
                        headers.Add(cell.Value);
                    }


                    foreach (var row in example.TableBody)
                    {
                        var paramRow = new JObject();
                        int index = 0;
                        localdataString = localdataString + $"<Table1>";
                        foreach (var cell in row.Cells)
                        {
                            var paramHeader = headers.ElementAt(index);
                            localdataString = localdataString + $"<{paramHeader}>{cell.Value}</{paramHeader}>";
                            index++;
                        }
                        localdataString = localdataString + $"</Table1>";
                        parameters.Add(paramRow);
                    }
                }
            }

            //Tags
            string tagString = string.Join(",", tags);
            //parameters
            parametersString = $"<parameters>{parametersString}</parameters>";
            //localdata
            localdataString = $"<NewDataSet>  <xs:schema id=\'NewDataSet\' xmlns=\'\' xmlns:xs=\'http://www.w3.org/2001/XMLSchema\' xmlns:msdata=\'urn:schemas-microsoft-com:xml-msdata\'>    <xs:element name=\'NewDataSet\' msdata:IsDataSet=\'true\' msdata:Locale=\'\'>      <xs:complexType>        <xs:choice minOccurs=\'0\' maxOccurs=\'unbounded\'>          <xs:element name=\'Table1\'>            <xs:complexType>              <xs:sequence>                {localDataParams}             </xs:sequence>            </xs:complexType>          </xs:element>        </xs:choice>      </xs:complexType>    </xs:element>  </xs:schema>{localdataString}</NewDataSet>";

            RestClient client;
            RestRequest request;
            if (createOrUpdate.Equals("create"))
            {
                client = new RestClient($"https://dev.azure.com/{Program.organization}/{Program.project}/_apis/wit/workitems/$Test%20Case?api-version=7.1-preview.3");
                request = new RestRequest("", Method.Post);
            }
            else
            {
                client = new RestClient($"https://dev.azure.com/{Program.organization}/{Program.project}/_apis/wit/workitems/{existingId}?api-version=7.1-preview.3");
                request = new RestRequest("", Method.Patch);
            }

            request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($":{Program.pat}"))}");
            request.AddHeader("Content-Type", "application/json-patch+json");

            var body = new JArray
        {
            new JObject { ["op"] = "add", ["path"] = "/fields/System.Title", ["value"] = title },
             new JObject { ["op"] = "add", ["path"] = "/fields/Microsoft.VSTS.Common.Priority", ["value"] = 2 },
              new JObject { ["op"] = "add", ["path"] = "/fields/System.Description", ["value"] = "desc" },
            new JObject { ["op"] = "add", ["path"] = "/fields/Microsoft.VSTS.TCM.Steps", ["value"] = stepBody},
            new JObject { ["op"] = "add", ["path"] = "/fields/System.Tags", ["value"] = tagString},
            new JObject { ["op"] = "add", ["path"] = "/fields/Microsoft.VSTS.TCM.Parameters", ["value"] = parametersString },
             new JObject { ["op"] = "add", ["path"] = "/fields/Microsoft.VSTS.TCM.LocalDataSource", ["value"] = localdataString }
        };

            //string _body = $"[\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/System.Title\",\r\n    \"value\": \"{title}\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.Common.Priority\",\r\n    \"value\": 2\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/System.Description\",\r\n    \"value\": \"Test desc\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.TCM.Steps\",\r\n    \"value\": \"{stepBody}\"\r\n  }}\r\n]";
            //  string _body = $"[\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/System.Title\",\r\n    \"value\": \"4) Verify message about sharing information when privacy policy is declined\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.TCM.Steps\",\r\n    \"value\": \"<steps><step id='1' type='Action'><parameterizedString isformatted='true'>Given  my privacy policy is '<privacy policy>'</parameterizedString><parameterizedString isformatted='true'></parameterizedString></step><step id='2' type='Action'><parameterizedString isformatted='true'>And  I am on Create Manual step 1 screen</parameterizedString><parameterizedString isformatted='true'></parameterizedString></step><step id='3' type='Action'><parameterizedString isformatted='true'>Then  I '<status>' a message regarding sharing of sensitive information</parameterizedString><parameterizedString isformatted='true'></parameterizedString></step></steps>\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.TCM.Parameters\",\r\n    \"value\": \"[\\r\\n  {{\\r\\n    \\\"not accepted\\\": \\\"not accepted\\\",\\r\\n    \\\"see\\\": \\\"see\\\"\\r\\n  }},\\r\\n  {{\\r\\n    \\\"accepted\\\": \\\"accepted\\\",\\r\\n    \\\"dont see\\\": \\\"dont see\\\"\\r\\n  }}\\r\\n]\"\r\n  }}\r\n]";
            //  string _body = $"[\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/System.Title\",\r\n    \"value\": \"{title}\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.Common.Priority\",\r\n    \"value\": 2\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/System.Description\",\r\n    \"value\": \"desc\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.TCM.Steps\",\r\n    \"value\": \"{stepBody}\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/System.Tags\",\r\n    \"value\": \"{tagString}\"\r\n  }}\r\n]";


            string _body = $"[\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/System.Title\",\r\n    \"value\": \"{title}\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.Common.Priority\",\r\n    \"value\": 2\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/System.Description\",\r\n    \"value\": \"desc\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.TCM.Steps\",\r\n    \"value\": \"{stepBody}\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/System.Tags\",\r\n    \"value\": \"{tagString}\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.TCM.Parameters\",\r\n    \"value\": \"{parametersString}\"\r\n  }},\r\n  {{\r\n    \"op\": \"add\",\r\n    \"path\": \"/fields/Microsoft.VSTS.TCM.LocalDataSource\",\r\n    \"value\": \"{localdataString}\"\r\n  }}\r\n]";
            request.AddJsonBody(_body);

            var response = client.Execute(request);
            var jsonResponse = JsonConvert.DeserializeObject<CreateTestcaseModal>(response.Content);
            return jsonResponse.id.ToString();
        }

        // Add test case to suite in Azure DevOps
        internal static void AddTestCaseToSuite(string suiteId, string testCaseId)
        {
            var client = new RestClient($"https://dev.azure.com/{Program.organization}/{Program.project}/_apis/test/Plans/{Program.planId}/suites/{suiteId}/testcases/{testCaseId}?api-version=5.0");
            var request = new RestRequest("", Method.Post);
            request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($":{Program.pat}"))}");

            client.Execute(request);
        }

        // Check if step text is valid (starts with Given, When, Then, And, or But)
        internal static bool IsStepValid(string stepText)
        {
            return stepText.StartsWith("Given") || stepText.StartsWith("When") || stepText.StartsWith("Then") || stepText.StartsWith("And") || stepText.StartsWith("But");
        }
    }
}
