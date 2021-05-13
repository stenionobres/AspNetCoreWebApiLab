using System.Collections.Generic;
using AspNetCoreWebApiLab.Api.Models.V1;

namespace AspNetCoreWebApiLab.Api.Models.SeedDatabase
{
    public class UsersModel
    {
        public static List<UserPostModel> Users = new List<UserPostModel>()
        {
            new UserPostModel() { FirstName = "Api user", LastName = "Api user", Email = "apiuser@email.com", Occupation = "User", Password = "Password@1234!" },
            new UserPostModel() { FirstName = "Jones", LastName = "Andersen", Email = "jones_andersen@email.com", Occupation = "Engineer", Password = "Password@1234!" },
            new UserPostModel() { FirstName = "Miller", LastName = "Bolton", Email = "miller_bolton@email.com", Occupation = "Architect", Password = "Password@1234!" },
            new UserPostModel() { FirstName = "Taylor", LastName = "Coleman", Email = "taylor_coleman@email.com", Occupation = "Policeman", Password = "Password@1234!" },
            new UserPostModel() { FirstName = "Martin", LastName = "Arnold", Email = "martin_arnold@email.com", Occupation = "Investor", Password = "Password@1234!" },
            new UserPostModel() { FirstName = "Robinson", LastName = "Austin", Email = "robinson_austin@email.com", Occupation = "Business man", Password = "Password@1234!" },
            new UserPostModel() { FirstName = "Lewis", LastName = "Bartlett", Email = "lewis_bartlett@email.com", Occupation = "Doctor", Password = "Password@1234!" },
            new UserPostModel() { FirstName = "Scott", LastName = "Donaldson", Email = "scott_donaldson@email.com", Occupation = "Teacher", Password = "Password@1234!" },
            new UserPostModel() { FirstName = "Nelson", LastName = "Chase", Email = "nelson_chase@email.com", Occupation = "Mechanic", Password = "Password@1234!" },
            new UserPostModel() { FirstName = "Edwards", LastName = "Bauer", Email = "edwards_bauer@email.com", Occupation = "Actor", Password = "Password@1234!" }
        };
    }
}
