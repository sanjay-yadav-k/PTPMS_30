using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Datalayer.Models;
using Microsoft.AspNetCore.Http;

namespace OnlineClass.Controllers
{
    public class LoginsController : Controller
    {
        private readonly OnlineclassContext _context;

        public LoginsController(OnlineclassContext context)
        {
            _context = context;
        }


        public  IActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Tbllogin login)
        {

            var loginuser = _context.Tbllogin.Where(e => e.UserName == login.UserName && e.Password == login.Password);
            if (loginuser != null && loginuser.Count() > 0)
            {
                TempData["Username"] = loginuser.FirstOrDefault().UserName;
                string unmae = Convert.ToString(TempData["Username"]);
                
                if (loginuser.FirstOrDefault().Typeid == 2)
                {
                    HttpContext.Session.SetString("name", unmae);
                    return RedirectToAction("Index", "Student", new { Id = loginuser.FirstOrDefault().Userid });

                   
                }
                   
                else if(loginuser.FirstOrDefault().Typeid == 1)
                {
                    
                    HttpContext.Session.SetString("name", unmae);
                    return RedirectToAction("Index", "Home", new { });
                }
                else
                {
                    HttpContext.Session.SetString("name", unmae);
                    HttpContext.Session.SetString("userid", loginuser.FirstOrDefault().Userid.ToString());
                    return RedirectToAction("Dashboard", "Teacher", new {Id= loginuser.FirstOrDefault().Userid });
                }
               
            }
            else
            {
                ViewBag.msg="Invalid Username & Password";
                return View();
            }
        }

        public ActionResult Logout()
        {
            //Session.Abandon();
            //Session.Clear();
            return RedirectToAction("Login");
        }
     

    }
}
