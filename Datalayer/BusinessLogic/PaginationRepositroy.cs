using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Datalayer.Models;
using System;
using ReflectionIT.Mvc.Paging;

namespace Datalayer.BusinessLogic
{
    public class PaginationRepositroy : GenericRepository<TblClass>, Ipagination
    {
        public static HttpContext currentContext;

        private readonly OnlineclassContext dbContext;
        private IHostingEnvironment _hostingEnv;
        public PaginationRepositroy(OnlineclassContext _dbContext, IHostingEnvironment hostingEnv) : base(_dbContext)
        {
            dbContext = _dbContext;
            _hostingEnv = hostingEnv;
        }
        public async Task<PagingList<TClass>> Getclass(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {
            
            //var Classnamelist = (from a in dbContext.TblClass select a).AsNoTracking().AsQueryable();
            //if (page == 0)
            //{
            //    page = 1;

            //}
            //if (!string.IsNullOrWhiteSpace(filter))
            //{
            //    Classnamelist = Classnamelist.Where(p => p.ClassName.Contains(filter));
            //}
            //var Class =await PagingList<TblClass>.CreateAsync(Classnamelist, 5, 1, "ClassName", "ClassName");



            var Classnamelist = (from a in dbContext.TblClass select new TClass { ClassId = a.ClassId, ClassName = a.ClassName }).AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                Classnamelist = Classnamelist.Where(p => p.ClassName.Contains(filter));
            }
            var Class = await PagingList<TClass>.CreateAsync(Classnamelist, 5, page, "ClassName", "ClassName");
            return Class;
        }

        public async Task<PagingList<TBatch>> GetBatchPagination(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

            var Batchnamelist = (from a in dbContext.TblBatch select new TBatch {BatchId=a.BatchId,BatchName=a.BatchName }).AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                Batchnamelist = Batchnamelist.Where(p => p.BatchName.Contains(filter));
            }
            var Batch = await PagingList<TBatch>.CreateAsync(Batchnamelist, 5, page, "BatchName", "BatchName");

            return Batch;
        }
        public async Task<PagingList<Tsubject>> GetSubjectPagination(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

            var subjectnamelist = (from a in dbContext.Tblsubject select new Tsubject { Subjectid=a.Subjectid,Subjectname=a.Subjectname}).AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                subjectnamelist = subjectnamelist.Where(p => p.Subjectname.Contains(filter));
            }
            var Subjectlist = await PagingList<Tsubject>.CreateAsync(subjectnamelist, 5, page, "Subjectname", "Subjectname");

