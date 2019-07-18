using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Datalayer.Models;
using Datalayer.Services.Lib;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Datalayer.BusinessLogic
{
   public class StudentRegistration
    {
       
        public StudentRegistration()
        {

        }
        public List<GetBatchSubject> getBatchSubject(OnlineclassContext _context,int value)
        {
            List<GetBatchSubject> lb = new List<GetBatchSubject>();
            DataTable dt = new DataTable();
            SqlConnection objCon = new SqlConnection();
            objCon.ConnectionString = _context.Database.GetDbConnection().ConnectionString; //"Data Source = WINSERVER; Initial Catalog = Onlineclass; User ID = sa ; password=karmick@123";
            //objCon.Open();
            SqlParameter[] parameter = new SqlParameter[]
                {

                new SqlParameter("@SB",value),
                };
            using (SQLInterfaceMgr sqlMgr = new SQLInterfaceMgr())
            {
                dt = sqlMgr.ExecuteStoreProcedure("stp_GetSubjectAndBatch",parameter, objCon.ConnectionString);
         
            }
            if(dt.Rows.Count>=0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    GetBatchSubject bb = new GetBatchSubject();
                    bb.tid = Convert.ToInt32(item["tid"]);
                    bb.id = Convert.ToInt64(item["id"]);
                    bb.name = Convert.ToString(item["name"]);
                    //bb.amount = Convert.ToDecimal(item["amount"]);
                    lb.Add(bb);
                }
                
            }

            return lb;
        }

        public subjectAmount SubjectFees(Int64 BatchSubjectid, Int64 Classid, Int32 Durationid, OnlineclassContext _context)
        {
            //List<GetBatchSubject> lb = new List<GetBatchSubject>();
            DataTable dt = new DataTable();
            SqlConnection objCon = new SqlConnection();
            objCon.ConnectionString = _context.Database.GetDbConnection().ConnectionString; //"Data Source = WINSERVER; Initial Catalog = Onlineclass; User ID = sa ; password=karmick@123";
            //objCon.Open();
            SqlParameter[] parameter = new SqlParameter[]
                {

               new SqlParameter("@batchsubjectid",BatchSubjectid),
               new SqlParameter("@classid",Classid),
               new SqlParameter("@durationid",Durationid),
                };
            using (SQLInterfaceMgr sqlMgr = new SQLInterfaceMgr())
            {
                dt = sqlMgr.ExecuteStoreProcedure("stp_getAmountandSubjectBasedonId",parameter, objCon.ConnectionString);

            }
            subjectAmount bb = new subjectAmount();
            if (dt.Rows.Count > 0)
            {

                    
                    bb.tid = Convert.ToInt32(dt.Rows[0]["tid"]);
                    bb.id = Convert.ToInt64(dt.Rows[0]["id"]);
                    bb.Fees = Convert.ToDecimal(dt.Rows[0]["Fees"]);
                    bb.Subjects = Convert.ToString(dt.Rows[0]["Subjects"]);
         
            }

            return bb;
        }

        public List<SubjectBatchTimming> SubjectBatcTimmings(Int64 id, OnlineclassContext _context)
        {
            List<SubjectBatchTimming> lb = new List<SubjectBatchTimming>();
            DataTable dt = new DataTable();
            SqlConnection objCon = new SqlConnection();
            
            objCon.ConnectionString = _context.Database.GetDbConnection().ConnectionString; //"Data Source = WINSERVER; Initial Catalog = Onlineclass; User ID = sa ; password=karmick@123";
            //objCon.Open();
            SqlParameter[] parameter = new SqlParameter[]
                {

               new SqlParameter("@id",id),
                };
            using (SQLInterfaceMgr sqlMgr = new SQLInterfaceMgr())
            {
                dt = sqlMgr.ExecuteStoreProcedure("stp_GetBatchSubjectTimming", parameter, objCon.ConnectionString);

            }
            //subjectAmount bb = new subjectAmount();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow item in dt.Rows)
                {
                    SubjectBatchTimming bb = new SubjectBatchTimming();
                    bb.tid = Convert.ToInt32(item["tid"]);
                    bb.batch_subjectid = Convert.ToInt64(item["batch_subjectid"]);
                    bb.Timming = Convert.ToString(item["Timming"]);
                    lb.Add(bb);
                }

            }

            return lb;
        }

        public int InsertUpdateStudent(int OP, string FirstName, string LastName, string ParentName, DateTime Dob, string Address, string City,
                                                            string State, string Pin, string Country, string Email, string Cell, string User, string Skype,
                                                            string Password, string ConfirmPass, Int64 BatchSubject, Int64 Class,
                                                            Int32 Duration, Int32 Period, decimal Amount, OnlineclassContext _context)
        {
            int ans=0;
            SqlConnection objCon = new SqlConnection();
            objCon.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
            DataTable dt = new DataTable();
            SqlParameter[] parameter = new SqlParameter[]
                {
                    new SqlParameter("@OP",OP),
                    new SqlParameter("@FirstName",FirstName),
                    new SqlParameter("@LastName",LastName),
                    new SqlParameter("@ParentName",ParentName),
                    new SqlParameter("@Dob",Dob),
                    new SqlParameter("@Address",Address),
                    new SqlParameter("@City",City),
                    new SqlParameter("@State",State),
                    new SqlParameter("@Pin",Pin),
                    new SqlParameter("@Country",Country),
                    new SqlParameter("@Email",Email),
                    new SqlParameter("@Cell",Cell),
                    new SqlParameter("@User",User),
                    new SqlParameter("@Skype",Skype),
                    new SqlParameter("@Password",Password),
                    new SqlParameter("@BatchSubject",BatchSubject),
                    new SqlParameter("@class",Class),
                    new SqlParameter("@Duration",Duration),
                    new SqlParameter("@Period",Period),
                    new SqlParameter("@Amount",Amount),
                };
            using (SQLInterfaceMgr sqlMgr = new SQLInterfaceMgr())
            {
                dt = sqlMgr.ExecuteStoreProcedure("stp_InsertUpdateStudent", parameter, objCon.ConnectionString);

            }
            if (dt.Rows.Count > 0)
            {
                ans = Convert.ToInt32(dt.Rows[0][0]);
            }
            return ans;
        }

        public int insertUpdateTransactionForBatch(int op,Int64 studentid, decimal amt, bool isSubject, Int64 batchid, int durationid, decimal totamt, int? runhr, OnlineclassContext _context,string txnid)
        {
            int ans = 0;
            SqlConnection objCon = new SqlConnection();
            objCon.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
            DataTable dt = new DataTable();
            SqlParameter[] parameter = new SqlParameter[]
                {
                    new SqlParameter("@op",op),
                    new SqlParameter("@studentid",studentid),
                    new SqlParameter("@amt",amt),
                    new SqlParameter("@isSubject",isSubject),
                    new SqlParameter("@batchid",batchid),
                    new SqlParameter("@durationid",durationid),
                    new SqlParameter("@totalAmt",totamt),
                    new SqlParameter("@period",runhr),
                    new SqlParameter("@txnid",txnid),
                };
            using (SQLInterfaceMgr sqlMgr = new SQLInterfaceMgr())
            {
                dt = sqlMgr.ExecuteStoreProcedure("stp_insertupdateTransactions", parameter, objCon.ConnectionString);

            }
            if (dt.Rows.Count > 0)
            {
                ans = Convert.ToInt32(dt.Rows[0][0]);
            }
            return ans;
        }

        public int SubjectMoneyTransactionInsertionUpdation(System.Xml.XmlDocument xml, OnlineclassContext _context,string txnid)
        {
            SqlConnection objCon = new SqlConnection();
            objCon.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
            DataTable dt = new DataTable();
            int ans = 0;

            SqlParameter[] parameter = new SqlParameter[]
           {
               new SqlParameter("@xmldoc",SqlDbType.NVarChar,-1,ParameterDirection.Input,true, 0, 0, "", DataRowVersion.Current,xml.InnerXml),
               new SqlParameter("@txnid",txnid),
           };
           // string msg = null;
            using (SQLInterfaceMgr sqlMgr = new SQLInterfaceMgr())
            {
                dt = sqlMgr.ExecuteStoreProcedure("stp_InsertSubjectWisePaymentInsertion", parameter, objCon.ConnectionString);

            }
            if (dt.Rows.Count > 0)
            {
                ans = Convert.ToInt32(dt.Rows[0][0]);
            }
            return ans;
        }


        public int ResponseFromPayUMoneyForSubject(string status, OnlineclassContext _context, string txnid)
        {
            SqlConnection objCon = new SqlConnection();
            objCon.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
            DataTable dt = new DataTable();
            int ans = 0;

            SqlParameter[] parameter = new SqlParameter[]
           {
               new SqlParameter("status",status),
               new SqlParameter("@txnid",txnid),
           };
            // string msg = null;
            using (SQLInterfaceMgr sqlMgr = new SQLInterfaceMgr())
            {
                dt = sqlMgr.ExecuteStoreProcedure("stp_GetResponseFromPayUMoneyForSubject", parameter, objCon.ConnectionString);

            }
            if (dt.Rows.Count > 0)
            {
                ans = Convert.ToInt32(dt.Rows[0][0]);
            }
            return ans;
        }

        public IEnumerable<TeacherMain> GetAllTeacherDetailForPayment(string month,string year,long tid, OnlineclassContext _context)
        {
            List<TeacherMain> lteacher = new List<TeacherMain>();
            //List<TeacherDetail> ldetail = new List<TeacherDetail>();
            SqlConnection objCon = new SqlConnection();
            objCon.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            SqlParameter[] parameter = new SqlParameter[]
           {
               new SqlParameter("@month",month),
               new SqlParameter("@year",year),
               new SqlParameter("@tid",tid),
           };
            using (SQLInterfaceMgr sqlMgr = new SQLInterfaceMgr())
            {
                ds = sqlMgr.ExecuteStoreProc("stp_TeacherREmunaration", parameter, objCon.ConnectionString);

            }
            //dt1 = ds.Tables[0];
            //dt2 = ds.Tables[1];
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    TeacherMain bb = new TeacherMain();
                    List<TeacherDetail> ttd = new List<TeacherDetail>();
                    foreach (DataRow list in ds.Tables[1].Rows)
                    {
                        var teacherid= Convert.ToInt64(item["Teacherid"]);
                        if (Convert.ToInt64(list["Teacherid"]) == teacherid)
                        {
                            TeacherDetail td = new TeacherDetail();
                            td.teacherid= Convert.ToInt64(list["Teacherid"]);
                            td.teacherstudentmapid = Convert.ToInt64(list["teacherstudentmapid"]);
                            td.Amt_To_Pay = Convert.ToDecimal(list["Amt_To_Pay"]);
                            td.Duration = Convert.ToString(list["Duration"]);
                            td.SubjectBatch = Convert.ToString(list["SubjectBatch"]);
                            td.Total_No_Classes = Convert.ToInt32(list["Total_No_Classes"]);
                            td.Total_No_hours = Convert.ToInt32(list["Total_No_hours"]);
                            td.Total_no_Mins = Convert.ToInt32(list["Total_no_Mins"]);
                            td.Name = Convert.ToString(list["Name"]);
                            ttd.Add(td);
                        }
                       
                    }
                    bb.Teacherid = Convert.ToInt64(item["Teacherid"]);
                    bb.TeacherName = Convert.ToString(item["TeacherName"]);
                    bb.Month = Convert.ToString(item["Month"]);
                    bb.Year = Convert.ToString(item["Year"]);
                    bb.TeacherDetail=ttd;

                    //    Teacher bb = new Teacher();
                    //bb.TeacherMain[1].Teacherid = Convert.ToInt64(item["Teacherid"]);
                    //bb.TeacherMain[2].TeacherName = Convert.ToString(item["TeacherName"]);
                    //bb.TeacherMain[4].Month = Convert.ToString(item["Month"]);
                    //bb.TeacherMain[3].Year= Convert.ToString(item["Year"]);
                    //bb.Year = Convert.ToString(item["Year"]);
                    lteacher.Add(bb);
                }
            }

            //if (dt2.Rows.Count > 0)
            //{
            //    foreach (DataRow item in dt2.Rows)
            //    {
            //        Teacher bb = new Teacher();
            //        bb.TeacherDetail[0].teacherid = Convert.ToInt64(item["Teacherid"]);
            //        bb.TeacherDetail[0].teacherstudentmapid= Convert.ToInt64(item["teacherstudentmapid"]);
            //        bb.TeacherDetail[0].Amt_To_Pay = Convert.ToDecimal(item["Amt_To_Pay"]);
            //        bb.TeacherDetail[0].Duration = Convert.ToString(item["Duration"]);
            //        bb.TeacherDetail[0].SubjectBatch = Convert.ToString(item["SubjectBatch"]);
            //        bb.TeacherDetail[0].Total_No_Classes = Convert.ToInt32(item["Total_No_Classes"]);
            //        bb.TeacherDetail[0].Total_No_hours = Convert.ToInt32(item["Total_No_hours"]);
            //        bb.TeacherDetail[0].Total_no_Mins = Convert.ToInt32(item["Total_no_Mins"]);
            //        lteacher.Add(bb);
            //    }
            //}

            return lteacher;

        }


    }

    public class GetBatchSubject
    {
        public int tid { get; set; }
        public Int64 id { get; set; }
        public string name { get; set; }
        public decimal amount { get; set; }
    }
    public class subjectAmount
    {
        public int tid { get; set; }
        public Int64 id { get; set; }
        public decimal Fees { get; set; }
        public string Subjects { get; set; }
    }

    public class SubjectBatchTimming
    {
        public int tid { get; set; }
        public Int64 batch_subjectid { get; set; }
        public string Timming { get; set; }
    }

   public class tblStudentTeacherClassList
    {
        public long? studentteacheridmap { get; set; }
        public long? TeacherId { get; set; }
        public string Subject { get; set; }
        public string ClassName { get; set; }
        public string StudentName { get; set; }
        public bool? IsStartClass { get; set; }
        public bool? IsCopletedClass { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string TeacherName { get; set; }
        public long? SudentId { get; set; }
     }
    public class tblStudentBatchList
    {
        public long? TeacherId { get; set; }
        public string Subject { get; set; }
        public string BatchName { get; set; }
        public string ClassName { get; set; }
        public bool? IsStartClass { get; set; }
        public bool? IsCopletedClass { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public int? Studentid { get; set; }
        public string Teacher { get; set; }
        public string Subjects { get; set; }
        public long teacherbatchid { get; set; }
    }

    public class TeacherMain
    {

        public long? Teacherid { get; set; }
        public string TeacherName { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public List<TeacherDetail> TeacherDetail { get; set; }


    }
    public class TeacherDetail
    {
        public long? teacherid { get; set; }
        public long? teacherstudentmapid { get; set; }
        public string Duration { get; set; }
        public int? Total_No_Classes { get; set; }
        public int? Total_No_hours { get; set; }
        public int? Total_no_Mins { get; set; }
        public decimal? Amt_To_Pay { get; set; }
        public string SubjectBatch { get; set; }
        public string Name { get; set; }
        public decimal TotalAmt { get; set; }                                   //public List<TeacherAmount> TeacherAmount { get; set; }
                                                                                  // public List<chapterSubjectChaptername> chapterSubjectChaptername { get; set; }
    }

    //public class TeacherAmount
    //{
    //    public long? teacherid { get; set; }
    //    public int? Total_No_Classes { get; set; }
    //    public int? Total_No_hours { get; set; }
    //    public int? Total_no_Mins { get; set; }
    //    public decimal? Amt_To_Pay { get; set; }
    //}
    //public class Teacher
    //{
    //    public List<TeacherMain> TeacherMain { get; set; }
    //    public List<TeacherDetail> TeacherDetail { get; set; }
    //}
    //public class chapterSubjectChaptername
    //{
    //    public long? Chapter_id { get; set; }
    //    public long? classid { get; set; }
    //    public long? SubjectId { get; set; }
    //    public string Subject_name { get; set; }
    //    public string Chaptername { get; set; }
    //    public string Syllubus { get; set; }
    //}

}
