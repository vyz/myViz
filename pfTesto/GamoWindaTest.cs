using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для GamoWindaTest, в котором должны
    ///находиться все модульные тесты GamoWindaTest
    ///</summary>
    [TestClass()]
    public class GamoWindaTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты теста
        // 
        //При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        //ClassInitialize используется для выполнения кода до запуска первого теста в классе
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize используется для выполнения кода перед запуском каждого теста
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Тест для vvStartOtrisovka
        ///</summary>
        [TestMethod()]
        public void vvStartOtrisovkaTest()
        {
            XElement aa = new XElement("Leo", new XAttribute("lid", "9b4ff681-7e77-40e6-8b3c-d2a5b776319a"), new XAttribute("typo", "Gamo"),
                            new XElement("Namo", "Geller, Efim P - Euwe, Max"),
                            new XElement("BigNamo", "Geller, Efim P - Euwe, Max 1953.??.?? 0-1 E28"),
                            new XElement("Parameters",
                                new XElement("TParo", "Geller, Efim P"),
                                new XElement("TParo", "Euwe, Max"),
                                new XElement("TParo", "0-1"),
                                new XElement("TParo", "1953.??.??"),
                                new XElement("TParo", "candidates tournament"),
                                new XElement("TParo", ">E28"),
                                new XElement("TParo", "52"),
                                new XElement("TParo", "?"),
                                new XElement("TParo", "2"),
                                new XElement("TParo", @"1. d4 Nf6 2. c4 e6 3. Nc3 Bb4 4. e3 c5 5. a3 Bxc3+ 6. bxc3 b6 7. Bd3 Bb7 8. f3
Nc6 9. Ne2 O-O 10. O-O Na5 11. e4 Ne8 12. Ng3 cxd4 13. cxd4 Rc8 14. f4 Nxc4 15.
f5 f6 16. Rf4 b5 17. Rh4 Qb6 18. e5 Nxe5 19. fxe6 Nxd3 20. Qxd3 Qxe6 21. Qxh7+
Kf7 22. Bh6 Rh8 23. Qxh8 Rc2 24. Rc1 Rxg2+ 25. Kf1 Qb3 26. Ke1 Qf3 0-1"),
                                new XElement("TParo", @"[Annotator] - [Bronstein]
[EventDate] - [1953.??.??]")),
                            new XElement("Dopico",
                                new XElement("VarQuant", new XAttribute("VarNumo", "0"),
                                    new XElement("Commento", "Одна из лучших партий турнира, получившая приз за красоту. Белые начали сильную атаку на короля, пожертвовав пешку с4. У Геллера были все шансы на успех, если бы черные по традиции контратаковали на ферзевом фланге. Однако Эйве реализовал две. замечательные идеи: 1) использование коммуникаций ферзевого фланга для атаки на королевском; 2) завлечение сил противника в свой глубокий тыл с целью отвлечь их от защиты короля.  Крайне интересно проследить, как белые фигуры прорывались все дальше и дальше в лобовой атаке на короля и зашли, наконец, туда, откуда нет возврата, а черные в это время перебрасывали свои силы обходными путями.")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "15"),
                                    new XElement("Commento", "Небольшая, но существенная тонкость дебюта: в результате перестановки пары ходов (b7- b6 й Сс8-b7 вместо обычных Кb8-сб и 0-0) белым, не сумевшим вовремя отреагировать правильным 7. Ке2, приходится затратить лишний темп на под-готовку еЗ-е4. Подобными ме-лочами нельзя пренебрегать, но не следует их и переоценивать. Иной раз говорят: преимущество белых состоит в праве первого хода; если они потеряли темп, то преимущество чуть ли не авто-матически должно перейти к черным. Однако преимущество игры белыми практически сводится к тому, что у них больше возможностей выбрать план игры по своему вкусу, а когда игра уже сложилась, потеря одного темпа не всегда бывает столь важна.")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "22"),
                                    new XElement("Commento", "Черные отвели коня, чтобы  не допустить связку Cel-g5 и на ход f3-f4 ответить f7-f5, блокируя королевский фланг. Поэтому белые, прежде чем надвигать пешку Ь, берут под контроль поле f5. Защищать пешку с4 бессмысленно: она была обречена уже 5-м ходом белых.")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "31"),
                                    new XElement("Commento", "Атака становится угрожающей Предыдущий ход черных был необходим, так как белые намеревались продвинуть пешку на f6, а в ответ на Ке8 : f6 все же связать коня и напасть на короля соединенными силами ферзя, ладьи и легких фигур. Но и теперь белым нужно всего два хода, чтобы перебросить ферзя и ладью на линию H, после чего, кажется, ничто уже не спасет короля черных.  Но Эйве нелегко смутить. Вспомним, что он в своей жизни сыграл более 70 партий с Але-хиным, наиболее грозным из ата-кующих шахматистов нашего века.")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "32"),
                                    new XElement("Commento", "Начало замечательного плана. Ясно, что любые оборонительные маневры на королевском фланге при помощи фигур, имеющих ничтожный радиус действия - Л!8-f7, Od8-е7 ит. п., заранее обречены на неудачу. Но у черных есть другой ресурс - контратака! Слон Ь7, ладья с8 и конь с4 занимают хорошие исходные позиции, остается перебросить ферзя. Основа контратаки - преимущество черных на центральных полях. Сыграв b6-b5, черные еще больше укрепляют положение коня и открывают дорогу ферзю на b6. Все же впечатление таково, что их операции запаздывают. ..")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "34"),
                                    new XElement("Commento", "Связывая ферзя защитой  пешки d4, черные препятствуют программному ходу Od1- h5. Впрочем, при 17. Фh5 Фb6 18. Ке2 Ке5 получался эхо-вариант - белые не успевали сыграть Лf4-h4.")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "39"),
                                    new XElement("Vario", new XAttribute("Feno", "2r1nrk1/pb1p2pp/1q2Pp2/1p6/3P3R/P2n2N1/6PP/R1BQ2K1 w - - 0 20"),
                                                          new XAttribute("Tshepa", "20. exd7 Qe6"),
                                        new XElement("VarQuant", new XAttribute("VarNumo", "0"),
                                            new XElement("Commento", "Сейчас, например, не годилось естественное")
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
                                new XElement("VarQuant", new XAttribute("VarNumo", "40"),
                                    new XElement("Commento", "Все ходы белых требовали тщательного и точного расчета.")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "41"),
                                    new XElement("Commento", "Итак, ценой небольших потерь белые все же прорвались. Положение черных снова кажется критическим.")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "44"),
                                    new XElement("Commento", "Если 16-й ход черных был началом стратегического плана контратаки, то жертва ладьи - центральный тактический удар, имеющий целью заманить ферзя подальше, отвлечь от поля с2 и напасть в это время на короля.")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "46"),
                                    new XElement("Commento", "Грозит мат в несколько ходов: 24. . .Л : g2+, 25. . .Фс4-Ь и т.,д. Тщательный анализ, для которого потребовалась не одна неделя, показал, что белые могли спастись от мата, сделав несколько единственных и очень трудных ходов.")
                                    ),
                                new XElement("VarQuant", new XAttribute("VarNumo", "47"),
                                    new XElement("Vario", new XAttribute("Feno", "4n2Q/pb1p1kp1/4qp1B/1p6/3P3R/P5N1/2r3PP/R5K1 w - - 1 24"),
                                                          new XAttribute("Tshepa", "24. d5 Qb6+ 25. Kh1 Qf2 26. Rg1 Bxd5 27. Re4"),
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
                                            new XElement("Vario", new XAttribute("Feno", "4n2Q/pb1p1kp1/4qp1B/1p1P4/7R/P5N1/2r3PP/R5K1 b - - 0 24"),
                                                                  new XAttribute("Tshepa", "24. ... Bxd5 25. Rd4"),
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
                                                    new XElement("Vario", new XAttribute("Feno", "4n2Q/p2p1kp1/4qp1B/1p1b4/7R/P5N1/2r3PP/R5K1 w - - 0 25"),
                                                                          new XAttribute("Tshepa", "25. Rd1 Rxg2+ 26. Kf1 gxh6 27. Rxh6"),
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
                                                            new XElement("Vario", new XAttribute("Feno", "4n2Q/p2p1k2/4qp1p/1p1b4/7R/P5N1/6rP/3R1K2 w - - 0 27"),
                                                                                  new XAttribute("Tshepa", "27. Rxd5"),
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
                                                            new XElement("Vario", new XAttribute("Feno", "4n2Q/p2p1k2/4qp1p/1p1b4/7R/P5N1/6rP/3R1K2 w - - 0 27"),
                                                                                  new XAttribute("Tshepa", "27. Qxh6"),
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
            GamoWinda target = new GamoWinda(); // TODO: инициализация подходящего значения
            vGamo gg = null; // TODO: инициализация подходящего значения
            target.vvStartOtrisovka(gg);
            Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }
    }
}
