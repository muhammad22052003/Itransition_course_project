using System.Text.Json;

namespace CourseProject_backend.Helpers
{
    public class AppSecrets
    {
        public string GoogleApi_clientId {  get; set; }
        public string GoogleApi_clientSecret {  get; set; }
        public string JwtToken_secretKey {  get; set; }
        public string NpgSql_connection {  get; set; }
        public string JiraUserName {  get; set; }
        public string JiraApiToken {  get; set; }
        public string JiraBaseUrl {  get; set; }


        public async Task<bool> InitData(string secretsUri)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (HttpClient client = new HttpClient(clientHandler))
                {
                    string jsonData = await client.GetStringAsync(secretsUri);

                    JsonDocument document = JsonDocument.Parse(jsonData);

                    GoogleApi_clientId = document.RootElement.GetProperty("google").GetProperty("googleApi_clientId").GetString();
                    GoogleApi_clientSecret = document.RootElement.GetProperty("google").GetProperty("googleApi_clientSecret").GetString();
                    JwtToken_secretKey = document.RootElement.GetProperty("jwtToken").GetProperty("jwtToken_secretKey").GetString();
                    NpgSql_connection = document.RootElement.GetProperty("database").GetProperty("npgsql_connection").GetString();

                    JiraUserName = document.RootElement.GetProperty("jira").GetProperty("UserName").GetString();
                    JiraApiToken = document.RootElement.GetProperty("jira").GetProperty("token").GetString();
                    JiraBaseUrl = document.RootElement.GetProperty("jira").GetProperty("baseUrl").GetString();
                }

                return AllDataIsValid();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool AllDataIsValid()
        {
            return (GoogleApi_clientId != null && GoogleApi_clientSecret != null &&
                    JwtToken_secretKey != null && NpgSql_connection != null);
        }
    }
}
