using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Datalayer.Services.Lib
{
    public class DataConnection :IDisposable
    {
        internal event Action<bool> OnConnected;
        internal event Action OnDisconnected;
        internal event Action<StateChangeEventArgs> OnConnectionStatusChanged;
        internal SqlDataAdapter DataAdapter { get; set; }
        public SqlDataAdapter _DataAdapter { get; set; }
        internal DataSet DS { get; set; }
        internal DataTable DT { get; set; }
        internal SqlCommandBuilder CommandBuilder { get; set; }
        internal SqlConnection Cn { get; private set; }
        internal SqlCommand SqlCommand { get; set; }
        internal SqlDataReader Reader { get; set; }
        public string strPortalMode { get; set; }
        //public enum PortalType
        //{
        //    AVA = 1,
        //    AVACommon = 2
        //}
        //public static string ConnectionStringForAVAPortal { get; set; }
        //public static string ConnectionStringForAVACommonPortal { get; set; }

        internal DataConnection(string con)
        {
            bool IsConnected = false;
            try
            {
                //if (_portalType == PortalType.AVA)
                //{
                Cn = new SqlConnection(con);
                Cn.StateChange -= new StateChangeEventHandler(Cn_StateChange);
                Cn.StateChange += new StateChangeEventHandler(Cn_StateChange);
                Cn.Open();
                IsConnected = true;
                //}
                //else if (_portalType == PortalType.AVACommon)
                //{
                //    Cn = new SqlConnection(ConnectionStringForAVACommonPortal);
                //    Cn.StateChange -= new StateChangeEventHandler(Cn_StateChange);
                //    Cn.StateChange += new StateChangeEventHandler(Cn_StateChange);
                //    Cn.Open();
                //    IsConnected = true;
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (OnConnected != null)
                OnConnected(IsConnected);
        }
        void Cn_StateChange(object sender, StateChangeEventArgs e)
        {
            if (OnConnectionStatusChanged != null)
                OnConnectionStatusChanged(e);
        }
        private void CloseConnection()
        {
            if (Cn != null && Cn.State == ConnectionState.Open)
            {
                if (SqlCommand != null)
                    SqlCommand.Dispose();

                if (DataAdapter != null)
                    DataAdapter.Dispose();

                if (DS != null)
                    DS.Dispose();

                if (CommandBuilder != null)
                    CommandBuilder.Dispose();

                if (Reader != null)
                    Reader.Dispose();
                Cn.Close();
                Cn.Dispose();

                if (OnDisconnected != null)
                    OnDisconnected();
            }
        }
        public void Dispose()
        {
            CloseConnection();
            GC.SuppressFinalize(this);
        }

    }
}
