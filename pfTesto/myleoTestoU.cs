using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для myleoTestoU, в котором должны
    ///находиться все модульные тесты myleoTestoU
    ///</summary>
    [TestClass()]
    public class myleoTestoU
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
        ///Тест для PoiskDiffov
        ///</summary>
        [TestMethod()]
        public void PoiskDiffovTesto()
        {
            myleo target = vOKPReport.ZagoCreate();
            myleo hiob = vOKPReport.ZagoTwoCreate();
            bool expected = false;
            bool actual;
            actual = target.PoiskDiffov(hiob);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для PoiskDiffov02
        ///</summary>
        [TestMethod()]
        public void PoiskDiffovTesto02()
        {
            myleo target = vOKPReport.ZagoCreate();
            vHistorySaver.AddNewObject(target);
            ((vOKPReport)target).SamoPicture.Commento = "Чисто для отладчика";
            bool expected = false;
            bool actual;
            actual = vHistorySaver.IsADifference(target);
            Assert.AreEqual(expected, actual);
        }

    }
}
