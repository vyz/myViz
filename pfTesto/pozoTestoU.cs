﻿using pfVisualisator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace pfTesto
{
    
    
    /// <summary>
    ///Это класс теста для pozoTestoU, в котором должны
    ///находиться все модульные тесты pozoTestoU
    ///</summary>
    [TestClass()]
    public class pozoTestoU
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
        ///Тест для fenout
        /// Модификация от 28 мая 2015 года
        /// Заложен 28 мая 2015 года
        ///</summary>
        [TestMethod()]
        public void fenoutTesto() {
            pozo target = pozo.Starto();
            string expected = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            string actual;
            actual = target.fenout();
            Assert.AreEqual(expected, actual);
            }

        /// <summary>
        ///Тест для VanBoardo
        /// Модификация от 10 декабря 2015 года
        /// Заложен 10 декабря 2015 года
        ///</summary>
        [TestMethod()]
        public void VanBoardoTesto() {
            pozo biba = pozo.Starto();
            Guid[] actual = new Guid[2];
            actual[0] = Guid.Parse("43521111-0000-0000-0000-00009999cbda");
            actual[1] = Guid.Parse("26341111-0000-0000-0000-00009999aebc");
            Guid[] target = biba.VanBoardo;
            for (int i = 0; i < 2; i++) {
                Assert.AreEqual(actual[i], target[i]);
                }
            }


        /// <summary>
        ///Тест для AvaList
        ///</summary>
        [TestMethod()]
        public void AvaListTesto() {
            pozo target = pozo.SluchaynoPozo();
            target.AvailableFill();
            List<Mova> actual;
            actual = target.AvaList;
            int sravo = 14;
            Assert.AreEqual(actual.Count, sravo);
            }

        /// <summary>
        ///Тест для Конструктор pozo(string)
        /// Модификация от 14 декабря 2015 года
        /// Заложен 14 декабря 2015 года
        ///</summary>
        [TestMethod()]
        public void pozoConstructorTestoStringo() {
            pozo Isko = pozo.SluchaynoPozo();
            string feno = Isko.fenout();
            pozo target = new pozo(feno);
            Assert.AreEqual(Isko.VanBoardo[0], target.VanBoardo[0]);
            Assert.AreEqual(Isko.VanBoardo[1], target.VanBoardo[1]);
            Assert.AreEqual(Isko.TwaFeno, target.TwaFeno);
            }

        /// <summary>
        ///Тест для Конструктор pozo(Guid[] van, int twa)
        /// Модификация от 14 декабря 2015 года
        /// Заложен 14 декабря 2015 года
        ///</summary>
        [TestMethod()]
        public void pozoConstructorTestoTwoVanoFeno() {
            pozo Isko = pozo.SluchaynoPozo();
            Guid[] va = Isko.VanBoardo;
            int tw = Isko.TwaFeno;
            pozo target = new pozo(va, tw);
            Assert.AreEqual(Isko.fenout(), target.fenout());
            }

        }
    }
