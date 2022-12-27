using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Food.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace Food.Controllers
{
    public class TestController : Controller
    {

        private readonly ApplicationDbContext _context;


        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Route("/test")]
        [HttpGet]
        public IActionResult Index()
        {
            

            return View();
        }
        [Route("create-checkout-session")]
        [HttpPost]
        public ActionResult Create()
        {
            var domain = "http://localhost:44319";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = "5571234",
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "/success.html",
                CancelUrl = domain + "/cancel.html",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public void Mail()
        {
            var smtpacountJson = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MailSettings")["Mail"];
            var smtppasswordJson = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MailSettings")["Password"];

            String noidung = "Test noi dung";
            String mailnhan = "thaibao0225@gmail.com";
            String mailgui = "tuanntmmo@gmail.com";
            String chude = "Kiểm tra email gửi đi";
            string smtpacount = smtpacountJson.ToString();
            string smtppassword = smtppasswordJson.ToString();

            MailUtils.MailUtils.SendMailGoogleSmtp(
                mailgui,
                mailnhan,
                chude,
                noidung,
                smtpacount,
                smtppassword

            ).Wait();
        }
    }
}
