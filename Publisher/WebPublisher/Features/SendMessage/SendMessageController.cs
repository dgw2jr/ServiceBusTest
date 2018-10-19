using System.Web.Mvc;
using MassTransit;
using Messages;
using WebPublisher.Features.SendMessage;

namespace WebPublisher.Controllers
{
    public class SendMessageController : Controller
    {
        private readonly IBus _serviceBus;

        public SendMessageController(IBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public ActionResult Index()
        {
            return View(new SendMessageViewModel());
        }

        public ActionResult SendMessage(SendMessageViewModel model)
        {
            _serviceBus.Publish<IHelloWorldMessage>(new { model.Message });

            return RedirectToAction("Index", new SendMessageViewModel());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}