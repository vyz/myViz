using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace pfVisualisator
{
    /// <summary>
    /// Модификация от 14 апреля 2016 года
    /// Заложен 4 октября 2015 года
    /// </summary>
    public class vGamo : myleo, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private int movacount;
        private Gamo etalo;
        private pozo currento;
        private List<VarQvant> lvaqv;


        /// <summary>
        /// Модификация от 4 октября 2015 года
        /// Заложен 4 октября 2015 года
        /// </summary>
        public vGamo()
            : base(leoType.Gamo) {

            string[] akva = new string[16];
            ltexto = akva.ToList();
            }

        /// <summary>
        /// Модификация от 14 апреля 2016 года
        /// Заложен 4 октября 2015 года
        /// </summary>
        /// <param name="gm"></param>
        public vGamo(Gamo gm)
            : this() {
                namo = gm.GamerWhite + ((gm.EloWhite.Length > 0) ? " (" + gm.EloWhite + ")" : "") + " - " + gm.GamerBlack + ((gm.EloBlack.Length > 0) ? " (" + gm.EloBlack + ")" : "");
                bignamo = namo + " " + gm.Date + " " + gm.Resulto + " " + gm.ECO;
                White = gm.GamerWhite;
                Black = gm.GamerBlack;
                Result = gm.Resulto;
                Date = gm.Date;
                Event = gm.Evento;
                ECO = gm.ECO;
                PlyCount = gm.Qavo;
                WElo = gm.EloWhite;
                BElo = gm.EloBlack;
                Site = gm.Sito;
                Round = gm.Roundo;
                Fen = gm.Fen;
                if (gm.Qavo.Length > 0) {
                    movacount = int.Parse(gm.Qavo) / 2;
                } else { 
                    movacount = 0; 
                    }
                OnlyMova = gm.VanStrokeMovaRegion();
                AddAtr = gm.VanStrokeRestAttributes();
                Descripto = string.Empty;
                Timingo = gm.VanStrokeTimingo();
                etalo = gm;
                lvaqv = gm.ListVaroCom;
                }

        /// <summary>
        /// Модификация от 19 ноября 2015 года
        /// Заложен 11 октября 2015 года
        /// </summary>
        /// <param name="Elemo"></param>
        public vGamo(XElement Elemo)
            : base(Elemo) {
                if (PlyCount.Length > 0) {
                    movacount = int.Parse(PlyCount) / 2;
                } else {
                    movacount = 0;
                    }
                if (ltexto.Count < 16) {
                    ltexto.Add(string.Empty);
                    }
                }

        /// <summary>
        /// Модификация от 15 апреля 2016 года
        /// Заложен 15 апреля 2016 года
        /// </summary>
        /// <param name="xel"></param>
        public override void Dopico(XElement xel) {
            foreach (XElement aa in xel.Elements("VarQuant")) {
                if (null == lvaqv) { lvaqv = new List<VarQvant>(); }
                lvaqv.Add(new VarQvant(aa));
                }
            }   

        /// <summary>
        /// Модификация от 15 апреля 2016 года
        /// Заложен 15 апреля 2016 года
        /// </summary>
        /// <returns></returns>
        public override XElement Dopico() {
            XElement reto = new XElement("Dopico");
            if (null != lvaqv) {
                foreach (VarQvant aa in lvaqv) {
                    reto.Add(aa.XMLOut());
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 29 апреля 2016 года
        /// Заложен 29 апреля 2016 года
        /// </summary>
        public void EtaloCreate() {
            if (etalo == null) {
                etalo = new Gamo(this);
                lvaqv = etalo.ListVaroCom;
                }
            }

        /// <summary>
        /// Модификация от 16 ноября 2015 года
        /// Заложен 8 ноября 2015 года
        /// </summary>
        /// <param name="offseto"></param>
        /// <returns></returns>
        public pozo GetPozoOnOffset(int offseto) {
            pozo reto = null;
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            if( offseto <= 0 ) { //Это стартовая позиция - самое начало. Необязательно нулевая позиция
                reto = etalo.GetFirstPozo();                
            } else {
                string issledo = this.OnlyMova.Substring(0, offseto);
                if (issledo.Length == 0) {
                    throw new VisualisatorException("vGamo-GetPozoOnOffset !!! Не существует набора ходов");
                } else {
                    string patnumbermove = @"\d+\.";
                    string patmovesymbol = @"[KQRBNa-hO][a-h1-8\-x=\+#QRBN]+";
                    Match aa = Regex.Match(issledo, patnumbermove, RegexOptions.RightToLeft);
                    if (aa.Index > 0) {
                        string zz = aa.Value;
                        string zp = zz.Substring(0, zz.Length - 1);
                        int nmove = int.Parse(zp);
                        zz = issledo.Substring(aa.Index + zz.Length).TrimStart();
                        MatchCollection bbc = Regex.Matches(zz, patmovesymbol);
                        zp = (bbc.Count == 0) ? "w" : "b";
                        reto = etalo.GetPozoAfterMove(nmove, zp);
                    } else {
                        reto = etalo.GetFirstPozo();
                        }
                    }
                }
            if (reto != null) {
                currento = reto;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 23 ноября 2015 года
        /// Заложен 22 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public pozo GetNextPozo() {
            pozo reto = null;
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            if (currento == null) {
                currento = etalo.GetFirstPozo();
                }
            reto = etalo.GetNextPozo(currento);
            if (reto != null) {
                currento = reto;
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 23 ноября 2015 года
        /// Заложен 23 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public pozo GetPrevPozo() {
            pozo reto = null;
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            if (currento == null) {
                currento = etalo.GetFirstPozo();
                reto = currento;
            } else {
                reto = etalo.GetPrevPozo(currento);
                if (reto != null) {
                    currento = reto;
                    }
                }
            return reto;
            }

        /// <summary>
        /// Модификация от 24 ноября 2015 года
        /// Заложен 18 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public bool SaveInDB() {
            bool reto = false;
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            bido dbisa = new bido(leoType.Gamo);
                //bido dbisa = new bido(); Проверка для заброса Leo-объекта в базу. Прошла на ура.
                //dbisa.PutLeoRecord(this);
            int vnutro = dbisa.PutGamoGam(this);
            if(vnutro == 1) { //Этой гамы еще не было в базе. Вводим её внутренности 
                bool firstpropusk = etalo.StartoFlag;
                foreach (pozo aa in etalo.ListoPozo) {
                    if(firstpropusk) {
                        firstpropusk = false;
                        continue;
                        }
                    dbisa.PutGamoPozo(LeoGuid, aa);
                    }
                if(etalo.TimingFlag) {
                    foreach(gTimo aa in etalo.ListoTimo) {
                        dbisa.PutGamoTimo(LeoGuid, aa);
                        }
                    }
                }
            return reto;
            }

#region--------------------------Свойства объекта-----------------------------------------
        public string Namo { get { return namo; } set { namo = value; } }
        public string BigNamo { get { return bignamo; } set { bignamo = value; } }
        public string White { get { return ltexto[0]; } set { ltexto[0] = value; } }
        public string Black { get { return ltexto[1]; } set { ltexto[1] = value; } }
        public string Result { get { return ltexto[2]; } set { ltexto[2] = value; } }
        public string Date { get { return ltexto[3]; } set { ltexto[3] = value; } }
        public string Event { get { return ltexto[4]; } set { ltexto[4] = value; } }
        public string ECO { get { return ltexto[5]; } set { ltexto[5] = value; } }
        public string PlyCount { get { return ltexto[6]; } set { ltexto[6] = value; } }
        public string WElo { get { return ltexto[7]; } set { ltexto[7] = value; } }
        public string BElo { get { return ltexto[8]; } set { ltexto[8] = value; } }
        public string Site { get { return ltexto[9]; } set { ltexto[9] = value; } }
        public string Round { get { return ltexto[10]; } set { ltexto[10] = value; } }
        public string Fen { get { return ltexto[11]; } set { ltexto[11] = value; } }
        public string OnlyMova { get { return ltexto[12]; } set { ltexto[12] = value; } }
        public string AddAtr { get { return ltexto[13]; } set { ltexto[13] = value; } }
        public string Descripto { get { return ltexto[14]; } set { ltexto[14] = value; } }
        public string Timingo { get { return ltexto[15]; } set { ltexto[15] = value; } }
        public int movoQvo { get { return movacount; } set { movacount = value; } }
        public myTago Tago { get { return tago; } }
        public string TagoTextStroke { get { return tago.TStroke; } }
        public string TimoView { get { 
            if (etalo == null) {
                etalo = new Gamo(this);
                }
            return etalo.VanStrokeTimoForView(); 
            } }
        public string BigAttroPerechen
        {
            get
            {
                return "Date - " + Date + Environment.NewLine + "Event - " + Event + Environment.NewLine + "ECO - " + ECO + Environment.NewLine
                       + "PlyCount - " + PlyCount + Environment.NewLine + "WElo - " + WElo + Environment.NewLine + "BElo - " + BElo + Environment.NewLine
                       + "Site - " + Site + Environment.NewLine + "Round - " + Round + Environment.NewLine + "AddAtr - " + AddAtr;
            }
        }
        public int iflagStartPos { get { return (etalo != null) ? ((etalo.StartoFlag) ? 1 : 0) : 0; } }
        public int iflagTiming { get { return (etalo != null) ? ((etalo.TimingFlag) ? 1 : 0) : 0; } }
        public int iflagCommto { get { return (etalo != null) ? ((etalo.CommtoFlag) ? 1 : 0) : 0; } }
        public int iflagVario { get { return (etalo != null) ? ((etalo.VariantoFlag) ? 1 : 0) : 0; } }
        public List<VarQvant> ListoVarCom { get { return lvaqv; } }
        public List<Mova> ListoMovo { get { return (etalo != null) ? etalo.ListoMovo : null; } }
        public Gamo Gamma { get { return etalo; } }

#endregion-----------------------Свойства объекта-----------------------------------------

        private void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
                }
            }

        /// <summary>
        /// Модификация от 11 декабря 2016 года
        /// Заложен 11 ноября 2015 года
        /// </summary>
        /// <returns></returns>
        public static vGamo CreateExemplarusForLife() {
            vGamo reto = new vGamo();
            reto.Namo = "Nikitenko, Mihail,  - Vorontsov, Pavlo, ";
            reto.BigNamo = "Nikitenko, Mihail,  - Vorontsov, Pavlo,   1-0 C10";
            reto.White = "Nikitenko, Mihail, ";
            reto.Black = "Vorontsov, Pavlo, ";
            reto.Result = "1-0";
            reto.Date = "2014.05.18";
            reto.Event = "Vanya Somov Memorial 2014";
            reto.ECO = "C10";
            reto.PlyCount = "75";
            reto.WElo = "2199";
            reto.BElo = "2413";
            reto.Site = "Kirishi (Russia)";
            reto.Round = "5.5";
            reto.Fen = "";
            reto.movoQvo = int.Parse(reto.PlyCount) / 2;
            reto.OnlyMova = @"1. e4 e6 2. d4 d5 3. Nd2 dxe4 4. Nxe4 Bd7 5. Nf3 Bc6 6. Bd3 Nd7 7. c3 Ngf6 8.
Qc2 Bxe4 9. Bxe4 Nxe4 10. Qxe4 c6 11. Qe2 Nf6 12. O-O Be7 13. c4 O-O 14. b3 Qa5
15. Qe5 Qxe5 16. dxe5 Nd7 17. Rd1 Rfd8 18. Be3 a5 19. Kf1 Kf8 20. Ke2 f6 21.
exf6 gxf6 22. Rd4 e5 23. Rh4 Kg8 24. Rg4+ Kf7 25. Rh4 Nf8 26. Rd1 Rd7 27. g4
Kg8 28. Rh5 Ne6 29. Rg1 e4 30. Nh4 a4 31. b4 Bxb4 32. Rh6 Nd4+ 33. Bxd4 Rxd4
34. g5 fxg5 35. Rxg5+ Kh8 36. Ng6+ Kg7 37. Ne5+ Kf8 38. Rxh7 1-0";
            reto.AddAtr = @"[Board] - [1]
[WhiteTitle] - [GM]
[WhiteCountry] - [BUL]
[WhiteFideId] - [2900084]
[WhiteEloChange] - [1]
[BlackTitle] - [IM]
[BlackCountry] - [NGR]
[BlackFideId] - [8500258]
[BlackEloChange] - [-2]";
            reto.Descripto = "Искусственный член";

            return reto;
            }

        /// <summary>
        /// Модификация от 20 октября 2016 года
        /// Заложен 20 октября 2016 года
        /// </summary>
        /// <returns>Экземпляр для отладки и тестирования</returns>
        public static vGamo CreateExempWithVariantoAndComments() {
            XElement aa = new XElement("Leo", new XAttribute("lid", "9b4ff681-7e77-40e6-8b3c-d2a5b776319a"), new XAttribute("typo", "Gamo"),
                new XElement("Namo", "Geller, Efim P - Euwe, Max"),
                new XElement("BigNamo", "Geller, Efim P - Euwe, Max 1953.08.31 0-1 E28"),
                new XElement("Parameters",
                    new XElement("TParo", "Geller, Efim P"),
                    new XElement("TParo", "Euwe, Max"),
                    new XElement("TParo", "0-1"),
                    new XElement("TParo", "1953.08.31"),
                    new XElement("TParo", "candidates tournament"),
                    new XElement("TParo", "E28"),
                    new XElement("TParo", "52"),
                    new XElement("TParo", ""),
                    new XElement("TParo", ""),
                    new XElement("TParo", "Цюрих"),
                    new XElement("TParo", "2"),
                    new XElement("TParo", ""),
                    new XElement("TParo", @"1. d4 Nf6 2. c4 e6 3. Nc3 Bb4 4. e3 c5 5. a3 Bxc3+ 6. bxc3 b6 7. Bd3 Bb7 8. f3
Nc6 9. Ne2 O-O 10. O-O Na5 11. e4 Ne8 12. Ng3 cxd4 13. cxd4 Rc8 14. f4 Nxc4 15.
f5 f6 16. Rf4 b5 17. Rh4 Qb6 18. e5 Nxe5 19. fxe6 Nxd3 20. Qxd3 Qxe6 21. Qxh7+
Kf7 22. Bh6 Rh8 23. Qxh8 Rc2 24. Rc1 Rxg2+ 25. Kf1 Qb3 26. Ke1 Qf3 0-1"),
                    new XElement("TParo", @"[Annotator] - [Bronstein]
[EventDate] - [1953.08.31]"),
                    new XElement("TParo", ""),
                    new XElement("TParo", "")
                    ),
                new XElement("Dopico",
                    new XElement("VarQuant", new XAttribute("VarNumo", "0"),
                        new XElement("Commento", "Одна из лучших партий турнира, получившая приз за красоту. Белые начали сильную атаку на короля, пожертвовав пешку с4. У Геллера были все шансы на успех, если бы черные по традиции контратаковали на ферзевом фланге. Однако Эйве реализовал две замечательные идеи: 1) использование коммуникаций ферзевого фланга для атаки на королевском; 2) завлечение сил противника в свой глубокий тыл с целью отвлечь их от защиты короля.  Крайне интересно проследить, как белые фигуры прорывались все дальше и дальше в лобовой атаке на короля и зашли, наконец, туда, откуда нет возврата, а черные в это время перебрасывали свои силы обходными путями.")
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "15"),
                        new XElement("Commento", "Небольшая, но существенная тонкость дебюта: в результате перестановки пары ходов (b7-b6 и Сс8-b7 вместо обычных Кb8-сб и 0-0) белым, не сумевшим вовремя отреагировать правильным 7.Ке2, приходится затратить лишний темп на подготовку еЗ-е4. Подобными мелочами нельзя пренебрегать, но не следует их и переоценивать. Иной раз говорят: преимущество белых состоит в праве первого хода; если они потеряли темп, то преимущество чуть ли не автоматически должно перейти к черным. Однако преимущество игры белыми практически сводится к тому, что у них больше возможностей выбрать план игры по своему вкусу, а когда игра уже сложилась, потеря одного темпа не всегда бывает столь важна.")
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "22"),
                        new XElement("Commento", "Черные отвели коня, чтобы  не допустить связку Ccl-g5 и на ход f3-f4 ответить f7-f5, блокируя королевский фланг. Поэтому белые, прежде чем надвигать пешку f, берут под контроль поле f5. Защищать пешку с4 бессмысленно: она была обречена уже 5-м ходом белых.")
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "31"),
                        new XElement("Commento", "Атака становится угрожающей. Предыдущий ход черных был необходим, так как белые намеревались продвинуть пешку на f6, а в ответ на Ке8:f6 все же связать коня и напасть на короля соединенными силами ферзя, ладьи и легких фигур. Но и теперь белым нужно всего два хода, чтобы перебросить ферзя и ладью на линию h, после чего, кажется, ничто уже не спасёт короля чёрных. Но Эйве нелегко смутить. Вспомним, что он в своей жизни сыграл более 70 партий с Алехиным, наиболее грозным из атакующих шахматистов нашего века.")
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "32"),
                        new XElement("Commento", "Начало замечательного плана. Ясно, что любые оборонительные маневры на королевском фланге при помощи фигур, имеющих ничтожный радиус действия - Лf8-f7, Фd8-е7 и т.п., заранее обречены на неудачу. Но у чёрных есть другой ресурс - контратака! Слон Ь7, ладья с8 и конь с4 занимают хорошие исходные позиции, остается перебросить ферзя. Основа контратаки - преимущество чёрных на центральных полях. Сыграв b6-b5, чёрные еще больше укрепляют положение коня и открывают дорогу ферзю на b6. Всё же впечатление таково, что их операции запаздывают...")
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "34"),
                        new XElement("Commento", "Связывая ферзя защитой пешки d4, чёрные препятствуют программному ходу Od1-h5. Впрочем, при 17.Фh5 Фb6 18.Ке2 Ке5 получался эхо-вариант - белые не успевали сыграть Лf4-h4.")
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "39"),
                        new XElement("Vario",
                            new XElement("Feno", "2r1nrk1/pb1p2pp/1q2Pp2/1p6/3P3R/P2n2N1/6PP/R1BQ2K1 w - - 0 20"),
                            new XElement("Tshepa", "20. exd7 Qe6"),
                            new XElement("VarQuant", new XAttribute("VarNumo", "0"),
                                new XElement("Commento", "Все ходы белых требовали тщательного и точного расчета. Сейчас, например, не годилось естественное")
                                ),
                            new XElement("Mova", new XAttribute("Fromo", "43"), new XAttribute("Tomo", "52"), new XAttribute("Coala", "W"),
                                new XElement("Zver", "None, Pawn"),
                                new XElement("Typon", "PieceEaten"),
                                new XElement("Nota", "exd7")
                                ),
                            new XElement("VarQuant", new XAttribute("VarNumo", "1"),
                                new XElement("Commento", "из-за")
                                ),
                            new XElement("Mova", new XAttribute("Fromo", "46"), new XAttribute("Tomo", "43"), new XAttribute("Coala", "B"),
                                new XElement("Zver", "None, Queen, Black"),
                                new XElement("Typon", "Normal"),
                                new XElement("Nota", "Qe6")
                                )
                            )
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "41"),
                        new XElement("Commento", "Итак, ценой небольших потерь белые все же прорвались. Положение чёрных снова кажется критическим.")
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "44"),
                        new XElement("Commento", "Если 16-й ход чёрных был началом стратегического плана контратаки, то жертва ладьи - центральный тактический удар, имеющий целью заманить ферзя подальше, отвлечь от поля с2 и напасть в это время на короля.")
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "46"),
                        new XElement("Commento", "Грозит мат в несколько ходов: 24...Л:g2+, 25...Фс4+ и т.д. Тщательный анализ, для которого потребовалась не одна неделя, показал, что белые могли спастись от мата, сделав несколько единственных и очень трудных ходов.")
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "47"),
                        new XElement("Vario",
                            new XElement("Feno", "4n2Q/pb1p1kp1/4qp1B/1p6/3P3R/P5N1/2r3PP/R5K1 w - - 1 24"),
                            new XElement("Tshepa", "24. d5 Qb6+ 25. Kh1 Qf2 26. Rg1 Bxd5 27. Re4"),
                            new XElement("VarQuant", new XAttribute("VarNumo", "0"),
                                new XElement("Commento", "Надо было играть")
                                ),
                            new XElement("Mova", new XAttribute("Fromo", "28"), new XAttribute("Tomo", "36"), new XAttribute("Coala", "W"),
                                new XElement("Zver", "None, Pawn"),
                                new XElement("Typon", "Normal"),
                                new XElement("Nota", "d5")
                                ),
                            new XElement("VarQuant", new XAttribute("VarNumo", "1"),
                                new XElement("Commento", "В случае")
                                ),
                            new XElement("Mova", new XAttribute("Fromo", "43"), new XAttribute("Tomo", "46"), new XAttribute("Coala", "B"),
                                new XElement("Zver", "None, Queen, Black"),
                                new XElement("Typon", "Normal"),
                                new XElement("Nota", "Qb6+")
                                ),
                            new XElement("VarQuant", new XAttribute("VarNumo", "2"),
                                new XElement("Vario",
                                    new XElement("Feno", "4n2Q/pb1p1kp1/4qp1B/1p1P4/7R/P5N1/2r3PP/R5K1 b - - 0 24"),
                                    new XElement("Tshepa", "24. ... Bxd5 25. Rd4"),
                                    new XElement("VarQuant", new XAttribute("VarNumo", "0"),
                                        new XElement("Commento", "а если сразу")
                                        ),
                                    new XElement("Mova", new XAttribute("Fromo", "54"), new XAttribute("Tomo", "36"), new XAttribute("Coala", "B"),
                                        new XElement("Zver", "None, Bishop, Black"),
                                        new XElement("Typon", "PieceEaten"),
                                        new XElement("Nota", "Bxd5")
                                        ),
                                    new XElement("VarQuant", new XAttribute("VarNumo", "1"),
                                        new XElement("Commento", "то не")
                                        ),
                                    new XElement("Mova", new XAttribute("Fromo", "24"), new XAttribute("Tomo", "28"), new XAttribute("Coala", "W"),
                                        new XElement("Zver", "None, Rook"),
                                        new XElement("Typon", "Normal"),
                                        new XElement("Nota", "Rd4")
                                        ),
                                    new XElement("VarQuant", new XAttribute("VarNumo", "2"),
                                        new XElement("Vario",
                                            new XElement("Feno", "4n2Q/p2p1kp1/4qp1B/1p1b4/7R/P5N1/2r3PP/R5K1 w - - 0 25"),
                                            new XElement("Tshepa", "25. Rd1 Rxg2+ 26. Kf1 gxh6 27. Rxh6"),
                                            new XElement("VarQuant", new XAttribute("VarNumo", "0"),
                                                new XElement("Commento", "а только")
                                                ),
                                            new XElement("Mova", new XAttribute("Fromo", "7"), new XAttribute("Tomo", "4"), new XAttribute("Coala", "W"),
                                                new XElement("Zver", "None, Rook"),
                                                new XElement("Typon", "Normal"),
                                                new XElement("Nota", "Rd1")
                                                ),
                                            new XElement("VarQuant", new XAttribute("VarNumo", "1"),
                                                new XElement("Commento", "и после")
                                                ),
                                            new XElement("Mova", new XAttribute("Fromo", "13"), new XAttribute("Tomo", "9"), new XAttribute("Coala", "B"),
                                                new XElement("Zver", "None, Rook, Black"),
                                                new XElement("Typon", "PieceEaten"),
                                                new XElement("Nota", "Rxg2+")
                                                ),
                                            new XElement("Mova", new XAttribute("Fromo", "1"), new XAttribute("Tomo", "2"), new XAttribute("Coala", "W"),
                                                new XElement("Zver", "None, King"),
                                                new XElement("Typon", "Normal"),
                                                new XElement("Nota", "Kf1")
                                                ),
                                            new XElement("Mova", new XAttribute("Fromo", "49"), new XAttribute("Tomo", "40"), new XAttribute("Coala", "B"),
                                                new XElement("Zver", "None, Pawn, Black"),
                                                new XElement("Typon", "PieceEaten"),
                                                new XElement("Nota", "gxh6+")
                                                ),
                                            new XElement("VarQuant", new XAttribute("VarNumo", "4"),
                                                new XElement("Commento", "не годится ни")
                                                ),
                                            new XElement("Mova", new XAttribute("Fromo", "24"), new XAttribute("Tomo", "40"), new XAttribute("Coala", "W"),
                                                new XElement("Zver", "None, Rook"),
                                                new XElement("Typon", "PieceEaten"),
                                                new XElement("Nota", "Rxh6")
                                                ),
                                            new XElement("VarQuant", new XAttribute("VarNumo", "5"),
                                                new XElement("Vario",
                                                    new XElement("Feno", "4n2Q/p2p1k2/4qp1p/1p1b4/7R/P5N1/6rP/3R1K2 w - - 0 27"),
                                                    new XElement("Tshepa", "27. Rxd5"),
                                                    new XElement("VarQuant", new XAttribute("VarNumo", "0"),
                                                        new XElement("Commento", "ни")
                                                        ),
                                                    new XElement("Mova", new XAttribute("Fromo", "4"), new XAttribute("Tomo", "36"), new XAttribute("Coala", "W"),
                                                        new XElement("Zver", "None, Rook"),
                                                        new XElement("Typon", "PieceEaten"),
                                                        new XElement("Nota", "Rxd5")
                                                        )
                                                    )
                                                ),
                                            new XElement("VarQuant", new XAttribute("VarNumo", "5"),
                                                new XElement("Vario",
                                                    new XElement("Feno", "4n2Q/p2p1k2/4qp1p/1p1b4/7R/P5N1/6rP/3R1K2 w - - 0 27"),
                                                    new XElement("Tshepa", "27. Qxh6"),
                                                    new XElement("VarQuant", new XAttribute("VarNumo", "0"),
                                                        new XElement("Commento", ", а нужно снова сделать единственный ход")
                                                        ),
                                                    new XElement("Mova", new XAttribute("Fromo", "56"), new XAttribute("Tomo", "40"), new XAttribute("Coala", "W"),
                                                        new XElement("Zver", "None, Queen"),
                                                        new XElement("Typon", "PieceEaten"),
                                                        new XElement("Nota", "Qxh6")
                                                        ),
                                                    new XElement("VarQuant", new XAttribute("VarNumo", "1"),
                                                        new XElement("Commento", "И все равно у черных остается слон и две пешки за ладью, что при открытом положении вражеского короля дает им хорошие шансы на победу. Понятно, что у Геллера за доской не было практической возможности найти все эти ходы.")
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    )
                                ),
                            new XElement("Mova", new XAttribute("Fromo", "1"), new XAttribute("Tomo", "0"), new XAttribute("Coala", "W"),
                                new XElement("Zver", "None, King"),
                                new XElement("Typon", "Normal"),
                                new XElement("Nota", "Kh1")
                                ),
                            new XElement("Mova", new XAttribute("Fromo", "46"), new XAttribute("Tomo", "10"), new XAttribute("Coala", "B"),
                                new XElement("Zver", "None, Queen, Black"),
                                new XElement("Typon", "Normal"),
                                new XElement("Nota", "Qf2")
                                ),
                            new XElement("Mova", new XAttribute("Fromo", "7"), new XAttribute("Tomo", "1"), new XAttribute("Coala", "W"),
                                new XElement("Zver", "None, Rook"),
                                new XElement("Typon", "Normal"),
                                new XElement("Nota", "Rg1")
                                ),
                            new XElement("Mova", new XAttribute("Fromo", "54"), new XAttribute("Tomo", "36"), new XAttribute("Coala", "B"),
                                new XElement("Zver", "None, Bishop, Black"),
                                new XElement("Typon", "PieceEaten"),
                                new XElement("Nota", "Bxd5")
                                ),
                            new XElement("VarQuant", new XAttribute("VarNumo", "6"),
                                new XElement("Commento", "они спасались ходом")
                                ),
                            new XElement("Mova", new XAttribute("Fromo", "24"), new XAttribute("Tomo", "27"), new XAttribute("Coala", "W"),
                                new XElement("Zver", "None, Rook"),
                                new XElement("Typon", "Normal"),
                                new XElement("Nota", "Re4")
                                )
                            )
                        ),
                    new XElement("VarQuant", new XAttribute("VarNumo", "47"),
                        new XElement("Commento", "Аналитики доказали также, что идея Лf8-h8 вообще была преждевременна. Сначала лучше было сыграть Лс8-с4. Однако любителям шахмат трудно с этим согласиться. Такие ходы, как 22. . .Лh8, не забываются.")
                        )
                    )
                );
            vGamo reto = new vGamo(aa);
            return reto;
            }
        }

