using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.BusinessLogic
{
   public  interface ITotalCount
    {

        List<Class_SubjectModel> TotalClassCount();
        List<Batch_SubjectModel> TotalBatch();
        List<StudentCountModel> TotalStudentCount();
        List<StudentCountModel> TotalNewStudentCount();
    }
}
