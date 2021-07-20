using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace fxl.codes.tumblr.web.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; init; }
        public string Username { get; init; }

        /// <summary>
        ///     Hashed password stored in database
        /// </summary>
        public string Password { get; init; }

        public string DisplayName { get; init; }

        public IEnumerable<Blog> Blogs { get; set; } = Array.Empty<Blog>();
    }
}