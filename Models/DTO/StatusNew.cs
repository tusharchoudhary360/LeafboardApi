namespace AuthApi.Models.DTO
{
    public class StatusNew
    {
        public StatusNew()
        {

        }
        public StatusNew(int status1, string status_message1)
        {
            StatusCode = status1;
            Message = status_message1;
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
        
    }
}
