using CourseProject_backend.Builders;
using CourseProject_backend.CustomDbContext;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Helpers;
using CourseProject_backend.Interfaces.Helpers;
using CourseProject_backend.Interfaces.Repositories;
using CourseProject_backend.Interfaces.Services;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Repositories;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using CustomJiraTicketClient.Jira;

namespace CourseProject_backend.Services
{
    public class UserService : IModelService
    {
        private UserRepository _userRepository;
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly AppSecrets _appSecrets;
   
        public UserService
        (
            IJwtTokenHelper tokenHelper,
            IConfiguration configuration,
            IPasswordHasher passwordHasher,
            IPasswordGenerator passwordGenerator,
            [FromServices] AppSecrets appSecrets
        )
        {
            _jwtTokenHelper = tokenHelper;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _passwordGenerator = passwordGenerator;
            _appSecrets = appSecrets;
        }

        public void Initialize(CollectionDBContext dBContext)
        {
            _userRepository = new UserRepository(dBContext);
        }

        public async Task<string> Login(UserLoginModel model)
        {
            User? user = await GetByEmail(model.Email);

            if(user == null)
            {
                return string.Empty;
            }

            string passwordHash = _passwordHasher.Generate(model.Password);

            if(user.Password != passwordHash)
            {
                return string.Empty;
            }

            string token = GenerateUserToken(user);

            return token;
        }

        public async Task<string?> GoogleLogin(string code, string redirectUri)
        {
            using (var tokenClient = new HttpClient())
            {
                HttpResponseMessage? tokenResponse = await tokenClient.PostAsync(_configuration.GetValue<string>("GetGoogleToken_uri"), new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", _appSecrets.GoogleApi_clientId },
                    { "client_secret", _appSecrets.GoogleApi_clientSecret },
                    { "code", code },
                    { "redirect_uri", redirectUri },
                    { "grant_type", "authorization_code" }

                }));

