namespace fxl.codes.tumblr.web.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        /// <summary>
        ///     Hashed password stored in database
        /// </summary>
        public string Password { get; set; }

        public string DisplayName { get; set; }
    }
}