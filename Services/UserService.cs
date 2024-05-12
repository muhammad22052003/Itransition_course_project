using CourseProject_backend.Builders;
using CourseProject_backend.Entities;
using CourseProject_backend.Enums;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Helpers;
using CourseProject_backend.Interfaces.Helpers;
using CourseProject_backend.Interfaces.Repositories;
using CourseProject_backend.Interfaces.Services;
using CourseProject_backend.Models.RequestModels;
using CourseProject_backend.Repositories;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;

namespace CourseProject_backend.Services
{
    public class UserService : IModelService
    {
        private readonly IRepository<User> _repository;
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;

        public UserService
        (
            IRepository<User> repository,
            IJwtTokenHelper tokenHelper,
            IConfiguration configuration,
            IPasswordHasher passwordHasher
        )
        {
            _repository = repository;
            _jwtTokenHelper = tokenHelper;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
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

            List<Claim> claims = [new("id", user.Id), new("empty", ""), new("email", user.Email)];

            string? key = _configuration.GetValue<string>("jwtTokenSettings:key");
            int? experiseHours = _configuration.GetValue<int>("jwtTokenSettings:experiseHours");

            if(key == null || experiseHours == null)
            {
                throw new Exception("Not founded values from appsettings.json");
            }

            string token = _jwtTokenHelper.GenerateToken(claims, key, experiseHours.Value);

            return token;
        }

        public async Task<bool> Registration(UserRegistrationModel model)
        {
            User? user = await GetByEmail(model.Email);

            if (user != null) { return false; }

            UserBuilder userBuilder = new UserBuilder(_configuration);

            User newUser = userBuilder.Build() as User;

            newUser.Name = model.Name;
            newUser.Email = model.Email;
            newUser.Password = _passwordHasher.Generate(model.Password);
            newUser.Role = UserRoles.User.ToString();

            await _repository.Add(newUser);

            return true;
        }

        public async Task<bool> AuthorizationFromToken(string token)
        {
            User? user = await GetUserFromToken(token);

            if(user.IsBlocked) { return false; }

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
            string? key = _configuration.GetValue<string>("jwtTokenSettings:key");

            if (key == null)
            {
                throw new Exception("Not founded values from appsettings.json");
            }

            List<Claim> claims = _jwtTokenHelper.DeserializeToken(token, key).ToList();

            string? email = claims.FirstOrDefault(c => c.Type == "email")?.Value;
            string? id = claims.FirstOrDefault(c => c.Type == "id")?.Value;

            User? user = (await _repository
                .GetValue((p) => p.Email.Equals(email)))
                .FirstOrDefault();

            return user;
        }

        public async Task<List<User>> GetUsers(UsersDataFilter filter, string value, int countLimit)
        {
            List<User> users = new List<User>();

            switch (filter)
            {
                case UsersDataFilter.byName:
                    {
                        users = (await _repository
                            .GetValue((p) => p.Name == value)).ToList();
                    }break;
                case UsersDataFilter.byStatus:
                    {
                        if(Enum.TryParse(value, out UserRoles parseData)){
                            users = (await _repository
                                    .GetValue((p) => p.Role.ToLower() == value.ToLower())).ToList();
                        }
                        else
                        {
                            users = (await _repository
                                    .GetValue((p) => true)).ToList();
                        }
                    }
                    break;
                case UsersDataFilter.byId:
                    {
                        users = (await _repository
                            .GetValue((p) => p.Id == value)).ToList();
                    }
                    break;
                case UsersDataFilter.byDefault:
                    {
                        users = (await _repository
                            .GetValue((p) => true)).ToList();
                    }
                    break;
            }

            return users;
        }

        public List<User> SortData(List<User> users, DataSort sort)
        {
            switch (sort)
            {
                
            }
        }

        public async Task SaveUpdates()
        {
            await _repository.SaveUpdates();
        }

        public async Task<User?> GetByEmail(string email)
        {
            return (await _repository.GetValue((p) => p.Email == email)).FirstOrDefault();
        }

        public async Task<User?> GetById(string id)
        {
            return (await _repository.GetValue((p) => p.Email.Equals(id))).FirstOrDefault();
        }

        public async Task UpdateUserStatus(UserRoles role, User user)
        {
            user.Role = role.ToString();

            await _repository.SaveUpdates();
        }

        public async Task<bool> UpdateUserStatus(UserRoles role, string id)
        {
            User? user = (await _repository.GetValue((p) => p.Id.Equals(id))).FirstOrDefault();

            if(user == null) { return false; }

            user.Role = role.ToString();

            await _repository.SaveUpdates();

            return true;
        }
    }
}