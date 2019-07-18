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
    public class TotalCountRepositery : GenericRepository<TblClass>, ITotalCount
    {

        public static HttpContext currentContext;

        private readonly OnlineclassContext dbContext;
        private IHostingEnvironment _hostingEnv;
        public TotalCountRepositery(OnlineclassContext _dbContext, IHostingEnvironment hostingEnv) : base(_dbContext)
        {
            dbContext = _dbContext;
            _hostingEnv = hostingEnv;
        }

        public  List<Class_SubjectModel> TotalClassCount()
        {

            var TotalClass = (from a in dbContext.TblClass
                              select new Class_SubjectModel
                              {
                                  ClassName = a.ClassName
                              }).ToList();
            return  TotalClass; 

        }


        public List<StudentCountModel> TotalStudentCount()
        {
            var TotalStudent = (from a in dbContext.TblStudent
                                join b in dbContext.TblClass on a.ClassId equals b.ClassId
                                select new StudentCountModel
                                {
                                    stuId =a.StudentId,
                                    stuName = a.Fname + " " + a.Lname,
                                    Class = b.ClassName,
                                    dob = a.Dob,
                                    email = a.Email,
                                    mobile = a.MobileNo,
                                    parentName = a.ParentName,
                                    skype = a.Skype,
                                    Pincode = Convert.ToInt32(a.Zipcode),
                                    state = a.State


                                }).ToList();

            return TotalStudent;
            
        }

        public List<Batch_SubjectModel> TotalBatch()
        {
            var TotalBatch = (from a in dbContext.TblBatch
                              select new Batch_SubjectModel
                              {
                                  BatchName = a.BatchName
                              }).ToList();
            return TotalBatch;
        }

        public List<StudentCountModel> TotalNewStudentCount()
        {

            var TotalNewstudent = (from a in dbContext.TblStudent
                               join b in dbContext.TblClass on a.ClassId equals b.ClassId
                               join c in dbContext.Tblstudentteacher on a.StudentId equals c.Studentid into ps
                                   from c in ps.DefaultIfEmpty()
                                   where c.Studentid == null
                                   select new StudentCountModel
                               {
                                       stuId = a.StudentId,
                                       stuName = a.Fname + " " + a.Lname,
                                   Class = b.ClassName,
                                   dob = a.Dob,
                                   email = a.Email,
                                   mobile = a.MobileNo,
                                   parentName = a.ParentName,
                                   skype = a.Skype,
                                   Pincode = Convert.ToInt32(a.Zipcode),
                                   state = a.State
                               }).ToList();


           

            return TotalNewstudent;

        }
    }
}
