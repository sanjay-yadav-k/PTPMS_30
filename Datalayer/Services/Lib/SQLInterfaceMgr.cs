using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Datalayer.Services.Lib
{
   public class SQLInterfaceMgr : IDisposable
    {
        public DataSet ExecuteQuery(string sqlQry, string con)
        {
            using (DataConnection dc = new DataConnection(con))
            {
                dc.SqlCommand = new SqlCommand(sqlQry, dc.Cn);
                dc.SqlCommand.CommandType = CommandType.Text;
                dc.DataAdapter = new SqlDataAdapter(dc.SqlCommand);
                dc.DS = new DataSet();

                dc.DataAdapter.Fill(dc.DS);

                return dc.DS;
            }
        }
        public DataTable PopulateDataTable(string sqlQry, string con)
        {
            using (DataConnection dc = new DataConnection(con))
            {
                dc.SqlCommand = new SqlCommand(sqlQry, dc.Cn);
                dc.SqlCommand.CommandType = CommandType.Text;
                dc.DataAdapter = new SqlDataAdapter(dc.SqlCommand);
                dc.DT = new DataTable();

                dc.DataAdapter.Fill(dc.DT);

                return dc.DT;
            }
        }
        public bool ExecuteNonQuery(string sqlQuery, string con)
        {
            try
            {
                using (DataConnection dc = new DataConnection(con))
                {
                    dc.SqlCommand = new SqlCommand(sqlQuery, dc.Cn);
                    dc.SqlCommand.CommandType = CommandType.Text;
                    int r = dc.SqlCommand.ExecuteNonQuery();
                    if (r == 0)
                        return false;
                    else
                        return true;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool ExecuteProcedure(string sqlProcName, SqlParameter[] sqlParam, Action<SqlParameterCollection> output, string con)
        {
            int result = 0;
            bool succeed = false;
            try
            {
                using (DataConnection dc = new DataConnection(con))
                {
                    dc.SqlCommand = new SqlCommand(sqlProcName, dc.Cn);
                    dc.SqlCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParam != null && sqlParam.Length > 0)
                        dc.SqlCommand.Parameters.AddRange(sqlParam);

                    result = dc.SqlCommand.ExecuteNonQuery();
                    if (output != null)
                    {
                        output(dc.SqlCommand.Parameters); // calls delegate
                    }
                    succeed = true;
                }

            }
            catch (Exception Ex)
            {
                result = 0;
                succeed = false;
            }
            return succeed; // not trapped in any exception
        }
        public SqlParameterCollection ExecuteProcedure(string sqlProcName, SqlParameter[] sqlParam, string con)
        {
            #region code comment out on 16.11.2018
            //SqlConnection con = null;
            //SqlCommand cmd = null;
            #endregion
            int result = 0;
            try
            {
                using (DataConnection dc = new DataConnection(con))
                {
                    dc.SqlCommand = new SqlCommand(sqlProcName, dc.Cn);
                    dc.SqlCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParam != null && sqlParam.Length > 0)
                        dc.SqlCommand.Parameters.AddRange(sqlParam);

                    result = dc.SqlCommand.ExecuteNonQuery();
                    return dc.SqlCommand.Parameters;
                }
                #region old code changed on 16.11.2018
                //using (con = new SqlConnection(DataConnection.ConnectionString))
                //{
                //    using (cmd = new SqlCommand(sqlProcName, con))
                //    {
                //        con.Open();

                //        cmd.CommandType = CommandType.StoredProcedure;
                //        if (sqlParam != null && sqlParam.Length > 0)
                //            cmd.Parameters.AddRange(sqlParam);
                //        result = cmd.ExecuteNonQuery();
                //        return cmd.Parameters;
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                result = 0;
            }
            #region Code commented on 16.11.2018
            //finally
            //{
            //    if (con != null && con.State == ConnectionState.Open)
            //        con.Close();
            //    cmd.Dispose();
            //}
            #endregion
            return null; // not trapped in any exception
        }
        public DataSet ExecuteStoreProc(string strProcName, SqlParameter[] sqlParam, string con)
        {
            DataSet ds = new DataSet();
            try
            {
                using (DataConnection dc = new DataConnection(con))
                {
                    dc.SqlCommand = new SqlCommand(strProcName, dc.Cn);
                    dc.SqlCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParam != null && sqlParam.Length > 0)
                        dc.SqlCommand.Parameters.AddRange(sqlParam);

                    dc.DataAdapter = new SqlDataAdapter();
                    dc.DataAdapter.SelectCommand = dc.SqlCommand;
                    dc.DataAdapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataTable ExecuteStoreProcedure(string strProcName, SqlParameter[] sqlParam, string con)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DataConnection dc = new DataConnection(con))
                {
                    dc.SqlCommand = new SqlCommand(strProcName, dc.Cn);
                    dc.SqlCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParam != null && sqlParam.Length > 0)
                        dc.SqlCommand.Parameters.AddRange(sqlParam);

                    dc.DataAdapter = new SqlDataAdapter();
                    dc.DataAdapter.SelectCommand = dc.SqlCommand;
                    dc.DataAdapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
