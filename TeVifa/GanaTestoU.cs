using OnlyWorko;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TeVifa
{
    
    
    /// <summary>
    ///Это класс теста для GanaTestoU, в котором должны
    ///находиться все модульные тесты GanaTestoU
    ///</summary>
    [TestClass()]
    public class GanaTestoU
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
        /// Отладочный тест для SavoInXmlFilo
        /// Реально не зависит от файла, но создаёт файл и размещает его в задаваемый в тексте каталог
        /// Этот каталог и есть компьютерозависимый
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        ///</summary>
        [TestMethod()]
        public void SavoInXmlFiloTesto()
        {
            string catalogo = @"D:\tempo\517";
            vGamo elemo = vGamo.CreateExempWithVariantoAndComments();
            elemo.EtaloCreate();
            Gana target = new Gana(elemo, 10, elemo.Gamma.ListoMovo.Count);
            target.SavoInXmlFilo(catalogo);
            catalogo = target.Fitchy;
            Assert.IsTrue(catalogo.Length > 20);
        }

        /// <summary>
        /// Тест для CreateExemplaroFromXmlFile
        /// Отладочный файлозависимый
        /// Модификация от 30 марта 2017 года
        /// Заложен 30 марта 2017 года
        ///</summary>
        [TestMethod()]
        public void CreateExemplaroFromXmlFileTesto() {
            string filo = @"D:\tempo\517\43a1cda558264c81967c13c2eff2b5a9.xml";
            Gana target;
            int actual = 52;
            target = Gana.CreateExemplaroFromXmlFile(filo);
            vGamo vv = target.Grusha;
            vv.EtaloCreate();
            Assert.AreEqual(vv.Gamma.ListoMovo.Count, actual);
            }

        /// <summary>
        /// Тест для OdinRaz
        /// Модификация от 31 марта 2017 года
        /// Заложен 31 марта 2017 года
        /// </summary>
        [TestMethod()]
        [DeploymentItem("OnlyWorko.dll")]
        public void OdinRazTesto()
        {
            vGamo elemo = vGamo.CreateExempWithVariantoAndComments();
            elemo.EtaloCreate();
            int nmc = 11;
            int fino = elemo.Gamma.ListoMovo.Count;
            Gana_Accessor target = new Gana_Accessor(elemo, nmc, fino);
            string outposodir = @"D:\tempo\517\posoout";
            bool expected = true;
            bool actual;
            actual = target.OdinRaz(outposodir);
            Assert.AreEqual(expected, actual);
        }
    }
}
