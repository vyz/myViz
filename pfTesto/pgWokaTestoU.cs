using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для pgWokaTestoU, в котором должны
    ///находиться все модульные тесты pgWokaTestoU
    ///</summary>
    [TestClass()]
    public class pgWokaTestoU {


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
    ///Тест для Конструктор pgWoka
    ///</summary>
    [TestMethod()]
    public void pgWokaConstructorTesto() {
        string fnamo = @"c:\tempo\nezh15_no9.pgn";
        pgWoka target = new pgWoka(fnamo);
        Assert.IsTrue(target.Listo.Count > 340);
        }

    /// <summary>
    ///Тест на идентичность области ходов во входном pgn-файле (соответствующем стандарту) и создаваемой объектом Gamo
    ///Тестируются чистые без комментариев тексты партий.
    ///</summary>
    [TestMethod()]
    public void pgWokaFuncGamoMoveRegion() {
        string fnamo = @"..\..\..\ForTestoBig.pgn";
        pgWoka BigObject = new pgWoka(fnamo);
        List<string> target = null;
        foreach (Gamo aa in BigObject.Listo) {
            aa.MovaControlling();
            target = aa.CreateMovaRegionWithoutComments();
            for (int i = 0; i < target.Count; i++) {
                string vanTarget = target[i];
                string twoEtalon = aa.pgnMoveRegione[i];
                Assert.AreEqual(twoEtalon, vanTarget);
                }
            }
        }


    /// <summary>
    ///Тест для FullListControl
    /// Модификация от 30 сентября 2015 года
    ///</summary>
    [TestMethod()]
    public void FullListControlTesto() {
        string[] latr = { "[Site \"Kazan\"]" };
        string[] intelpredo = { "EventDateHasAnswer$4$2015.06.03",
                                "EventDateHasAnswer$5$2015.06.04"};
        string fnamo = @"..\..\..\ForTestoBig.pgn";
        pgWoka target = new pgWoka(fnamo);
        List<string> attrolist = latr.ToList();
        List<string> ipredolist = intelpredo.ToList();
        bool expected = true;
        bool actual;
        actual = target.FullListControl(attrolist, ipredolist);
        Assert.AreEqual(expected, actual);
        }
    }
}
