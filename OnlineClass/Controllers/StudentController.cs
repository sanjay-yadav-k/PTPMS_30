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
using Newtonsoft.Json;

namespace OnlineClass.Controllers
{
    public class StudentController : Controller
    {
        private readonly OnlineclassContext _context;
        GetPayment GetPayment;
        public StudentController(OnlineclassContext context, GetPayment _GetPayment)
        {
            _context = context;
            GetPayment = _GetPayment;
        }
        // GET: Student
        public async Task<IActionResult> Index(int? Id,string filter, int page = 0, bool Isbatch = true, string sortExpression = "TeacherName")
        {
            ViewBag.type = 2;
            string studentName = "";
            Int32 student_id = 0;
            if (Id != null)
            {
                var student = (from m in _context.TblStudent where m.StudentId == Id select m).FirstOrDefault();
                studentName = student.Fname + " " + student.Lname;
                TempData["studenid"] = student.StudentId;
                TempData.Keep("studenid");
                student_id =Convert.ToInt32(Id);
            }
            else
            {
                 student_id = Convert.ToInt32(TempData.Peek("studenid"));
                var student = (from m in _context.TblStudent where m.StudentId == student_id select m).FirstOrDefault();
                studentName = student.Fname + " " + student.Lname;
            }
            
            ViewBag.user = studentName;// HttpContext.Session.GetString("Studentname");
   
            var qry1 = (from n in _context.TblStudent
                        join m in _context.Tblstudentteacher on n.StudentId
                        equals m.Studentid
                        join s in _context.TblstudentSubjectBatch on n.StudentId equals s.StudentId
                        join t in _context.Tblsubject on s.SubjectId equals t.Subjectid
                        join c in _context.TblClass on n.ClassId equals c.ClassId
                        join tt in _context.Tblteacher on m.Teacherid equals tt.TeacherId
                        where m.Subjectid == s.SubjectId && m.Studentid == student_id

                        select new tblStudentTeacherClassList
                        {

                            studentteacheridmap = m.Teacherstudentmapid,
                            TeacherId = m.Teacherid,
                            Subject = t.Subjectname,
                            ClassName = c.ClassName,
                            StudentName = n.Fname + " " + n.Lname,
                            SudentId=n.StudentId,
                            TeacherName=tt.Fname + " " +tt.Lname
                            
                        });
            var qryBatch = (from n in _context.TblStudent
                            join m in _context.TblstudentSubjectBatch on n.StudentId equals m.StudentId
                            join c in _context.TblClass on n.ClassId equals c.ClassId
                            join bb in _context.TblBatch on m.BatchId equals bb.BatchId
                            join st in _context.Tblteacherclassbatchmap on m.BatchId equals st.Batchid
                            join b in _context.Tblsubject on st.Subjectid equals b.Subjectid
                            join t in _context.Tblteacher on st.TeacherId equals t.TeacherId
                            where n.StudentId == student_id && m.BatchId == st.Batchid

                            select new tblStudentBatchList
                            {
                                TeacherId = st.TeacherId,
                                BatchName = bb.BatchName,
                                ClassName = c.ClassName,
                                Date = bb.Dateofcommencement,
                                Studentid = n.StudentId,
                                Teacher = t.Fname + " " + t.Lname == null ? "" : t.Fname + " " + t.Lname,
                                Subjects = b.Subjectname == null? "": b.Subjectname,
                                teacherbatchid=st.Techermapid
                            }).Distinct();

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
                var model1 = await PagingList<tblStudentTeacherClassList>.CreateAsync(qry1, 5, 1, "TeacherName", "TeacherName");

                var modelbatch = await PagingList<tblStudentBatchList>.CreateAsync(qryBatch, 5, page, "BatchName", "BatchName");

                ViewBag.studentmap = model1;
                ViewBag.batch = modelbatch;
            }
            else if (Isbatch == false && page != 0)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    qry1 = qry1.Where(p => p.TeacherName.Contains(filter));
                }
                var student = qry1.AsNoTracking().AsQueryable();
                if (page == 0)
                {
                    page = 1;

                }
                var model1 = await PagingList<tblStudentTeacherClassList>.CreateAsync(qry1, 5, page, "TeacherName", "TeacherName");
                var modelbatch = await PagingList<tblStudentBatchList>.CreateAsync(qryBatch, 5, 1, "BatchName", "BatchName");
                ViewBag.studentmap = model1;
                ViewBag.batch = modelbatch;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    var qrystudent = qry1.Where(p => p.TeacherName.Contains(filter));
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
                var model1 = await PagingList<tblStudentTeacherClassList>.CreateAsync(qry1, 5, page, "TeacherName", "TeacherName");
                var modelbatch = await PagingList<tblStudentBatchList>.CreateAsync(qryBatch, 5, page, "BatchName", "BatchName");
                if (model1.Count > 0)
                {
                    ViewBag.studentmap = model1;
                }
                else
                {
                    ViewBag.studentmap = null;
                }
                if (modelbatch.Count > 0)
                {
                    ViewBag.batch = modelbatch;
                }
                else
                {
                    ViewBag.batch = null;
                }

            }


            return View();
        }
        
        public async Task<IActionResult> Addsubject()
        {
            try
            {
                ViewBag.type = 2;
                var userlogin = (from m in _context.Tbllogin where m.UserName == HttpContext.Session.GetString("name") select m).FirstOrDefault();
                var studentlist = (from n in _context.TblStudent where n.StudentId == userlogin.Userid select n).FirstOrDefault();
                ViewBag.user = studentlist.Fname +" "+ studentlist.Lname;
                var subjectlist = (from m in _context.Tblsubject
                                       join n in _context.TblClassSubjectMap on m.Subjectid equals n.Subjectid
                                       where n.Classid == studentlist.ClassId
                                       select new subject
                                       {
                                           subjectname = m.Subjectname,
                                           SubjectId= m.Subjectid,
                                           Selected=false
                                       }
                                     );
                List<subject> subjects = subjectlist.ToList();
                ViewBag.subjectlist = subjects;
                TempData["studenid"] = studentlist.StudentId;
                TempData.Keep("studenid");
                TempData["classid"] = studentlist.ClassId;
                TempData.Keep("classid");

                var className = (from m in _context.TblClass
                                 where m.ClassId == studentlist.ClassId
                                 select m).FirstOrDefault();

                TempData["ClassName"] = className.ClassName;
                TempData.Keep("ClassName");
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
         }

        [HttpGet]
        public IActionResult GetDuration(Int64 batchid,Int64 classid)
        {
            if (classid != 0)
            {
                var duration = (from m in _context.Tblfeessetting
                                join n in _context.TblDuration on m.DurationTypeId equals n.Durationid
                                where m.SubjecBatchtId == batchid && m.ClassId==classid
                                orderby m.DurationTypeId descending
                                select new
                                {
                                    durationid = m.DurationTypeId,
                                    durationname = n.Durationname


                                }).Distinct().ToList();

                return Json(duration);
            }
            else
            {
                var duration = (from m in _context.Tblfeessetting
                                join n in _context.TblDuration on m.DurationTypeId equals n.Durationid
                                where m.SubjecBatchtId == batchid
                                orderby m.DurationTypeId descending
                                select new
                                {
                                    durationid = m.DurationTypeId,
                                    durationname = n.Durationname


                                }).Distinct().ToList();

                return Json(duration);
            }
           

            
        }

        [HttpGet]
        public JsonResult GetAmount(Int32 value, Int64 subjectid)
        {
            var classid = TempData["classid"];
            TempData.Keep("classid");
            var amount = (from m in _context.Tblfeessetting
                          where m.ClassId == Convert.ToInt64(classid) && m.DurationTypeId == value && m.SubjecBatchtId == subjectid
                          select m).FirstOrDefault();
            decimal amt = 0;
            if (amount != null)
            {
                amt = Convert.ToDecimal(amount.Ammount);
            }
            //decimal amt =amount.Ammount==null ? 0 : Convert.ToDecimal(amount.Ammount);

            return Json(amt);
        }
        [HttpGet]
        public JsonResult GetAmountBatch(Int32 value, Int64 Batchid)
        {
            //var classid = TempData["classid"];
            //TempData.Keep("classid");
            var amount = (from m in _context.Tblfeessetting
                          where m.DurationTypeId == value && m.SubjecBatchtId == Batchid
                          select m).FirstOrDefault();
            decimal amt = 0;
            if (amount != null)
            {
                amt = Convert.ToDecimal(amount.Ammount);
            }
            //decimal amt =amount.Ammount==null ? 0 : Convert.ToDecimal(amount.Ammount);

            return Json(amt);
        }

        public async Task<IActionResult> JoinBatch()
        {
            ViewBag.type = 2;
            var userlogin = (from m in _context.Tbllogin where m.UserName == HttpContext.Session.GetString("name") select m).FirstOrDefault();
            var studentlist = (from n in _context.TblStudent where n.StudentId == userlogin.Userid select n).FirstOrDefault();
            ViewBag.user = studentlist.Fname + " " + studentlist.Lname;
            ViewBag.loginid = userlogin.Userid;
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                return View();
            }

        }

        public JsonResult insertUpdateTransactionForBatch(int Op, Int64 studentid, decimal amt, bool isSubject, Int64 batchid, int durationid, decimal totamt, int? runhr)
        {
            string msg = string.Empty;
            int ans = 0;
            StudentRegistration sr = new StudentRegistration();
            try
            {
                Random r = new Random();
                string txnid = "Txn" + r.Next(100, 9999);
                ans = sr.insertUpdateTransactionForBatch(Op, studentid, amt, isSubject, batchid, durationid, totamt, runhr, _context, txnid);
                if (ans == 0)
                {
                    msg = "error";
                    return Json(msg);
                }
                else
                {
                   
                    var payment = GetPayment.Payment(totamt.ToString(),Convert.ToInt32(studentid), txnid);
                    msg = "Successfully inserted";
                    return Json(payment);
                }
               
            }
            catch (Exception ex)
            {

                return Json("Operation Fieled");
            }


        }
        public JsonResult GetSubjects(long batch)
        {
            try
            {
                /*
                    select a.Subjectid,b.Subjectname 
                    from tblClassSubjectMap a
                    inner join tblsubject b
                    on a.Subjectid=b.Subjectid
                    where batchid=10004
     
                 */
                var subjects = (from m in _context.TblClassSubjectMap
                                join t in _context.Tblsubject on m.Subjectid equals t.Subjectid
                                where m.Batchid==batch
                                select new
                                {
                                    id=m.Subjectid,
                                    name=t.Subjectname
                                }
                           ).ToList();
                //var nodata ="[{ "id":"0","name":"No Data Found" }]"
                //if (subjects != null)
                //{
                //    return Json(subjects);
               // }
                //else
                //{
                    //[{"id":100014,"name":"Science"},{"id":100016,"name":"Physics"},{"id":100018,"name":"Mathematics"}]
                    return Json(subjects);
                //}
                
            }
            catch (Exception)
            {

                return Json("No Data Found");
            }
        }

            // GET: Student/Details/5
            public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult StudentConfiguration()
        {
            try
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
                ViewBag.StudentList = (from a in _context.TblStudent select a).ToList();
                ViewBag.ClassName = new SelectList(_context.TblClass, "ClassId", "ClassName");

                ViewBag.StudentName = new SelectList(_context.TblStudent, "StudentId", "Fname");
                List<offlineStudentList> offlineStudentLists = null;

                var StudentBatch = (from a in _context.TblstudentSubjectBatch
                                     
                                  join c in _context.TblBatch on a.BatchId equals c.BatchId
                                  //join d in _context.Tblsubject on a.SubjectId equals d.Subjectid
                                  join e in _context.TblStudent on a.StudentId equals e.StudentId
                                  join f in _context.TblDuration on a.DurationId equals Convert.ToInt32(f.Durationid)
                                 
                                  select new offlineStudentList

                                  {
                                      Id = a.Id,
                                      StudentName = e.Fname + " " + e.Lname,
                                      batchName = c.BatchName,
                                     // SubName = d.Subjectname,
                                      Typename = f.Durationname,
                                      Period = (a.DurationId==1)?a.Period:null


                                  }).ToList();


                var Studentsubject = (from a in _context.TblstudentSubjectBatch

                                            //  join c in _context.TblBatch on a.BatchId equals c.BatchId
                                              join d in _context.Tblsubject on a.SubjectId equals d.Subjectid
                                              join e in _context.TblStudent on a.StudentId equals e.StudentId
                                              join f in _context.TblDuration on a.DurationId equals Convert.ToInt32(f.Durationid)

                                              select new offlineStudentList

                                              {
                                                  Id = a.Id,
                                                  StudentName = e.Fname + " " + e.Lname,
                                                 // batchName = c.BatchName,
                                                  SubName = d.Subjectname,
                                                  Typename = f.Durationname,
                                                  Period = (a.DurationId == 1) ? a.Period : null


                                              }).ToList();
                if (StudentBatch.Any() == true && Studentsubject.Any() == true)
                {

                    offlineStudentLists = StudentBatch.Union(Studentsubject).OrderBy(x => x.Id).ToList();
                }
                else if (StudentBatch.Any() == true)
                {
                    offlineStudentLists = StudentBatch.OrderBy(x => x.Id).ToList();
                }
                else if (Studentsubject.Any() == true)

                {
                    offlineStudentLists = Studentsubject.OrderBy(x => x.Id).ToList();
                }
               

                ViewBag.Studentbatchsubject = offlineStudentLists;

                ViewBag.DDLBatch = new SelectList(_context.TblBatch, "BatchId", "BatchName");
                ViewBag.DDlType = new SelectList(_context.TblDuration, "Durationid", "Durationname");
                ViewBag.DDlSubject = new SelectList(_context.Tblsubject, "Subjectid", "Subjectname");





            }
            catch (Exception ex)
            {

            }
            return View();
        }


        [HttpPost]
        public IActionResult StudentDetail(StudentModel student)
        {
            TblStudent tblStudent = new TblStudent();
            Tbllogin tbllogin = new Tbllogin();

            var b = (from a in _context.TblStudent where a.StudentId == student.studentId select a).FirstOrDefault();

            if (b != null)
            {
                b.Fname = student.Fname;
                b.Lname = student.Lname;
                b.Username = student.Username;
                b.Dob = student.Dob;
                b.ParentName = student.ParentName;
                b.ClassId = student.ClassId;
                b.Email = student.Email;
                b.MobileNo = student.MobileNo;
                b.Address = student.Address;
                b.State = student.administrative_area_level_1;
                b.Skype = student.Skype;
                b.Zipcode = student.postal_code;
                b.Country = student.country;
                _context.SaveChanges();
            }
            else
            {
                tblStudent.Fname = student.Fname;
                tblStudent.Lname = student.Lname;
                tblStudent.Username = student.Username;
                tblStudent.Dob = student.Dob;
                tblStudent.ParentName = student.ParentName;
                tblStudent.ClassId = student.ClassId;
                tblStudent.Email = student.Email;
                tblStudent.MobileNo = student.MobileNo;
                tblStudent.Address = student.Address;
                tblStudent.State = student.administrative_area_level_1;
                tblStudent.Skype = student.Skype;
                tblStudent.Zipcode = student.postal_code;
                tblStudent.Country = student.country;

                _context.TblStudent.Add(tblStudent);
                _context.SaveChanges();

            }

            int studentid = (from a in _context.TblStudent where a.Username == tblStudent.Username select a.StudentId).FirstOrDefault();
            tbllogin.UserName = student.Username;
            tbllogin.Password = student.Pswd;
            tbllogin.Userid = Convert.ToInt32(studentid);
            tbllogin.Typeid = 2;//for student
            _context.Tbllogin.Add(tbllogin);
            _context.SaveChanges();

            return RedirectToAction("StudentConfiguration", "Student", new { });


        }





       

        public JsonResult GetStudent(int StudentId)
        {
            var Related_query = (from a in _context.TblStudent

                                 where a.StudentId == StudentId
                                 select new
                                 {
                                     firstname = a.Fname,
                                     lastname = a.Lname,
                                     email = a.Email,
                                     mobile = a.MobileNo,
                                     address = a.Address,
                                     state = a.State,
                                     dob = a.Dob,
                                     parent = a.ParentName,
                                     skype = a.Skype,

                                     country = a.Country,
                                     pin = a.Zipcode,
                                     className = (from b in _context.TblClass
                                                  where b.ClassId == a.ClassId
                                                  select new
                                       SelectListItem()
                                                  { Text = b.ClassName, Value = b.ClassId.ToString() }).ToList(),


                                 }).ToList();
            return Json(Related_query);
        }


        [HttpPost]
        public JsonResult DeleteConfirmed(int Id, string Tabletype)
        {
            try
            {
                if (Tabletype == "Student")
                {
                    TblStudent tblStudent = _context.TblStudent.Find(Id);

                    var tblstudentteacher = (from a in _context.Tblstudentteacher where a.Studentid == Id select a).ToList();
                    var tblmapstudent = (from a in _context.Tblmapstudentduration where a.Studentid == Id select a).ToList();
                    var tblstusubBatch = (from a in _context.TblstudentSubjectBatch where a.StudentId == Id select a).ToList();

                    if (tblstudentteacher.Any() == true || tblmapstudent.Any() == true || tblstusubBatch.Any() == true)
                    {
                        var DeleteMsg = "Delete the child tables first!";
                        return Json(DeleteMsg);


                    }

                    _context.TblStudent.Remove(tblStudent);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {


            }

            return Json("Delete successfuly");

        }

        public JsonResult Generatehascode(string jsonString,string Ammounts) //,string Ammounts
        {
            int ans = 0;
            StudentRegistration sr = new StudentRegistration();
            try
            {
                Random r = new Random();
                string txnid = "Txn" + r.Next(100, 9999);
                //string Ammounts = "";
                var doc = JsonConvert.DeserializeXmlNode(jsonString, "root");
                ans = sr.SubjectMoneyTransactionInsertionUpdation(doc, _context,txnid);
                //SubjectInsertionUpdation
                Int32 studentid = Convert.ToInt32(TempData.Peek("studenid"));
                var payment = GetPayment.Payment(Ammounts,studentid, txnid);


                // string json = "{\"success\":\"" + 

               // GetStringFromHash(hash) + "\"}";
                return Json(payment);
            }
            catch (Exception ex)
            {
                return Json("error");

            }

        }

        public IActionResult TeacherToBeAlloted()
        {
            try
            {
                ViewBag.type = 2;

                //foreach (var item in obj) {
                //    var NoTeacher = (from d in _context.Tblteacherclassbatchmap where d.Batchid == item.BatchId && d.Subjectid == item.SubjectId select d.TeacherId).FirstOrDefault();

                //        }
                //var result = _context.TblstudentSubjectBatch
                // .Where(e => !_context.Tblteacherclassbatchmap.Where(m => m.Subjectid == e.SubjectId).Any()).Select();

                Int32 stuId = Convert.ToInt32(TempData.Peek("studenid"));
                var student = (from m in _context.TblStudent where m.StudentId == stuId select m).FirstOrDefault();
                ViewBag.user = student.Fname + " " + student.Lname;
                //ViewBag.Subject = (from e in _context.TblstudentSubjectBatch
                //                   join a in _context.Tblsubject on e.SubjectId equals a.Subjectid
                //                   where e.StudentId == stuId && !_context.Tblstudentteacher.All(m => m.Subjectid == e.SubjectId && m.Teacherid==null)  
                //                   select new Batch_SubjectModel
                //                   {

                //                       subjectname = a.Subjectname
                //                   }).ToList();

                ViewBag.Subject = (from c in _context.TblstudentSubjectBatch
                                   join a in _context.Tblsubject on c.SubjectId equals a.Subjectid
                                   join pp in _context.Tblstudentteacher on c.SubjectId equals pp.Subjectid
                                   join p in _context.Tblstudentteacher on c.StudentId equals p.Studentid into ps
                                   from p in ps.DefaultIfEmpty()

                                   where c.StudentId == stuId && p.Subjectid == null 
                                   select new Batch_SubjectModel { subjectname = a.Subjectname }).ToList();

                //ViewBag.Batch = (from e in _context.TblstudentSubjectBatch
                //                 join a in _context.TblBatch on e.BatchId equals a.BatchId
                //                 where e.StudentId == stuId && !_context.Tblteacherclassbatchmap.All(m => m.Batchid == e.BatchId)
                //                 select new Batch_SubjectModel
                //                 {

                //                     BatchName = a.BatchName
                //                 }).ToList();

                ViewBag.Batch = (from c in _context.TblstudentSubjectBatch
                                 join a in _context.TblBatch on c.BatchId equals a.BatchId
                                 join p in _context.Tblteacherclassbatchmap on c.BatchId equals p.Batchid into ps
                                 from p in ps.DefaultIfEmpty()

                                 where c.StudentId == stuId && p.Batchid == null
                                 select new Batch_SubjectModel { BatchName = a.BatchName }).ToList();

                var objTranscationsubject = (from a in _context.TblTempTransaction
                                             join b in _context.TblTransaction on a.TransactionId equals b.TransactionId
                                             join c in _context.Tblsubject on a.SubBatchId equals c.Subjectid
                                             join d in _context.TblDuration on a.DurationType equals d.Durationid
                                             where a.StudentId == stuId && a.IsSubject == true && b.Status== "success"
                                             select new TransactionHistory
                                             {
                                                 SubjectName = c.Subjectname,
                                                 DateofTrans = a.DateOfTransaction,
                                                 Amount_Paid = a.Amount,
                                                 status = b.Status,
                                                 duration = d.Durationname,
                                                 period = (a.DurationType == 1) ? a.Period : null

                                             }).ToList();


                var objTranscationBatch = (from a in _context.TblTempTransaction
                                           join b in _context.TblTransaction on a.TransactionId equals b.TransactionId
                                           join c in _context.TblBatch on a.SubBatchId equals c.BatchId
                                           join d in _context.TblDuration on a.DurationType equals d.Durationid
                                           where a.StudentId == stuId && a.IsSubject == false && b.Status == "success"
                                           select new TransactionHistory
                                           {
                                               batchName = c.BatchName,
                                               DateofTrans = a.DateOfTransaction,
                                               Amount_Paid = a.Amount,
                                               status = b.Status,
                                               duration = d.Durationname


                                           }).ToList();


                ViewBag.subjectTransaction = objTranscationsubject;
                ViewBag.BatchTranscation = objTranscationBatch;


            }
            catch (Exception ex)
            {

            }
            return View();
        }


        public IActionResult SaveStudentSubjectBatchMap(StudentModel student)
        {
            try
            {
                TblstudentSubjectBatch tblstudentSubjectBatch = new TblstudentSubjectBatch();
                var b = (from a in _context.TblstudentSubjectBatch where a.StudentId == student.studentId select a).FirstOrDefault();

                if (b != null)
                {
                    b.BatchId = student.BatchId;
                    b.DurationId = student.DurationId;
                    b.Period = student.Period;
                    b.StudentId = student.StudentId;
                    b.SubjectId = student.SubjectId;
                    _context.SaveChanges();
                }
                else
                {
                    tblstudentSubjectBatch.BatchId = student.BatchId;
                    tblstudentSubjectBatch.DurationId = student.DurationId;
                    tblstudentSubjectBatch.Period = student.Period;
                    tblstudentSubjectBatch.StudentId = student.StudentId;
                    tblstudentSubjectBatch.SubjectId = student.SubjectId;
                    _context.TblstudentSubjectBatch.Add(tblstudentSubjectBatch);
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("StudentConfiguration", "Student", new { });
        }


        public JsonResult GetClass(int id)
        {


           
                var objClass = (from a in _context.TblStudent
                                join b in _context.TblClass on a.ClassId equals b.ClassId
                                where a.StudentId == id
                                select new
                                                SelectListItem()
                                { Text = b.ClassName, Value = b.ClassId.ToString() }).ToList();
                return Json(objClass);
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



        public JsonResult GetStudentMapData(int Id)
        {
            try
            {
                var obj = (from a in _context.TblstudentSubjectBatch
                           where a.Id == Id
                           select new
                           {
                               studentName = (from b in _context.TblStudent
                                              where b.StudentId == a.StudentId
                                              select new
                                                 SelectListItem()
                                              { Text = (b.Fname + " " + b.Lname), Value = b.StudentId.ToString() }).ToList(),

                               className = (
                                            from b in _context.TblClass
                                            join c in _context.TblStudent on b.ClassId equals c.ClassId
                                          where a.StudentId==c.StudentId
                                           select new
                                              SelectListItem()
                                           { Text = b.ClassName, Value = b.ClassId.ToString() }).ToList(),

                               PlanType = (from b in _context.TblDuration
                                           where b.Durationid == a.DurationId
                                           select new
                                              SelectListItem()
                                           { Text = b.Durationname, Value = b.Durationid.ToString() }).ToList(),
                               SubjectName = (from b in _context.Tblsubject
                                              where b.Subjectid == a.SubjectId
                                              select new
                                                 SelectListItem()
                                              { Text = b.Subjectname, Value = b.Subjectid.ToString() }).ToList(),
                               BatchName = (from b in _context.TblBatch
                                            where b.BatchId == a.BatchId
                                            select new
                                               SelectListItem()
                                            { Text = b.BatchName, Value = b.BatchId.ToString() }).ToList(),
                              period=a.Period
                              



                           }).ToList();

                return Json(obj);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

        }


    }
}