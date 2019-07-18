using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Datalayer.BusinessLogic;
using Datalayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineClass.Controllers
{
    public class TeacherPaymentController : Controller
    {
        private readonly OnlineclassContext _context;
        private IHostingEnvironment _hostingEnv;
        Ipagination Ipagination;
        public TeacherPaymentController(OnlineclassContext context, IHostingEnvironment hostingEnv, Ipagination _Ipagination)
        {
            _context = context;
            _hostingEnv = hostingEnv;
            Ipagination = _Ipagination;
        }


        public IActionResult TeacherPayment(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {
            try
            {
                ViewBag.type = 1;
                ViewBag.mthnum = 0;
                var userlogin = (from m in _context.Tbllogin where m.UserName == HttpContext.Session.GetString("name") select m).FirstOrDefault();
                var teacher = (from m in _context.Tblteacher where m.TeacherId == userlogin.Userid select m).FirstOrDefault();
                if (teacher == null)
                {
                    var student = (from m in _context.TblStudent where m.StudentId == userlogin.Userid select m).FirstOrDefault();
                    ViewBag.user = student.Fname + " " + student.Lname;
                }
                else
                {
                    ViewBag.user = teacher.Fname + " " + teacher.Lname;
                }
                ViewBag.ClassName = new SelectList(_context.TblClass, "ClassId", "ClassName");
                ViewBag.DDlTeacher = new SelectList(_context.Tblteacher, "TeacherId", "Fname");
                // ViewBag.DDlTeacher = (from a in _context.Tblteacher select new { teacherid = a.TeacherId, name = a.Fname + " " + a.Lname }).ToList();
                ViewBag.DDlSubject = new SelectList(_context.Tblsubject, "Subjectid", "Subjectname");
                ViewBag.DDLBatch = new SelectList(_context.TblBatch, "BatchId", "BatchName");
                ViewBag.DDlType = new SelectList(_context.TblDuration, "Durationid", "Durationname");
                if (IsType == 1 && page != 0)
                {

                    var Teacherlist = Ipagination.GetTeacherlist(filter, page, IsType, sortExpression = "Fname").Result;
                    ViewBag.Teacher = Teacherlist;
                    filter = "";
                    var tTeacherPayment = Ipagination.GetTeacherPaymentlist(filter, 1, 2, sortExpression = "TeacherName").Result;
                    ViewBag.TeacherPay = tTeacherPayment;
                    ViewBag.Types = IsType;
                    return View();
                }
                else if (IsType == 2 && page != 0)
                {
                    var tTeacherPayment = Ipagination.GetTeacherPaymentlist(filter, page, 2, sortExpression = "TeacherName").Result;
                    ViewBag.TeacherPay = tTeacherPayment;
                    filter = "";
                    var Teacherlist = Ipagination.GetTeacherlist(filter, 1, IsType, sortExpression = "Fname").Result;
                    ViewBag.Teacher = Teacherlist;

                   
                    ViewBag.Types = IsType;
                    return View();
                }
                else
                {
                    if(IsType == 1)
                    {
                        
                        var Teacherlist = Ipagination.GetTeacherlist(filter, page, IsType, sortExpression = "Fname").Result;
                        ViewBag.Teacher = Teacherlist;
                        filter = "";
                        var tTeacherPayment = Ipagination.GetTeacherPaymentlist(filter, page, 2, sortExpression = "TeacherName").Result;
                        ViewBag.TeacherPay = tTeacherPayment;
                    }
                    else if (IsType == 3)
                    {
                        StudentRegistration sr = new StudentRegistration();
                        List<TeacherMain> lt = new List<TeacherMain>();
                        
                        lt = sr.GetAllTeacherDetailForPayment(TempData["month"].ToString(), TempData["year"].ToString(),0, _context).ToList();
                        TempData.Keep("month");
                        TempData.Keep("year");
                        /////
                        //ViewBag.month = month;
                        //ViewBag.year = year;
                        if (ViewBag.teacherMain == null) { 
                            ViewBag.teacherMain = lt; //TempData["teacherMain"];
                        }
                       ViewBag.mthnum = DateTime.ParseExact(TempData["month"].ToString(), "MMMM", CultureInfo.CurrentCulture).Month;

                        var Teacherlist = Ipagination.GetTeacherlist(filter, page, IsType, sortExpression = "Fname").Result;
                        ViewBag.Teacher = Teacherlist;
                        filter = "";
                        var tTeacherPayment = Ipagination.GetTeacherPaymentlist(filter, page, 2, sortExpression = "TeacherName").Result;
                        ViewBag.TeacherPay = tTeacherPayment;
                    }
                    else 
                    {
                        var tTeacherPayment = Ipagination.GetTeacherPaymentlist(filter, page, 2, sortExpression = "TeacherName").Result;
                        ViewBag.TeacherPay = tTeacherPayment;
                        filter = "";
                        var Teacherlist = Ipagination.GetTeacherlist(filter, page, IsType, sortExpression = "Fname").Result;
                        ViewBag.Teacher = Teacherlist;
                      
                       
                    }
                   
                    ViewBag.Types = IsType;
                    
                    return View();
                }

                //var teach = (from a in _context.Tblteacher
                //             select new
                //             {
                //                 TeacherId= a.TeacherId,
                //                 TeacherName=a.Fname + "" + a.Lname
                //             }).ToList();
                //// ViewBag.DDlTeacher = teach;
                //var teacheramountCalculate = (from a in _context.Tblmapteacherammount
                //                              join b in _context.Tblscheduleclassbatch on a.TeacherId equals b.Techermapid
                //                              join c in _context.Tblteacher on a.TeacherId equals c.TeacherId
                //                            join d in _context.TblstudentSubjectBatch on a.ClassBatchId equals d.BatchId
                //                            join e in _context.TblBatch on a.ClassBatchId equals e.BatchId
                //                              where b.IsCopletedClass == true && b.Isbatch == true
                //                              select new
                //                              {
                //                                  teachername=c.Fname+" "+c.Lname,
                //                                  amt=a.AmountTopay,
                //                                  batchname=e.BatchName
                //                              }).ToList();


             
              


            }
            catch (Exception ex)
            {
                return null;
            }
          
        }


        public JsonResult GetteacherPayment(Int64 Id)
        {
            try
            {
                var obj = (from a in _context.Tblmapteacherammount
                           where a.TecherammountId == Id
                           select new
                           {
                               teacherName = (from b in _context.Tblteacher
                                              where b.TeacherId == a.TeacherId
                                              select new
                                                 SelectListItem()
                                              { Text = (b.Fname + " "+ b.Lname), Value = b.TeacherId.ToString() }).ToList(),

                               className = (from b in _context.TblClass
                                            where b.ClassId == a.ClassBatchId
                                            select new
                                               SelectListItem()
                                            { Text = b.ClassName, Value = b.ClassId.ToString() }).ToList(),

                               PlanType = (from b in _context.TblDuration
                                           where b.Durationid == a.Typeid
                                           select new
                                              SelectListItem()
                                           { Text = b.Durationname, Value = b.Durationid.ToString() }).ToList(),
                               SubjectName = (from b in _context.Tblsubject
                                              where b.Subjectid == a.SubjectId
                                              select new
                                                 SelectListItem()
                                              { Text = b.Subjectname, Value = b.Subjectid.ToString() }).ToList(),
                               BatchName = (from b in _context.TblBatch
                                            where b.BatchId == a.ClassBatchId
                                            select new
                                               SelectListItem()
                                            { Text = b.BatchName, Value = b.BatchId.ToString() }).ToList(),
                               amount = a.AmountTopay,
                               isBatch=a.Isbatch



                           }).ToList();

                return Json(obj);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

        }

        public JsonResult GetSubject(Int64 id, string type)
        {


            if (type == "Batch")
            {
                var objBatch = (from a in _context.TblClassSubjectMap
                                join b in _context.Tblsubject on a.Subjectid equals b.Subjectid
                                where a.Batchid == id
                                select new
                                                SelectListItem()
                                { Text = b.Subjectname, Value = b.Subjectid.ToString() }).ToList();
                return Json(objBatch);

            }
            else if (type == "Class")
            {
                var objBatch = (from a in _context.TblClassSubjectMap
                                join b in _context.Tblsubject on a.Subjectid equals b.Subjectid
                                where a.Classid == id
                                select new
                                            SelectListItem()
                                { Text = b.Subjectname, Value = b.Subjectid.ToString() }).ToList();
                return Json(objBatch);

            }
            else
            {
                return Json("Something went wrong!");
            }


        }


        [HttpPost]
        public IActionResult SaveTeacherPayment(TeacherPaymentModel teacherPaymentModel )
        {
            try
            {
                Tblmapteacherammount tblmapteacherammount = new Tblmapteacherammount();
               

                var b = (from a in _context.Tblmapteacherammount where a.TecherammountId == teacherPaymentModel.TecherammountId select a).FirstOrDefault();

                if (b != null)
                {
                    b.SubjectId = teacherPaymentModel.Subjectid;
                    if(teacherPaymentModel.Isbatch==true)
                    {
                        b.Isbatch = true;
                        b.ClassBatchId = teacherPaymentModel.BatchId;
                    }
                    if (teacherPaymentModel.Isbatch == false)
                    {
                        b.Isbatch = false;
                        b.ClassBatchId = teacherPaymentModel.ClassId;
                    }
                    b.TeacherId = teacherPaymentModel.TeacherId;
                   
                    b.Typeid = teacherPaymentModel.DurationTypeid;
                    b.AmountTopay = teacherPaymentModel.AmountTopay;
                    _context.SaveChanges();
                }
                else
                {
                    tblmapteacherammount.SubjectId = teacherPaymentModel.Subjectid;
                    if (teacherPaymentModel.Isbatch == true)
                    {
                        tblmapteacherammount.Isbatch = true;
                        tblmapteacherammount.ClassBatchId = teacherPaymentModel.BatchId;
                    }
                    if (teacherPaymentModel.Isbatch == false)
                    {
                        tblmapteacherammount.Isbatch = false;
                        tblmapteacherammount.ClassBatchId = teacherPaymentModel.ClassId;
                    }
                  
                    tblmapteacherammount.TeacherId = teacherPaymentModel.TeacherId;

                    tblmapteacherammount.Typeid = teacherPaymentModel.DurationTypeid;
                    tblmapteacherammount.AmountTopay = teacherPaymentModel.AmountTopay;
                    _context.Tblmapteacherammount.Add(tblmapteacherammount);
                    _context.SaveChanges();

                }
              
            }
            catch(Exception ex)
            {

            }
            return RedirectToAction("TeacherPayment", "TeacherPayment", new { });
        }


        [HttpPost]
        public JsonResult DeleteConfirmed(long Id, string Tabletype)
        {
            try
            {
                if (Tabletype == "TeacherPayment")
                {
                    Tblmapteacherammount tblTeacherAmt = _context.Tblmapteacherammount.Find(Id);
                    
                    _context.Tblmapteacherammount.Remove(tblTeacherAmt);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return Json("Delete successfuly");
        }

        [HttpGet]
        public IActionResult GetAllTeachersPayment(string month,string year,int IsType)
        {
            ViewBag.Types = 3;
            StudentRegistration sr = new StudentRegistration();
            List<TeacherMain> lt = new List<TeacherMain>();
            string userid = HttpContext.Session.GetString("userid");
            lt = sr.GetAllTeacherDetailForPayment(month, year,0,_context).ToList();
            TempData["month"] = month;
            TempData["year"] = year;
            TempData["mthNum"] = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month;
            /////
            ViewBag.teacherMain = lt;
            //TempData["teacherMain"] = lt;
            //TempData.Keep();
            //////
            return RedirectToAction("TeacherPayment", "TeacherPayment", new { IsType =3});
        }
    }
}