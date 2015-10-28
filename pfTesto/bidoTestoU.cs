using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для bidoTestoU, в котором должны
    ///находиться все модульные тесты bidoTestoU
    ///</summary>
    [TestClass()]
    public class bidoTestoU
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
        ///Тест для PutRepoRecord
        ///</summary>
        [TestMethod()]
        public void PutRepoRecordTesto()
        {
            bido target = new bido(); 
            vOKPReport pp = vOKPReport.ZagoCreate();
            Random aa = new Random();
            string ks = "Тестуша + " + aa.Next(32000).ToString();
            pp.BigNamo = ks; 
            target.PutLeoRecord(pp);
            //Пока только это
            Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }

        /// <summary>
        ///Тест для GetRepoBinoData
        ///</summary>
        [TestMethod()]
        public void GetRepoBinoDataTesto()
        {
            bido target = new bido(); // TODO: инициализация подходящего значения
            string suido = "A0A12C90-E5BA-4C3E-ACAE-0C153718BB42";
            Guid pp =  Guid.Parse(suido); 
            byte[] expected = null; 
            byte[] actual;
            actual = target.GetRepoBinoData(pp);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }
    }
}
