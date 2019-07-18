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
using System.Net;
using System.Net.Mail;
using System.Globalization;

namespace OnlineClass.Controllers
{
    public class TeacherController : Controller
    {
        private readonly OnlineclassContext _context;

        public TeacherController(OnlineclassContext context)
        {
            _context = context;
        }
        // GET: Teacher
        public async Task<IActionResult> Dashboard(int? Id,string filter, int page = 0, bool Isbatch = true, string sortExpression = "StudentName")
        {
            string teachername = "";
            if (Id != null)
            {
                TempData["teacherid"] = Id;
                TempData.Keep("teacherid");
                var teacher =(from m in _context.Tblteacher where m.TeacherId==Id select m).FirstOrDefault();
                 teachername = teacher.Fname + " " + teacher.Lname;
            }
            else if (HttpContext.Session.GetString("userid") !=null)
            {
                var teacher = (from m in _context.Tblteacher where m.TeacherId ==Convert.ToInt32(HttpContext.Session.GetString("userid")) select m).FirstOrDefault();
                teachername = teacher.Fname + " " + teacher.Lname;
                TempData["teacherid"] = teacher.TeacherId;
                TempData.Keep("teacherid");
                Id=Convert.ToInt32(teacher.TeacherId);
            }
            DateTime dates = DateTime.Now.Date;
            HttpContext.Session.SetString("Teachername", teachername);
            ViewBag.user = HttpContext.Session.GetString("Teachername");

            var qry1 = (from n in _context.TblStudent
                        join m in _context.Tblstudentteacher on n.StudentId equals m.Studentid
                        join s in _context.TblstudentSubjectBatch on n.StudentId equals s.StudentId
                        join t in _context.Tblsubject on s.SubjectId equals t.Subjectid
                        join c in _context.TblClass on n.ClassId equals c.ClassId
                        where m.Subjectid ==s.SubjectId && m.Teacherid==Convert.ToInt32(Id)

                        select new tblTeacherClassList
                        {
                            studentteacheridmap=m.Teacherstudentmapid,
                            TeacherId=m.Teacherid,
                            Subject=t.Subjectname,
                            ClassName=c.ClassName,
                            StudentName=n.Fname +" "+n.Lname,
                            Totime= (from su in _context.Tblscheduleclassbatch where su.Teacherstudentmapid == m.Teacherstudentmapid && su.Techermapid == m.Teacherid && su.Date.Value.Date == dates && su.Isbatch == false select su).FirstOrDefault().Totime,
                            Fromtime = (from su in _context.Tblscheduleclassbatch where su.Teacherstudentmapid == m.Teacherstudentmapid && su.Techermapid == m.Teacherid && su.Date.Value.Date == dates && su.Isbatch == false select su).FirstOrDefault().Fromtime,
                            IsStartClass =(from su in _context.Tblscheduleclassbatch where su.Teacherstudentmapid==m.Teacherstudentmapid && su.Techermapid ==m.Teacherid && su.Date.Value.Date == dates && su.Isbatch == false select  su ).FirstOrDefault().IsStartClass,
                            IsCopletedClass= (from su in _context.Tblscheduleclassbatch where su.Teacherstudentmapid == m.Teacherstudentmapid && su.Techermapid == m.Teacherid && su.IsStartClass == true && su.Date.Value.Date == dates && su.Isbatch == false select su).FirstOrDefault().IsCopletedClass,
                        });
            var qryBatch = (from n in _context.Tblteacher
                            join m in _context.Tblteacherclassbatchmap on n.TeacherId equals m.TeacherId
                           join c in _context.TblClass on m.Classid equals c.ClassId
                           join b in _context.Tblsubject on m.Subjectid equals b.Subjectid
                           join bb in _context.TblBatch on m.Batchid equals bb.BatchId
                        where m.Subjectid == b.Subjectid && n.TeacherId == Convert.ToInt32(Id)

                            select new tblTeacherBatchList
                        {

                            TeacherId = n.TeacherId,
                            BatchName = bb.BatchName,
                            Subject=b.Subjectname,
                            ClassName = c.ClassName,
                            Date = bb.Dateofcommencement,
                            teacherbatchid=m.Techermapid,
                            Teacher=n.Fname +" "+n.Lname,
                                Totime = (from su in _context.Tblscheduleclassbatch where su.Teacherstudentmapid == m.Techermapid && su.Techermapid == m.TeacherId && su.Date.Value.Date == dates && su.Isbatch==true select su).FirstOrDefault().Totime,
                                Fromtime = (from su in _context.Tblscheduleclassbatch where su.Teacherstudentmapid == m.Techermapid && su.Techermapid == m.TeacherId && su.Date.Value.Date == dates && su.Isbatch == true select su).FirstOrDefault().Fromtime,
                                IsStartClass = (from su in _context.Tblscheduleclassbatch where su.Teacherstudentmapid == m.Techermapid && su.Techermapid == m.TeacherId && su.Date.Value.Date == dates && su.Isbatch == true select su).FirstOrDefault().IsStartClass,
                                IsCopletedClass = (from su in _context.Tblscheduleclassbatch where su.Teacherstudentmapid == m.Techermapid && su.Techermapid == m.TeacherId && su.IsStartClass == true && su.Date.Value.Date == dates && su.Isbatch == true select su).FirstOrDefault().IsCopletedClass,
                            });
            if (Isbatch == true && page != 0)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    qryBatch = qryBatch.Where(p => p.BatchName.Contains(filter));
                }
            
