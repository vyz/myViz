using OnlyWorko;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;

namespace TeVifa
{
    
    
    /// <summary>
    ///Это класс теста для AnalyaTestoU, в котором должны
    ///находиться все модульные тесты AnalyaTestoU
    ///</summary>
    [TestClass()]
    public class AnalyaTestoU
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
        /// Проверочный тест для Конструктора Analya по XElement
        /// Модификация от 5 апреля 2017 года
        /// Заложен 5 апреля 2017 года
        ///</summary>
        [TestMethod()]
        public void AnalyaConstructorTesto()
        {
            XElement xel = new XElement("Analya", @"D:\tempo\517\posoout\43a1cda558264c81967c13c2eff2b5a9_011.rdy",
                                                  new XElement("Zety", new XAttribute("Z1", 6), new XAttribute("ZN", 6), new XAttribute("ZR", 6)),
                                                  new XElement("Delty", new XAttribute("DZ1", 6), new XAttribute("DZN", 6), new XAttribute("Nody", 1)),
                                                  new XElement("Muvy", new XAttribute("Besto", "bxc3"), new XAttribute("Rilo", "bxc3"), new XAttribute("Orda", 1)),
                                                  new XElement("Sovok", new XAttribute("Kavo", 1), "6w:Z(bxc3)=6 R(bxc3 - 1)=6 N(1)=6 Dl 6 n 6 из 4")
                                                  );
            Analya target = new Analya(xel);
            Assert.AreEqual(target.FFPoso, @"D:\tempo\517\posoout\43a1cda558264c81967c13c2eff2b5a9_011.rdy");
        }
    }
}
