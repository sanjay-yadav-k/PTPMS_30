using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Datalayer.BusinessLogic
{
    class ModalAnnotation
    {
    }
    public partial class tblstudentbatchmap
    {
        

            public int Id { get; set; }
            public string Subject { get; set; }
            public bool? IsComplete { get; set; }
            public string ClassName { get; set; }
            public string Fname { get; set; }
            public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }

    }
    public partial class tblTeacherClassList
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
        public string teachername { get; set; }
        public TimeSpan? Totime { get; set; }
        public TimeSpan? Fromtime { get; set; }

    }
    public partial class tblTeacherBatchList
    {


        public long TeacherId { get; set; }
        public string Subject { get; set; }
        public string Teacher { get; set; }
        public string BatchName { get; set; }
        public string ClassName { get; set; }
        public bool? IsStartClass { get; set; }
        public bool? IsCopletedClass { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public long Techermapid { get; set; }
        public long teacherbatchid { get; set; }
        public TimeSpan? Totime { get; set; }
        public TimeSpan? Fromtime { get; set; }
    }
    public class ClassBatchModel
    {
        public string ClassName { get; set; }
        public string BatchName { get; set; }
        public TimeSpan? Timing { get; set; }
        public string SubscriptionType { get; set; }
        public decimal? Fees { get; set; }
        public int? ClassId { get; set; }
        public DateTime? Dateofcommencement { get; set; }
        public int? CourseDuration { get; set; }

    }
    public class ClassBatchSubjectTeacherModel
    { 
        public long? ClassId { get; set; }
        public long? SubjectId { get; set; }
        public long? BatchId { get; set; }
        public string ClassName { get; set; }
        public string BatchName { get; set; }
        public TimeSpan Timing { get; set; }
        public string DurationType { get; set; }
        public decimal? Fees { get; set; }
        public int feeid { get; set; }
        public long? durationid { get; set; }
        public long? SubjecBatchtId { get; set; }
       public long? ClassIds { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }


        public DateTime? Dateofcommencement { get; set; }

        public int? CourseDuration { get; set; }
        public string SubjectName { get; set; }
        public string imagename { get; set; }
        public bool IsShow { get; set; }



    }
    //public class fee_SubjectBatchModel
    //{
    //    public long? ClassId { get; set; }
    //    public long? SubjectId { get; set; }
    //    //public long? BatchId { get; set; }
    //    public string ClassName { get; set; }
    //   // public string BatchName { get; set; }
       
    //    public string DurationType { get; set; }
    //    public string subjectname { get; set; }
    //    public decimal? feenew { get; set; }
    //    public int feeid { get; set; }
    //    public long? durationid { get; set; }
    //    public long? SubjecBatchtId { get; set; }
       

    //}
    public class TeacherModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string Fname { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string Lname { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile No. is required.")]
        public int? MobileNo { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "District is required.")]
        public string Distict { get; set; }

        [Required(ErrorMessage = "zipcode is required.")]
        public int? Zipcode { get; set; }
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

        public string studentname { get; set; }
        public int? studentid { get; set; }
        public long subjectid { get; set; }
        public string subjectname { get; set; }
        public long Classid { get; set; }
        public string Classname { get; set; }
        public bool Ispresent { get; set; }

       public string teachername { get; set; }
        public DateTime startdate { get; set; }
    }

    public class fee_SubjectBatchModel
    {
        public long? ClassId { get; set; }
        public long? SubjectId { get; set; }
        //public long? BatchId { get; set; }
        public string ClassName { get; set; }
        // public string BatchName { get; set; }

        public string DurationType { get; set; }
        public string subjectname { get; set; }
        public decimal? feenew { get; set; }
        public decimal? feenew2 { get; set; }
        public int feeid { get; set; }
        public int sfeeid { get; set; }
        public long? durationid { get; set; }
        public long? SubjecBatchtId { get; set; }


    }
    public class Class_SubjectModel
    {
        public long? ClassId { get; set; }
        public long? SubjectId { get; set; }
        public string ClassName { get; set; }
        public string subjectname { get; set; }
    }
    public class subject
    {
        public long? SubjectId { get; set; }
        public string subjectname { get; set; }
        public bool Selected { get; set; }
    }
    public class Batch_SubjectModel
    {

        public long? BatchId { get; set; }
        public long? SubjectId { get; set; }
        public string BatchName { get; set; }
        public string subjectname { get; set; }
    }
    public class TeacherModelNew
    {
        public long? Teacherid { get; set; }
        public string Fname { get; set; }

        public string Lname { get; set; }


        public string Email { get; set; }

        // public int? MobileNo { get; set; }
        public string MobileNo1 { get; set; }

        public string Address { get; set; }

        public string country { get; set; }

        //State
        public string administrative_area_level_1 { get; set; }

        //City
        public string administrative_area_level_2 { get; set; }

        //Pincode
        public int? postal_code { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string studentname { get; set; }
        public int? studentid { get; set; }
        public long subjectid { get; set; }
        public string subjectname { get; set; }
        public long Classid { get; set; }
        public string Classname { get; set; }
        public bool Ispresent { get; set; }

        public string teachername { get; set; }
        public DateTime startdate { get; set; }
    }
    public class StudentModel
    {
        public int studentId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Username { get; set; }
        public DateTime? Dob { get; set; }
        public decimal? MobileNo { get; set; }
        public long? ClassId { get; set; }
        public string Classname { get; set; }
        public string ParentName { get; set; }
        public string Address { get; set; }
        public string country { get; set; }

        //State
        public string administrative_area_level_1 { get; set; }



        //Pincode
        public int? postal_code { get; set; }

        public string Email { get; set; }
        public string Skype { get; set; }
        public string Pswd { get; set; }

        public string ConfirmPassword { get; set; }









        public int Id { get; set; }
        public int? StudentId { get; set; }
        public long? SubjectId { get; set; }
        public long? BatchId { get; set; }
        public int? DurationId { get; set; }
        public int? Period { get; set; }
        //  public string Classname { get; set; }
        //  public string BatchName { get; set; }
        //  public string SubjectName { get; set; }
        //  public string TypeName { get; set; }
    }

    public class payutransation
    {

        public string txnid { get; set; }
        public string Fname { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }

        public string Ammount { get; set; }
        public string key { get; set; }
        public string Salt { get; set; }
        public string Hashcode { get; set; }
        public string surl { get; set; }

    }

    public class TeacherPaymentModel
    {
        public long TecherammountId { get; set; }
        public long? TeacherId { get; set; }
        public long? BatchId { get; set; }
        public long? ClassId { get; set; }
        public bool? Isbatch { get; set; }
        public string Batch_Class { get; set; }
        public long? Subjectid { get; set; }
        public int? DurationTypeid { get; set; }
        public decimal? AmountTopay { get; set; }
        public string Classname { get; set; }
        public string BatchName { get; set; }
        public string SubjectName { get; set; }
        public string TypeName { get; set; }



        ////////////Add new Teacher///////////////

        public long? Teacherid { get; set; }
        public string Fname { get; set; }

        public string Lname { get; set; }


        public string Email { get; set; }

        public string MobileNo1 { get; set; }

        public string Address { get; set; }

        public string country { get; set; }

        //State
        public string administrative_area_level_1 { get; set; }

        //City
        public string administrative_area_level_2 { get; set; }

        //Pincode
        public int? postal_code { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string studentname { get; set; }
        public int? studentid { get; set; }
        public long subjectid { get; set; }
        public string subjectname { get; set; }
        public long Classid { get; set; }

        public bool Ispresent { get; set; }

        public string teachername { get; set; }
        public DateTime startdate { get; set; }



    }
    public class Teacherlist
    {
        
        public long? Id { get; set; }
        public string TeacherName { get; set; }
        public string batchName { get; set; }
        public string className { get; set; }
        public string SubName { get; set; }
        public string Typename { get; set; }
        public decimal? Amount { get; set; }
    }
    public class Teacherlistpay
    {

        public long? Id { get; set; }
        public string TeacherName { get; set; }
        public string batchName { get; set; }
        public string className { get; set; }
        public string SubName { get; set; }
        public string Typename { get; set; }
        public decimal? Amount { get; set; }
    }
    public partial class Tteacher
    {
        public long TeacherId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int? Pincode { get; set; }
    }
    public class chapter
    {
        
        public long? classid { get; set; }
        public string classname { get; set; }
        public List<chapterSubject> chapterSubject { get; set; }
       

    }
    public class chapterSubject
    {
        public long? classid { get; set; }
        public long? SubjectId { get; set; }
        public string Subject_name { get; set; }
        public List<chapterSubjectChaptername> chapterSubjectChaptername { get; set; }
    }
    public class chapterSubjectChaptername
    {
        public long? Chapter_id { get; set; }
        public long? classid { get; set; }
        public long? SubjectId { get; set; }
        public string Subject_name { get; set; }
        public string Chaptername { get; set; }
        public string Syllubus { get; set; }
    }
    public class TransactionHistory
    {
        public string batchName { get; set; }
        public DateTime? DateofTrans { get; set; }
        public Decimal? Amount_Paid { get; set; }
        public string status { get; set; }
        public string duration { get; set; }
        public string SubjectName { get; set; }
        public int? period { get; set; }

    }
    public class TClass
    {
        public long ClassId { get; set; }
        public string ClassName { get; set; }
    }
    public partial class TBatch
    {
        public long BatchId { get; set; }
        public string BatchName { get; set; }
        public long? ClassId { get; set; }
        public string Classname { get; set; }
    }
    public partial class Tsubject
    {
        public long Subjectid { get; set; }
        public string Subjectname { get; set; }
        public long? ClassId { get; set; }
        public string Classname { get; set; }
    }
    public class StudentCountModel
    {
        public int stuId { get; set; }
        public string stuName { get; set; }
        public string Class { get; set; }

        public DateTime? dob { get; set; }
        public string email { get; set; }
        public decimal? mobile { get; set; }
        public string parentName { get; set; }
        public string skype { get; set; }
        public int? Pincode { get; set; }
        public string state { get; set; }

    }
    public class offlineStudentList
    {

        public long? Id { get; set; }
        public string StudentName { get; set; }
        public string batchName { get; set; }

        public string SubName { get; set; }
        public string Typename { get; set; }
        public int? Period { get; set; }

    }
    public class subjectList
    {
        public string subjectName { get; set; }
        public decimal? unitAmt { get; set; }
        public string batchName { get; set; }
    }
    public class InvoiceDetailModel
    {
        public string StudentName { get; set; }
        public string invoiceDate { get; set; }
        public string CompleteAddress { get; set; }
        public string state { get; set; }
        public List<subjectList> subjectLists { get; set; }
        public decimal? totalAmt { get; set; }
        public string invoiceNumber { get; set; }

    }



    public class contactByMail
    {
        public int MailId { get; set; }
        public string Subject { get; set; }
        public string SenderMailId { get; set; }
        public string RecieverMailId { get; set; }
        public string Body { get; set; }
        public DateTime? SendDateTime { get; set; }
        public long? TeacherId { get; set; }
        public int? SenderTypeId { get; set; }
        public int? RecieverTypeId { get; set; }
        public int? StudentId { get; set; }
        public string Teachername { get; set; }
        public string StudentName { get; set; }
        public bool IsSent { get; set; }
        public bool Isadmin { get; set; }
        public string TeacherOrStudentMailid { get; set; }

    }

    public class innerlist
    {
        public int MailId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? SendDateTime { get; set; }
    }
    public class MailIterate
    {
        public string TeacherName { get; set; }
        public string SendEmail { get; set; }
        public string RecieveEmail { get; set; }
        public string StudentName { get; set; }
        public long? SenderUserid { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public long teacherid { get; set; }

        // public List<innerlist> innerlists { get; set; }

    }


    public class ContactbyMailModel
    {
        public int Id { get; set; }
        public string EmailId { get; set; }
        public bool? IsSent { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public long? UserId { get; set; }
        public int? UserTypeId { get; set; }
        public string fromMailId { get; set; }
        public string SenderMailId { get; set; }
        public long? SenderUserId { get; set; }
    }
    public class StudentTeacherList
    {

        public long TeacherId { get; set; }


        public string Email { get; set; }

        public string Name { get; set; }
    }

}
