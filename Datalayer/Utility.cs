using System;
using System.Collections.Generic;
using System.Text;

namespace Datalayer
{
    public static class Utility
    {
       
    }
    public static class Extention
    {
        public static Exception GetInnerException(this Exception ex)
        {
            Exception exnew = ex;
            while (exnew.InnerException != null)
            {
                exnew = exnew.InnerException;
            }
            return exnew;
        }
        public static string ErrorMessage(this Exception ex)
        {
            return "Error: " + ex.Message;
        }
        public static string ErrorMessage(this string msg)
        {
            return "Error: " + msg;
        }
        public static string GetExactValue(this object obj)
        {
            if (obj == null) return null;
            else
            {
                string value = obj.ToString();
                switch (obj.GetType().Name)
                {
                    case "DateTime":
                        if (Convert.ToDateTime(obj).TimeOfDay == TimeSpan.Parse("00:00"))
                            value = Convert.ToDateTime(obj).ToString("MM/dd/yyyy");
                        //value = Convert.ToDateTime(obj).ToString("MM/dd/yyyy");
                        else value = Convert.ToDateTime(obj).ToString("MM/dd/yyyy hh:mm tt");

                        break;
                    case "TimeSpan":
                        value = Convert.ToDateTime(obj.ToString()).ToString("hh:mm tt");
                        break;
                    default:
                        break;
                }
                return value;
            }
        }
    }

    }
