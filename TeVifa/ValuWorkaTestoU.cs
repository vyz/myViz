using OnlyWorko;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace TeVifa
{
    
    
    /// <summary>
    ///Это класс теста для ValuWorkaTestoU, в котором должны
    ///находиться все модульные тесты ValuWorkaTestoU
    ///</summary>
    [TestClass()]
    public class ValuWorkaTestoU
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
        /// Проверочный тест для AnalizeVanStroke
        /// Модификация от 7 апреля 2017 года
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OnlyWorko.dll")]
        public void AnalizeVanStrokeTesto()
        {
            pozo spo = pozo.SluchaynoPozo();
            ValuWorka_Accessor azzz = new ValuWorka_Accessor(spo);
//Good            string xx = @"info multipv 1 depth 28 seldepth 77 score cp -227 time 5399545 nodes 24894621412 nps 4610000 tbhits 0 hashfull 1000 pv f5d4 e2d1 b4b3 c2c3 d4c2 e1e2 f6g4 h2h3 g4f6 c4d2 c7b6 a4a5 b6b7 d2b3 b7b3 f1e1 d8d7 d1c2 b3c2 e2c2 e8g8 c2e2 e7d8 g2f3 d8a5 e1a1 a5c7 a1a6 g7g6 g1g2 g8g7 f4g5 f8b8 a6a7 f6h7 g5f4";
//Komod           string xx = @"info multipv 5 depth 18 time 55950 nodes 203111980 score cp -199 nps 3630228 pv b4b3 c2c3 e8f8 f4g5 f5h6 e2d1 d8b8 h2h3 a6a5 d1e2 h6g8 e2f3 b8e8 f3d1 e8b8 d1d2 h8h7 g5f4 c7d7 d2e2";
//Stock           string xx = @"info depth 23 seldepth 45 score cp -260 lowerbound nodes 202867521 nps 3466931 time 58515 multipv 3 pv g7g6 f4g5 f6g8 g5d2 g8f6 c2c3 e8f8 g2e4 b4c3 b2c3 f5h6 d2g5 f6e4 g5h6 h8h6 e2e4 g6g5 f1f5 e7f6";
            //Good mate
            string xx = @"info multipv 1 depth 23 seldepth 64 score mate 12 time 179623 nodes 893018459 nps 4971000 tbhits 0 hashfull 1000 pv c2g2 g1f1 e6b3 h8e8 f7e8 c1e1 e8d8 h4f4 b7f3 h6g7 b3d3 g3e2 g2e2 g7f6 d8e8 h2h3 e2e1 f1f2 d3e2 f2g3 e1g1 g3h4 e2e1";
            int pvalo = 0;
            int pvaloExpected = 333012;    //-260;      //-199;      //-227;
            int pdep = 0;
            int pdepExpected = 23;         // 18;         // 28;
            long pnod = 0;
            long pnodExpected = 893018459; //202867521; // 203111980; // 24894621412;
            int prng = 0;
            int prngExpected = 1;          // 3;          // 5;          // 1;
//Good        string expected = @"f5d4 e2d1 b4b3 c2c3 d4c2 e1e2 f6g4 h2h3 g4f6 c4d2 c7b6 a4a5 b6b7 d2b3 b7b3 f1e1 d8d7 d1c2 b3c2 e2c2 e8g8 c2e2 e7d8 g2f3 d8a5 e1a1 a5c7 a1a6 g7g6 g1g2 g8g7 f4g5 f8b8 a6a7 f6h7 g5f4";
//Komod       string expected = @"b4b3 c2c3 e8f8 f4g5 f5h6 e2d1 d8b8 h2h3 a6a5 d1e2 h6g8 e2f3 b8e8 f3d1 e8b8 d1d2 h8h7 g5f4 c7d7 d2e2";
//Stock            string expected = @"g7g6 f4g5 f6g8 g5d2 g8f6 c2c3 e8f8 g2e4 b4c3 b2c3 f5h6 d2g5 f6e4 g5h6 h8h6 e2e4 g6g5 f1f5 e7f6";
            string expected = @"c2g2 g1f1 e6b3 h8e8 f7e8 c1e1 e8d8 h4f4 b7f3 h6g7 b3d3 g3e2 g2e2 g7f6 d8e8 h2h3 e2e1 f1f2 d3e2 f2g3 e1g1 g3h4 e2e1";
            string actual;
            actual = ValuWorka_Accessor.AnalizeVanStroke(xx, out pvalo, out pdep, out pnod, out prng);
            Assert.AreEqual(pvaloExpected, pvalo);
            Assert.AreEqual(pdepExpected, pdep);
            Assert.AreEqual(pnodExpected, pnod);
            Assert.AreEqual(prngExpected, prng);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Тест для AddValuSet
        /// Модификация от 1 февраля 2016 года
        /// Заложен 1 февраля 2016 года
        ///</summary>
        [TestMethod()]
        public void AddValuSetTesto()
        {
            string filfi = @"..\..\..\reso\a01_90min.txt";
            TextReader fafo = new StreamReader(File.Open(filfi, FileMode.Open));
            string aa = fafo.ReadLine();
            List<string> naboro = new List<string>();
            while (aa != null)
            {
                naboro.Add(aa);
                aa = fafo.ReadLine();
            }
            fafo.Close();
            pozo pp = pozo.SluchaynoPozo();
            ValuWorka target = new ValuWorka(pp);
            List<string> seta = naboro;
            vlEngino ten = vlEngino.Houdini_3a_Pro_w32;
            int dlito = 90;
            int kvo = 5;
            target.AddValuSet(seta, ten, dlito, kvo);
            Assert.AreEqual(target.LiValus.Count, kvo);
        }
    }
}
