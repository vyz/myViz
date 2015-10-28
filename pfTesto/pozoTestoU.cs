using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для pozoTestoU, в котором должны
    ///находиться все модульные тесты pozoTestoU
    ///</summary>
    [TestClass()]
    public class pozoTestoU
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
        ///Тест для fenout
        /// Модификация от 28 мая 2015 года
        /// Заложен 28 мая 2015 года
        ///</summary>
        [TestMethod()]
        public void fenoutTesto() {
            pozo target = pozo.Starto();
            string expected = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            string actual;
            actual = target.fenout();
            Assert.AreEqual(expected, actual);
            }
        }
    }
