using Bogus;
using Demo.API.Models;

namespace Demo.API.Functions
{
    public static class User
    {
        /// <summary>
        /// Uses the Bogus library to create a fake user:
        /// </summary>
        public static UserRecord CreateFake()
        {

            var fakePerson = new Person();
            var user = new UserRecord(
                Guid.NewGuid(),
                fakePerson.FirstName,
                fakePerson.LastName);

            return user;
        }
    }
}
