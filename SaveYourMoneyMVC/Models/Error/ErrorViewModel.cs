using System.Net;

namespace SaveYourMoneyMVC.Models.Error
{
    public class ErrorViewModel
    {
        public string Title { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}