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
using System.Web;
using System.Net.Mail;
using System.Net;

namespace OnlineClass.Controllers
{
    public class GetInTouchByMailController : Controller
    {

        private readonly OnlineclassContext _context;
      
        public GetInTouchByMailController(OnlineclassContext context)
        {
            _context = context;
           
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ContactUsByMail()
        {
            
           
             var userlogin = (from m in _context.Tbllogin where m.UserName == HttpContext.Session.GetString("name") select m).FirstOrDefault();
            var teacher = (from m in _context.Tblteacher where m.TeacherId == userlogin.Userid select m).FirstOrDefault();
            if(teacher == null)
            {
                var student = (from m in _context.TblStudent where m.StudentId == userlogin.Userid select m).FirstOrDefault();
                ViewBag.user = student.Fname + " " + student.Lname;
            }
            else
            {
                ViewBag.user = teacher.Fname + " " + teacher.Lname;
            }
     
            // var GotList ;
            string Email_id = null;
            int senderId=0;
            Int64 Teachid = 0;
            Int64 stuId = 0;
            int RecieverTypeId=0;

        
            if (userlogin.Typeid==3)  ///////////Teacher
            {
                var teacherList = (from n in _context.Tblteacher where n.TeacherId == userlogin.Userid select n).FirstOrDefault();
                ViewBag.senderMail = teacherList.Email;
                Email_id= teacherList.Email;
                Teachid = teacherList.TeacherId;
                stuId = teacherList.TeacherId;
                senderId = 3;
                RecieverTypeId = 2;

                ViewBag.type = userlogin.Typeid;

                var studentlist = (from a in _context.Tblstudentteacher
                                   join b in _context.TblStudent on a.Studentid equals b.StudentId
                                   where a.Teacherid == Teachid
                                   select new StudentTeacherList
                                   {
                                      TeacherId= b.StudentId,
                                      Email= b.Email,
                                       Name = b.Fname + " " + b.Lname + " (" + b.Email + "(Student))"
                                   }).ToList();


                ViewBag.ReciepintList = new SelectList(studentlist, "TeacherId", "Name");
               

            }
            if (userlogin.Typeid == 1)  ///////////Admin
            {
                var teacherList = (from n in _context.Tblteacher where n.TeacherId == userlogin.Userid select n).FirstOrDefault();
                ViewBag.senderMail = teacherList.Email;
                Email_id = teacherList.Email;
                Teachid = teacherList.TeacherId;
                senderId = 1;
                RecieverTypeId = 2;

                ViewBag.type = userlogin.Typeid;
                var StudentTeacherlist = (from a in _context.Tblteacher
                                                                     select new StudentTeacherList
                                   {
                                                                         TeacherId= a.TeacherId,
                                                                         Email= a.Email,
                                      Name= a.Fname+ " "+a.Lname+" ("+a.Email+ "(Teacher))"
                                   }).ToList().Union((from a in _context.TblStudent
                                                      select new StudentTeacherList
                                                      {
                                                          TeacherId = Convert.ToInt64( a.StudentId),
                                                          Email = a.Email,
                                                          Name = a.Fname + " " + a.Lname + " (" + a.Email + "(Student))"
                                                      }).ToList()).OrderBy(x=>x.Name);

                ViewBag.ReciepintList = new SelectList(StudentTeacherlist, "Email", "Name");

            }

            if (userlogin.Typeid==2) ////Student
            {
                var studentlist = (from n in _context.TblStudent where n.StudentId == userlogin.Userid select n).FirstOrDefault();
                ViewBag.senderMail = studentlist.Email;
                
                Email_id = studentlist.Email;
                stuId = studentlist.StudentId;
                senderId = 2;
                RecieverTypeId = 3;
                ViewBag.type = userlogin.Typeid;
               // ViewBag.ReciepintList = new SelectList(_context.Tblteacher, "TeacherId", "Fname");
                var teacherlist = (from a in _context.Tblstudentteacher
                                   join b in _context.Tblteacher on a.Teacherid equals b.TeacherId
                                   where a.Studentid == stuId
                                   select new StudentTeacherList
                                   {
                                       TeacherId=b.TeacherId,
                                       Email= b.Email,
                                        Name = b.Fname + " " + b.Lname + " (" + b.Email + "(Teacher))"
                                   }).ToList();


                ViewBag.ReciepintList = new SelectList(teacherlist, "TeacherId", "Name");


            }
           
            var mailHistory = (from a in _context.TblEmailSend
                               join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                               //join c in _context.TblStudent on a.StudentId equals c.StudentId
                               join c in _context.TblStudent on a.StudentId equals c.StudentId into ps
                               from c in ps.DefaultIfEmpty()
                               where a.StudentId == stuId || a.TeacherId==Teachid
                               select new contactByMail
                               {
                                   MailId = a.MailId,
                                   SenderMailId=a.SenderMailId,
                                   SenderTypeId=a.SenderTypeId,
                                   RecieverTypeId=a.RecieverTypeId,
                                   SendDateTime = a.SendDateTime,
                                   RecieverMailId = a.RecieverMailId,
                                   Subject = a.Subject,
                                   Body = a.Body,
                                   Teachername= b.Fname + " " + b.Lname,
                                   StudentName = c.Fname + " " + c.Lname,
                                   TeacherId=a.TeacherId,
                                   StudentId =a.StudentId
                               }).ToList().Union((from a in _context.TblEmailSend
                                                  join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                                 
                                                  where a.TeacherId == Teachid
                                                  select new contactByMail
                                                  {
                                                      MailId = a.MailId,
                                                      SenderMailId = a.SenderMailId,
                                                      SenderTypeId = a.SenderTypeId,
                                                      RecieverTypeId = a.RecieverTypeId,
                                                      SendDateTime = a.SendDateTime,
                                                      RecieverMailId = a.RecieverMailId,
                                                      Subject = a.Subject,
                                                      Body = a.Body,
                                                      Teachername = b.Fname + " " + b.Lname,
                                                      StudentName = null,
                                                      TeacherId = a.TeacherId,
                                                      StudentId = a.StudentId
                                                  }).ToList());

            List<MailIterate> maillisting = null;
            List<MailIterate> maillistingfinal = new List<MailIterate>();
            string recieverlist = "";
            int i = 0;
            int pos;
            foreach (var item in mailHistory)
            {

                if (userlogin.Typeid == 1)
                {
                    string[] strArray = recieverlist.Split(';');
                    //string[] stringArray = { "text1", "text2", "text3", "text4" };
                    string value = item.StudentId.ToString();
                     pos = Array.IndexOf(strArray, value);
                }
                else if(userlogin.Typeid == 2)
                {
                    string[] strArray = recieverlist.Split(';');
                    //string[] stringArray = { "text1", "text2", "text3", "text4" };
                    string value = item.TeacherId.ToString();
                     pos = Array.IndexOf(strArray, value);
                }
                else
                {
                    string[] strArray = recieverlist.Split(';');
                    //string[] stringArray = { "text1", "text2", "text3", "text4" };
                    string value = item.StudentId.ToString()+"_"+ item.TeacherId.ToString();
                    string value1 = item.TeacherId.ToString() + "_" + item.StudentId.ToString();
                    
                   int pos1 = Array.IndexOf(strArray, value);
                  int  pos2 = Array.IndexOf(strArray, value1);
                    if(pos1==0 && pos2 == -1)
                    {
                        pos = 0;
                    }
                    else if(pos1 == -1 && pos2 == 0)
                    {
                        pos = 0;
                    }
                    else if(pos1 == -1 && pos2 == -1)
                    {
                        pos = -1;
                    }
                    else
                    {
                        pos = 0;
                    }
                }
                
                if(pos == -1)
                {
                    if (userlogin.Typeid == 1)
                    {
                        var maillisting1 = (from a in _context.TblEmailSend
                                            join b in _context.Tblteacher on Convert.ToInt64(a.StudentId) equals b.TeacherId

                                            where a.TeacherId == item.TeacherId && a.StudentId == item.StudentId && a.Isadmin==true
                                            select new MailIterate
                                            {
                                                TeacherName = b.Fname + " " + b.Lname,
                                                SendEmail = a.SenderMailId,
                                                RecieveEmail = a.RecieverMailId,
                                                StudentName = null,

                                            }).Union((from a in _context.TblEmailSend

                                                               join c in _context.TblStudent on a.StudentId equals c.StudentId
                                                               where a.TeacherId == item.TeacherId && a.StudentId == item.StudentId && a.Isadmin == false
                                                      select new MailIterate
                                                               {
                                                                   TeacherName = null,
                                                                   SendEmail = a.SenderMailId,
                                                                   RecieveEmail = a.RecieverMailId,
                                                                   StudentName = c.Fname + " " + c.Lname,

                                                               })).FirstOrDefault();
                        maillistingfinal.Add(maillisting1);
                    }
                    else if(userlogin.Typeid == 3)
                    {

                        var maillisting1 = (from a in _context.TblEmailSend
                                            join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                            join c in _context.TblStudent on a.StudentId equals c.StudentId
                                           // join c in _context.TblStudent on a.StudentId equals c.StudentId into ps
                                            //from c in ps.DefaultIfEmpty()
                                            where a.TeacherId == item.TeacherId && a.StudentId == item.StudentId && a.Isadmin == false
                                            select new MailIterate
                                            {
                                                TeacherName = b.Fname + " " + b.Lname,
                                                SendEmail = a.SenderMailId,
                                                RecieveEmail = a.RecieverMailId,
                                                StudentName = c.Fname + " " + c.Lname,

                                            }).Union(from a in _context.TblEmailSend
                                                     join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                                     //join c in _context.TblStudent on a.StudentId equals c.StudentId
                                                     // join c in _context.TblStudent on a.StudentId equals c.StudentId into ps
                                                     //from c in ps.DefaultIfEmpty()
                                                     where a.TeacherId == item.TeacherId && a.StudentId == item.StudentId && a.Isadmin == true
                                                     select new MailIterate
                                                     {
                                                         TeacherName = b.Fname + " " + b.Lname,
                                                         SendEmail = a.SenderMailId,
                                                         RecieveEmail = a.RecieverMailId,
                                                         StudentName = b.Fname + " " + b.Lname,

                                                     }).FirstOrDefault();
                        maillistingfinal.Add(maillisting1);
                    }
                    else
                    {
                        var maillisting1 = (from a in _context.TblEmailSend
                                            join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                            join c in _context.TblStudent on a.StudentId equals c.StudentId
                                            where a.TeacherId == item.TeacherId && a.StudentId == item.StudentId && a.Isadmin == false
                                            select new MailIterate
                                            {
                                                TeacherName = b.Fname + " " + b.Lname,
                                                SendEmail = a.SenderMailId,
                                                RecieveEmail = a.RecieverMailId,
                                                StudentName = c.Fname + " " + c.Lname,

                                            }).FirstOrDefault();
                        maillistingfinal.Add(maillisting1);
                    }
                    
                }
                if (userlogin.Typeid == 1)
                {
                    if (recieverlist == "")
                    {

                        recieverlist = item.StudentId.ToString();
                    }
                    else
                    {
                        recieverlist = recieverlist + ";" + item.StudentId.ToString();
                    }
                }
                else if(userlogin.Typeid == 2)
                {
                    if (recieverlist == "")
                    {

                        recieverlist = item.TeacherId.ToString();
                    }
                    else
                    {
                        recieverlist = recieverlist + ";" + item.TeacherId.ToString();
                    }
                }
                else if (userlogin.Typeid == 3)
                {
                    if (recieverlist == "")
                    {

                        recieverlist = item.StudentId.ToString()+"_"+ item.TeacherId.ToString();
                    }
                    else
                    {
                        recieverlist = recieverlist + ";" + item.StudentId.ToString() + "_" + item.TeacherId.ToString();
                    }
                }
                i++;
            }
          
            ViewBag.SentHistory = mailHistory;
            ViewBag.listing = maillistingfinal;
            ViewBag.studentid = stuId;
            ViewBag.teacherid = Teachid;

            TempData["teacherid"] = Teachid;
            TempData.Keep("teacherid");
            TempData["studentid"] = stuId;
            TempData.Keep("studentid");


            ViewBag.senderType = senderId;
            ViewBag.RecieverTypeId = RecieverTypeId;



            return View();

        }




        //public IActionResult ContactUsByMailz()
        //{


        //    var userlogin = (from m in _context.Tbllogin where m.UserName == HttpContext.Session.GetString("name") select m).FirstOrDefault();
        //    // var GotList ;
        //    string Email_id = null;
        //    int senderId = 0;
        //    Int64 Teachid = 0;
        //    Int64 stuId = 0;
        //    int RecieverTypeId = 0;
        //    Int64 User_id = 0;
          
        //    if (userlogin.Typeid == 3)  ///////////Teacher
        //    {
        //        var teacherList = (from n in _context.Tblteacher where n.TeacherId == userlogin.Userid select n).FirstOrDefault();
        //        User_id = teacherList.TeacherId;

        //        ViewBag.senderMail = teacherList.Email;
        //        Email_id = teacherList.Email;
        //        Teachid = teacherList.TeacherId;
        //        senderId = 3;
        //        RecieverTypeId = 2;

        //        ViewBag.type = userlogin.Typeid;
        //        ViewBag.ReciepintList = new SelectList(_context.TblStudent, "StudentId", "Fname");

        //    }
        //    if (userlogin.Typeid == 1)  ///////////Admin
        //    {
        //        var teacherList = (from n in _context.Tblteacher where n.TeacherId == userlogin.Userid select n).FirstOrDefault();
        //        User_id = teacherList.TeacherId;
        //        ViewBag.senderMail = teacherList.Email;
        //        Email_id = teacherList.Email;
        //        Teachid = teacherList.TeacherId;
        //        senderId = 3;
        //        RecieverTypeId = 2;

        //        ViewBag.type = userlogin.Typeid;
        //        ViewBag.ReciepintList = new SelectList(_context.TblStudent, "StudentId", "Fname");

        //    }

        //    if (userlogin.Typeid == 2) ////Student
        //    {
        //        var studentlist = (from n in _context.TblStudent where n.StudentId == userlogin.Userid select n).FirstOrDefault();
        //        User_id = studentlist.StudentId;
        //        ViewBag.senderMail = studentlist.Email;
        //        Email_id = studentlist.Email;
        //        stuId = studentlist.StudentId;
        //        senderId = 2;
        //        RecieverTypeId = 3;
        //        ViewBag.type = userlogin.Typeid;
        //        ViewBag.ReciepintList = new SelectList(_context.Tblteacher, "TeacherId", "Fname");
               


        //    }

        //    var mailHistory = (from a in _context.TblContactByMail
        //                            join c in _context.TblStudent on a.UserId equals c.StudentId
        //                            join d in _context.Tblstudentteacher on c.StudentId equals d.Studentid
        //                       join b in _context.Tblteacher on d.Teacherid equals b.TeacherId
        //                       where a.UserId == User_id || a.UserId ==d.Teacherid 
        //                       select new ContactbyMailModel
        //                       {
        //                           Id = a.Id,
        //                           UserId=a.UserId,
        //                           EmailId=a.EmailId,   
        //                           Subject = a.Subject,
        //                           Body = a.Body,
                                   
                                
        //                       }).ToList();

        //    List<MailIterate> maillisting = null;
        //    List<MailIterate> maillistingfinal = new List<MailIterate>();
        //    string recieverlist = "";
        //    int i = 0;
        //    foreach (var item in mailHistory)
        //    {


        //        string[] strArray = recieverlist.Split(';');
        //        //string[] stringArray = { "text1", "text2", "text3", "text4" };
        //        string value = item.EmailId;
        //        int pos = Array.IndexOf(strArray, value);
        //        if (pos == -1)
        //        {
        //            var maillisting1 = (from a in _context.TblContactByMail
        //                                join d in _context.Tblstudentteacher on a.UserId equals d.Studentid
        //                                join b in _context.Tblteacher on d.Teacherid equals b.TeacherId
        //                                where (a.EmailId == b.Email && a.UserId== d.Teacherid) || (a.EmailId == item.EmailId && a.UserId == User_id)
        //                                select new MailIterate
        //                                {
        //                                    SenderUserid=a.UserId,
        //                                    TeacherName = b.Fname + " " + b.Lname,
        //                                    SendEmail = a.EmailId,
        //                                    Body=a.Body,
        //                                    Subject=a.Subject,
        //                                    teacherid=b.TeacherId
        //                                    //RecieveEmail = a.IsSent == false ? a.EmailId : null,
                                           
        //                                }).FirstOrDefault();

        //            maillistingfinal.Add(maillisting1);
        //        }

        //        if (recieverlist == "")
        //        {
        //            recieverlist = item.EmailId;
        //        }
        //        else
        //        {
        //            recieverlist = recieverlist + ";" + item.EmailId;
        //        }
        //        i++;
        //    }

        //    ViewBag.SentHistory = mailHistory;
        //    ViewBag.listing = maillistingfinal;
        //    ViewBag.studentid = stuId;
        //    ViewBag.teacherid = Teachid;
        //    ViewBag.senderType = senderId;
        //    ViewBag.RecieverTypeId = RecieverTypeId;
        //    ViewBag.userId = User_id;
        //    return View();

        //}

        [HttpPost]
        public IActionResult SendMailtoTeacher(contactByMail contactByMailobj)
        {
            try
            {

                var userlogin = (from m in _context.Tbllogin where m.UserName == HttpContext.Session.GetString("name") select m).FirstOrDefault();
                ViewBag.user = HttpContext.Session.GetString("name");

                if (userlogin.Typeid == 3 )  ///////////Teacher
                {
                    var studentemail = (from a in _context.TblStudent where a.StudentId == contactByMailobj.StudentId select a.Email).FirstOrDefault();
                    var teachermail = (from a in _context.Tblteacher where a.TeacherId == userlogin.Userid select a).FirstOrDefault();
                    contactByMailobj.RecieverMailId = studentemail;
                    contactByMailobj.SenderMailId = teachermail.Email;
                    contactByMailobj.TeacherId = teachermail.TeacherId;
                    contactByMailobj.SenderTypeId = userlogin.Typeid;
                    contactByMailobj.RecieverTypeId = 2;
                    contactByMailobj.Isadmin = false;


                }
                if ( userlogin.Typeid == 1)  /////////// Super Admin
                {
                   // string studentemail = (from a in _context.TblStudent where a.StudentId == contactByMailobj.StudentId select a.Email).FirstOrDefault();
                    var teachermailsender = (from a in _context.Tblteacher where a.TeacherId == userlogin.Userid select a).FirstOrDefault();

                    var studentemail = (from a in _context.TblStudent where a.Email == contactByMailobj.TeacherOrStudentMailid select a).FirstOrDefault();
                    var teachermailreciever = (from a in _context.Tblteacher where a.Email == contactByMailobj.TeacherOrStudentMailid select a).FirstOrDefault();
                    if (studentemail != null)  //for reciever student
                    {
                        contactByMailobj.RecieverMailId = studentemail.Email;
                        contactByMailobj.StudentId = studentemail.StudentId;
                        contactByMailobj.RecieverTypeId = 2;
                        contactByMailobj.SenderTypeId = 1;
                        contactByMailobj.Isadmin = false;
                    }
                    else if (teachermailreciever != null)
                    {
                        contactByMailobj.RecieverMailId = teachermailreciever.Email;
                        contactByMailobj.StudentId = Convert.ToInt32(teachermailreciever.TeacherId);
                        contactByMailobj.RecieverTypeId = 3;
                        contactByMailobj.SenderTypeId = 1;
                        contactByMailobj.Isadmin = true;

                    }
                    
                


                    contactByMailobj.SenderMailId = teachermailsender.Email;
                    contactByMailobj.TeacherId = teachermailsender.TeacherId;
                    


                }

                if (userlogin.Typeid == 2)  ///////////Student
                {
                    var teacherEmail = (from a in _context.Tblteacher where a.TeacherId == contactByMailobj.TeacherId select a).FirstOrDefault();
                    var studentemail = (from a in _context.TblStudent where a.StudentId == userlogin.Userid select a).FirstOrDefault();
                    contactByMailobj.RecieverMailId = teacherEmail.Email;
                    contactByMailobj.SenderMailId = studentemail.Email;
                    contactByMailobj.StudentId = studentemail.StudentId;
                    contactByMailobj.TeacherId = teacherEmail.TeacherId;
                    contactByMailobj.SenderTypeId = userlogin.Typeid;
                    contactByMailobj.RecieverTypeId = 3;
                    contactByMailobj.Isadmin = false;

                }
               

                bool response = MailSend(contactByMailobj);
                TblEmailSend tblEmailSend = new TblEmailSend();
                
                if (response == true)
                {
                    //tblContactByMail.Body = contactByMailobj.Body;
                    //tblContactByMail.Subject = contactByMailobj.Subject;
                    //tblContactByMail.IsSent = true;
                    //tblContactByMail.UserId = contactByMailobj.SenderUserId;  //SenserUserId
                    //tblContactByMail.Body = contactByMailobj.Body;
                    //tblContactByMail.UserTypeId = userlogin.Typeid;           //SenderTypeId
                    //tblContactByMail.EmailId = contactByMailobj.EmailId;  //Reciever EmailId
                    //tblContactByMail.SendDateTime = DateTime.Now;

                    tblEmailSend.RecieverMailId = contactByMailobj.RecieverMailId;
                    tblEmailSend.RecieverTypeId = contactByMailobj.RecieverTypeId;
                    tblEmailSend.SenderMailId = contactByMailobj.SenderMailId;
                    tblEmailSend.SenderTypeId = contactByMailobj.SenderTypeId;
                    tblEmailSend.StudentId = contactByMailobj.StudentId;
                    tblEmailSend.TeacherId = contactByMailobj.TeacherId;
                    tblEmailSend.Body = contactByMailobj.Body;
                    tblEmailSend.Subject = contactByMailobj.Subject;
                    tblEmailSend.Isadmin = contactByMailobj.Isadmin;
                    tblEmailSend.SendDateTime = DateTime.Now;




                    _context.TblEmailSend.Add(tblEmailSend);
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {

            }
            return RedirectToAction("ContactUsByMail", "GetInTouchByMail", new { });
        }

        public static bool MailSend(contactByMail sendCredential)
        {
            try
            {
                using (MailMessage mm = new MailMessage(sendCredential.SenderMailId, sendCredential.RecieverMailId))
                {
                    mm.Subject = sendCredential.Subject;
                    mm.Body = sendCredential.Body;
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("karmickmail4test@gmail.com", "karmick@123");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public IActionResult CommunicationWindow( string senderEmail,string recieverEmail)
        {
            
            var userlogin = (from m in _context.Tbllogin where m.UserName == HttpContext.Session.GetString("name") select m).FirstOrDefault();
            ViewBag.user = HttpContext.Session.GetString("name");
            List<contactByMail> contactByMails = new List<contactByMail>();

            List<contactByMail> sendalllist = new List<contactByMail>();
            List<contactByMail> recievealllist = new List<contactByMail>();

            /////////// Super Admin/////////////////////
            if (userlogin.Typeid == 1)  
            {
                var objEmailDetailSend = (from a in _context.TblEmailSend
                                          //join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                         join c in _context.TblStudent on a.RecieverMailId equals c.Email

                                          //join c in _context.TblStudent on a.RecieverMailId equals c.Email into ps
                                          //from c in ps.DefaultIfEmpty()
                                              //                                          join d in _context.TblEmailSend on senderEmail equals d.SenderMailId
                                          where a.SenderMailId == senderEmail && a.RecieverMailId == recieverEmail
                                          select new contactByMail
                                          {
                                              MailId = a.MailId,
                                              StudentName = c.Fname + " " + c.Lname,
                                              Teachername =null,
                                              Body = a.Body,
                                              Subject = a.Subject,
                                              SendDateTime = a.SendDateTime,
                                              SenderMailId = a.SenderMailId,
                                              RecieverMailId = a.RecieverMailId,
                                              IsSent = true

                                          }
                                ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());

                var objEmailDetailSendTeacher = (from a in _context.TblEmailSend
                                          join c in _context.Tblteacher on a.RecieverMailId equals c.Email 
                                          where a.SenderMailId == senderEmail && a.RecieverMailId == recieverEmail && a.Isadmin==true
                                          select new contactByMail
                                          {
                                              MailId = a.MailId,
                                              StudentName = c.Fname + " " + c.Lname,
                                              Teachername = null,
                                              Body = a.Body,
                                              Subject = a.Subject,
                                              SendDateTime = a.SendDateTime,
                                              SenderMailId = a.SenderMailId,
                                              RecieverMailId = a.RecieverMailId,
                                              IsSent = true

                                          }
                                ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());
                  sendalllist = objEmailDetailSend.Union(objEmailDetailSendTeacher).ToList();
                var objEmailDetailRecieve = (from a in _context.TblEmailSend
                                             join b in _context.Tblteacher on a.SenderMailId equals b.Email
                                            // join c in _context.TblStudent on a.SenderMailId equals c.Email
                                             where a.SenderMailId == recieverEmail && a.RecieverMailId == senderEmail && a.Isadmin==true
                                             select new contactByMail
                                             {
                                                 MailId = a.MailId,
                                                 StudentName = null,
                                                 Teachername = b.Fname + " " + b.Lname,
                                                 Body = a.Body,
                                                 Subject = a.Subject,
                                                 SendDateTime = a.SendDateTime,
                                                 SenderMailId = a.SenderMailId,
                                                 RecieverMailId = a.RecieverMailId,
                                                 IsSent = false

                                             }
                                  ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());
                var objEmailDetailStudentRecieve = (from a in _context.TblEmailSend
                                             //join b in _context.Tblteacher on a.SenderMailId equals b.Email
                                             join c in _context.TblStudent on a.SenderMailId equals c.Email
                                             where a.SenderMailId == recieverEmail && a.RecieverMailId == senderEmail && a.Isadmin==false
                                             select new contactByMail
                                             {
                                                 MailId = a.MailId,
                                                 StudentName = c.Fname + " " + c.Lname,
                                                 Teachername =null,// b.Fname + " " + b.Lname,
                                                 Body = a.Body,
                                                 Subject = a.Subject,
                                                 SendDateTime = a.SendDateTime,
                                                 SenderMailId = a.SenderMailId,
                                                 RecieverMailId = a.RecieverMailId,
                                                 IsSent = false

                                             }
                                 ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());
                recievealllist = objEmailDetailRecieve.Union(objEmailDetailStudentRecieve).ToList();
                if (sendalllist.Any() == true && recievealllist.Any() == true)
                {
                    contactByMails = sendalllist.Union(recievealllist).ToList();
                    //ViewBag.RecieverMailId = (from a in contactByMails select a).FirstOrDefault().RecieverMailId;
                }
                else if (sendalllist.Any() == true)
                {
                    contactByMails = sendalllist.ToList();
                    //ViewBag.RecieverMailId = (from a in contactByMails select a).FirstOrDefault().RecieverMailId;
                }
                else if (recievealllist.Any() == true)
                {
                    contactByMails = recievealllist.ToList();
                    //ViewBag.RecieverMailId = (from a in contactByMails select a).FirstOrDefault().RecieverMailId;
                }
               

                

            }
            else if (userlogin.Typeid == 3)
            {
                ////////////////////////For Teacher///////////////////////////////////
                var objEmailDetailSend = (from a in _context.TblEmailSend
                                          join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                          join c in _context.TblStudent on a.StudentId equals c.StudentId
                                          where a.SenderMailId == senderEmail && a.RecieverMailId == recieverEmail && a.Isadmin==false
                                          select new contactByMail
                                          {
                                              MailId = a.MailId,
                                              StudentName = c.Fname + " " + c.Lname,
                                              Teachername = b.Fname + " " + b.Lname,
                                              Body = a.Body,
                                              Subject = a.Subject,
                                              SendDateTime = a.SendDateTime,
                                              SenderMailId = a.SenderMailId,
                                              RecieverMailId = a.RecieverMailId,
                                              IsSent = true,
                                              TeacherId=b.TeacherId

                                          }
                            ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());


                var objTeacherAdminsend=(from a in _context.TblEmailSend
                                     join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                    // join c in _context.TblStudent on a.StudentId equals c.StudentId
                                     where a.SenderMailId == recieverEmail  && a.RecieverMailId == senderEmail && a.Isadmin==true
                                     select new contactByMail
                                     {
                                         MailId = a.MailId,
                                         StudentName = null,
                                         Teachername = b.Fname + " " + b.Lname,
                                         Body = a.Body,
                                         Subject = a.Subject,
                                         SendDateTime = a.SendDateTime,
                                         SenderMailId = a.SenderMailId,
                                         RecieverMailId = a.RecieverMailId,
                                         IsSent = true,
                                         TeacherId = b.TeacherId

                                     }
                            ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());


                sendalllist = objEmailDetailSend.Union(objTeacherAdminsend).ToList();




                var objEmailDetailRecieve = (from a in _context.TblEmailSend
                                             join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                             join c in _context.TblStudent on a.StudentId equals c.StudentId
                                             where a.SenderMailId == recieverEmail && a.RecieverMailId == senderEmail && a.Isadmin==false
                                             select new contactByMail
                                             {
                                                 MailId = a.MailId,
                                                 StudentName = c.Fname + " " + c.Lname,
                                                 Teachername = b.Fname + " " + b.Lname,
                                                 Body = a.Body,
                                                 Subject = a.Subject,
                                                 SendDateTime = a.SendDateTime,
                                                 SenderMailId = a.SenderMailId,
                                                 RecieverMailId = a.RecieverMailId,
                                                 IsSent = false,
                                                 TeacherId = b.TeacherId

                                             }
                                  ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());



                var objTeacherAdminRecieve = (from a in _context.TblEmailSend
                                             join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                            // join c in _context.TblStudent on a.StudentId equals c.StudentId
                                             where a.SenderMailId == senderEmail && a.RecieverMailId == recieverEmail && a.Isadmin==true
                                             select new contactByMail
                                             {
                                                 MailId = a.MailId,
                                                 StudentName = null,
                                                 Teachername = b.Fname + " " + b.Lname,
                                                 Body = a.Body,
                                                 Subject = a.Subject,
                                                 SendDateTime = a.SendDateTime,
                                                 SenderMailId = a.SenderMailId,
                                                 RecieverMailId = a.RecieverMailId,
                                                 IsSent = false,
                                                 TeacherId = b.TeacherId
                                             }
                                  ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());


                recievealllist = objEmailDetailRecieve.Union(objTeacherAdminRecieve).ToList();


                if (sendalllist.Any() == true && recievealllist.Any() == true)
                {
                    contactByMails = sendalllist.Union(recievealllist).ToList();

                }
                else if (sendalllist.Any() == true)
                {
                    contactByMails = sendalllist.ToList();
                }
                else if (recievealllist.Any() == true)
                {
                    contactByMails = recievealllist.ToList();
                }


            }
            else
            {  ////////////////////////For Student///////////////////////////////////

                var objEmailDetailSend = (from a in _context.TblEmailSend
                                          join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                          join c in _context.TblStudent on a.StudentId equals c.StudentId
                                          where a.SenderMailId == senderEmail && a.RecieverMailId == recieverEmail
                                          select new contactByMail
                                          {
                                              MailId = a.MailId,
                                              StudentName = c.Fname + " " + c.Lname,
                                              Teachername = b.Fname + " " + b.Lname,
                                              Body = a.Body,
                                              Subject = a.Subject,
                                              SendDateTime = a.SendDateTime,
                                              SenderMailId = a.SenderMailId,
                                              RecieverMailId = a.RecieverMailId,
                                              IsSent = true

                                          }
                                ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());

                var objEmailDetailRecieve = (from a in _context.TblEmailSend
                                             join b in _context.Tblteacher on a.TeacherId equals b.TeacherId
                                             join c in _context.TblStudent on a.StudentId equals c.StudentId
                                             where a.SenderMailId == recieverEmail && a.RecieverMailId == senderEmail
                                             select new contactByMail
                                             {
                                                 MailId = a.MailId,
                                                 StudentName = c.Fname + " " + c.Lname,
                                                 Teachername = b.Fname + " " + b.Lname,
                                                 Body = a.Body,
                                                 Subject = a.Subject,
                                                 SendDateTime = a.SendDateTime,
                                                 SenderMailId = a.SenderMailId,
                                                 RecieverMailId = a.RecieverMailId,
                                                 IsSent = false

                                             }
                                  ).ToList().OrderByDescending(a => a.SendDateTime.Value.ToShortTimeString());

                if (objEmailDetailSend.Any() == true && objEmailDetailRecieve.Any() == true)
                {
                    contactByMails = objEmailDetailSend.Union(objEmailDetailRecieve).ToList();

                }
                else if (objEmailDetailSend.Any() == true)
                {
                    contactByMails = objEmailDetailSend.ToList();
                }
                else if (objEmailDetailRecieve.Any() == true)
                {
                    contactByMails = objEmailDetailRecieve.ToList();
                }


            }


            string Email_id = null;
            Int64 senderId = 0;
            Int64 Teachid = 0;
            Int64 stuId = 0;
            if (userlogin.Typeid == 3 )  ///////////Teacher
            {
                var teacherList = (from n in _context.Tblteacher where n.TeacherId == userlogin.Userid select n).FirstOrDefault();
                Email_id = teacherList.Email;
                senderId = teacherList.TeacherId;
                ViewBag.Teacherid = teacherList.TeacherId;
                ViewBag.studentid = senderId;
                ViewBag.senderType = 3;
                ViewBag.RecieverTypeId = 2;
                if((from m in contactByMails select m).FirstOrDefault().TeacherId == 1)
                {
                    ViewBag.RecieverMailId = senderEmail;
                }
                else
                {
                    ViewBag.RecieverMailId = recieverEmail;
                }
               

            }

            if (userlogin.Typeid == 1)  ///////////Admin
            {
                var teacherList = (from n in _context.Tblteacher where n.TeacherId == userlogin.Userid select n).FirstOrDefault();
                Email_id = teacherList.Email;
                senderId = teacherList.TeacherId;
                ViewBag.Teacherid = teacherList.TeacherId;
                ViewBag.studentid = senderId;
                ViewBag.senderType = 1;
                ViewBag.RecieverTypeId = 2;
                ViewBag.RecieverMailId = recieverEmail;

            }

            if (userlogin.Typeid == 2)  ///////////Student
            {
                var studentlist = (from n in _context.TblStudent where n.StudentId == userlogin.Userid select n).FirstOrDefault();
                
                Email_id = studentlist.Email;
                senderId = studentlist.StudentId;
                //ViewBag.Teacherid = objEmailReciever.TeacherId;
                ViewBag.studentid = senderId;
                ViewBag.senderType = 2;
                ViewBag.RecieverTypeId = 3;
                ViewBag.RecieverMailId = senderEmail;

            }


            ViewBag.type = userlogin.Typeid;
            ViewBag.Detail = contactByMails;

            return View();
        }

        [HttpPost]
        public IActionResult SubmitReplyMail(contactByMail ReplyObj)
        {
            try
            {
              
                var userlogin = (from m in _context.Tbllogin where m.UserName == HttpContext.Session.GetString("name") select m).FirstOrDefault();
                ViewBag.user = HttpContext.Session.GetString("name");
                ViewBag.type = userlogin.Typeid;
                if (userlogin.Typeid == 3 )  ///////////Teacher
                {
                    var teacherList = (from n in _context.Tblteacher where n.TeacherId == userlogin.Userid select n).FirstOrDefault();
                    var studentlist = (from n in _context.TblStudent where n.Email == ReplyObj.RecieverMailId select n).FirstOrDefault();
                    var AdminListrecieve = (from n in _context.Tblteacher where n.Email == ReplyObj.RecieverMailId select n).FirstOrDefault();
                    if (studentlist != null)    /////////////when reciever is Student
                    {
                        ReplyObj.SenderMailId = teacherList.Email;
                        ReplyObj.StudentId = studentlist.StudentId;
                        ReplyObj.TeacherId = teacherList.TeacherId;

                        ReplyObj.SenderTypeId = userlogin.Typeid;
                        ReplyObj.RecieverTypeId = 2;
                        ReplyObj.Isadmin = false;

                    }
                    else if (AdminListrecieve != null)  /////////////when reciever is Teacher
                    {
                        ReplyObj.SenderMailId = teacherList.Email;
                        ReplyObj.StudentId =Convert.ToInt32(AdminListrecieve.TeacherId);
                        ReplyObj.TeacherId = teacherList.TeacherId;

                        ReplyObj.SenderTypeId = userlogin.Typeid;
                        ReplyObj.RecieverTypeId = 1;
                        ReplyObj.Isadmin = true;


                    }


                }

                if (userlogin.Typeid == 1)  ///////////Admin
                {
                    var teacherList = (from n in _context.Tblteacher where n.TeacherId == userlogin.Userid select n).FirstOrDefault();
                    var studentlistrecieve = (from n in _context.TblStudent where n.Email == ReplyObj.RecieverMailId select n).FirstOrDefault();
                    var teacherListrecieve = (from n in _context.Tblteacher where n.Email == ReplyObj.RecieverMailId select n).FirstOrDefault();
                    if(studentlistrecieve!=null)    /////////////when reciever is Student
                    {
                        ReplyObj.StudentId = studentlistrecieve.StudentId;
                        ReplyObj.RecieverTypeId = 2;
                        ReplyObj.Isadmin = false;
                    }
                    else if(teacherListrecieve!=null)  /////////////when reciever is Teacher
                    {
                        ReplyObj.StudentId = Convert.ToInt32(teacherListrecieve.TeacherId);
                        ReplyObj.RecieverTypeId = 3;
                        ReplyObj.Isadmin = true;

                    }
                    ReplyObj.SenderMailId = teacherList.Email;
                   
                    ReplyObj.TeacherId = teacherList.TeacherId;

                    ReplyObj.SenderTypeId = userlogin.Typeid;
                    


                }

                if (userlogin.Typeid == 2)  ///////////Student
                {
                    var studentlist = (from n in _context.TblStudent where n.StudentId == userlogin.Userid select n).FirstOrDefault();
                    var teacherList = (from n in _context.Tblteacher where n.Email == ReplyObj.RecieverMailId select n).FirstOrDefault();
                    ReplyObj.SenderMailId = studentlist.Email;
                    //ReplyObj.RecieverMailId = Convert.ToString(TempData.Peek("RecieverMailId"));
                    ReplyObj.StudentId = studentlist.StudentId;
                    ReplyObj.TeacherId = teacherList.TeacherId;
                    ReplyObj.SenderTypeId = userlogin.Typeid;
                    ReplyObj.RecieverTypeId = 3;
                    ReplyObj.Isadmin = false;
                }



                bool response = MailSend(ReplyObj);
                TblEmailSend tblEmailSend = new TblEmailSend();
                if (response == true)
                {

                    tblEmailSend.SenderMailId = ReplyObj.SenderMailId;
                    tblEmailSend.RecieverMailId = ReplyObj.RecieverMailId;
                    tblEmailSend.SendDateTime = DateTime.Now;
                    tblEmailSend.Subject = ReplyObj.Subject;
                    tblEmailSend.Body = ReplyObj.Body;
                    tblEmailSend.SenderTypeId = ReplyObj.SenderTypeId;
                    tblEmailSend.RecieverTypeId = ReplyObj.RecieverTypeId;
                    tblEmailSend.TeacherId = ReplyObj.TeacherId;
                    tblEmailSend.StudentId = ReplyObj.StudentId;
                    tblEmailSend.Isadmin = ReplyObj.Isadmin;
                    _context.TblEmailSend.Add(tblEmailSend);
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("ContactUsByMail", "GetInTouchByMail", new { });
        }

    }
}