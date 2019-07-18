using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datalayer.BusinessLogic;
using Datalayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineClass.Controllers
{
    public class PayUmoneyresponseController : Controller
    {
        private readonly OnlineclassContext _context;
        public PayUmoneyresponseController(OnlineclassContext context, GetPayment _GetPayment)
        {
            _context = context;
            
        }
        // GET: PayUmoneyresponse
        public ActionResult Index()
        {
            String key = Request.Form["key"];
            String salt = Request.Form["salt"];
            String txnid = Request.Form["txnid"];
            String amount = Request.Form["amount"];
            String firstname = Request.Form["firstname"]; 
            String email = Request.Form["email"]; 
            String mihpayid = Request.Form["mihpayid"];
            String status = Request.Form["status"];
            String udf5 = Request.Form["udf5"]; 
            String hash = Request.Form["hash"];

            StudentRegistration sr = new StudentRegistration();
            int ans = 0;
            ans = sr.ResponseFromPayUMoneyForSubject(status, _context, txnid);

            return View();
        }
    }
}