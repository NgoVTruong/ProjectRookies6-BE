namespace FinalAssignment.DTOs.User
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public IList<string> Roles { get; set; }
        public string? User { get; set; }
        public string Location { get; set; }
        public int LoginState { get; set; }
    }
}