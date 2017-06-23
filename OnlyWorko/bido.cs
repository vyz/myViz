using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace OnlyWorko {
    public sealed class bido : IDisposable {
        SqlConnection conq;
        bool isDisposed = false;

        public bido() {
            string constr = Properties.Settings.Default.BidoGamos;
            conq = new SqlConnection(constr);
            }

        public bido(string bistringo) {
            string constr = bistringo;
            conq = new SqlConnection(constr);
            }

        /// <summary>
        /// Модификация от 24 ноября 2015 года
        /// Заложен 24 ноября 2015 года
        /// </summary>
        /// <param name="aa"></param>
        public bido(clasterType aa)
        {
            string constr = string.Empty;
            if (aa == clasterType.Gamo)
            {
                constr = Properties.Settings.Default.BidoGamos;
                }
            conq = new SqlConnection(constr);
            }

        /// <summary>
        /// Модификация от 22 июня 2017 года
        /// Заложен 22 июня 2017 года
        /// </summary>
        public void Dispose() {
            if (isDisposed) {
                return;
                }
            if (null != conq) {
                conq.Close();
                }
            isDisposed = true;
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
            SqlCommand cmd = null;
            try
            {
                conq.Open();
                cmd = new SqlCommand(@"SELECT pointa, Name, scriptFileName, Comment, query, Comto
                                                  FROM RepoTri
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
                cmd.Dispose();
                conq.Close();
            }
            return rlist;
        }

        /// <summary>
        /// Модификация от 26 января 2015 года
        /// Заложен от 26 января 2015 года
        /// </summary>
        /// <param name="pp"></param>
        public void PutLeoRecord(myclast pp)
        {
            SqlCommand cmd = null;
            try {
                conq.Open();
                cmd = new SqlCommand("OBG.ObagoAddRecord", conq);
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
                cmd.Dispose();
                conq.Close();
                }
            }

        /// <summary>
        /// Модификация от 23 декабря 2015 года
        /// Заложен 22 декабря 2015 года
        /// </summary>
        /// <param name="pp"></param>
        /// <returns></returns>
        public int PutGamoGam(vGamo pp) {
            int reto = 0;
            SqlCommand cmd = null;
            try
            {
                conq.Open();
                cmd = new SqlCommand("Gam.pPutGamosiko", conq);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pgvido", pp.LeoGuid);
                cmd.Parameters.AddWithValue("@pwplayer", pp.White);
                cmd.Parameters.AddWithValue("@pbplayer", pp.Black);
                cmd.Parameters.AddWithValue("@preso", (pp.Result == "1/2-1/2" ? 3 : (pp.Result == "1-0" ? 1 : (pp.Result == "0-1" ? 2 : 0))));
                DateTime dtt = new DateTime(1769, 8, 15);
                if (!DateTime.TryParse(pp.Date, out dtt)) {
                    int idt = 1769;
                    if (!int.TryParse(pp.Date.Substring(0, 4), out idt)) {
                        dtt = new DateTime(1769, 8, 15);
                    } else {
                        dtt = new DateTime(idt, 1, 1);
                        }
                    }
                cmd.Parameters.AddWithValue("@pdato", dtt);
                cmd.Parameters.AddWithValue("@pcompeto", pp.Event);
                cmd.Parameters.AddWithValue("@peco", pp.ECO);
                int tinto = 0;
                if( !int.TryParse(pp.PlyCount, out tinto) ) {
                    tinto = 0;
                    }
                cmd.Parameters.AddWithValue("@pplyco", tinto);
                if (!int.TryParse(pp.WElo, out tinto)) {
                    tinto = 0;
                    }
                cmd.Parameters.AddWithValue("@pwelo", tinto);
                if (!int.TryParse(pp.BElo, out tinto)) {
                    tinto = 0;
                    }
                cmd.Parameters.AddWithValue("@pbelo", tinto);
                cmd.Parameters.AddWithValue("@psito", pp.Site);
                cmd.Parameters.AddWithValue("@prondo", pp.Round);
                cmd.Parameters.AddWithValue("@pflStartPos", pp.iflagStartPos);
                cmd.Parameters.AddWithValue("@pflTimoshky", pp.iflagTiming);
                cmd.Parameters.AddWithValue("@pflCommento", pp.iflagCommto);
                cmd.Parameters.AddWithValue("@pMovaText", pp.OnlyMova);
                cmd.Parameters.AddWithValue("@pAddParo", pp.AddAtr);
                cmd.Parameters.AddWithValue("@pDescripto", pp.Descripto);
                SqlParameter outo = new SqlParameter("@otypo", 0);
                outo.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outo);
                int spqvo = cmd.ExecuteNonQuery();
                if (!int.TryParse(outo.Value.ToString(), out reto)) {
                    reto = -1;
                    }
            } catch (SqlException exa) {
                Console.WriteLine(exa.Message);
            } finally {
                cmd.Dispose();
                conq.Close();
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 23 декабря 2015 года
        /// Заложен 23 декабря 2015 года
        /// </summary>
        /// <param name="gg"></param>
        /// <param name="pp"></param>
        public void PutGamoPozo(Guid gg, pozo pp) {
            SqlCommand cmd = null;
            try
            {
                conq.Open();
                cmd = new SqlCommand("Gam.pPutPosoSet", conq);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pgvido", gg);
                cmd.Parameters.AddWithValue("@pnumo", pp.NumberMove);
                cmd.Parameters.AddWithValue("@pwhom", pp.IsQueryMoveWhite ? 1 : 0);
                cmd.Parameters.AddWithValue("@pvanpos1", pp.VanBoardo[0]);
                cmd.Parameters.AddWithValue("@pvanpos2", pp.VanBoardo[1]);
                cmd.Parameters.AddWithValue("@ptwa", pp.TwaFeno);
                int spqvo = cmd.ExecuteNonQuery();
            } catch (SqlException exa) {
                Console.WriteLine(exa.Message);
            } finally {
                cmd.Dispose();
                conq.Close();
                }
            }

        /// <summary>
        /// Модификация от 23 декабря 2015 года
        /// Заложен 23 декабря 2015 года
        /// </summary>
        /// <param name="gg"></param>
        /// <param name="pp"></param>
        public void PutGamoTimo(Guid gg, gTimo pp) {
            SqlCommand cmd = null;
            try
            {
                conq.Open();
                cmd = new SqlCommand("Gam.pPutTimoshka", conq);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pgvido", gg);
                cmd.Parameters.AddWithValue("@pnumo", pp.NumberForMove);
                cmd.Parameters.AddWithValue("@pwhom", pp.ColorTimoIsWhite ? 1 : 0);
                cmd.Parameters.AddWithValue("@ptima", pp.TimoValue);
                int spqvo = cmd.ExecuteNonQuery();
            } catch (SqlException exa) {
                Console.WriteLine(exa.Message);
            } finally {
                cmd.Dispose();
                conq.Close();
                }
            }

        }
    }
