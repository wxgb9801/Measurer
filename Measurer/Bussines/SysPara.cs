using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace Measurer
{
    public static class SysPara
    {
        public static string ComPort = "Com3";
        public static int RefreshTime = 500;
        public static int setSN = 1;
        public static int Para_A = 1;
        public static int Para_B = 500;
        public static int Para_C = 1;
        public static int computeRatio = 1;
        public static bool AllowUpdateLength = false;
        static SysPara()
        {
            #region Load Parameter From Config
            if (ConfigurationManager.AppSettings.AllKeys.Contains("Port"))
            {
                ComPort = ConfigurationManager.AppSettings["Port"].ToString().Trim();
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("RefreshTime"))
            {
                int.TryParse(ConfigurationManager.AppSettings["RefreshTime"].ToString().Trim(), out RefreshTime);
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("SetSN"))
            {
                int.TryParse(ConfigurationManager.AppSettings["SetSN"].ToString().Trim(), out setSN);
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("Para_A"))
            {
                int.TryParse(ConfigurationManager.AppSettings["Para_A"].ToString().Trim(), out Para_A);
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("Para_B"))
            {
                int.TryParse(ConfigurationManager.AppSettings["Para_B"].ToString().Trim(), out Para_B);
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("Para_C"))
            {
                int.TryParse(ConfigurationManager.AppSettings["Para_C"].ToString().Trim(), out Para_C);
                
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("AllowUpdateLength"))
            {
                bool.TryParse(ConfigurationManager.AppSettings["AllowUpdateLength"].ToString(), out AllowUpdateLength);

            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("ComputeRatio"))
            {
                int.TryParse(ConfigurationManager.AppSettings["ComputeRatio"].ToString(), out computeRatio);

            }
            #endregion
        }
    }
}
