using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer.BusinessLogic
{
    public interface GetPayment:IGenericRepository<payutransation>
    {
        payutransation Payment(string amount,Int32 studentid,string txnid);
    }
}
