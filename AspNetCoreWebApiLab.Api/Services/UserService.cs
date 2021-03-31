using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebApiLab.Api.Tools;
using AspNetCoreWebApiLab.Api.Models.V1;
using AspNetCoreWebApiLab.Api.Models.V2;
using AspNetCoreWebApiLab.Persistence.DataTransferObjects;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;

        public UserService(UserManager<User> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
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

        public async Task<UserModel> GetAsync(int userId)
        {
            var user = await GetUserAsyncBy(userId);

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

        public UserModel Save(UserPostModel user)
        {
            var identityUser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Occupation = user.Occupation,
                Email = user.Email,
                UserName = user.Email,
            };

            var identityResult = _userManager.CreateAsync(identityUser, user.Password).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);

            return new UserModel()
            {
                Id = identityUser.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Occupation = user.Occupation,
                Email = user.Email
            };
        }

        public async Task<UserModel> SaveAsync(UserPostModel user)
        {
            var identityUser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Occupation = user.Occupation,
                Email = user.Email,
                UserName = user.Email,
            };

            var identityResult = await _userManager.CreateAsync(identityUser, user.Password);

            CustomIdentityError.CatchErrorIfNeeded(identityResult);

            return new UserModel()
            {
                Id = identityUser.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Occupation = user.Occupation,
                Email = user.Email
            };
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

        public async Task UpdateAsync(int userSavedId, UserModel user)
        {
            var identityUser = await GetUserAsyncBy(userSavedId);

            identityUser.FirstName = user.FirstName;
            identityUser.LastName = user.LastName;
            identityUser.Occupation = user.Occupation;

            var identityResult = await _userManager.UpdateAsync(identityUser);

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public void Delete(UserModel user)
        {
            var identityUser = GetUserBy(user.Id);
            var identityResult = _userManager.DeleteAsync(identityUser).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public async Task DeleteAsync(UserModel user)
        {
            var identityUser = await GetUserAsyncBy(user.Id);
            var identityResult = await _userManager.DeleteAsync(identityUser);

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public User GetUserBy(int id) => _userManager.Users.FirstOrDefault(r => r.Id.Equals(id));

        public async Task<User> GetUserAsyncBy(int id) => await _userManager.FindByIdAsync(id.ToString());

        public string SignIn(SignInModel signInModel)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == signInModel.Email);

            if (user == null) return string.Empty;

            var userSignInResult = _userManager.CheckPasswordAsync(user, signInModel.Password).Result;

            if (userSignInResult)
            {
                return _jwtService.GenerateToken(user);
            }
            else
            {
                throw new ApplicationException("Invalid user password");
            }
        }
    }
}