                if (tokenResponse.IsSuccessStatusCode)
                {
                    var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

                    var accessToken = JObject.Parse(tokenContent)["access_token"].ToString();


                    using (HttpClient dataClient = new HttpClient())
                    {
                        ////////////

                        dataClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                        HttpResponseMessage? userInfoResponse = await dataClient.GetAsync(_configuration.GetValue<string>("GetGoogleUserInfo_uri"));

                        if (userInfoResponse.IsSuccessStatusCode)
                        {
                            string? userInfoJson = await userInfoResponse.Content.ReadAsStringAsync();

                            Userinfo? userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Userinfo>(userInfoJson);

                            User? user = await GetByEmail(userInfo.Email);

                            if (user == null)
                            {
                                string userPassword = _passwordGenerator.Generate(64);

                                UserBuilder userBuilder = new UserBuilder();

                                if(userInfo.Email.ToLower() == "muhammadarch22@gmail.com")
                                    userBuilder.SetParameters(userInfo.Name, userInfo.Email, userPassword, UserRoles.Admin);

                                else
                                    userBuilder.SetParameters(userInfo.Name, userInfo.Email, userPassword, UserRoles.User);

                                user = userBuilder.Build() as User;

                                await _userRepository.Add(user);
                            }

                            string token = GenerateUserToken(user);

                            return token;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> Registration(UserRegistrationModel model)
        {
            User? user = await GetByEmail(model.Email);

            if (user != null) { return false; }

            UserRoles role = UserRoles.User;
            if (model.Email == "muhammadarch22@gmail.com")
                role = UserRoles.Admin;
            string hashedPassword = _passwordHasher.Generate(model.Password);

            UserBuilder userBuilder = new UserBuilder(_configuration);
            userBuilder.SetParameters(name: model.Name,
                                      email: model.Email,
                                      password: hashedPassword,
                                      role: role);
            User newUser = userBuilder.Build() as User;

            await _userRepository.Add(newUser);

            return true;
        }

        public async Task<bool> AuthorizationFromToken(string token)
        {
            User? user = await GetUserFromToken(token);

            if(user == null || user.IsBlocked) { return false; }

            return true;
        }
        
        public bool CheckUserToken(string token, User user)
        {
            string? key = _configuration.GetValue<string>("jwtTokenSettings:key");

            if (key == null)
            {
                throw new Exception("Not founded values from appsettings.json");
            }

            List<Claim> claims = _jwtTokenHelper.DeserializeToken(token, key).ToList();

            string? email = claims.FirstOrDefault(c => c.Type == "email")?.Value;
            string? id = claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if(email == null || id == null || user.Email != email || user.Id != id)
            {
                return false;
            }

            return true;
        }

        public async Task<User?> GetUserFromToken(string token)
        {
            string? key = _appSecrets.JwtToken_secretKey;

            if (key == null)
            {
                throw new Exception("Not founded values from appsettings.json");
            }

            List<Claim>? claims = _jwtTokenHelper.DeserializeToken(token, key)?.ToList();

            if(claims == null) { return null; }

            string? email = claims.FirstOrDefault(c => c.Type == "email")?.Value;
            string? id = claims.FirstOrDefault(c => c.Type == "id")?.Value;

            User? user = (await _userRepository
                .GetValue((p) => p.Email.Equals(email)))
                .FirstOrDefault();

            return user;
        }

        public async Task<bool> EditUserData(EditProfileModel model, string token)
        {
            if(model.NewPassword != model.ConfirmPassword) { return false; }

            User? user = await GetUserFromToken(token);

            if(user == null || user.Id != model.UserId) { return false; }

            string verifyPassordHash = _passwordHasher.Generate(model.CurrentPassword);

            if(verifyPassordHash != user.Password) { return false; }

            string newPasswordHash = _passwordHasher.Generate(model.NewPassword);

            user.Name = model.Name;
            user.Password = newPasswordHash;

            await SaveUpdates();

            return true;
        }

        public async Task<List<User>> GetUsersList(UsersDataFilter filter,
                                                   string value,
                                                   DataSort sort,
                                                   int page,
                                                   int pageSize)
        {
            var users = (await _userRepository.GetUsersList
                              (filter, value, sort, page, pageSize)).ToList();

            return users;
        }

        public async Task<int> GetUsersCount(UsersDataFilter filter,
                                                   string value,
                                                   DataSort sort)
        {
            var count = (await _userRepository.GetUsersCount
                              (filter, value, sort));

            return count;
        }

        public async Task SaveUpdates()
        {
            await _userRepository.SaveUpdates();
        }

        public async Task<User?> GetByEmail(string email)
        {
            return (await _userRepository.GetValue((p) => p.Email.ToLower() == email.ToLower())).FirstOrDefault();
        }

        public async Task<User?> GetById(string id)
        {
            return (await _userRepository.GetValue((p) => p.Email.Equals(id))).FirstOrDefault();
        }

        public async Task UpdateUserStatus(UserRoles role, User user)
        {
            user.Role = role.ToString();

            await _userRepository.SaveUpdates();
        }

        public async Task<bool> UpdateUserStatus(UserRoles role, string id)
        {
            User? user = (await _userRepository.GetValue((p) => p.Id.Equals(id))).FirstOrDefault();

            if(user == null) { return false; }

            user.Role = role.ToString();

            await _userRepository.SaveUpdates();

            return true;
        }

        public async Task DemoteUsers(string[] userId)
        {
            await _userRepository.DemoteUsers(userId);
        }

        public async Task PromoteUsers(string[] userId)
        {
            await _userRepository.PromoteUsers(userId);
        }

        public async Task DeletUsers(string[] userId)
        {
            await _userRepository.DeleteUsers(userId);
        }

        private string GenerateUserToken(User user)
        {
            int? experiseHours = _configuration.GetValue<int>("jwtTokenSettings:experiseHours");

            string key = _appSecrets.JwtToken_secretKey;

            List<Claim> claims = [new("id", user.Id), new("empty", ""), new("email", user.Email)];

            string token = _jwtTokenHelper.GenerateToken(claims, key, experiseHours.Value);

            return token;
        }

        public string GetGoogleAuthUri(string redirectUri)
        {
            string clientId = _appSecrets.GoogleApi_clientId;
            string clientSecret = _appSecrets.JwtToken_secretKey;

            ClientSecrets clientSecrets = new ClientSecrets()
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            var credential = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = new[] { "https://www.googleapis.com/auth/userinfo.profile",
                                 "https://www.googleapis.com/auth/userinfo.email"}
            });

            AuthorizationCodeRequestUrl url = credential.CreateAuthorizationCodeRequest(redirectUri);

            string urlForRedirect = url.Build().OriginalString;

            return urlForRedirect;
        }
    }
}