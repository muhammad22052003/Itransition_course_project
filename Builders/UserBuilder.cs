using CourseProject_backend.Entities;
using CourseProject_backend.Enums.Entities;
using CourseProject_backend.Interfaces;
using CourseProject_backend.Interfaces.Entities;

namespace CourseProject_backend.Builders
{
    public class UserBuilder : IBuilder
    {
        protected IDBModel _model { get; set; }
        protected IConfiguration _configuration { get; set; }

        public UserBuilder(IConfiguration configuration = null)
        {
            _configuration = configuration;

            User user = new User();

            user.Id = Guid.NewGuid().ToString().Replace("-", "");
            user.Role = UserRoles.Viewer.ToString();
            user.RegistrationTime = DateTime.UtcNow;
            user.Comments = new List<Comment>();
            user.PositiveReactions = new List<PositiveReaction>();
            user.Collections = new List<MyCollection>();
            user.IsBlocked = false;

            _model = user;
        }

        public void SetParameters
        (
            string name,
            string email,
            string password,
            UserRoles role,
            bool isBlocked = false
        )
        {
            User user = _model as User;

            user.Name = name;
            user.Email = email;
            user.Password = password;
            user.Role = role.ToString();
            user.IsBlocked = isBlocked;

            _model = user;
        }

        public IDBModel Build()
        {
            if (_model == null)
            {
                throw new Exception("Error trying retry build");
            }

            IDBModel result = _model;
            _model = null;
            return result;
        }
    }
}
