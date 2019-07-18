using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Datalayer.Models;
using Microsoft.AspNetCore.Http;
using Datalayer.BusinessLogic;

namespace OnlineClass.Controllers
{
    public class StudentRegistrationController : Controller
    {
        private readonly OnlineclassContext _context;

        public StudentRegistrationController(OnlineclassContext context)
        {
            _context = context;
        }
        // GET: StudentRegistration
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetClassName()
        {
            var classname = (from m in _context.TblClass select m);
            if (classname.Count() > 0)
            {
                return Json(classname);
            }
            else
            {
                return Json("");
            }
        }

    [HttpGet]
        public IActionResult CheckUser(string user)
        {
            string msg = "";
            var username = _context.Tbllogin.Where(i => i.UserName == user);
            if(username!=null && username.Count()>0)
            {
                msg = "UserName Already Exist Please Select another username";
            }

            return Json(msg);
        }
        [HttpGet]
        public IActionResult GetSubjectBatch(int value)
        {
            List<Datalayer.BusinessLogic.GetBatchSubject> lb = new List<GetBatchSubject>();
            StudentRegistration sr = new StudentRegistration();
            lb = sr.getBatchSubject(_context,value);
            return Json(lb);
        }

        [HttpGet]
        public IActionResult GetAmountAndSubjects(Int64 BatchSubjectid,Int64 Classid,Int32 Durationid)
        {
            Datalayer.BusinessLogic.subjectAmount bb = new subjectAmount();
            StudentRegistration sr = new StudentRegistration();
            bb = sr.SubjectFees(BatchSubjectid, Classid,Durationid, _context);
            return Json(bb);
        }

        [HttpGet]
        public IActionResult GetTimmingsForBatchSubject(Int64 id)
        {
            List<Datalayer.BusinessLogic.SubjectBatchTimming> sbt = new List<SubjectBatchTimming>();
            StudentRegistration sr = new StudentRegistration();
            sbt = sr.SubjectBatcTimmings(id, _context);
            return Json(sbt);
        }



        [HttpGet]
        public IActionResult GetDuration()
        {
            var duration = from m in _context.TblDuration
                           select m;

            return Json(duration);
        }

        [HttpPost]
        //[Route("InsertUpdateStudentRegistration")]
        public JsonResult InsertUpdateStudentRegistration(int OP,string FirstName,string LastName,string ParentName,DateTime Dob,string Address,string City,
                                                           string State,string Pin,string Country,string Email,string Cell,string User,string Skype,
                                                           string Password,string ConfirmPass,Int64 BatchSubject,Int64 Class,
                                                           Int32 Duration,Int32 Period,decimal Amount)
        {
            int ans;
            StudentRegistration sr = new StudentRegistration();
            ans = sr.InsertUpdateStudent(OP, FirstName, LastName, ParentName, Dob, Address, City,
                                                            State, Pin, Country, Email, Cell, User, Skype,
                                                            Password, ConfirmPass, BatchSubject, Class,
                                                            Duration, Period, Amount, _context);
            string msg = "";
            if (ans == 0)
            {
                msg = "Operetion Failed";
            }
            else
            {
                msg = "Successfully inserted";
            }
            return Json(msg);
        }
        // GET: StudentRegistration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StudentRegistration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentRegistration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentRegistration/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentRegistration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentRegistration/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StudentRegistration/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}