using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для mybinoTestoU, в котором должны
    ///находиться все модульные тесты mybinoTestoU
    ///</summary>
    [TestClass()]
    public class mybinoTestoU
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
        ///Тест для Конструктор mybino
        ///</summary>
        [TestMethod()]
        public void mybinoConstructorTesto()
        {
            char[] aa = "Тест для залезания вовнутрь отладчиком и почти бесполезен для отладки, но должен отрабатывать нормально".ToCharArray();
            List<byte> bb = new List<byte>();
            foreach (char dd in aa)
            {
                bb.Add((byte)dd);
            }
            byte[] pbival = bb.ToArray();
            string pex = "По боку";
            string pfname = "ОКПBaseReport";
            string pcommo = "Второй бок";
            mybino target = new mybino(pbival, pex, pfname, pcommo);
            Assert.AreEqual(target.SelfBinaryObj[0], (byte)0x66);
        }
    }
}
