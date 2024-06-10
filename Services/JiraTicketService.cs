using CourseProject_backend.Entities;
using CourseProject_backend.Models.RequestModels;
using CustomJiraTicketClient.Jira;
using CustomJiraTicketClient.Jira.Models;
using CustomJiraTicketClient.Jira.Models.FieldTypes;

namespace CourseProject_backend.Services
{
    public class JiraTicketService
    {
        private readonly JiraTicketClient _ticketClient;
        private string _projectKey;

        public JiraTicketService
        (
            JiraTicketClient ticketClient,
            string projectKey
        )
        {
            _projectKey = projectKey;
            _ticketClient = ticketClient;
        }

        public async Task<bool> AddTicketAsync(JiraTicketCreateModel model, User user)
        {
            JiraUser? userJira = await _ticketClient.GetOrCreateUserAsync(user.Email);

            if(userJira == null)
                return false;

            JiraTicket ticket = new JiraTicket(summary : model.Summary,
                                               reporter : userJira,
                                               description : model.Description,
                                               prioritet : model.Prioritet,
                                               collection : model.Collection,
                                               link : model.Link,
                                               projectKey : _projectKey);

            TicketCreatedInfo? createdInfo = await _ticketClient.CreateTicketAsync(ticket);

            if (createdInfo == null || createdInfo.Id == null || createdInfo.Key == null)
                return false;

            return true;
        }

        public async Task<bool> DeleteTicketAsync(string idOrKey)
        {
            return await _ticketClient.DeleteTicketAsync(idOrKey);
        } 

        public async Task<IEnumerable<JiraTicket>> GetUserTickets(string email, int startAt = 0, int maxCount = 50)
        {
            JiraUser? userJira = await _ticketClient.GetOrCreateUserAsync(email);

            if (userJira == null)
                return new List<JiraTicket>();

            IEnumerable<JiraTicket> tickets = await _ticketClient.GetTicketsByReporterIdAsync(reporterId: userJira.AccounId,
                                                      startAt: startAt,
                                                      limit: maxCount);

            return tickets;
        }

        public async Task<JiraUser?> GetOrCreateUserAsync(string email)
        {
            JiraUser? user = await _ticketClient.GetOrCreateUserAsync(email);

            if (user == null)
                throw new BadHttpRequestException("No Response from Jira Rest Api");

            return user;
        }

        public async Task<bool> DeleteUserAsync(string email)
        {
            JiraUser? userJira = await _ticketClient.GetUserByEmailAsync(email);

            if (userJira == null)
                return false;

            return await _ticketClient.DeleteUserByAccountIdAsync(userJira.AccounId);
        }
    }
}
