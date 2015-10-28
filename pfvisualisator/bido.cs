using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace pfVisualisator
{
    public class bido
    {
        SqlConnection conq;

        public bido() {
            string constr = Properties.Settings.Default.BidoStringo;
            conq = new SqlConnection(constr);
            }

        public bido(string bistringo) {
            string constr = bistringo;
            conq = new SqlConnection(constr);
            }

        /// <summary>
        /// Модификация от 17 декабря 2014 года
        /// Заложен 17 декабря 2014 года
        /// </summary>
        /// <param name="gvido"></param>
        /// <returns></returns>
        public List<string> GetRepoStartSet(string gvido)
        {
            List<string> rlist = new List<string>();
            try
            {
                conq.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT pointa, Name, scriptFileName, Comment, query, Comto
                                                  FROM Reports
                                                  where uid = convert(uniqueidentifier, @pgvido)", conq);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@pgvido", gvido);
                SqlDataReader creader = cmd.ExecuteReader();

                if (creader.Read()) {  //Предполагаем и даже уверены, что строка всего одна, если она есть
                    string pointa = creader.IsDBNull(0) ? string.Empty : creader.GetString(0);
                    string namo = creader.GetString(1);
                    string scriptFileName = creader.GetString(2);
                    string commento = creader.GetString(3);
                    string quero = creader.GetString(4);
                    string descro = creader.IsDBNull(5) ? string.Empty : creader.GetString(5);
                    rlist.Add(namo);
                    rlist.Add(pointa + " " + namo);
                    rlist.Add(commento);
                    rlist.Add(descro);
                    rlist.Add(scriptFileName);
                    rlist.Add(quero);
                }
                creader.Close();
            }
            catch (SqlException exa)
            {
                Console.WriteLine(exa.Message);
            }
            finally
            {
                conq.Close();
            }
            return rlist;
        }

        /// <summary>
        /// Модификация от 15 мая 2015 года
        /// Заложен 15 мая 2015 года
        /// </summary>
        /// <param name="gvido"></param>
        /// <returns></returns>
        public List<string> GetRepoStartSetFrom4(string gvido) {
            string tablo = Properties.Settings.Default.RepoForoTablo;
            string quer = @"SELECT Name, scriptFileName, Comment, query, Description FROM " + tablo + @" where uid = convert(uniqueidentifier, @pgvido)";
            List<string> rlist = new List<string>();
            try {
                conq.Open();
                SqlCommand cmd = new SqlCommand(quer, conq);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@pgvido", gvido);
                SqlDataReader creader = cmd.ExecuteReader();

                if (creader.Read()) {  //Предполагаем и даже уверены, что строка всего одна, если она есть
                    string namo = creader.GetString(0);
                    string scriptFileName = creader.GetString(1);
                    string commento = creader.GetString(2);
                    string quero = creader.GetString(3);
                    string descro = creader.IsDBNull(4) ? string.Empty : creader.GetString(4);
                    rlist.Add(namo);
                    rlist.Add("Вольно " + namo);
                    rlist.Add(commento);
                    rlist.Add(descro);
                    rlist.Add(scriptFileName);
                    rlist.Add(quero);
                    }
                creader.Close();
            } catch (SqlException exa) {
                Console.WriteLine(exa.Message);
            } finally {
                conq.Close();
                }
            return rlist;
            }

        /// <summary>
        /// Модификация от 27 января 2015 года
        /// Заложен от 27 января 2015 года
        /// </summary>
        /// <returns></returns>
        public List<XElement> GetListRepoXML() {
            List<XElement> lxReto = new List<XElement>();
            try {
                conq.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT Gvido, Typo, Soda FROM OBG.Obago WHERE Typo = @pTypo
                                                  order by Id", conq);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@pTypo", leoType.OKPReport.ToString());
                SqlDataReader creader = cmd.ExecuteReader();

                Guid Gvida;
                string Typo;
                string Elemo;
                int i = 0;

                while (creader.Read()) {
                    Gvida = creader.GetGuid(0);
                    Typo = creader.GetString(1);
                    Elemo = creader.GetString(2);
                    i++;
                    XElement aa = XElement.Parse(Elemo);
                    lxReto.Add(aa);
                    }
                creader.Close();
                }
            catch (SqlException exa) {
                Console.WriteLine(exa.Message);
                }
            finally {
                conq.Close();
                }
            return lxReto;
            }

        /// <summary>
        /// Модификация от 26 января 2015 года
        /// Заложен от 26 января 2015 года
        /// </summary>
        /// <param name="pp"></param>
        public void PutLeoRecord(myleo pp)
        {
            try {
                conq.Open();
                SqlCommand cmd = new SqlCommand("OBG.ObagoAddRecord", conq);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pGvido", pp.LeoGuid);
                cmd.Parameters.AddWithValue("@pTypo", pp.LeoTypo.ToString()); 
                cmd.Parameters.AddWithValue("@pSXML", pp.LeoToXML().ToString());
                int spqvo = cmd.ExecuteNonQuery();
                }
            catch (SqlException exa) {
                Console.WriteLine(exa.Message);
                }
            finally {
                conq.Close();
                }
            }
        
        /// <summary>
        /// Модификация от 13 мая 2015 года
        /// Заложен 13 мая 2015 года
        /// </summary>
        /// <param name="pp"></param>
        /// <returns></returns>
        public byte[] GetRepoBinoData( Guid pp ) {
            byte[] reto = null;
            string tablo = Properties.Settings.Default.BinoRepoTablo;
            string quer = @"SELECT Bino FROM " + tablo + @" WHERE SUID = @pGuido";
            try {
                conq.Open();
                SqlCommand cmd = new SqlCommand(quer, conq);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@pGuido", pp);
                SqlDataReader creader = cmd.ExecuteReader();

                if (creader.Read()) {
                    reto = creader.GetValue(0) as byte[];
                    }
                creader.Close();
            } catch (SqlException exa) {
                Console.WriteLine(exa.Message);
            } finally {
                conq.Close();
                }
            return reto;
            }

        }
    }
