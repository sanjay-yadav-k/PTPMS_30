using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.BusinessLogic
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        #region to fetch data
        Task<IEnumerable<TEntity>> GetAllDataAsynchronously();
        IQueryable<TEntity> GetQueryableData();
        IEnumerable<TEntity> GetEnumetatedData();
        List<TEntity> GetListOfData();
        #endregion

        #region to fetch selected data
        Task<TEntity> GetDataById(object id);
        #endregion

        #region to insert data
        Task<string> CreateAsync(TEntity entity);
        #endregion

        #region to update data
        Task<string> UpdateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(object id, TEntity entity);
        #endregion

        #region to delete data
        Task<string> DeleteAsyn(TEntity entity);
        #endregion

        #region to count record
        Task<int> CountAsync();
        #endregion
    }
   
}
