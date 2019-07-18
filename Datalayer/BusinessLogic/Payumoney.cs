using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Datalayer.BusinessLogic
{
    public class Payumoney:GenericRepository<payutransation>, GetPayment
    {
        public static HttpContext currentContext;

        private readonly OnlineclassContext dbContext;
        private IHostingEnvironment _hostingEnv;
        public Payumoney(OnlineclassContext _dbContext, IHostingEnvironment hostingEnv) : base(_dbContext)
        {
            dbContext = _dbContext;
            _hostingEnv = hostingEnv;
        }
        public payutransation Payment(string amount,Int32 studentid, string txnid)
        {

            var student = (from m in dbContext.TblStudent where  m.StudentId == studentid select m).FirstOrDefault();
            byte[] hash;
            string d = Key.Keys + "|" + txnid + "|" + amount + "|" + "subject" + "|" + student.Fname + "|" + student.Email + "|||||" + "BOLT_KIT_ASP.NET" + "||||||" + Key.Salt;
            var datab = Encoding.UTF8.GetBytes(d);
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(datab);
            }
            payutransation payutransation = new payutransation();
            payutransation.key = Key.Keys;
            payutransation.Salt = Key.Salt;
            payutransation.Hashcode = Convert.ToString(GetStringFromHash(hash));
            payutransation.Fname = student.Fname;
            payutransation.Email = student.Email;
            payutransation.phone = Convert.ToString(student.MobileNo);
            payutransation.txnid = txnid;
            payutransation.Ammount = amount;
            payutransation.surl = _hostingEnv.ContentRootPath + "/PayUmoneyresponse/Index";

            return payutransation;
        }
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2").ToLower());
            }
            return result.ToString();
        }
        //public static string GetBaseUrl()
        //{
        //    try
        //    {
        //        var request = currentContext.;

        //        var host = request.Host.ToUriComponent();

        //        var pathBase = request.PathBase.ToUriComponent();

        //        return $"{request.Scheme}://{host}{pathBase}";
        //    }
        //   catch(Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}