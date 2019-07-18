using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineClass.Models;
using Microsoft.AspNetCore.Http;
using ReflectionIT.Mvc.Paging;
using Datalayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Datalayer.BusinessLogic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace OnlineClass.Controllers
{
    public class HomeController : Controller
    {
        private readonly OnlineclassContext _context;
        private IHostingEnvironment _hostingEnv;
        Ipagination Ipagination;
        ITotalCount ITotalCount;
        public HomeController(OnlineclassContext context, IHostingEnvironment hostingEnv, Ipagination _Ipagination, ITotalCount _ITotalCount)
        {
            _context = context;
            _hostingEnv = hostingEnv;
            Ipagination = _Ipagination;
            ITotalCount = _ITotalCount;
        }
        //public IActionResult Index()
        //{
        //    ViewBag.user = HttpContext.Session.GetString("name"); ;
        //    return View();
        //}
        public async Task<IActionResult> Index(string filter, int page = 0, bool Isbatch=true,string sortExpression = "BatchName")
        {
            try
            {
                var tclass = ITotalCount.TotalClassCount().ToList();
                ViewBag.ClassList = tclass;
                ViewBag.TotalClass = tclass.Count();

                var tstudent = ITotalCount.TotalStudentCount().ToList();
                ViewBag.StuList = tstudent;
                ViewBag.StuCount = tstudent.Count();
                var batch = ITotalCount.TotalBatch().ToList();
                ViewBag.BatchTotalList = batch;

                ViewBag.BatchTotalCount = batch.Count();
                var tNstudent = ITotalCount.TotalNewStudentCount().ToList();
                ViewBag.NewStuList = tNstudent;
                ViewBag.NewStuCount = tNstudent.Count();
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
                //ViewBag.user = HttpContext.Session.GetString("name");
                ViewBag.type = 1;
                var qry1 = (from n in _context.TblStudent
                            join m in _context.TblstudentSubjectBatch on n.StudentId
                            equals m.StudentId
                            join t in _context.Tblsubject on m.SubjectId equals t.Subjectid
                            join c in _context.TblClass on n.ClassId equals c.ClassId
                            select new tblstudentbatchmap
                            {
                                Fname = n.Fname + " " + n.Lname,
                                Id = m.Id,
                                Subject = t.Subjectname,
                                ClassName = c.ClassName,

                            });
                if (Isbatch == true && page !=0)
                {
                   
                   
                    var student = qry1.AsNoTracking().AsQueryable();
                    var model1 = await PagingList<tblstudentbatchmap>.CreateAsync(qry1, 5, 1, "Fname", "Fname");

                    ViewBag.studentmap = model1;
                    var qry = _context.TblBatch.AsNoTracking().AsQueryable();

                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        qry = qry.Where(p => p.BatchName.Contains(filter));
                    }
                    if (page == 0)
                    {
                        page = 1;

                    }
                    var model = await PagingList<TblBatch>.CreateAsync(qry, 5, page, sortExpression, "BatchName");

                    model.RouteValue = new RouteValueDictionary {{ "filter", filter}};
                    ViewBag.user = HttpContext.Session.GetString("name");
                    return View(model);
                }
                else if(Isbatch == false && page != 0)
                {
                    var qry = _context.TblBatch.AsNoTracking().AsQueryable();

                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        qry = qry.Where(p => p.BatchName.Contains(filter));
                    }
                   
                    var model = await PagingList<TblBatch>.CreateAsync(qry, 5, 1, sortExpression, "BatchName");

                    model.RouteValue = new RouteValueDictionary { { "filter", filter } };

                    if (page == 0)
                    {
                        page = 1;

                    }
                    var student = qry1.AsNoTracking().AsQueryable();
                    var model1 = await PagingList<tblstudentbatchmap>.CreateAsync(qry1, 5, page, "Fname", "Fname");

                    ViewBag.studentmap = model1;
                    ViewBag.user = HttpContext.Session.GetString("name");
                    return View(model);
                }
                else
                {
                   
                    if (page == 0)
                    {
                        page = 1;

                    }
                    var student= qry1.AsNoTracking().AsQueryable();
                    var model1= await PagingList<tblstudentbatchmap>.CreateAsync(qry1, 5, page, "Fname", "Fname");

                      ViewBag.studentmap = model1;
                      var qry = _context.TblBatch.AsNoTracking().AsQueryable();

                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        qry = qry.Where(p => p.BatchName.Contains(filter));
                    }
                    if (page == 0)
                    {
                        page = 1;

                    }
                    var model = await PagingList<TblBatch>.CreateAsync(qry, 5, page, sortExpression, "BatchName");

                    model.RouteValue = new RouteValueDictionary {{ "filter", filter}};
                  
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View();
            }
           
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpPost]
        //public JsonResult batchdata()
        //{
            
        //    var dt = (from a in _context.TblBatch select a).ToList();

        //    return Json(dt);
        //}
        public async Task<IActionResult> Configuration(string DeleteMsg =null,string filter="", int page = 0, int IsType=0, string sortExpression = "BatchName")
        {
            if(DeleteMsg!=null)
            {
                ViewBag.DeleteMsg = DeleteMsg;
            }
            ViewBag.type = 1;
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
            ViewBag.DDlSubject = new SelectList(_context.Tblsubject, "Subjectid", "Subjectname");
            ViewBag.DDLBatch = new SelectList(_context.TblBatch, "BatchId", "BatchName");
            ViewBag.BatchClassName = new SelectList(_context.TblClass, "ClassId", "ClassName");
            ViewBag.Subscription = new SelectList(_context.TblDuration, "Durationid", "Durationname");
            ViewBag.subclass = new SelectList(_context.TblClass, "ClassId", "ClassName");
            if (IsType == 1 && page != 0)
            {

                var classlist = Ipagination.Getclass(filter,page,IsType,sortExpression = "Classname").Result;
                ViewBag.ClassName = classlist;

                var Subjectlist = Ipagination.GetSubjectPagination(filter, 1, 2, sortExpression = "Subjectname").Result;
                ViewBag.Subject = Subjectlist;
                var Class_Subject = Ipagination.GetSubject_ClassPagination(filter, 1, 3, sortExpression = "Classname").Result;
                ViewBag.Class_Subject = Class_Subject;
                var Btch = Ipagination.GetBatchPagination(filter, 1, 4, sortExpression = "BatchName").Result;
                ViewBag.Batch = Btch;
                var Batch_Subject = Ipagination.GetBatch_SubjectPagination(filter, 1, 5, sortExpression = "BatchName").Result;
                ViewBag.Batch_Subject = Batch_Subject;
                var Fee_Subject = Ipagination.Getsubjectfee(filter, 1, 6, sortExpression = "FeeSubject").Result;
                ViewBag.subjectdees = Fee_Subject;
                var Fee_Batch = Ipagination.GetBatchfee(filter, 1, 7, sortExpression = "FeeBatch").Result;
                ViewBag.Batchfees = Fee_Batch;
                ViewBag.Types = IsType;
                return View();
            }
            else if (IsType == 2 && page != 0)
            {

                var Subjectlist = Ipagination.GetSubjectPagination(filter, page, IsType, sortExpression = "Classname").Result;
                ViewBag.Subject = Subjectlist;

                var classlist = Ipagination.Getclass(filter, 1, 1, sortExpression = "Classname").Result;
                ViewBag.ClassName = classlist;

                var Class_Subject = Ipagination.GetSubject_ClassPagination(filter, 1, 3, sortExpression = "Classname").Result;
                ViewBag.Class_Subject = Class_Subject;

                var Btch = Ipagination.GetBatchPagination(filter, 1, 4, sortExpression = "BatchName").Result;
                ViewBag.Batch = Btch;

                var Batch_Subject = Ipagination.GetBatch_SubjectPagination(filter, 1, 5, sortExpression = "BatchName").Result;
                ViewBag.Batch_Subject = Batch_Subject;

                var Fee_Subject = Ipagination.Getsubjectfee(filter, 1, 6, sortExpression = "FeeSubject").Result;
                ViewBag.subjectdees = Fee_Subject;

                var Fee_Batch = Ipagination.GetBatchfee(filter, 1, 7, sortExpression = "FeeBatch").Result;
                ViewBag.Batchfees = Fee_Batch;
                ViewBag.Types = IsType;
                return View();
            }
            else if (IsType == 3 && page != 0)
            {

                var Class_Subject = Ipagination.GetSubject_ClassPagination(filter, page, IsType, sortExpression = "Classname").Result;
                ViewBag.Class_Subject = Class_Subject;

                var classlist = Ipagination.Getclass(filter, 1, 1, sortExpression = "Classname").Result;
                ViewBag.ClassName = classlist;
                var Subjectlist = Ipagination.GetSubjectPagination(filter, 1, 2, sortExpression = "Subjectname").Result;
                ViewBag.Subject = Subjectlist;
                var Btch = Ipagination.GetBatchPagination(filter, 1, 4, sortExpression = "BatchName").Result;
                ViewBag.Batch = Btch;
                var Batch_Subject = Ipagination.GetBatch_SubjectPagination(filter, 1, 5, sortExpression = "BatchName").Result;
                ViewBag.Batch_Subject = Batch_Subject;
                var Fee_Subject = Ipagination.Getsubjectfee(filter, 1, 6, sortExpression = "FeeSubject").Result;
                ViewBag.subjectdees = Fee_Subject;
                var Fee_Batch = Ipagination.GetBatchfee(filter, 1, 7, sortExpression = "FeeBatch").Result;
                ViewBag.Batchfees = Fee_Batch;
                ViewBag.Types = IsType;
                return View();
            }
            else if (IsType ==4 && page != 0)
            {

                var Btch= Ipagination.GetBatchPagination(filter, page, IsType, sortExpression = "BatchName").Result;
                ViewBag.Batch = Btch;

                var classlist = Ipagination.Getclass(filter, 1, 1, sortExpression = "Classname").Result;
                ViewBag.ClassName = classlist;
                var Subjectlist = Ipagination.GetSubjectPagination(filter, 1, 2, sortExpression = "Subjectname").Result;
                ViewBag.Subject = Subjectlist;
                var Class_Subject = Ipagination.GetSubject_ClassPagination(filter, 1, 3, sortExpression = "Classname").Result;
                ViewBag.Class_Subject = Class_Subject;
                var Batch_Subject = Ipagination.GetBatch_SubjectPagination(filter, 1, 5, sortExpression = "BatchName").Result;
                ViewBag.Batch_Subject = Batch_Subject;
                var Fee_Subject = Ipagination.Getsubjectfee(filter, 1, 6, sortExpression = "FeeSubject").Result;
                ViewBag.subjectdees = Fee_Subject;
                var Fee_Batch = Ipagination.GetBatchfee(filter, 1, 7, sortExpression = "FeeBatch").Result;
                ViewBag.Batchfees = Fee_Batch;
                ViewBag.Types = IsType;
                return View();
            }
            else if (IsType == 5 && page != 0)
            {

                var Batch_Subject = Ipagination.GetBatch_SubjectPagination(filter, page, IsType, sortExpression = "BatchName").Result;
                ViewBag.Batch_Subject = Batch_Subject;
                var classlist = Ipagination.Getclass(filter, 1, 1, sortExpression = "Classname").Result;
                ViewBag.ClassName = classlist;

                var Subjectlist = Ipagination.GetSubjectPagination(filter, 1, 2, sortExpression = "Subjectname").Result;
                ViewBag.Subject = Subjectlist;
                var Class_Subject = Ipagination.GetSubject_ClassPagination(filter, 1, 3, sortExpression = "Classname").Result;
                ViewBag.Class_Subject = Class_Subject;
                var Btch = Ipagination.GetBatchPagination(filter, 1, 4, sortExpression = "BatchName").Result;
                ViewBag.Batch = Btch;
                var Fee_Subject = Ipagination.Getsubjectfee(filter, 1, 6, sortExpression = "FeeSubject").Result;
                ViewBag.subjectdees = Fee_Subject;
                var Fee_Batch = Ipagination.GetBatchfee(filter, 1, 7, sortExpression = "FeeBatch").Result;
                ViewBag.Batchfees = Fee_Batch;
                ViewBag.Types = IsType;
                return View();
            }
            else if (IsType == 6 && page != 0)
            {

                var Fee_Subject = Ipagination.Getsubjectfee(filter, page, IsType, sortExpression = "FeeSubject").Result;
                ViewBag.subjectdees = Fee_Subject;
                var classlist = Ipagination.Getclass(filter, 1, 1, sortExpression = "Classname").Result;
                ViewBag.ClassName = classlist;

                var Subjectlist = Ipagination.GetSubjectPagination(filter, 1, 2, sortExpression = "Subjectname").Result;
                ViewBag.Subject = Subjectlist;
                var Class_Subject = Ipagination.GetSubject_ClassPagination(filter, 1, 3, sortExpression = "Classname").Result;
                ViewBag.Class_Subject = Class_Subject;
                var Btch = Ipagination.GetBatchPagination(filter, 1, 4, sortExpression = "BatchName").Result;
                ViewBag.Batch = Btch;
                var Batch_Subject = Ipagination.GetBatch_SubjectPagination(filter, 1, 5, sortExpression = "BatchName").Result;
                ViewBag.Batch_Subject = Batch_Subject;
                var Fee_Batch = Ipagination.GetBatchfee(filter, 1, 7, sortExpression = "FeeBatch").Result;
                ViewBag.Batchfees = Fee_Batch;
                ViewBag.Types = IsType;
                return View();
            }
            else if (IsType == 7 && page != 0)
            {

                var Fee_Batch = Ipagination.GetBatchfee(filter, page, IsType, sortExpression = "FeeBatch").Result;
                ViewBag.Batchfees = Fee_Batch;
                var classlist = Ipagination.Getclass(filter, 1, 1, sortExpression = "Classname").Result;
                ViewBag.ClassName = classlist;

                var Subjectlist = Ipagination.GetSubjectPagination(filter, 1, 2, sortExpression = "Subjectname").Result;
                ViewBag.Subject = Subjectlist;
                var Class_Subject = Ipagination.GetSubject_ClassPagination(filter, 1, 3, sortExpression = "Classname").Result;
                ViewBag.Class_Subject = Class_Subject;
                var Btch = Ipagination.GetBatchPagination(filter, 1, 4, sortExpression = "BatchName").Result;
                ViewBag.Batch = Btch;
                var Batch_Subject = Ipagination.GetBatch_SubjectPagination(filter, 1, 5, sortExpression = "BatchName").Result;
                ViewBag.Batch_Subject = Batch_Subject;
                var Fee_Subject = Ipagination.Getsubjectfee(filter, 1, 6, sortExpression = "FeeSubject").Result;
                ViewBag.subjectdees = Fee_Subject;
                ViewBag.Types = IsType;
                return View();
            }
            else
            {

                var classlist = Ipagination.Getclass(filter, page, IsType, sortExpression = "Subjectname").Result;
                ViewBag.ClassName = classlist;
                var Subjectlist = Ipagination.GetSubjectPagination(filter, page, IsType, sortExpression = "Subjectname").Result;
                ViewBag.Subject = Subjectlist;
                var Class_Subject = Ipagination.GetSubject_ClassPagination(filter, page, IsType, sortExpression = "Classname").Result;
                ViewBag.Class_Subject = Class_Subject;
                var Btch = Ipagination.GetBatchPagination(filter, page, IsType, sortExpression = "BatchName").Result;
                ViewBag.Batch = Btch;
                var Batch_Subject = Ipagination.GetBatch_SubjectPagination(filter, page, IsType, sortExpression = "BatchName").Result;
                ViewBag.Batch_Subject = Batch_Subject;
                var Fee_Subject = Ipagination.Getsubjectfee(filter, page, IsType, sortExpression = "FeeSubject").Result;
                ViewBag.subjectdees = Fee_Subject;
                var Fee_Batch = Ipagination.GetBatchfee(filter, page, IsType, sortExpression = "FeeBatch").Result;
                ViewBag.Batchfees = Fee_Batch;
                ViewBag.Types = 0;
                return View();
            }
            //else if (IsType == 2 && page != 0)
            //{
            //    if (!string.IsNullOrWhiteSpace(filter))
            //    {
            //        Batchlist = Batchlist.Where(p => p.BatchName.Contains(filter));
            //    }
            //    var model1 = PagingList<TblBatch>.CreateAsync(Batchlist, 5, 1, "BatchName", "BatchName");

            //    ViewBag.Batch = model1;

            //    return View();
            //}
            //else if (IsType == 3 && page != 0)
            //{

            //}

            // ViewBag.Subject = (from a in _context.Tblsubject select a).ToList();
          
            //var subjectfee = (from m in _context.Tblfeessetting
            //                  join n in _context.Tblsubject on m.SubjecBatchtId equals n.Subjectid
            //                  join c in _context.TblClass on m.ClassId equals c.ClassId
            //                  join d in _context.TblDuration on m.DurationTypeId equals d.Durationid
            //                  select new fee_SubjectBatchModel
            //                  {
            //                      ClassName = c.ClassName,
            //                      subjectname = n.Subjectname,
            //                      feeid = m.Feeid,
            //                      feenew =Convert.ToDecimal( m.Ammount),
            //                      DurationType = d.Durationname
            //                  }
            //                ).ToList();
            //var Batchfee = (from m in _context.Tblfeessetting
            //                  join n in _context.TblBatch on m.SubjecBatchtId equals n.BatchId
            //                  join c in _context.TblClass on m.ClassId equals c.ClassId
            //                  join d in _context.TblDuration on m.DurationTypeId equals d.Durationid
            //                  select new fee_SubjectBatchModel
            //                  {
            //                      ClassName = c.ClassName,
            //                      subjectname = n.BatchName,
            //                      feeid = m.Feeid,
            //                      feenew = Convert.ToDecimal(m.Ammount),
            //                      DurationType = d.Durationname
            //                  }
            //               ).ToList();
            //List<fee_SubjectBatchModel> fee_SubjectBatchModel = subjectfee.ToList();
            //List<fee_SubjectBatchModel> fee_BatchModel = Batchfee.ToList();
            //ViewBag.subjectdees = fee_SubjectBatchModel;
            //ViewBag.Batchfees = fee_BatchModel;
            //var subjectclass = (from m in _context.TblClassSubjectMap
            //                    join n in _context.TblClass on m.Classid equals n.ClassId
            //                    select new Class_SubjectModel
            //                    {
            //                        ClassId = m.Classid,
            //                        ClassName = n.ClassName
            //                    }).Distinct();
            //List<Class_SubjectModel> Class_SubjectModel = subjectclass.ToList();
            ////ViewBag.Class_Subject = Class_SubjectModel;
            //var batchclass = (from m in _context.TblClassSubjectMap
            //                    join n in _context.TblBatch on m.Batchid equals n.BatchId
            //                    select new Batch_SubjectModel
            //                    {
            //                        BatchId = m.Batchid,
            //                        BatchName = n.BatchName
            //                    }).Distinct();
            //List<Batch_SubjectModel> batch_SubjectModels = batchclass.ToList();

          //  ViewBag.Batch_Subject = batch_SubjectModels;

            //if (objclass!=null && objclass.Count()>0)
            //{
            //    ViewData["ClassName"] = objclass;
            //}
            //if(objBatch!=null && objBatch.Count()>0)
            //{

            //    ViewData["Batch"] = objBatch;
            //}
            
        }



        public IActionResult TeacherConfiguration(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {
            ViewBag.type = 1;
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
            ViewBag.Teacher = (from a in _context.Tblteacher select a).ToList();
            ViewBag.Batchlist = (from a in _context.TblBatch select a).ToList();
            ViewBag.subjectlist = (from a in _context.Tblsubject select a).ToList();
            ViewBag.Classlist = (from a in _context.TblClass select a).ToList();

            ViewBag.Teachers = (from a in _context.Tblteacher select new {teacherid= a.TeacherId,name=a.Fname +" "+a.Lname }).ToList();

            ViewBag.studentlist = (from m in _context.TblStudent
                                   join
                                    n in _context.TblstudentSubjectBatch on m.StudentId equals n.StudentId
                                   join s in _context.Tblsubject on n.SubjectId equals s.Subjectid
                                   //join cs in _context.TblClassSubjectMap on n.SubjectId equals cs.Subjectid
                                   join c in _context.TblClass on m.ClassId equals c.ClassId
                                   select new TeacherModel
                                   {
                                       studentname = m.Fname + " " + m.Lname,
                                       studentid = m.StudentId,
                                       subjectid = s.Subjectid,
                                       subjectname = s.Subjectname,
                                       Classid = c.ClassId,
                                       Classname = c.ClassName,
                                       Ispresent = ((from z in _context.Tblstudentteacher where z.Studentid == m.StudentId && z.Subjectid == s.Subjectid select z.Studentid).Count()) > 0 ? true : false
                                   }).ToList();

            if (IsType == 1 && page != 0)
            {

                var teacherstudentmap = Ipagination.GetStudentmap(filter, page, IsType, sortExpression = "Classname").Result;
                ViewBag.teacherstudentmap = teacherstudentmap;
                filter = "";
                var teacherbatch = Ipagination.Getteacherbatchmap(filter, 1, 2, sortExpression = "Subjectname").Result;
                ViewBag.teacherbatch = teacherbatch;
                //var GetStudentlists = Ipagination.GetStudentlist(filter, page, IsType, sortExpression = "studentname").Result;
                //ViewBag.studentlist = GetStudentlists;
                ViewBag.Types = IsType;
                return View();
            }
            else if (IsType == 2 && page != 0)
            {
                var teacherbatch = Ipagination.Getteacherbatchmap(filter, 1, 2, sortExpression = "Subjectname").Result;
                ViewBag.teacherbatch = teacherbatch;
                filter = "";
                var teacherstudentmap = Ipagination.GetStudentmap(filter, page, IsType, sortExpression = "Classname").Result;
                ViewBag.teacherstudentmap = teacherstudentmap;

              
                //var GetStudentlists = Ipagination.GetStudentlist(filter, page, IsType, sortExpression = "studentname").Result;
                //ViewBag.studentlist = GetStudentlists;
                ViewBag.Types = IsType;
                return View();
            }
            else
            {
                if(IsType == 1)
                {
                    var teacherstudentmap = Ipagination.GetStudentmap(filter, page, 1, sortExpression = "Classname").Result;
                    ViewBag.teacherstudentmap = teacherstudentmap;
                    filter = "";
                    var teacherbatch = Ipagination.Getteacherbatchmap(filter, page, 2, sortExpression = "Subjectname").Result;
                    ViewBag.teacherbatch = teacherbatch;
                }
                else
                {
                    var teacherbatch = Ipagination.Getteacherbatchmap(filter, page, 2, sortExpression = "Subjectname").Result;
                    ViewBag.teacherbatch = teacherbatch;
                    filter = "";
                    var teacherstudentmap = Ipagination.GetStudentmap(filter, page, 1, sortExpression = "Classname").Result;
                    ViewBag.teacherstudentmap = teacherstudentmap;

                   
                }
              
                //var GetStudentlists = Ipagination.GetStudentlist(filter, page, 3, sortExpression = "studentname").Result;
                //ViewBag.studentlist = GetStudentlists;
                ViewBag.Types = IsType;
                return View();
            }
            //ViewBag.teacherstudentmap= (from n in _context.TblStudent
            //                                       join m in _context.Tblstudentteacher on n.StudentId
            //                                       equals m.Studentid
            //                                       join s in _context.TblstudentSubjectBatch on n.StudentId equals s.StudentId
            //                                       join t in _context.Tblsubject on s.SubjectId equals t.Subjectid
            //                                       join c in _context.TblClass on n.ClassId equals c.ClassId
            //                                       join tt in _context.Tblteacher on m.Teacherid equals tt.TeacherId
            //                                       where m.Subjectid == s.SubjectId

            //                                       select new tblTeacherClassList
            //                                       {
            //                                           studentteacheridmap = m.Teacherstudentmapid,
            //                                           TeacherId = m.Teacherid,
            //                                           Subject = t.Subjectname,
            //                                           ClassName = c.ClassName,
            //                                           StudentName = n.Fname + " " + n.Lname,
            //                                           teachername = tt.Fname + " " + tt.Lname,
            //                                       }).ToList();

            //ViewBag.teacherbatch= (from n in _context.Tblteacher
            //                       join m in _context.Tblteacherclassbatchmap on n.TeacherId equals m.TeacherId
            //                       join c in _context.TblClass on m.Classid equals c.ClassId
            //                       join b in _context.Tblsubject on m.Subjectid equals b.Subjectid
            //                       join bb in _context.TblBatch on m.Batchid equals bb.BatchId
            //                       where m.Subjectid == b.Subjectid

            //                       select new tblTeacherBatchList
            //                       {
            //                           TeacherId = n.TeacherId,
            //                           BatchName = bb.BatchName,
            //                           ClassName = c.ClassName,
            //                           Date = bb.Dateofcommencement,
            //                           Subject=b.Subjectname,
            //                           Teacher=n.Fname +" "+n.Lname,
            //                           Techermapid=m.Techermapid
            //                       }).ToList();
       
            //ViewBag.studentlist = (from m in _context.TblStudent
            //                       join
            //                        n in _context.TblstudentSubjectBatch on m.StudentId equals n.StudentId
            //                       join s in _context.Tblsubject on n.SubjectId equals s.Subjectid
            //                       //join cs in _context.TblClassSubjectMap on n.SubjectId equals cs.Subjectid
            //                       join c in _context.TblClass on m.ClassId equals c.ClassId
            //                       select new TeacherModel
            //                       {
            //                           studentname = m.Fname + " " + m.Lname,
            //                           studentid = m.StudentId,
            //                           subjectid = s.Subjectid,
            //                           subjectname = s.Subjectname,
            //                           Classid = c.ClassId,
            //                           Classname = c.ClassName,
            //                           Ispresent =((from z in _context.Tblstudentteacher where z.Studentid ==m.StudentId && z.Subjectid ==s.Subjectid select z.Studentid).Count())>0?true:false
            //                       }).ToList();
           // return View();
        }

        [HttpPost]
        public IActionResult ClassDetail(ClassBatchSubjectTeacherModel classmodel)
        {

            TblClass tblClass = new TblClass();
            if (classmodel.ClassId != null)
            {
                var objclass = (from a in _context.TblClass where a.ClassId == classmodel.ClassId select a).FirstOrDefault();
                objclass.ClassName = classmodel.ClassName;
                _context.SaveChanges();

            }
            else
            {
                var objclass = (from a in _context.TblClass where a.ClassName == classmodel.ClassName select a).FirstOrDefault();
                if (objclass == null)
                {
                    tblClass.ClassName = classmodel.ClassName;
                    _context.TblClass.Add(tblClass);
                    _context.SaveChanges();
                }
            }
            

            return RedirectToAction("Configuration", "Home", new { });
        }


        [HttpPost]
        public IActionResult BatchDetail(ClassBatchSubjectTeacherModel batch)
        {

            TblBatch tblBatch = new TblBatch();
            var b = (from a in _context.TblBatch where a.BatchId == batch.BatchId select a).FirstOrDefault();
            long classId = (from m in _context.TblClass where m.ClassName == batch.ClassName select m.ClassId).FirstOrDefault();
            long durationId = (from m in _context.TblDuration where m.Durationname == batch.DurationType select m.Durationid).FirstOrDefault();
            if (b != null)
            {
                b.BatchName = batch.BatchName;
                //b.ClassId = classId;
                //b.CourseDuration = batch.CourseDuration;
                //b.Dateofcommencement = batch.Dateofcommencement;
                //b.Fees = batch.Fees;
                //b.DurationTypeId = durationId;
                //b.Timing = batch.Timing;
                //b.CourseDuration = batch.CourseDuration;
                _context.SaveChanges();
            }
            else
            {
                tblBatch.BatchName = batch.BatchName;
                //tblBatch.ClassId = classId;
                //tblBatch.CourseDuration = batch.CourseDuration;
                //tblBatch.Dateofcommencement = batch.Dateofcommencement;
                //tblBatch.Fees = batch.Fees;
                //tblBatch.DurationTypeId = durationId;
                //tblBatch.Timing = batch.Timing;
                //tblBatch.CourseDuration = batch.CourseDuration;
                _context.TblBatch.Add(tblBatch);
                _context.SaveChanges();

            }
            return RedirectToAction("Configuration", "Home", new { });
        }

        [HttpPost]
        public IActionResult SubjectDetail(ClassBatchSubjectTeacherModel subject)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                var keysva = HttpContext.Request.Form.Keys;
                var vUplodedFile = Path.GetTempFileName();
                string fName = "";
                string pathss = "";
                string leadid = "";
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var inputStream = new FileStream(vUplodedFile, FileMode.Create))
                        {
                            formFile.CopyTo(inputStream);
                            byte[] array = new byte[inputStream.Length];
                            inputStream.Seek(0, SeekOrigin.Begin);
                            inputStream.Read(array, 0, array.Length);
                            fName = Request.Form.Files[0].FileName;

                        }
                    }
                }
                if (files[0].Length > 0)
                {
                    var fileName = Path.GetFileName(files[0].FileName);
                    var targetDirectory = Path.Combine(_hostingEnv.WebRootPath, string.Format("images\\"));
                    var savePath = Path.Combine(targetDirectory, fileName);
                    using (FileStream fs = System.IO.File.Create(savePath))
                    {
                        files[0].CopyTo(fs);
                        fs.Flush();
                    }

                    fName = fileName;

                }

               
                long classId = (from m in _context.TblClass where m.ClassName == subject.ClassName select m.ClassId).FirstOrDefault();
                if (subject.SubjectId != null)
                {
                    var objsub = (from a in _context.Tblsubject where a.Subjectid == subject.SubjectId select a).FirstOrDefault();
                    objsub.Subjectname = subject.SubjectName;
                    objsub.Imagename = fName;
                    objsub.Isshow = subject.IsShow;
                    _context.SaveChanges();

                }
                else
                {
                    
                    var objsub = (from a in _context.Tblsubject where a.Subjectid == subject.SubjectId select a).FirstOrDefault();
                    if (objsub == null)
                    {
                        Tblsubject tblsubject = new Tblsubject();
                        tblsubject.Subjectname = subject.SubjectName;
                        tblsubject.Imagename = fName;
                        tblsubject.Isshow = subject.IsShow;
                        _context.Tblsubject.Add(tblsubject);
                        _context.SaveChanges();

                    }
                 
                }

                return RedirectToAction("Configuration", "Home", new { });

            }
            catch(Exception ex)
            {
                return View();
            }
           
        }

        public JsonResult GetTeacher(Int64 TeacherId)
        {
            var Related_query = (from a in _context.Tblteacher
                                 where a.TeacherId == TeacherId
                                 select new
                                 {
                                     firstname = a.Fname,
                                     lastname = a.Lname,
                                     email = a.Email,
                                     mobile = a.MobileNo,
                                     address = a.Address,
                                     state = a.State,
                                     city = a.City,
                                     country = a.Country,
                                     pin = a.Pincode

                                 }).ToList();
            return Json(Related_query);
        }


        [HttpPost]
        public IActionResult TeacherDetail(TeacherModelNew teacher)
        {
            Tblteacher tblteacher = new Tblteacher();
            Tbllogin tbllogin = new Tbllogin();

            var b = (from a in _context.Tblteacher where a.Email == teacher.Email && a.MobileNo == teacher.MobileNo1 select a).FirstOrDefault();

            if (b != null)
            {
                b.Fname = teacher.Fname;
                b.Lname = teacher.Lname;
                b.Email = teacher.Email;
                b.MobileNo = teacher.MobileNo1;
                b.City = teacher.administrative_area_level_2;
                b.State = teacher.administrative_area_level_1;
                b.Address = teacher.Address;
                b.Pincode = teacher.postal_code;
                b.Country = teacher.country;
                _context.SaveChanges();
            }
            else
            {
                tblteacher.Fname = teacher.Fname;
                tblteacher.Lname = teacher.Lname;
                tblteacher.Email = teacher.Email;
                tblteacher.MobileNo = teacher.MobileNo1;
                tblteacher.City = teacher.administrative_area_level_2;
                tblteacher.State = teacher.administrative_area_level_1;
                tblteacher.Address = teacher.Address;
                tblteacher.Pincode = teacher.postal_code;
                tblteacher.Country = teacher.country;


                _context.Tblteacher.Add(tblteacher);
                _context.SaveChanges();

            }

            long teacherid = (from a in _context.Tblteacher where a.Email == tblteacher.Email select a.TeacherId).FirstOrDefault();
            tbllogin.UserName = teacher.UserName;
            tbllogin.Password = teacher.Password;
            tbllogin.Userid = Convert.ToInt32(teacherid);
            tbllogin.Typeid = 3;//for teacher
            _context.Tbllogin.Add(tbllogin);
            _context.SaveChanges();

            return RedirectToAction("TeacherPayment", "TeacherPayment", new { });

            //return RedirectToAction("TeacherConfiguration", "Home", new { });
        }



        public JsonResult CheckUserNamePassword(string username)
        {

            var exist = (from a in _context.Tbllogin where a.UserName == username select a).ToList();

               // !_context.Tbllogin.ToList().Exists(p => p.UserName.Equals(username));

            
            return Json(exist);
        }



        public JsonResult GetClassdata(Int64 ClassId)
        {
            var Related_query = (from a in _context.TblClass where a.ClassId == ClassId  select new { className = a.ClassName, id= a.ClassId}).ToList();
            return Json(Related_query);
        }

        public JsonResult GetBatchData(Int64 Batchid)
        {
            var Related_query = (from a in _context.TblBatch where a.BatchId == Batchid select new
            { batchName = a.BatchName,
              //  Durationtype = (from b in _context.TblDuration
              //           where b.Durationid == a.DurationTypeId
              //           select new
              //SelectListItem()
              //           { Text = b.Durationname, Value = b.Durationid.ToString() }).ToList(),
              //  className = (from b in _context.TblClass
              //               where b.ClassId == a.ClassId
              //               select new
              //    SelectListItem()
              //               { Text = b.ClassName, Value = b.ClassId.ToString() }).ToList(),
              //  fees =a.Fees
            }).ToList();
            return Json(Related_query);
        }


        public JsonResult GetSubjectData(Int64 subjectId)
        {
            var targetDirectory = Path.Combine(_hostingEnv.WebRootPath, string.Format("images\\"));

            var Related_query = (from a in _context.Tblsubject where a.Subjectid == subjectId select new
            { subjectName = a.Subjectname,
              //Imagepath= Path.Combine(targetDirectory, a.Imagename),
              Isshowimage=a.Isshow
            //className =(from b in _context.TblClass where b.ClassId==a.ClassId select new
            //              SelectListItem() { Text = b.ClassName, Value = b.ClassId.ToString() }).ToList(),
            //   fees = a.Ammount

        }).ToList();
            return Json(Related_query);
        }


        [HttpPost]
        public JsonResult DeleteConfirmed(long Id, string Tabletype)
        {
            try
            {
                if (Tabletype == "Class")
                {
                    TblClass tblClass = _context.TblClass.Find(Id);
                    // if()
                    // Tblsubject tblsubject = _context.Tblsubject.Find(tblClass.ClassId);
                    // TblBatch tblBatch = _context.TblBatch.Find(tblClass.ClassId);
                    // TblStudent tblStudent = _context.TblStudent.Find(tblClass.ClassId);
                    //Int64 classid = Convert.ToInt64(Id);
                    var tblsubject = (from a in _context.TblClassSubjectMap where a.Classid == Id select a).ToList();
                    var tblBatch = (from a in _context.TblBatch where a.ClassId == Id select a).ToList();
                    var tblStudent = (from a in _context.TblStudent where a.ClassId == Id select a).ToList();
                    var tblteacherclassbatchmaps = (from a in _context.Tblteacherclassbatchmap where a.Classid == Id select a).ToList();
                    if (tblsubject.Any() == true || tblBatch.Any() == true || tblStudent.Any() == true || tblteacherclassbatchmaps.Any() == true)
                    {
                        var DeleteMsg = "Delete the child tables first!";
                        return Json(DeleteMsg);


                    }

                    _context.TblClass.Remove(tblClass);
                }
                else if (Tabletype == "Subject")
                {
                    Tblsubject tblsubject = _context.Tblsubject.Find(Id);
                    //var tblsubjectbatchmap = (from a in _context.Tblsubjectbatchmap where a.SubjectId == Id select a).ToList();
                    var tblstudentteacher = (from a in _context.Tblstudentteacher where a.Subjectid == Id select a).ToList();
                    var tblteacherclassbatchmap = (from a in _context.Tblteacherclassbatchmap where a.Subjectid == Id select a).ToList();
                    var tblstudent_subject_batch = (from a in _context.TblstudentSubjectBatch where a.SubjectId == Id select a).ToList();

                    if (tblteacherclassbatchmap.Any() == true || tblstudent_subject_batch.Any() == true)
                    {
                        var DeleteMsg = "Delete the child tables first!";
                        return Json(DeleteMsg);
                    }
                    _context.Tblsubject.Remove(tblsubject);

                }
                else if (Tabletype == "Batch")
                {
                    TblBatch tblBatch = _context.TblBatch.Find(Id);
                    //var tblsubjectbatchmap = (from a in _context.Tblsubjectbatchmap where a.Batchid == Id select a).ToList();
                    var tblteacherclassbatchmap = (from a in _context.Tblteacherclassbatchmap where a.Batchid == Id select a).ToList();
                    var tblmapbatchtime = (from a in _context.Tblmapbatchtime where a.BatchId == Id select a).ToList();
                    var tblstudent_subject_batch = (from a in _context.TblstudentSubjectBatch where a.BatchId == Id select a).ToList();
                    if (tblteacherclassbatchmap.Any() == true || tblmapbatchtime.Any() == true || tblstudent_subject_batch.Any() == true)
                    {
                        var DeleteMsg = "Delete the child tables first!";
                        return Json(DeleteMsg);
                    }
                    _context.TblBatch.Remove(tblBatch);

                }

                else if (Tabletype == "FeeSetting")
                {

                    Tblfeessetting tblfeessetting = _context.Tblfeessetting.Find(Convert.ToInt32(Id));
                    _context.Tblfeessetting.Remove(tblfeessetting);

                }
                else if (Tabletype == "teacherbatchmap")
                {
                    var tblteacherbatch = (from a in _context.Tblteacherclassbatchmap where a.Techermapid == Convert.ToInt32(Id) select a).ToList();
                    _context.Tblteacherclassbatchmap.RemoveRange(tblteacherbatch);
                }
                else if (Tabletype == "ClassSubjectMap")
                {
                    var tblclasssubmap = (from a in _context.TblClassSubjectMap where a.Classid == Convert.ToInt32(Id) select a).ToList();
                    _context.TblClassSubjectMap.RemoveRange(tblclasssubmap);
                }


                else if (Tabletype == "BatchSubjectMap")
                {
                    var tblclasssubmap = (from a in _context.TblClassSubjectMap where a.Batchid == Convert.ToInt32(Id) select a).ToList();
                    _context.TblClassSubjectMap.RemoveRange(tblclasssubmap);
                }
                else if (Tabletype == "Teacher")
                {
                    Tblteacher tblteacher = _context.Tblteacher.Find(Id);

                    var tblstudentteachers = (from a in _context.Tblstudentteacher where a.Teacherid == Id select a).ToList();
                    var tblteacherclsbatchmap = (from a in _context.Tblteacherclassbatchmap where a.TeacherId == Id select a).ToList();

                    if (tblstudentteachers.Any() == true || tblteacherclsbatchmap.Any() == true)
                    {
                        var DeleteMsg = "Delete the child tables first!";
                        return Json(DeleteMsg);
                    }
                    _context.Tblteacher.Remove(tblteacher);

                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {


            }
            
            return Json("Delete successfuly");

        }

        //[HttpPost]
        //public IActionResult FeeDetail(fee_SubjectBatchModel fees)
        //{
        //    Tblfeessetting tblfeessetting = new Tblfeessetting();

        //    //long batchId = (from m in _context.TblBatch where m.BatchName == fees.BatchName select m.BatchId).FirstOrDefault();
        //    //long subid = (from m in _context.Tblsubject where m.Subjectname == fees.SubjectName select m.Subjectid).FirstOrDefault();
        //    //long classId = (from m in _context.TblClass where m.ClassName == fees.ClassName select m.ClassId).FirstOrDefault();

        //    // long durationid = (from m in _context.TblDuration where m.Durationname == fees.DurationType select m.Durationid).FirstOrDefault();
        //    if (fees.feeid != 0)
        //    {
        //        var objfee = (from a in _context.Tblfeessetting where a.Feeid == fees.feeid select a).FirstOrDefault();
        //        objfee.Ammount = fees.feenew;
        //        objfee.ClassId = fees.ClassId;
        //        objfee.DurationTypeId = fees.durationid;
        //        objfee.SubjecBatchtId = fees.SubjecBatchtId;



        //        _context.SaveChanges();

        //    }
        //    else
        //    {
        //        var objfee = (from a in _context.Tblfeessetting where a.Feeid == fees.feeid select a).FirstOrDefault();
        //        if (objfee == null)
        //        {
        //            tblfeessetting.Ammount = fees.feenew;
        //            tblfeessetting.ClassId = fees.ClassId;
        //            tblfeessetting.DurationTypeId = fees.durationid;

        //            tblfeessetting.SubjecBatchtId = fees.SubjecBatchtId;


        //            _context.Tblfeessetting.Add(tblfeessetting);
        //            _context.SaveChanges();
        //        }
        //    }

        //    return RedirectToAction("Configuration", "Home", new { });

        //}




        public JsonResult GetFeeSubjectData(Int64 FeeId)
        {
            var Related_query = (from a in _context.Tblfeessetting
                                 where a.Feeid == FeeId
                                 select new
                                 {

                                     FeesubjectName = (from b in _context.Tblsubject
                                                       where b.Subjectid == a.SubjecBatchtId
                                                       select new
                                                          SelectListItem()
                                                       { Text = b.Subjectname, Value = b.Subjectid.ToString() }).ToList(),

                                     FeeBatchName = (from b in _context.TblBatch
                                                     where b.BatchId == a.SubjecBatchtId
                                                     select new
                                                        SelectListItem()
                                                     { Text = b.BatchName, Value = b.BatchId.ToString() }).ToList(),

                                     className = (from b in _context.TblClass
                                                  where b.ClassId == a.ClassId
                                                  select new
                                                     SelectListItem()
                                                  { Text = b.ClassName, Value = b.ClassId.ToString() }).ToList(),

                                     PlanType = (from b in _context.TblDuration
                                                 where b.Durationid == a.DurationTypeId
                                                 select new
                                                    SelectListItem()
                                                 { Text = b.Durationname, Value = b.Durationid.ToString() }).ToList(),

                                     fees = a.Ammount,
                                     feeId=a.Feeid
                                 }
            ).ToList();
            return Json(Related_query);
        }

        [HttpPost]
        public IActionResult FeeDetail(fee_SubjectBatchModel fees)
        {
            Tblfeessetting tblfeessetting = new Tblfeessetting();

            //long batchId = (from m in _context.TblBatch where m.BatchName == fees.BatchName select m.BatchId).FirstOrDefault();
            //long subid = (from m in _context.Tblsubject where m.Subjectname == fees.SubjectName select m.Subjectid).FirstOrDefault();
            //long classId = (from m in _context.TblClass where m.ClassName == fees.ClassName select m.ClassId).FirstOrDefault();

           // long durationid = (from m in _context.TblDuration where m.Durationname == fees.DurationType select m.Durationid).FirstOrDefault();
            if (fees.feeid != 0)
            {
                var objfee = (from a in _context.Tblfeessetting where a.Feeid == fees.feeid select a).FirstOrDefault();
                objfee.Ammount = fees.feenew2;
                objfee.ClassId = fees.ClassId;
                objfee.DurationTypeId = fees.durationid;
                objfee.SubjecBatchtId =fees.SubjecBatchtId ;
               
               

                _context.SaveChanges();

            }
            else
            {
                var objfee = (from a in _context.Tblfeessetting where a.Feeid == fees.feeid select a).FirstOrDefault();
                if (objfee == null)
                {
                    tblfeessetting.Ammount = fees.feenew2;
                    tblfeessetting.ClassId = fees.ClassId;
                    tblfeessetting.DurationTypeId = fees.durationid;
                  
                    tblfeessetting.SubjecBatchtId = fees.SubjecBatchtId;


                    _context.Tblfeessetting.Add(tblfeessetting);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Configuration", "Home", new { });

        }

        [HttpPost]
        public IActionResult FeeDetailsubject(fee_SubjectBatchModel fees)
        {
            Tblfeessetting tblfeessetting = new Tblfeessetting();
            if (fees.sfeeid != 0)
            {
                var objfee = (from a in _context.Tblfeessetting where a.Feeid == fees.sfeeid select a).FirstOrDefault();
                objfee.Ammount = fees.feenew;
                objfee.ClassId = fees.ClassId;
                objfee.DurationTypeId = fees.durationid;
                objfee.SubjecBatchtId = fees.SubjecBatchtId;



                _context.SaveChanges();

            }
            else
            {
                var objfee = (from a in _context.Tblfeessetting where a.Feeid == fees.sfeeid select a).FirstOrDefault();
                if (objfee == null)
                {
                    tblfeessetting.Ammount = fees.feenew;
                    tblfeessetting.ClassId = fees.ClassId;
                    tblfeessetting.DurationTypeId = fees.durationid;

                    tblfeessetting.SubjecBatchtId = fees.SubjecBatchtId;


                    _context.Tblfeessetting.Add(tblfeessetting);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Configuration", "Home", new { });

        }

        //public JsonResult SaveClasssubject(Int64 ClassId,string subjectclass)
        //{
        //    string[] subjectclasssplit= subjectclass.Split(',');
        //    foreach(var item in subjectclasssplit)
        //    {
        //        if(item != "")
        //        {
        //            TblClassSubjectMap tblClassSubjectMap = new TblClassSubjectMap();
        //            tblClassSubjectMap.Classid = ClassId;
        //            tblClassSubjectMap.Subjectid =Convert.ToInt64(item);
        //            _context.TblClassSubjectMap.Add(tblClassSubjectMap);
        //            _context.SaveChanges();
        //        }
        //    }



        //    return Json(true);
        //}

        public JsonResult SaveClasssubject(Int64 ClassId, string subjectclass)
        {
            var tblsubjectcls = (from a in _context.TblClassSubjectMap where a.Classid == Convert.ToInt32(ClassId) select a).ToList();
            if (tblsubjectcls.Any() == true)
            {
                _context.TblClassSubjectMap.RemoveRange(tblsubjectcls);
                _context.SaveChanges();
            }

            string[] subjectclasssplit = subjectclass.Split(',');
            foreach (var item in subjectclasssplit)
            {
                if (item != "")
                {
                    TblClassSubjectMap tblClassSubjectMap = new TblClassSubjectMap();
                    tblClassSubjectMap.Classid = ClassId;
                    tblClassSubjectMap.Subjectid = Convert.ToInt64(item);
                    _context.TblClassSubjectMap.Add(tblClassSubjectMap);
                    _context.SaveChanges();
                }
            }



            return Json(true);
        }



        public JsonResult SaveBatchsubject(Int64 BatchId, string subjectclass)
        {
            var tblbatchsubject = (from a in _context.TblClassSubjectMap where a.Batchid == Convert.ToInt32(BatchId) select a).ToList();
            if (tblbatchsubject.Any() == true)
            {
                _context.TblClassSubjectMap.RemoveRange(tblbatchsubject);
                _context.SaveChanges();
            }

            string[] subjectclasssplit = subjectclass.Split(',');
            foreach (var item in subjectclasssplit)
            {
                if (item != "")
                {
                    TblClassSubjectMap tblClassSubjectMap = new TblClassSubjectMap();
                    tblClassSubjectMap.Batchid = BatchId;
                    tblClassSubjectMap.Subjectid = Convert.ToInt64(item);
                    _context.TblClassSubjectMap.Add(tblClassSubjectMap);
                    _context.SaveChanges();
                }
            }



            return Json(true);
        }



        public JsonResult GetSubjectMapClass(Int64 id)
        {
            var Related_query = (from a in _context.TblClassSubjectMap
                                 join b in _context.Tblsubject on a.Subjectid equals b.Subjectid
                                 where a.Classid == id
                                 select new
                                 {
                                     b.Subjectname,
                                     b.Subjectid


                                 }
            ).ToList();
            return Json(Related_query);
        }


        public JsonResult GetSubjectMapBatch(Int64 id)
        {
            var Related_query = (from a in _context.TblClassSubjectMap
                                 join b in _context.Tblsubject on a.Subjectid equals b.Subjectid
                                 where a.Batchid == id
                                 select new
                                 {
                                     b.Subjectname,
                                     b.Subjectid


                                 }
            ).ToList();
            return Json(Related_query);
        }






        public JsonResult getSubjectlist(int id)
        {
            // var ddlsub = _context.TblClassSubjectMap.Where(x => x.Classid == id).ToList();

            var ddlsub = (from a in _context.TblClassSubjectMap
                          join b in _context.Tblsubject on a.Subjectid equals b.Subjectid
                          where a.Classid == id
                          select new
                          {
                              sId = a.Subjectid,
                              sName = b.Subjectname

                          }).ToList();

            List<SelectListItem> liSub = new List<SelectListItem>();

            liSub.Add(new SelectListItem { Text = "--Select Subject--", Value = "0" });
            if (ddlsub.Any() == true)
            {
                foreach (var x in ddlsub)
                {
                    liSub.Add(new SelectListItem { Text = x.sName, Value = x.sId.ToString() });
                }
            }
            return Json(liSub);
        }
        public JsonResult GetShowSubject(Int64 ClassId)
        {
            var subject = (from n in _context.TblClassSubjectMap
                           join t in _context.Tblsubject on n.Subjectid equals t.Subjectid
                           where n.Classid == ClassId
                           select new { t.Subjectname }).ToList();
            return Json(subject);
        }

        public JsonResult GetShowSubjectBatch(Int64 BatchId)
        {
            var subject = (from n in _context.TblClassSubjectMap
                           join t in _context.Tblsubject on n.Subjectid equals t.Subjectid
                           where n.Batchid == BatchId
                           select new { t.Subjectname }).ToList();
            return Json(subject);
        }
        public JsonResult TeacherBasedOnsubject(Int64 subid)
        {
            var subject = (from n in _context.Tblmapteacherammount
                           join t in _context.Tblteacher on n.TeacherId equals t.TeacherId
                           where n.SubjectId == subid
                           select new
                                             SelectListItem()
                           { Text = t.Fname, Value = t.TeacherId.ToString() }).ToList();
            return Json(subject);
        }
    }
}