                if (page == 0)
                {
                    page = 1;

                }
                var model1 = await PagingList<tblTeacherClassList>.CreateAsync(qry1, 5, 1, "StudentName", "StudentName");

                var modelbatch = await PagingList<tblTeacherBatchList>.CreateAsync(qryBatch, 5, page, "BatchName", "BatchName");

                ViewBag.studentmap = model1;
                ViewBag.batch = modelbatch;
            }
            else if (Isbatch == false && page != 0)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    qry1 = qry1.Where(p => p.StudentName.Contains(filter));
                }
                var student = qry1.AsNoTracking().AsQueryable();
                if (page == 0)
                {
                    page = 1;

                }
                var model1 = await PagingList<tblTeacherClassList>.CreateAsync(qry1, 5, page, "StudentName", "StudentName");
                var modelbatch = await PagingList<tblTeacherBatchList>.CreateAsync(qryBatch, 5, 1, "BatchName", "BatchName");
                ViewBag.studentmap = model1;
                ViewBag.batch = modelbatch;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filter))
                {
                   var qrystudent = qry1.Where(p => p.StudentName.Contains(filter));
                    if (qrystudent.ToList().Count == 0)
                    {
                        qryBatch = qryBatch.Where(p => p.BatchName.Contains(filter));
                    }
                    else
                    {
                        qry1 = qrystudent;
                    }
                }
                
                var student = qry1.AsNoTracking().AsQueryable();
                if (page == 0)
                {
                    page = 1;

                }
                var model1 = await PagingList<tblTeacherClassList>.CreateAsync(qry1, 5, page, "StudentName", "StudentName");
                var modelbatch = await PagingList<tblTeacherBatchList>.CreateAsync(qryBatch, 5, page, "BatchName", "BatchName");
                ViewBag.studentmap = model1;
                ViewBag.batch = modelbatch;
            }
            
            ViewBag.type = 3;
            return View();
        }
        //public JsonResult Updateisclass(Int64 id)
        //{
        //    var Related_query = (from a in _context.Tblstudentteacher where a.Teacherid == id select  a).FirstOrDefault();
        //    if(Related_query != null)
        //    {
        //        Related_query.IsStartClass = true;
        //        _context.SaveChanges();
        //    }
        //    return Json(true);
        //}
        public JsonResult UpdateTeacherstudent(Int32 id,Int32 subjectid,Int32 Teacherid)
        {
            
                var Related_query = (from a in _context.Tblstudentteacher where a.Teacherid == id && a.Subjectid == subjectid select a).FirstOrDefault();
            if(Related_query != null)
            {
                Related_query.Studentid = id;
                Related_query.Subjectid = subjectid;
                _context.SaveChanges();
            }
            else
            {
                Tblstudentteacher tblstudentteacher = new Tblstudentteacher();
                tblstudentteacher.Subjectid = subjectid;
                tblstudentteacher.Teacherid = Teacherid;
                tblstudentteacher.Studentid =Convert.ToInt32(id);
                _context.Tblstudentteacher.Add(tblstudentteacher);
                _context.SaveChanges();
            }
           
            return Json(true);
        }
        public JsonResult UpdateTeacherstudentedits(string id)
        {
            string[] studentteacher = id.Split('_');

            var Related_query = (from a in _context.Tblstudentteacher where a.Teacherstudentmapid == Convert.ToInt32(studentteacher[1]) select a).FirstOrDefault();
            if (Related_query != null)
            {
                Related_query.Teacherid = Convert.ToInt32(studentteacher[0]);
                _context.SaveChanges();
            }
            return Json(true);
        }
        public JsonResult UpdateTeacherstudentEdit(Int32 id)
        {

           var teachers= (from a in _context.Tblteacher select new { teacherid = a.TeacherId, name = a.Fname + " " + a.Lname }).ToList();

            return Json(teachers);
        }
        public JsonResult SaveScheduleDate(Int32 id,string scheduledate,Int32 Teacherid,string isstudentbatch)
        {
            if(isstudentbatch== "student")
            {
                var scheduulemap = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Isbatch == false select a).ToList();
                if (scheduulemap.Count() > 0)
                {
                    _context.Tblscheduleclassbatch.RemoveRange(scheduulemap);
                    _context.SaveChanges();
                }
                string[] scheduledates = scheduledate.Split(',');
                foreach (var itemem in scheduledates)
                {
                    DateTime dates;
                    if (itemem != "")
                    {
                        dates = Convert.ToDateTime(itemem);
                        var scheduulemapcheck = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Date == dates && a.Isbatch == false select a).ToList();
                        if (scheduulemapcheck.Count() == 0)
                        {
                            Tblscheduleclassbatch tblscheduleclassbatch = new Tblscheduleclassbatch();
                            tblscheduleclassbatch.Teacherstudentmapid = id;
                            tblscheduleclassbatch.Date = Convert.ToDateTime(itemem);
                            tblscheduleclassbatch.Techermapid = Teacherid;
                            tblscheduleclassbatch.Isbatch = false;
                            _context.Tblscheduleclassbatch.Add(tblscheduleclassbatch);
                            _context.SaveChanges();
                        }

                    }

                }
                return Json(scheduulemap);
            }
            else
            {
                var scheduulemap = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Isbatch == true select a).ToList();
                if (scheduulemap.Count() > 0)
                {
                    _context.Tblscheduleclassbatch.RemoveRange(scheduulemap);
                    _context.SaveChanges();
                }
                string[] scheduledates = scheduledate.Split(',');
                foreach (var itemem in scheduledates)
                {
                    DateTime dates;
                    if (itemem != "")
                    {
                        dates = Convert.ToDateTime(itemem);
                        var scheduulemapcheck = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Date == dates && a.Isbatch == true select a).ToList();
                        if (scheduulemapcheck.Count() == 0)
                        {
                            Tblscheduleclassbatch tblscheduleclassbatch = new Tblscheduleclassbatch();
                            tblscheduleclassbatch.Teacherstudentmapid = id;
                            tblscheduleclassbatch.Date = Convert.ToDateTime(itemem);
                            tblscheduleclassbatch.Techermapid = Teacherid;
                            tblscheduleclassbatch.Isbatch = true;
                            _context.Tblscheduleclassbatch.Add(tblscheduleclassbatch);
                            _context.SaveChanges();
                        }

                    }

                }
                return Json(scheduulemap);
            }

            //return null;
        }

        public JsonResult SaveScheduleTime(Int32 id,TimeSpan fromtime,TimeSpan ToTime, string scheduledate, Int32 Teacherid, string isstudentbatch,string selecteddate)
        {
            if (isstudentbatch == "student")
            {
                string[] scheduledates = scheduledate.Split(',');
                foreach (var itemem in scheduledates)
                {
                    DateTime dates;
                    if (itemem != "")
                    {
                        dates = Convert.ToDateTime(itemem);
                        var scheduulemapcheck = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Date == dates && a.Isbatch == false select a).ToList();
                        if (scheduulemapcheck.Count() == 0)
                        {
                            Tblscheduleclassbatch tblscheduleclassbatch = new Tblscheduleclassbatch();
                            tblscheduleclassbatch.Teacherstudentmapid = id;
                            tblscheduleclassbatch.Date = Convert.ToDateTime(itemem);
                            tblscheduleclassbatch.Techermapid = Teacherid;
                            tblscheduleclassbatch.Isbatch = false;
                            _context.Tblscheduleclassbatch.Add(tblscheduleclassbatch);
                            _context.SaveChanges();
                        }

                    }

                }



                DateTime date = Convert.ToDateTime(selecteddate);
                var scheduulemap1 = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Isbatch == false && a.Date== date select a).FirstOrDefault();
                if (scheduulemap1 != null)
                {
                    scheduulemap1.Totime = ToTime;
                    scheduulemap1.Fromtime = fromtime;
                    _context.SaveChanges();
                }
               
                return Json(scheduulemap1);
            }
            else
            {
               
                string[] scheduledates = scheduledate.Split(',');
                foreach (var itemem in scheduledates)
                {
                    DateTime dates;
                    if (itemem != "")
                    {
                        dates = Convert.ToDateTime(itemem);
                        var scheduulemapcheck = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Date == dates && a.Isbatch == true select a).ToList();
                        if (scheduulemapcheck.Count() == 0)
                        {
                            Tblscheduleclassbatch tblscheduleclassbatch = new Tblscheduleclassbatch();
                            tblscheduleclassbatch.Teacherstudentmapid = id;
                            tblscheduleclassbatch.Date = Convert.ToDateTime(itemem);
                            tblscheduleclassbatch.Techermapid = Teacherid;
                            tblscheduleclassbatch.Isbatch = true;
                            _context.Tblscheduleclassbatch.Add(tblscheduleclassbatch);
                            _context.SaveChanges();
                        }

                    }

                }
                DateTime date = Convert.ToDateTime(selecteddate);
                var scheduulemap = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Isbatch == true && a.Date == date select a).FirstOrDefault();
                if (scheduulemap != null)
                {
                    scheduulemap.Totime = ToTime;
                    scheduulemap.Fromtime = fromtime;
                    _context.SaveChanges();
                }

                return Json(scheduulemap);
            }

            //return null;
        }


        public JsonResult RemoveScheduleDateTime(Int32 id, Int32 Teacherid, string isstudentbatch, string selecteddate)
        {
            DateTime date = Convert.ToDateTime(selecteddate);
            if (isstudentbatch == "student")
            {
                var scheduulemap = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Isbatch == false && a.Date== date select a).ToList();
                if (scheduulemap.Count() > 0)
                {
                    _context.Tblscheduleclassbatch.RemoveRange(scheduulemap);
                    _context.SaveChanges();
                }
                return Json(true);
            }
            else
            {
                var scheduulemap = (from a in _context.Tblscheduleclassbatch where a.Teacherstudentmapid == id && a.Isbatch == true && a.Date == date select a).ToList();
                if (scheduulemap.Count() > 0)
                {
                    _context.Tblscheduleclassbatch.RemoveRange(scheduulemap);
                    _context.SaveChanges();
                }
                return Json(scheduulemap);
            }

            //return null;
        }
        public JsonResult GetScheduleDate(Int32 id,Int32 Teacherid,string types)
        {
            if(types== "student")
            {
                string value = "";
                var scheduulemap = (from a in _context.Tblscheduleclassbatch

                                    where a.Teacherstudentmapid == id && a.Techermapid == Teacherid && a.Isbatch == false
                                    select new
                                    {
                                        date = a.Date.Value.Date.ToString(),
                                        note = ((TimeSpan)a.Fromtime).ToString(@"hh\:mm") + " - "+((TimeSpan)a.Totime).ToString(@"hh\:mm")
                                    }).ToList();
                if (scheduulemap.Count() == 0)
                {
                    var valu1 = (from a in _context.Tblscheduleclassbatch
                                 join b in _context.Tblstudentteacher on a.Techermapid equals b.Teacherid
                                 where a.Teacherstudentmapid == id && b.Teacherid == Teacherid && a.Isbatch == false
                                 select new
                                 {
                                     date = "2018-05-25",
                                     note = (from m in _context.Tbltype select new { m.Type }).ToList()
                                 }).ToList();
                    return Json(valu1);
                }
                else
                {
                    return Json(scheduulemap);
                }
            }
            else if(types == "batch")
            {
                string value = "";
                var scheduulemap = (from a in _context.Tblscheduleclassbatch
                                    where a.Teacherstudentmapid == id && a.Techermapid == Teacherid && a.Isbatch == true
                                    select new
                                    {
                                        date = a.Date.Value.Date.ToString(),
                                        note = ((TimeSpan)a.Fromtime).ToString(@"hh\:mm") + " - " + ((TimeSpan)a.Totime).ToString(@"hh\:mm")
                                    }).ToList();
                if (scheduulemap.Count() == 0)
                {
                    var valu1 = (from a in _context.Tblscheduleclassbatch
                                 join b in _context.Tblstudentteacher on a.Techermapid equals b.Teacherid
                                 where a.Teacherstudentmapid == id && b.Teacherid == Teacherid && a.Isbatch == true
                                 select new
                                 {
                                     date = "2018-05-25",
                                     note = (from m in _context.Tbltype select new { m.Type }).ToList()
                                 }).ToList();
                    return Json(valu1);
                }
                else
                {
                    return Json(scheduulemap);
                }
            }
            return null;
          
        }
        public JsonResult GetsubjectBasedOnBatch(Int64 batchid)
        {
            try
            {
                var subjects = (from m in _context.TblClassSubjectMap
                                join n in _context.Tblsubject on m.Subjectid equals n.Subjectid
                               
                                where m.Batchid == batchid  
                                select new
                                {
                                    id = n.Subjectid,
                                    name = n.Subjectname,
                                   
                                }).Distinct().ToList();

                return Json(subjects);
            }
            catch (Exception)
            {

                return Json(true);
            }
        }


        public JsonResult GetTeacherbasedonBatchSubject(Int64 batchid,Int64 subjectId)
        {
            try
            {
                var teachers = (from m in _context.Tblmapteacherammount
                                
                                join c in _context.Tblteacher on m.TeacherId equals c.TeacherId

                                where m.ClassBatchId == batchid && m.SubjectId == subjectId && m.Isbatch == true
                                select new
                                {
                                    
                                    teacherid = m.TeacherId,
                                    teachername = c.Fname + " " + c.Lname
                                }).Distinct().ToList();

                return Json(teachers);
            }
            catch (Exception)
            {

                return Json(true);
            }
        }
        public JsonResult savebatchmapdata(Int32 Batch_id, Int32 classid, Int64 subjectid, Int32 teacherid)
        {
            try
            {
                //var scheduulemapcheck = (from a in _context.Tblteacherclassbatchmap where a.Teacherstudentmapid == id && a.Date == dates && a.Isbatch == false select a).ToList();
                //if (scheduulemapcheck.Count() == 0)
                //{
                Tblteacherclassbatchmap tblteacherclassbatchmap = new Tblteacherclassbatchmap();
                tblteacherclassbatchmap.Subjectid = subjectid;
                tblteacherclassbatchmap.Classid = classid;
                tblteacherclassbatchmap.Batchid = Batch_id;
                tblteacherclassbatchmap.TeacherId = teacherid;
                _context.Tblteacherclassbatchmap.Add(tblteacherclassbatchmap);
                _context.SaveChanges();

                //}
                return Json(true);
            }
            catch (Exception ex)
            {

                return Json(true);
            }
        }
        public JsonResult Sendmail(Int32 id, Int32 studentmapid, string Date ,string type)
        {
            try
            {
                if (type== "student")
                {
                    var student = (from m in _context.Tblstudentteacher
                                   join n in _context.TblStudent on m.Studentid equals n.StudentId
                                   join t in _context.Tblteacher on m.Teacherid equals t.TeacherId
                                   where m.Teacherstudentmapid == studentmapid
                                   select new
                                   {
                                       studebntemail = n.Email,
                                       teacheremail = t.Email
                                   }).FirstOrDefault();
                    string msg = sendmail.sendmailtostudent(student.studebntemail, student.teacheremail, Date, "Your class start please login");
                    DateTime dates = Convert.ToDateTime(Date);
                    var teacher = (from m in _context.Tblscheduleclassbatch where m.Teacherstudentmapid == studentmapid && m.Date == dates && m.Isbatch == false select m).FirstOrDefault();
                    teacher.IsStartClass = true;
                    _context.SaveChanges();
                }
                else
                {
                    DateTime dates = Convert.ToDateTime(Date);
                    var teacher = (from m in _context.Tblscheduleclassbatch where m.Teacherstudentmapid == studentmapid && m.Date == dates && m.Isbatch==true select m).FirstOrDefault();
                    teacher.IsStartClass = true;
                    _context.SaveChanges();
                }
               

               // string sensms=sendmail.sensms(student.studebntemail, student.teacheremail, Date);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(true);

            }
          
        }
        public JsonResult StopWarning()
        {
            try
            {
                DateTime dt = DateTime.Now;
                string Timeonly = dt.ToString("HH:mm");
                TimeSpan time = TimeSpan.Parse(Timeonly);
                Int32 teacherid = Convert.ToInt32(TempData.Peek("teacherid"));
                var teacher = (from m in _context.Tblteacher where m.TeacherId == teacherid  select m).FirstOrDefault();
                string msg = sendmail.sendmailtostudent("skysanajay16@gmail.com", "skysanjay88@gmail.com","", "Please stop class");
                // string sensms=sendmail.sensms(student.studebntemail, student.teacheremail, Date);
                return Json(msg);
            }
            catch (Exception ex)
            {
                return Json(true);

            }

        }
        public JsonResult StopClass(Int32 id, Int32 Teacherid, string types)
        {
            try
            {
                if (types == "student")
                {
                    var teacher = (from m in _context.Tblscheduleclassbatch where m.Teacherstudentmapid == id && m.Techermapid == Teacherid && m.IsStartClass == true && m.Isbatch == false select m).FirstOrDefault();
                    teacher.IsCopletedClass = true;
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    var teacher = (from m in _context.Tblscheduleclassbatch where m.Teacherstudentmapid == id && m.Techermapid == Teacherid && m.IsStartClass == true && m.Isbatch==true select m).FirstOrDefault();
                    teacher.IsCopletedClass = true;
                    _context.SaveChanges();
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return Json(true);

            }

        }

        public IActionResult Remuneration(string month, string year)
        {
            var userlogin = (from m in _context.Tbllogin where m.UserName == HttpContext.Session.GetString("name") select m).FirstOrDefault();
            var teacher = (from m in _context.Tblteacher where m.TeacherId == userlogin.Userid select m).FirstOrDefault();
            ViewBag.user = teacher.Fname + " " + teacher.Lname;
            string userid = HttpContext.Session.GetString("userid");
            ViewBag.type = 3;
            StudentRegistration sr = new StudentRegistration();
            List<TeacherMain> lt = new List<TeacherMain>();
            if (TempData["Month"] != null)
            {
                ViewBag.mthnum = DateTime.ParseExact(TempData["Month"].ToString(), "MMMM", CultureInfo.CurrentCulture).Month;
                
                lt = sr.GetAllTeacherDetailForPayment(TempData["Month"].ToString(), TempData["Year"].ToString(),Convert.ToInt32(userid), _context).ToList();
                //TempData.Keep("Month");
                //TempData.Keep("Year");
            }
            else
            {
                ViewBag.mthnum = DateTime.Today.Month;
            }
            
      
            if (ViewBag.teacherMain == null)
            {
                ViewBag.teacherMain = lt; //TempData["teacherMain"];
            }
           
            
            return View();
        }

        public IActionResult GetTeacherPayment(string month, string year)
        {
            ViewBag.mthnum = month;
            TempData["Month"] = month;
            TempData.Keep("Month");
            TempData["Year"] = year;
            TempData.Keep("Year");
            StudentRegistration sr = new StudentRegistration();
            List<TeacherMain> lt = new List<TeacherMain>();
            string userid = HttpContext.Session.GetString("userid");
            lt = sr.GetAllTeacherDetailForPayment(month, year,Convert.ToInt32(userid), _context).ToList();
            ViewBag.teacherMain = lt;
            return RedirectToAction("Remuneration", "Teacher", new {});
        }

        //public JsonResult savebatchmapdata(Int32 Batch_id, Int32 classid, Int64 subjectid, Int32 teacherid)
        //{
        //    try
        //    {
        //        //var scheduulemapcheck = (from a in _context.Tblteacherclassbatchmap where a.Teacherstudentmapid == id && a.Date == dates && a.Isbatch == false select a).ToList();
        //        //if (scheduulemapcheck.Count() == 0)
        //        //{
        //        Tblteacherclassbatchmap tblteacherclassbatchmap = new Tblteacherclassbatchmap();
        //        tblteacherclassbatchmap.Subjectid = subjectid;
        //        tblteacherclassbatchmap.Classid = classid;
        //        tblteacherclassbatchmap.Batchid = Batch_id;
        //        tblteacherclassbatchmap.TeacherId = teacherid;
        //        _context.Tblteacherclassbatchmap.Add(tblteacherclassbatchmap);
        //        _context.SaveChanges();

        //        //}
        //        return Json(true);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(true);
        //    }
        //}

        //public JsonResult GetsubjectBasedOnBatch(Int64 batchid)
        //{
        //    try
        //    {
        //        var subjects = (from m in _context.TblClassSubjectMap
        //                        join n in _context.Tblsubject on m.Subjectid equals n.Subjectid
        //                        where m.Batchid == batchid
        //                        select new
        //                        {
        //                            id = n.Subjectid,
        //                            name = n.Subjectname
        //                        }).ToList();

        //        return Json(subjects);
        //    }
        //    catch (Exception)
        //    {

        //        return Json(true);
        //    }
        //}


        //// GET: Teacher/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Teacher/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Teacher/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Teacher/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Teacher/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Teacher/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Teacher/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}