using OnlyWorko;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TeVifa
{
    
    
    /// <summary>
    ///Это класс теста для VarioTestoU, в котором должны
    ///находиться все модульные тесты VarioTestoU
    ///</summary>
    [TestClass()]
    public class VarioTestoU
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
        ///Тест для Конструктор Vario
        /// Модификация от 25 апреля 2017 года
        /// Заложен 27 января 2016 года
        ///</summary>
        [TestMethod()]
        public void VarioConstructorTesto()
        {
            pozo pp = new pozo(@"8/8/5PR1/1p2K2p/1kp5/6P1/5r1P/8 b - - 2 44");
//            pozo pp = pozo.SluchaynoPozo();
//            string mvlist = @"g7g6 f4d2 f6g4 c2c3 d8d7 g2e4 g4h6 e4f3 e8f8 f3h5 b4c3 d2c3 h8g8 h5f3 f5d4 c3d4 c5d4 b2b4 f8g7 e2b2 g8b8 b4b5 a6b5 a4b5 c7c5 b5b6 e7f6 f3e4";
            string mvlist = @"b4a4 g3g4 c4c3 g4h5 c3c2 g6g1 b5b4 h5h6 f2e2 e5d6 e2d2 d6e7 d2d1 h6h7 d1g1 h7h8q c2c1q h8a8 a4b3 a8d5 c1c4 d5f3 c4c3 f3d5 b3b2 d5d6 g1e1 e7f7 b4b3 d6f4 c3a5 f4d6 a5h5 f7g7 e1g1 g7f8 g1d1 d6c6 h5h8 f8e7 h8h7 e7e6 d1e1";
            Vario target = new Vario(pp, mvlist);
/*            string etalon = @"22. ... g6 23. Bd2 Ng4 24. c3 Rd7 25. Be4 Ngh6 26. Bf3 Kf8 27. Bxh5 bxc3 28.
Bxc3 Rg8 29. Bf3 Nd4 30. Bxd4 cxd4 31. b4 Kg7 32. Qb2 Rb8 33. b5 axb5 34. axb5
Qc5 35. b6 Bf6 36. Be4";*/
            string etalon = @"44. ... Ka4 45. g4 c3 46. gxh5 c2 47. Rg1 b4 48. h6 Re2+ 49. Kd6 Rd2+ 50. Ke7
Rd1 51. h7 Rxg1 52. h8=Q c1=Q 53. Qa8+ Kb3 54. Qd5+ Qc4 55. Qf3+ Qc3 56. Qd5+
Kb2 57. Qd6 Re1+ 58. Kf7 b3 59. Qf4 Qa5 60. Qd6 Qh5+ 61. Kg7 Rg1+ 62. Kf8 Rd1
63. Qc6 Qh8+ 64. Ke7 Qh7+ 65. Ke6 Re1+";
            Assert.AreEqual(etalon, target.OnlyMova);
        }
    }
}