            return Subjectlist;
        }
        public async Task<PagingList<Tsubject>> GetSubject_ClassPagination(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

         var subjectclass = (from m in dbContext.TblClassSubjectMap
                                                      join n in dbContext.TblClass on m.Classid equals n.ClassId
                                                      select new Tsubject
                                                      {
                                                          ClassId = m.Classid,
                                                          Classname = n.ClassName
                                                      }).Distinct().AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                subjectclass = subjectclass.Where(p => p.Classname.Contains(filter));
            }
            var Subject_Classlist = await PagingList<Tsubject>.CreateAsync(subjectclass, 5, page, "Classname", "Classname");

            return Subject_Classlist;
        }
        public async Task<PagingList<Batch_SubjectModel>> GetBatch_SubjectPagination(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

            var batchclass = (from m in dbContext.TblClassSubjectMap
                              join n in dbContext.TblBatch on m.Batchid equals n.BatchId
                              select new Batch_SubjectModel
                              {
                                  BatchId = m.Batchid,
                                  BatchName = n.BatchName
                              }).Distinct().AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                batchclass = batchclass.Where(p => p.BatchName.Contains(filter));
            }
            var Subject_Batchlist = await PagingList<Batch_SubjectModel>.CreateAsync(batchclass, 5, page, "BatchName", "BatchName");

            return Subject_Batchlist;
        }
        public async Task<PagingList<fee_SubjectBatchModel>> Getsubjectfee(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

            var subjectfee = (from m in dbContext.Tblfeessetting
                              join n in dbContext.Tblsubject on m.SubjecBatchtId equals n.Subjectid
                              join c in dbContext.TblClass on m.ClassId equals c.ClassId
                              join d in dbContext.TblDuration on m.DurationTypeId equals d.Durationid
                              select new fee_SubjectBatchModel
                              {
                                  ClassName = c.ClassName,
                                  subjectname = n.Subjectname,
                                  feeid = m.Feeid,
                                  feenew = Convert.ToDecimal(m.Ammount),
                                  DurationType = d.Durationname
                              }
                            ).AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                subjectfee = subjectfee.Where(p => p.subjectname.Contains(filter));
            }
            var Subject_Feelist = await PagingList<fee_SubjectBatchModel>.CreateAsync(subjectfee, 5, page, "subjectname", "subjectname");

            return Subject_Feelist;
        }
        public async Task<PagingList<fee_SubjectBatchModel>> GetBatchfee(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

            var Batchfee = (from m in dbContext.Tblfeessetting
                            join n in dbContext.TblBatch on m.SubjecBatchtId equals n.BatchId
                            join c in dbContext.TblClass on m.ClassId equals c.ClassId
                            join d in dbContext.TblDuration on m.DurationTypeId equals d.Durationid
                            select new fee_SubjectBatchModel
                            {
                                ClassName = c.ClassName,
                                subjectname = n.BatchName,
                                feeid = m.Feeid,
                                feenew = Convert.ToDecimal(m.Ammount),
                                DurationType = d.Durationname
                            }
                           ).AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                Batchfee = Batchfee.Where(p => p.subjectname.Contains(filter));
            }
            var Subject_Feelist = await PagingList<fee_SubjectBatchModel>.CreateAsync(Batchfee, 5, page, "subjectname", "subjectname");

            return Subject_Feelist;
        }

        public async Task<PagingList<tblTeacherClassList>> GetStudentmap(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

            var Studentmap = (from n in dbContext.TblStudent
                            join m in dbContext.Tblstudentteacher on n.StudentId
                            equals m.Studentid
                            join s in dbContext.TblstudentSubjectBatch on n.StudentId equals s.StudentId
                            join t in dbContext.Tblsubject on s.SubjectId equals t.Subjectid
                            join c in dbContext.TblClass on n.ClassId equals c.ClassId
                            join tt in dbContext.Tblteacher on m.Teacherid equals tt.TeacherId
                            where m.Subjectid == s.SubjectId

                            select new tblTeacherClassList
                            {
                                studentteacheridmap = m.Teacherstudentmapid,
                                TeacherId = m.Teacherid,
                                Subject = t.Subjectname,
                                ClassName = c.ClassName,
                                StudentName = n.Fname + " " + n.Lname,
                                teachername = tt.Fname + " " + tt.Lname,
                            }).AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                Studentmap = Studentmap.Where(p => p.StudentName.Contains(filter));
            }
            var Studentlist = await PagingList<tblTeacherClassList>.CreateAsync(Studentmap, 5, page, "StudentName", "StudentName");

            return Studentlist;
        }
        public async Task<PagingList<tblTeacherBatchList>> Getteacherbatchmap(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

            var teacherbatchmap = (from n in dbContext.Tblteacher
                              join m in dbContext.Tblteacherclassbatchmap on n.TeacherId equals m.TeacherId
                              join c in dbContext.TblClass on m.Classid equals c.ClassId
                              join b in dbContext.Tblsubject on m.Subjectid equals b.Subjectid
                              join bb in dbContext.TblBatch on m.Batchid equals bb.BatchId
                              where m.Subjectid == b.Subjectid

                              select new tblTeacherBatchList
                              {
                                  TeacherId = n.TeacherId,
                                  BatchName = bb.BatchName,
                                  ClassName = c.ClassName,
                                  Date = bb.Dateofcommencement,
                                  Subject = b.Subjectname,
                                  Teacher = n.Fname + " " + n.Lname,
                                  Techermapid = m.Techermapid
                              }).AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
        
            if (!string.IsNullOrWhiteSpace(filter))
            {
                teacherbatchmap = teacherbatchmap.Where(p => p.BatchName.Contains(filter));
            }
            var teacherbatclist = await PagingList<tblTeacherBatchList>.CreateAsync(teacherbatchmap, 5, page, "BatchName", "BatchName");

            return teacherbatclist;
        }

        public async Task<PagingList<TeacherModel>> GetStudentlist(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

            var Studentlist = (from m in dbContext.TblStudent
                                   join
                                    n in dbContext.TblstudentSubjectBatch on m.StudentId equals n.StudentId
                                   join s in dbContext.Tblsubject on n.SubjectId equals s.Subjectid
                                   //join cs in _context.TblClassSubjectMap on n.SubjectId equals cs.Subjectid
                                   join c in dbContext.TblClass on m.ClassId equals c.ClassId
                                   select new TeacherModel
                                   {
                                       studentname = m.Fname + " " + m.Lname,
                                       studentid = m.StudentId,
                                       subjectid = s.Subjectid,
                                       subjectname = s.Subjectname,
                                       Classid = c.ClassId,
                                       Classname = c.ClassName,
                                       Ispresent = ((from z in dbContext.Tblstudentteacher where z.Studentid == m.StudentId && z.Subjectid == s.Subjectid select z.Studentid).Count()) > 0 ? true : false
                                   }).AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                Studentlist = Studentlist.Where(p => p.studentname.Contains(filter));
            }
            var Studentlistlist = await PagingList<TeacherModel>.CreateAsync(Studentlist, 5, page, "studentname", "studentname");

            return Studentlistlist;
        }

        public async Task<PagingList<Tteacher>> GetTeacherlist(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

           var Teacher = (from a in dbContext.Tblteacher select new Tteacher { Fname=a.Fname +" "+ a.Lname ,Email =a.Email,MobileNo=a.MobileNo,State=a.State,TeacherId=a.TeacherId}).AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                Teacher = Teacher.Where(p => p.Fname.Contains(filter));
            }
            var Teacherlist = await PagingList<Tteacher>.CreateAsync(Teacher, 10, page, "Fname", "Fname");

            return Teacherlist;
        }

        public async Task<PagingList<Teacherlistpay>> GetTeacherPaymentlist(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName")
        {

            var BatchBased = (from a in dbContext.Tblmapteacherammount
                                  // join b in _context.TblClass on a.ClassBatchId equals b.ClassId
                              join c in dbContext.TblBatch on a.ClassBatchId equals c.BatchId
                              join d in dbContext.Tblsubject on a.SubjectId equals d.Subjectid
                              join e in dbContext.Tblteacher on a.TeacherId equals e.TeacherId
                              join f in dbContext.TblDuration on a.Typeid equals f.Durationid
                              where a.Isbatch == true
                              select new Teacherlist

                              {
                                  Id = a.TecherammountId,
                                  TeacherName = e.Fname + " " + e.Lname,
                                  batchName = c.BatchName,
                                  // className = b.ClassName,
                                  SubName = d.Subjectname,
                                  Typename = f.Durationname,
                                  Amount = a.AmountTopay


                              }).Union(
                            (from a in dbContext.Tblmapteacherammount
                              join b in dbContext.TblClass on a.ClassBatchId equals b.ClassId
                              // join c in _context.TblBatch on a.ClassBatchId equals c.BatchId
                              join d in dbContext.Tblsubject on a.SubjectId equals d.Subjectid
                              join e in dbContext.Tblteacher on a.TeacherId equals e.TeacherId
                              join f in dbContext.TblDuration on a.Typeid equals f.Durationid
                              where a.Isbatch == false
                              select
                              new Teacherlist
                              {
                                  Id = a.TecherammountId,
                                  TeacherName = e.Fname + " " + e.Lname,
                                      // batchName = c.BatchName,
                                      className = b.ClassName,
                                  SubName = d.Subjectname,
                                  Typename = f.Durationname,
                                  Amount = a.AmountTopay


                              }));
            //if (BatchBased.Any() == true && ClassBased.Any() == true)
            //{

            //    teacherlists = BatchBased.Union(ClassBased).OrderBy(x => x.Id).ToList();
            //}
            //else if (BatchBased.Any() == true)
            //{
            //    teacherlists = BatchBased.OrderBy(x => x.Id).ToList();
            //}
            //else if (ClassBased.Any() == true)

            //{
            //    teacherlists = ClassBased.OrderBy(x => x.Id).ToList();
            //}
            var Teacherpay =(from m in BatchBased
                             select new Teacherlistpay
            {
                Id = m.Id,
                TeacherName = m.TeacherName,
                batchName = m.batchName,
                SubName =m.SubName,
                Typename =m.Typename,
                Amount =m.Amount
            }).AsNoTracking().AsQueryable();
            if (page == 0)
            {
                page = 1;

            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                Teacherpay = Teacherpay.Where(p => p.TeacherName.Contains(filter));
            }
            var Teacherlist = await PagingList<Teacherlistpay>.CreateAsync(Teacherpay, 10, page, "TeacherName", "TeacherName");

            return Teacherlist;
        }
    }
}
