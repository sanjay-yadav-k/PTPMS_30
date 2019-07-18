using Datalayer.Models;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.BusinessLogic
{
   public interface Ipagination : IGenericRepository<TblClass>
    {
        Task<PagingList<TClass>> Getclass(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<TBatch>> GetBatchPagination(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<Tsubject>> GetSubject_ClassPagination(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<Tsubject>> GetSubjectPagination(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<Batch_SubjectModel>> GetBatch_SubjectPagination(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<fee_SubjectBatchModel>> Getsubjectfee(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<fee_SubjectBatchModel>> GetBatchfee(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<tblTeacherBatchList>> Getteacherbatchmap(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<tblTeacherClassList>> GetStudentmap(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<TeacherModel>> GetStudentlist(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<Teacherlistpay>> GetTeacherPaymentlist(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
        Task<PagingList<Tteacher>> GetTeacherlist(string filter = "", int page = 0, int IsType = 0, string sortExpression = "BatchName");
    }
}
