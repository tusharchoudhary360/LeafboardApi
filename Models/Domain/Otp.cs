namespace AuthApi.Models.Domain
{
    public class Otp
    {
        public int Id { get; set; }
        public int otp { get; set; }
        public string UserEmail { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
