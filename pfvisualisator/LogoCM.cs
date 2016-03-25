using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pfVisualisator
{
    public class LogoCM : Singleton<LogoCM>
    {
        private static int swa = 0;
        private static bool inya = false;

        private LogoCM() { }

        public static void OutString(string aa)
        {
            zOutString(aa);
        }

        private static void zOutString(string aa)
        {
            if (!inya) {
                initologo();
            }

        }

        private static void initologo()
        {
        }
    }

/*
    public class logoSalo
    {

        private RPDs.LogoOut funco;
        private StreamWriter vf = null;
        private SqlConnection conq = null;
        private SqlCommand cmd;
        private bool btype;

        public logoSalo(string confile, bool variant)
        {
            btype = variant;
            if (variant)
            {
                vf = new StreamWriter(confile, true);
                funco = FiledgInsertLogStroke;
            }
            else
            {
                conq = new SqlConnection();
                conq.ConnectionString = confile;
                conq.Open();
                cmd = new SqlCommand("rpd_insLogs", conq);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@sMesso", SqlDbType.NVarChar, 2048));
                funco = SQLdgInsertLogStroke;
            }
        }

        public void finish()
        {
            if (btype)
            {
                vf.Close();
            }
            else
            {
                conq.Close();
            }
        }

        private void SQLdgInsertLogStroke(string logstr)
        {
            cmd.Parameters[0].Value = logstr;
            cmd.ExecuteNonQuery();
        }

        private void FiledgInsertLogStroke(string logstr)
        {
            vf.WriteLine(logstr);
        }

        public RPDs.LogoOut Fu { get { return funco; } }
    }
*/
}
