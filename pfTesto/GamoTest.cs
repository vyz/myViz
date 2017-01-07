using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для GamoTest, в котором должны
    ///находиться все модульные тесты GamoTest
    ///</summary>
    [TestClass()]
    public class GamoTest
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
        ///Тест для MovaControlling
        ///</summary>
        [TestMethod()]
        public void MovaControllingTest() {
            string[] latr = { "[Event \"37 Nezhmetdinov Memorial\"]",
                              "[Site \"Kazan\"]",
                              "[Date \"2015.06.01\"]",
                              "[Round \"2.6\"]",
                              "[White \"Hovhannisyan, Robert\"]",
                              "[Black \"Murtazin, Bulat\"]",
                              "[Result \"1-0\"]",
                              "[ECO \"C67\"]",
                              "[WhiteElo \"2599\"]",
                              "[BlackElo \"2342\"]",
                              "[PlyCount \"105\"]",
                              "[EventDate \"2015.??.??\"]"
                            };
            string[] lmov = {
                             "1. e4 e5 2. Nf3 Nc6 3. Bc4 Nf6 4. d4 exd4 5. O-O Nxe4 6. Re1 d5 7. Bxd5 Qxd5 8.",
                             "Nc3 Qa5 9. Nxe4 Be6 10. Neg5 O-O-O 11. Nxe6 fxe6 12. Rxe6 Bd6 13. Bg5 Rdf8 14.",
                             "Qe2 h6 15. Bd2 Qh5 16. h3 Qf7 17. Re1 Nd8 18. Re4 Qxa2 19. Nxd4 Qxb2 20. Qg4+",
                             "Kb8 21. Qxg7 Qb6 22. Qd7 a6 23. Qa4 Bc5 24. Ba5 Qf6 25. Nf3 Nc6 26. Re6 Qb2 27.",
                             "Bd2 Rhg8 28. Qe4 Qg7 29. Qg4 Qf7 30. Qe4 Qg7 31. Qg4 Qf7 32. Qe4 Qxf3 33. Qxf3",
                             "Rxf3 34. Re8+ Ka7 35. Rxg8 Bxf2+ 36. Kh1 Bxe1 37. gxf3 Bxd2 38. Kg2 a5 39. Kf2",
                             "a4 40. Ke2 Bg5 41. h4 a3 42. hxg5 a2 43. gxh6 a1=Q 44. h7 Qe5+ 45. Kf1 Nd4 46.",
                             "h8=Q Qe2+ 47. Kg1 Nxf3+ 0-1",

/*                           "1. Nf3 Nf6 2. c4 g6 3. Nc3 Bg7 4. e4 d6 5. d4 O-O 6. Be2 e5 7. d5 Nbd7 8. O-O",
                             "Nc5 9. Qc2 a5 10. Bg5 h6 11. Be3 b6 12. Nd2 Nh7 13. b3 f5 14. f3 f4 15. Bf2 g5",
                             "16. a3 Na6 17. Rfb1 h5 18. b4 axb4 19. axb4 Nxb4 20. Qb2 Rxa1 21. Rxa1 Na6 22.",
                             "Nb5 Rf6 23. Na7 Bb7 24. Qb5 Nc5 25. Nc6 Qd7 26. Nb4 Qc8 27. Nc6 Bf8 28. Bxc5",
                             "dxc5 29. Nxe5 Rd6 30. Nd3 c6 31. Qb3 cxd5 32. e5 Re6 33. cxd5 c4 34. Nxc4 Bxd5",
                             "35. Nxb6 Qc5+ 36. Nxc5 Bxc5+ 37. Kf1 Bxb3 38. Nd7 Be7 39. Rb1 Ba4 40. Nb6 Bc2",
                             "41. Rb2 Rc6 42. Bb5 Rc5 43. Nd7 Rxb5 44. Rxc2 Rb7 45. e6 Nf8 46. Rc8 Kg7 47.",
                             "Re8 Bd6 48. h4 gxh4 49. Ne5 Bxe5 50. e7 Bd6 51. exf8=Q+ Bxf8 52. Re5 Kg6 53.",
                             "Re6+ Kf5 54. Re8 Bc5 55. Rh8 Rb1+ 56. Ke2 Rb2+ 57. Kd3 Rxg2 58. Rxh5+ Rg5 59.",
                             "Rxh4 Be3 0-1",
 */ 
/*                           "1. Nf3 Nf6 2. g3 b6 3. Bg2 Bb7 4. O-O g6 5. c4 Bg7 6. d4 O-O 7. Qc2 d6 8. Nc3",
                             "Nc6 9. Rd1 e6 10. e4 Qe7 11. h3 e5 12. d5 Nb4 13. Qe2 a5 14. a3 Na6 15. Rb1 Nc5",
                             "16. b4 axb4 17. axb4 Na4 18. Nb5 c6 19. dxc6 Bxc6 20. Rb3 Nxe4 21. Ng5 d5 22.",
                             "Nxe4 dxe4 23. Bxe4 Bxe4 24. Qxe4 f5 25. Qd5+ Qf7 26. Nc7 Rad8 27. Qxd8 Rxd8 28.",
                             "Rxd8+ 1-0",
 */ 
/*                           "1. e4 e5 2. Nf3 Nc6 3. Bb5 Nf6 4. O-O Nxe4 5. d4 Nd6 6. Bxc6 dxc6 7. dxe5 Nf5",
                             "8. Qxd8+ Kxd8 9. Rd1+ Ke8 10. Nc3 Ne7 11. h3 Ng6 12. Be3 Be6 13. Ng5 Nxe5 14.",
                             "Re1 Ng6 15. Bd2 Kd7 16. Nxe6 fxe6 17. Ne4 Bd6 18. Rad1 b6 19. Bc3 Rhg8 20. Ng5",
                             "Nf8 21. Rd3 c5 22. Red1 Ke8 23. Ne4 Be7 24. Be5 Rc8 25. c4 Ng6 26. Bg3 e5 27.",
                             "Nc3 Kf7 28. Nd5 Bd6 29. Ra3 Ra8 30. Nxc7 Bxc7 31. Rd7+ Kf6 32. Rxc7 h5 33. Rc6+",
                             "Kf5 34. Rc7 h4 35. Bh2 Rad8 36. g4+ hxg3 37. Bxg3 Rd2 38. Rb3 Nf4 39. Rf7+ Kg6",
                             "40. Rxa7 Rf8 41. Rxb6+ Kh5 42. Kh2 Nd3 43. Rd6 e4 44. Rxg7 e3 45. h4 Rf5 46.",
                             "Kg2 Ne5 47. Bxe5 Rfxf2+ 48. Kh3 Rf3+ 49. Rg3 e2 50. Rxf3 Rd3 51. Bg3 Rxf3 52.",
                             "Rd5+ Kg6 53. Re5 1-0",
 */
                            };
            Gamo target = new Gamo(latr.ToList(), lmov.ToList()); 
            target.MovaControlling();
            List<string> zz = target.CreateMovaRegionWithoutComments();
            for (int i = 0; i < zz.Count; i++) {
                string vanTarget = zz[i];
                string twoEtalon = lmov[i];
                Assert.AreEqual(twoEtalon, vanTarget);
                }
            }

        /// <summary>
        ///Тест для FillVarioMoves
        ///</summary>
        [TestMethod()]
        [DeploymentItem("pfVisualisator.exe")]
        public void FillVarioMovesTest()
        {
            Gamo_Accessor target = new Gamo_Accessor(); 
            string vv = "{Однако и после корчновского } 15. Nxf8 {инициатива в руках чёрных. И инициатива эта имеет тенденцию к наращиванию мускулов. Например,} Bxf8 16. exd4  { (опять ход логичнее не придумаешь)} Ndf6  17. h3 {(единственное!)} Qxd4+ 18. Kh1 Nh5  19. hxg4  Ng3+ 20. Kh2 Nxf1+ 21. Bxf1 Bc5  22. Qe2  ({или} 22. Kg3 g5  ) 22... g5  23. Be3 {(возвращая всё назад)} Qxe3 24. Qxe3 Bxe3  {. К тому же нехорошо здесь} 25. Nxb5  ({ещё хуже} 25. Bxb5  Kg7   ) 25... Bxf4+ 26. Kg1 Rc2 {с полной доминацией.}";
            List<string> naboro = null;
            List<string> naboroExpected = null; 
            bool expected = false; 
            bool actual;
            actual = target.FillVarioMoves(vv, out naboro);
            Assert.AreEqual(naboroExpected, naboro);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }
        }
    }
