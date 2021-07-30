namespace CoolParking.WebAPI.Models
{
    public class ErrorResponseData
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }

        public ErrorResponseData(int status, string title, string message)
        {
            this.Title = title;
            this.Status = status;
            this.Message = message;
        }
    }
}
