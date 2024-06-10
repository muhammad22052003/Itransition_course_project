using CustomJiraTicketClient.Jira.Models;
using CustomJiraTicketClient.Jira.Models.FieldTypes;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomJiraTicketClient.Jira
{
    public class JiraTicketClient
    {
        public RestClient RestClient { get; private set; }

        public JiraTicketClient(string baseUrl, string userName, string tokenOrPassword)
        {
            ConfigureRestClient configureRestClient = new ConfigureRestClient((x) =>
            {
                x.Authenticator = new HttpBasicAuthenticator(userName, tokenOrPassword);
            });

            RestClient = new RestClient(baseUrl, configureRestClient : configureRestClient);
        }

        public async Task<JiraUser?> GetOrCreateUserAsync(string email)
        {
            RestRequest request = new RestRequest("rest/api/3/user", Method.Post);

            request.AddBody(new
            {
                emailAddress = email,
                products = new[] { "jira-software" }
            });

            var response = await RestClient.ExecuteAsync(request);

            if (response == null ||
               !response.IsSuccessStatusCode)
                return null;

            JiraUser? jiraUser = JsonSerializer.Deserialize<JiraUser>(response.Content);

            return jiraUser;
        }

        public async Task<JiraUser?> GetUserByEmailAsync(string email)
        {
            RestRequest request = new RestRequest($"rest/api/3/user/search?query={email}", Method.Get);

            var response =  await RestClient.ExecuteAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var data = JsonSerializer.Deserialize<IEnumerable<JiraUser>>(response.Content);

            var user = data.FirstOrDefault();

            if (user == null)
                return null;

            user.Email = email;

            return user;
        }

        public async Task<bool> DeleteUserByAccountIdAsync(string accountId)
        {
            RestRequest request = new RestRequest($"rest/api/3/user?accountId={accountId}", Method.Delete);

            var response = await RestClient.ExecuteAsync(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
                return false;

            return true;
        }

        public async Task<JiraTicket?> GetTicketAsync(string idOrIssueKey)
        {
            RestRequest request = new RestRequest($"rest/api/3/issue/{idOrIssueKey}", Method.Get);

            var response = await RestClient.ExecuteAsync(request);

            if (response == null ||
               !response.IsSuccessStatusCode)
                return null;

            TicketDeSerializedData? data = JsonSerializer
                                           .Deserialize<TicketDeSerializedData>(response.Content);

            if (data == null)
                return null;

            JiraTicket jiraTicket = data.GetTicket();

            return jiraTicket;
        }

        public async Task<IEnumerable<JiraTicket>> GetTicketsByReporterIdAsync(string reporterId, int startAt = 0, int limit = 50)
        {
            string query = $"project = BYT AND 'Reported' = \"{reporterId}\"";
            RestRequest request = new RestRequest($"rest/api/3/search", Method.Post);

            request.AddJsonBody(new
            {
                fields = new[]
                {
                    "*all"
                },
                fieldsByKeys = false,
                jql = query,
                maxResults = limit,
                startAt = startAt
            });

            var response = await RestClient.ExecuteAsync(request);

            if (response == null || !response.IsSuccessStatusCode)
                return null;

            JsonDocument jsonDocument = JsonDocument.Parse(response.Content);
            string json = jsonDocument.RootElement.GetProperty("issues").ToString();

            IEnumerable<TicketDeSerializedData>? data = JsonSerializer.Deserialize<IEnumerable<TicketDeSerializedData>>(json);

            IEnumerable<JiraTicket> tickets = data.Select(x => x.GetTicket());

            return tickets;
        }

        public async Task<TicketCreatedInfo?> CreateTicketAsync(JiraTicket ticket)
        {
            var requestBody = new
            {
                fields = new
                {
                    summary = ticket.Summary,
                    customfield_10053 = new
                    {
                        value = ticket.Prioritet.Value,
                    },
                    customfield_10050 = new[]
                    {
                        new
                        {
                            id = ticket.Reporter.AccounId,
                            emailAddress = ticket.Reporter.Email
                        }
                    },
                    customfield_10056 = ticket.Collection,
                    customfield_10052 = ticket.Link,
                    description = new
                    {
                        content = new[]
                        {
                            new
                            {
                                content = new[]
                                {
                                    new
                                    {
                                        text = ticket.Description,
                                        type = "text"
                                    }
                                },
                                type = "paragraph"
                            }
                        },
                        type = "doc",
                        version = 1
                    },
                    customfield_10015 = ticket.Created,
                    project = new
                    {
                        id = ticket.Project.Id,
                        key = ticket.Project.Key,
                    },
                    issuetype = new
                    {
                        id = ticket.Type.Id,
                        name = ticket.Type.Name, 
                    },
                }
            };

            string json = JsonSerializer.Serialize(requestBody);

            RestRequest request = new RestRequest($"rest/api/3/issue", Method.Post);

            request.AddBody(json);

            var response = await RestClient.ExecuteAsync(request);

            TicketCreatedInfo? data = JsonSerializer
                                           .Deserialize<TicketCreatedInfo>(response.Content);

            if (data == null)
                return null;

            return data;
        }

        public async Task<bool> DeleteTicketAsync(string keyOrId)
        {
            RestRequest request = new RestRequest($"rest/api/3/issue/{keyOrId}", Method.Delete);

            var response = await RestClient.ExecuteAsync(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
                return false;

            return true;
        }
    }
}
