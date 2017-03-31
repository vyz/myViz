using OnlyWorko;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TeVifa
{
    
    
    /// <summary>
    ///Это класс теста для vPozaTestoU, в котором должны
    ///находиться все модульные тесты vPozaTestoU
    ///</summary>
    [TestClass()]
    public class vPozaTestoU
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
        ///Тест для rasschet
        /// Модификация от 21 марта 2017 года
        /// Заложен 20 марта 2017 года
        ///</summary>
        [TestMethod()]
        public void rasschetTesto() {
            vPoza target = new vPoza(pozo.SluchaynoPozo());
            vlEngino ptypo = vlEngino.Stockfish_2_3_1_JA_64bit;
            int minutka = 1;
            int pvarqvo = 5;
            target.rasschet(ptypo, minutka, pvarqvo);
            Assert.IsTrue(target.AnaRes.Length > 30);
        }

        /// <summary>
        /// Тест для одновременной проверки нескольких движков
        /// Долговременные и файлозависимые
        /// Модификация от 22 марта 2017 года
        /// Заложен 22 марта 2017 года
        ///</summary>
        [TestMethod()]
        public void rasschetTestoBig() {
            vPoza target = new vPoza(pozo.SluchaynoPozo());
            int minutka = 1;
            int pvarqvo = 5;
            string bigflo = "bigflo.xml";
            vlEngino ptypo = vlEngino.Stockfish_2_3_1_JA_64bit;
            target.rasschet(ptypo, minutka, pvarqvo);
            Assert.AreEqual(target.SetoAnalo.Count, 5);
            ptypo = vlEngino.Komodo_TCECr_64_bit;
            target.rasschet(ptypo, minutka, pvarqvo);
            Assert.AreEqual(target.SetoAnalo.Count, 10);
            ptypo = vlEngino.Houdini_3a_Pro_w32;
            target.rasschet(ptypo, minutka, pvarqvo);
            target.SavoInXmlFilo(bigflo);
            Assert.AreEqual(target.SetoAnalo.Count, 15);
            }

    }
}
