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
        /// Модификация от 27 января 2016 года
        /// Заложен 27 января 2016 года
        ///</summary>
        [TestMethod()]
        public void VarioConstructorTesto()
        {
            pozo pp = pozo.SluchaynoPozo();
            string mvlist = @"g7g6 f4d2 f6g4 c2c3 d8d7 g2e4 g4h6 e4f3 e8f8 f3h5 b4c3 d2c3 h8g8 h5f3 f5d4 c3d4 c5d4 b2b4 f8g7 e2b2 g8b8 b4b5 a6b5 a4b5 c7c5 b5b6 e7f6 f3e4";
            Vario target = new Vario(pp, mvlist);
            string etalon = @"22. ... g6 23. Bd2 Ng4 24. c3 Rd7 25. Be4 Ngh6 26. Bf3 Kf8 27. Bxh5 bxc3 28.
Bxc3 Rg8 29. Bf3 Nd4 30. Bxd4 cxd4 31. b4 Kg7 32. Qb2 Rb8 33. b5 axb5 34. axb5
Qc5 35. b6 Bf6 36. Be4";
            Assert.AreEqual(etalon, target.OnlyMova);
        }
    }
}
