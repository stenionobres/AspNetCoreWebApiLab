using System.Linq;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebApiLab.Api.Tools;
using AspNetCoreWebApiLab.Api.Models.V1;
using AspNetCoreWebApiLab.Persistence.DataTransferObjects;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public UserModel Get(int userId)
        {
            var user = GetUserBy(userId);

            return user == null ? null :
            new UserModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Occupation = user.Occupation,
                Email = user.Email
            };
        }

        public void Save(UserModel user)
        {
            var identityUser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Occupation = user.Occupation,
                Email = user.Email,
                UserName = user.Email
            };

            var identityResult = _userManager.CreateAsync(identityUser).Result;
            user.Id = identityUser.Id;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public void Update(int userSavedId, UserModel user)
        {
            var identityUser = GetUserBy(userSavedId);

            identityUser.FirstName = user.FirstName;
            identityUser.LastName = user.LastName;
            identityUser.Occupation = user.Occupation;

            var identityResult = _userManager.UpdateAsync(identityUser).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public void Delete(UserModel user)
        {
            var identityUser = GetUserBy(user.Id);
            var identityResult = _userManager.DeleteAsync(identityUser).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public User GetUserBy(int id) => _userManager.Users.FirstOrDefault(r => r.Id.Equals(id));
    }
}