#region--------------------------------ДРУГОЙ КЛАСС--vlGamo------------------------------
    /// <summary>
    /// Модификация от 6 октября 2015 года
    /// Заложен 6 октября 2015 года
    /// </summary>
    public class vLGamo {
        private List<vGamo> manolist = vGamoList.Listo;
        private bool edina = vGamoList.Edina;

        public vLGamo() { }

        /// <summary>
        /// Модификация от 6 октября 2015 года
        /// Заложен 6 октября 2015 года
        /// </summary>
        /// <param name="lga"></param>
        public vLGamo(List<Gamo> lga) {
            vGamoList.FillListFromListGamo(lga);
            }

        /// <summary>
        /// Модификация от 11 октября 2015 года
        /// Заложен 11 октября 2015 года
        /// </summary>
        /// <param name="XMLFiloName"></param>
        public vLGamo(string XMLFiloName) {
            XDocument doca = XDocument.Load(XMLFiloName);
            vGamoList.FillListFromXML(doca.Element("Gamosyky").Elements("Leo").ToList());
            }

        /// <summary>
        /// Модификация от 10 октября 2015 года
        /// Заложен 10 октября 2015 года
        /// </summary>
        /// <param name="filo">Имя файла для сохранения XML-содержимого</param>
        public void SavoInXmlFilo(string filo) {
            XComment xcom = new XComment(string.Format("Набор партеечек от {0} ", DateTime.Now.ToLongDateString()));
            XDocument doca = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), xcom);
            XElement ruta = new XElement("Gamosyky");
            foreach (vGamo aa in manolist) {
                ruta.Add(aa.LeoToXML());
                }
            doca.Add(ruta);
            doca.Save(filo);
            }

        public bool Edina { set { edina = value; } get { return edina; } }
        public List<vGamo> Listo { get { return manolist; } }
        }
