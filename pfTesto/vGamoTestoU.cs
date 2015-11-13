using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для vGamoTestoU, в котором должны
    ///находиться все модульные тесты vGamoTestoU
    ///</summary>
    [TestClass()]
    public class vGamoTestoU
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
        ///Тест для GetPozoOnOffset
        ///</summary>
        [TestMethod()]
        public void GetPozoOnOffsetTesto()
        {
            vGamo target = vGamo.CreateExemplarusForLife();
            int offseto = 24;
            pozo expected = null; // TODO: инициализация подходящего значения
            pozo actual;
            actual = target.GetPozoOnOffset(offseto);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }
    }
}
