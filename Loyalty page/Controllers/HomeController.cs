using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AuthorizeNet.Api.Contracts.V1;
using Loyalty_page.AuthorizeNET;
using Microsoft.Ajax.Utilities;


namespace Loyalty_page.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }


        [HttpGet]
        public ActionResult GetDiscount(decimal amount = 0)
        {
            ViewBag.Points = Decimal.ToInt32(amount/10);
            ViewBag.Discount = ViewBag.Points > 1000;

            return View();
        }

        

        [HttpGet]
        public ActionResult Pay()
        {
            //firstName = "John", 

            //lastName = "Doe",
            string apiLoginId = WebConfigurationManager.AppSettings["ApiLoginID"];
            string apiTransactionKey = WebConfigurationManager.AppSettings["ApiTransactionKey"];
            ChargeCreditCard.Run(apiLoginId, apiTransactionKey, 1000);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetProfileByName()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetProfileByName(Customer customer)
        {
            string apiLoginId = WebConfigurationManager.AppSettings["ApiLoginID"];
            string apiTransactionKey = WebConfigurationManager.AppSettings["ApiTransactionKey"];

            string profileId = "";

            var profileIds = GetCustomerProfileIds.Run(apiLoginId, apiTransactionKey);

            foreach (var id in profileIds)
            {   
                var profiles = GetCustomerProfile.Run(apiLoginId, apiTransactionKey, id);
                for (int i = 0; i < profiles.Length; i++)
                {
                    if (profiles[i].FirstName == customer.FirstName && profiles[i].LastName == customer.LastName
                        && profiles[i].Email == customer.Email)
                    {
                        profileId = profiles[i].ProfileId;
                    }
                }
            }
            decimal totalAmnt = 0;
            if (profileId.IsNullOrWhiteSpace())
            {

            }
            else
            {
                totalAmnt = GetCustomerProfileAmount.Run(apiLoginId, apiTransactionKey, profileId);
            }

            return RedirectToAction("GetDiscount", new {amount = totalAmnt});
        }

    }
}