#endregion-----------------------------ДРУГОЙ КЛАСС--vlGamo------------------------------
    /// <summary>
    /// Модификация от 6 октября 2015 года
    /// Заложен 6 октября 2015 года
    /// </summary>
    public class vGamoList : Singleton<vGamoList> {

        private vGamoList() { }

        private static List<vGamo> manolist = new List<vGamo>();
        private static bool edina = false;
        private static vGamo curgamo = null;

        /// <summary>
        /// Модификация от 6 октября 2015 года
        /// Заложен 6 октября 2015 года
        /// </summary>
        /// <param name="lga"></param>
        public static void FillListFromListGamo(List<Gamo> lga) {
            vGamo tema;
            foreach (Gamo aa in lga) {
                tema = new vGamo(aa);
                manolist.Add(tema);
                }
            }

        /// <summary>
        /// Модификация от 11 октября 2015 года
        /// Заложен 11 октября 2015 года
        /// </summary>
        /// <param name="lxe"></param>
        public static void FillListFromXML(List<XElement> lxe) {
            vGamo tema;
            foreach (XElement aa in lxe) {
                tema = new vGamo(aa);
                manolist.Add(tema);
                }
            }

        public static bool Edina { set { edina = value; } get { return edina; } }
        public static List<vGamo> Listo { get { return manolist; } }

        }
}
