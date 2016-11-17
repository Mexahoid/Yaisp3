﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Yaisp3
{
    static class Program
    {
        public static _FormAgency formCreateAgency;
        public static _FormMain formMain;
        public static _FormCity formCity;
        public static FormProximity formProximity;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            formMain = new _FormMain();
            formCreateAgency = new _FormAgency();
            formCity = new _FormCity();
            formProximity = new FormProximity();
            Application.Run(formMain);
        }
    }
}
