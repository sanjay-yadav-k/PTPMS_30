using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datalayer.BusinessLogic;
using Datalayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineClass.Controllers
{
    public class ChapterController : Controller
    {
        private readonly OnlineclassContext _context;

        public ChapterController(OnlineclassContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            List<chapter> chapters = new List<chapter>();
            
            var classname = (from m in _context.Tblchepter
                           join n in _context.TblClass on m.ClassId equals n.ClassId
                           select new
                           {
                               classid = m.ClassId,
                               classname=n.ClassName
                           }).Distinct().ToList();
            foreach(var item in classname)
            {
                List<chapterSubject> _chapterSubject = new List<chapterSubject>();
                chapter _chapter = new chapter();
                _chapter.classid = item.classid;
                _chapter.classname = item.classname;
                var subject = (from m in _context.Tblchepter
                               join n in _context.Tblsubject on m.SubjectId equals n.Subjectid
                               where m.ClassId== item.classid
                               select
                               new chapterSubject
                               {
                                   classid = m.ClassId,
                                   SubjectId = m.SubjectId,
                                   Subject_name = n.Subjectname
                               }).Distinct().ToList();
                
                foreach(var item2 in subject)
                {
                    chapterSubject chapterSubject = new chapterSubject();
                    var subjectChaptername = (from m in _context.Tblchepter
                                              join n in _context.Tblsubject on m.SubjectId equals n.Subjectid
                                              where m.ClassId == item.classid && m.SubjectId== item2.SubjectId
                                              select
                                              new chapterSubjectChaptername
                                              {
                                                  Chapter_id=m.ChepterId,
                                                  classid = m.ClassId,
                                                  SubjectId = m.SubjectId,
                                                  Subject_name = n.Subjectname,
                                                  Chaptername = m.ChepterName,
                                                  Syllubus = m.Syllabus
                                              }).Distinct().ToList();
                    chapterSubject.classid = item2.classid;
                    chapterSubject.SubjectId = item2.SubjectId;
                    chapterSubject.Subject_name = item2.Subject_name;
                    chapterSubject.chapterSubjectChaptername = subjectChaptername;
                    _chapterSubject.Add(chapterSubject);
                }

               
                _chapter.chapterSubject = _chapterSubject;
                chapters.Add(_chapter);
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
            ViewBag.ClassName = new SelectList(_context.TblClass, "ClassId", "ClassName");
            ViewBag.chaptersdetail = chapters;
            return View();
        }
        // GET: Chepter
        public ActionResult Chapter()
        {
            ViewBag.type = 1;
            ViewBag.user = HttpContext.Session.GetString("name");
            ViewBag.ClassName = new SelectList(_context.TblClass, "ClassId", "ClassName");
            return View();
        }
        [HttpPost]
        public ActionResult Chapter(Tblchepter tblchepter)
        {
   
            _context.Tblchepter.Add(tblchepter);
            _context.SaveChanges();
            ViewBag.type = 1;
            ViewBag.user = HttpContext.Session.GetString("name");
            ViewBag.ClassName = new SelectList(_context.TblClass, "ClassId", "ClassName");
            return RedirectToAction("Index");
        }
        public ActionResult Edit(Int32 Chapterid)
        {

            Tblchepter tblchepter = (from m in _context.Tblchepter where m.ChepterId == Chapterid select m).FirstOrDefault();
            ViewBag.type = 1;
            ViewBag.user = HttpContext.Session.GetString("name");
            ViewBag.ClassName = new SelectList(_context.TblClass, "ClassId", "ClassName");
            return View(tblchepter);
        }
        [HttpPost]
        public ActionResult Edit(Tblchepter tblchepter)
        {
            var chapter = (from m in _context.Tblchepter where m.ChepterId == tblchepter.ChepterId select m).FirstOrDefault();
            chapter.ChepterName = tblchepter.ChepterName;
            chapter.ClassId = tblchepter.ClassId;
            chapter.Syllabus = tblchepter.Syllabus;
            _context.SaveChanges();
            ViewBag.type = 1;
            ViewBag.user = HttpContext.Session.GetString("name");
            ViewBag.ClassName = new SelectList(_context.TblClass, "ClassId", "ClassName");
            return RedirectToAction("Index");
        }
    }
}