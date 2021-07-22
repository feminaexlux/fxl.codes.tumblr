namespace fxl.codes.tumblr.web.Entities
{
    public class User
    {
        public int Id { get; init; }
        public string Username { get; init; }

        /// <summary>
        ///     Hashed password stored in database
        /// </summary>
        public string Password { get; init; }

        public string DisplayName { get; init; }
    }
}