namespace AuthApi.Models.Domain
{
    public class Token
    {

        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
