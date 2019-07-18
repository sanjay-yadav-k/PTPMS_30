using Datalayer.BusinessLogic;
using Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Datalayer
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly OnlineclassContext _Context;

        public GenericRepository(OnlineclassContext Context)
        {
            _Context = Context;
        }
        #region to fetch data
        /// <summary>
        /// get data Asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllDataAsynchronously()
        {
            //ChangeDBConstr();
            return await _Context.Set<TEntity>().ToListAsync();
        }
        /// <summary>
        /// get list of data as (IQueryable)
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetQueryableData()
        {
            // ChangeDBConstr();
            return _Context.Set<TEntity>().AsNoTracking();
        }
        /// <summary>
        /// get all data as (IEnumerable)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetEnumetatedData()
        {
            // ChangeDBConstr();
            return _Context.Set<TEntity>();
        }
        /// <summary>
        /// get data as (list)
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetListOfData()
        {
            // ChangeDBConstr();
            return _Context.Set<TEntity>().ToList();
        }
        #endregion

        #region to fetch selected data
        /// <summary>
        /// get selected data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetDataById(object id)
        {
            ///ChangeDBConstr();
            return await _Context.Set<TEntity>().FindAsync(id);
        }
        #endregion

        #region to insert data
        /// <summary>
        /// insert data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<string> CreateAsync(TEntity entity)
        {
            //ChangeDBConstr();
            string strRes = string.Empty;
            int intRes = 0;
            try
            {
                await _Context.Set<TEntity>().AddAsync(entity);
                intRes = await _Context.SaveChangesAsync();
                if (intRes > 0)
                    strRes = "Data Saved";
                else
                    strRes = "Unable to save data";
            }
            catch (Exception Ex)
            {
                strRes = "Ex: " + Ex.Message + "In Ex: " + Ex.InnerException;
            }
            return strRes;
        }
        #endregion

        #region to update data
        /// <summary>
        /// update data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<string> UpdateAsync(TEntity entity)
        {
            //ChangeDBConstr();
            string strRes = "";
            int intRes = 0;
            try
            {
                _Context.Entry(entity).CurrentValues.SetValues(entity);
                intRes = await _Context.SaveChangesAsync();
                if (intRes > 0)
                    strRes = "Data Updated";
                else
                    strRes = "Unable to update data";
            }
            catch (Exception ex)
            {
                strRes = "Ex: " + ex.Message + "In Ex: " + ex.InnerException;
            }
            return strRes;
        }
        /// <summary>
        /// update data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> UpdateAsync(object id, TEntity entity)
        {
            // ChangeDBConstr();
            try
            {
                if (entity == null)
                    return null;
                TEntity exist = await _Context.Set<TEntity>().FindAsync(id);
                if (exist != null)
                {
                    _Context.Entry(exist).CurrentValues.SetValues(entity);
                    await _Context.SaveChangesAsync();
                }
                return exist;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        #endregion

        #region to delete data
        public virtual async Task<string> DeleteAsyn(TEntity entity)
        {
            //ChangeDBConstr();
            string strRes = "";
            int intRes = 0;
            try
            {
                _Context.Set<TEntity>().Remove(entity);
                intRes = await _Context.SaveChangesAsync();
                if (intRes > 0)
                    strRes = "Data Deleted";
                else
                    strRes = "Unable to delete data";
            }
            catch (Exception Ex)
            {
                strRes = "Ex: " + Ex.Message + "In Ex: " + Ex.InnerException;
            }
            return strRes;
        }
        #endregion

        #region to count record
        public async Task<int> CountAsync()
        {
            //ChangeDBConstr();
            return await _Context.Set<TEntity>().CountAsync();
        }
        #endregion

        //#region change connection string
        //public void ChangeDBConstr()
        //{
        //    if (ConnectionSetup.strConn != null)
        //        this._dbContext.Database.GetDbConnection().ConnectionString = ConnectionSetup.strConn;
        //}
        //#endregion

    }




    //    public class GenericRepository<C, T> : IGenericRepository<T> where T : class
    //    {
    //        private readonly OnlineclassContext _Context;

    //        public GenericRepository(OnlineclassContext Context)
    //        {
    //            _Context = Context;
    //        }
    //        public virtual IQueryable<T> GetAll()

    //    {

    //        IQueryable<T> query = _Context.Set<T>();
    //        return query;
    //    }

    //    public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    //    {

    //        IQueryable<T> query = _Context.Set<T>().Where(predicate);
    //        return query;
    //    }

    //    public virtual void Add(T entity)
    //    {
    //        _Context.Set<T>().Add(entity);
    //    }

    //    public virtual void Delete(T entity)
    //    {
    //        _Context.Set<T>().Remove(entity);
    //    }

    //    //public virtual void Edit(T entity)
    //    //{
    //    //    _Context.Entry(entity).State = EntityState.Modified;
    //    //}

    //    public virtual void Save()
    //    {
    //        try
    //        {
    //            _Context.SaveChanges();
    //        }
    //        catch (Exception ex)
    //        {

    //        }

    //    }

    //    public virtual string Insert(T entity)
    //    {
    //        string result = "";
    //        try
    //        {
    //            _Context.Set<T>().Add(entity);
    //            _Context.SaveChanges();
    //        }
    //        catch (DbEntityValidationException ex)
    //        {
    //            result = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage)).ErrorMessage();

    //        }
    //        catch (Exception ex)
    //        {
    //            return ex.GetInnerException().ErrorMessage();
    //        }
    //        return result;
    //    }
    //    //public virtual string Update(T entity)
    //    //{
    //    //    string result = "";
    //    //    try
    //    //    {
    //    //        _Context.Entry(entity).State = EntityState.Modified;
    //    //        _Context.SaveChanges();
    //    //    }
    //    //    catch (DbEntityValidationException ex)
    //    //    {
    //    //        result = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage)).ErrorMessage();

    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        return ex.GetInnerException().ErrorMessage();
    //    //    }
    //    //    return result;
    //    //}

    //    public virtual string Delete(object id)
    //    {
    //        string result = "";
    //        try
    //        {
    //            T entity = _Context.Set<T>().Find(id);
    //            _Context.Set<T>().Remove(entity);
    //            _Context.SaveChanges();
    //        }
    //        catch (Exception ex)
    //        {
    //            return ex.GetInnerException().ErrorMessage();
    //        }
    //        return result;
    //    }
    //    public virtual T Find(object id)
    //    {
    //        return _Context.Set<T>().Find(id);
    //    }

    //    public void Dispose()
    //    {
    //        _Context.Dispose();
    //    }
    //}
}
