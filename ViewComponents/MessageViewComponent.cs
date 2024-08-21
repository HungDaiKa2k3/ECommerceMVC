using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.ViewComponents
{
    public class MessageViewComponent:ViewComponent
    {
        public const string COMPONENTNAME = "Message";
        public class Message
        {
            public string title { get; set; } = "Thông báo";
            public string htmlcontent { get; set; } = "";
            public string urlredirect { get; set; } = "/";
            public int secondwait { set; get; } = 3;
        }

        public MessageViewComponent() { }

        public IViewComponentResult Invoke(Message message) 
        {
            //HttpContext.Response.Headers.Add("REFRESH", $"{message.secondwait};URL={message.urlredirect}");
            return View("Message",message);
        }
    }
}
