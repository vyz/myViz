using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для GamoTestoU, в котором должны
    ///находиться все модульные тесты GamoTestoU
    ///</summary>
    [TestClass()]
    public class GamoTestoU
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
        ///Тест для FillVarioMoves
        ///</summary>
        [TestMethod()]
        [DeploymentItem("pfVisualisator.exe")]
        public void FillVarioMovesTesto()
        {
            Gamo_Accessor target = new Gamo_Accessor();
            //string vv = @"{Надо было играть} 24. d5 {В случае} Qb6+ ({а если сразу} 24... Bxd5 {то не} 25. Rd4 ({а только} 25. Rd1 { и после} Rxg2+ 26. Kf1 gxh6 {не годится ни} 27. Rxh6 ({ни} 27. Rxd5) ({, а нужно снова сделать единственный ход} 27. Qxh6 {И все равно у черных остается слон и две пешки за ладью, что при открытом положении вражеского короля дает им хорошие шансы на победу. Понятно, что у Геллера за доской не было практической возможности найти все эти ходы.}))) 25. Kh1 Qf2 ({Конечно же не} 25... Qg1) 26. Rg1 Bxd5 {они спасались ходом} 27. Re4";
            //string vv = @"{Надо было играть} 24. d5 {В случае} Qb6+ ({а если сразу} 24... Bxd5 {то не} 25. Rd4 ({а только} 25. Rd1 { и после} Rxg2+ 26. Kf1 gxh6 {не годится ни} 27. Rxh6 ({ни} 27. Rxd5) ({, а нужно снова сделать единственный ход} 27. Qxh6 {И все равно у черных остается слон и две пешки за ладью, что при открытом положении вражеского короля дает им хорошие шансы на победу. Понятно, что у Геллера за доской не было практической возможности найти все эти ходы.}) ) ) 25. Kh1 Qf2 26. Rg1 Bxd5 { они спасались ходом} 27. Re4";
            //string vv = @"{а только} 25. Rd1 { и после} Rxg2+ 26. Kf1 gxh6 {не годится ни} 27. Rxh6 ({ни} 27. Rxd5) ({, а нужно снова сделать единственный ход} 27. Qxh6 {И все равно у черных остается слон и две пешки за ладью, что при открытом положении вражеского короля дает им хорошие шансы на победу. Понятно, что у Геллера за доской не было практической возможности найти все эти ходы.}) ";
            string vv = @"({В случае} 32. c4 Nb6 33. Rc1 ({или} 33. d5 exd5 34. c5 Nxa4 35. Bd4 Rc8 36. Qf3 Qe6 {белые пешки блокировались, а черные становились крайне опасными.}) 33... Nxa4 34. Ba1 Qc6)";
            List<string> naboro = null;
            string [] naboroExpected = { "a101",
                              "a102",
                              "a103"
                            };
            bool expected = true;
            bool actual;
            actual = target.FillVarioMoves(vv, out naboro);
            Assert.AreEqual(naboroExpected.ToList(), naboro);
            Assert.AreEqual(expected, actual);
        }
    }
}
