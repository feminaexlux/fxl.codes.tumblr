using System.Security;
using System.Threading.Tasks;
using Dapper;
using fxl.codes.tumblr.web.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Sodium;

namespace fxl.codes.tumblr.web.Services
{
    public class UserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<User> FindUser(User user)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetConnectionString("tumblr"));
            await connection.OpenAsync();

            var dbUser = connection.QuerySingle<User>("select * from users where users.username = @Username", user);
            await connection.CloseAsync();
            
            // User password is coming in unencrypted
            var valid = PasswordHash.ScryptHashStringVerify(dbUser.Password, user.Password);
            if (!valid) throw new SecurityException("Incorrect password");
            
            return dbUser;
        }
    }
}