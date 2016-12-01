﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Yaisp3
{
    public partial class FormMain : Form
    {
        private bool drawingMap = false;
        private MouseEventArgs eOld;
        private MainDrawingProcessor drawers;

        private List<Tuple<AgencyHandler, StrategyHandler>> Agencies;

        private AgencyHandler Agency;       //Создается соответствующей формой.
        private CityHandler City;           //Создается соответствующей формой.
        private StrategyHandler Strategy;   //Создается соответствующей формой.
        private DateHandler Date;
        private QueueHandler Queue;
        
        public FormMain()
        {
            InitializeComponent();
            Agencies = new List<Tuple<AgencyHandler, StrategyHandler>>();
            drawers = new MainDrawingProcessor();
            drawers.SetCanvas(CtrlPicBxMap);
            Date = new DateHandler();
            City = new CityHandler();
            Queue = new QueueHandler();
            Agency = new AgencyHandler();
            Strategy = new StrategyHandler();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
            CtrlPicBxMap.MouseWheel += new MouseEventHandler(CtrlPicBxMap_MouseScroll);
        }

        private void CtrlPicBxMap_MouseScroll(object sender, MouseEventArgs e)
        {
            drawers.Zoom(e.X, e.Y, e.Delta);
            drawers.Draw();
        }
        private void CtrlPicBxMap_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                case MouseButtons.Right:
                    eOld = e;
                    drawingMap = true;
                    break;
                case MouseButtons.Middle:
                    drawers.SetNormalZoom();
                    break;
            }
            drawers.Draw();
        }
        private void CtrlPicBxMap_MouseUp(object sender, MouseEventArgs e)
        {
            drawingMap = false;
        }
        private void CtrlPicBxMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawingMap)
            {
                drawers.Move(e.X, e.Y, eOld.X, eOld.Y);
                eOld = e;
                drawers.Draw();
            }
        }

        private void CtrlTSMIAgencyMenuClick(object sender, EventArgs e)
        {
            FormAgency Af = new FormAgency(Agency, Strategy);
            if (Af.ShowDialog() == DialogResult.OK)
            {
                CtrlChBIndAgen.Checked = true;
                CtrlTSMIAgencyDelete.Enabled = true;
                if (CtrlChBIndCity.Checked)
                    CtrlButTimerStart.Enabled = true;
                Agency.AgencySetLink(City.GetCityLink(), Queue.GetQueueLink(), drawers);
                Agencies.Add(Tuple.Create(Agency, Strategy));
            }
        }
        private void CtrlTSMIAgencyDeleteClick(object sender, EventArgs e)
        {
            Agency.AgencyDestroy();
            Agency = new AgencyHandler();
            Strategy = new StrategyHandler();
            drawers.CheckList();
            CtrlChBIndAgen.Checked = false;
        }
        private void CtrlTSMICreateCityClick(object sender, EventArgs e)
        {
            FormCity Cf = new FormCity(drawers, City);
            if (Cf.ShowDialog() == DialogResult.OK)
            {
                CtrlChBIndCity.Checked = true;
                CtrlTSMIAgencyMenu.Enabled = true;
                if (CtrlChBIndAgen.Checked)
                    CtrlButTimerStart.Enabled = true;
            }
            else
                if (!CtrlChBIndCity.Checked)
                City = null;
            drawers.SetCanvas(CtrlPicBxMap);
            CtrlPicBxMap.Invalidate();
        }
        private void CtrlTSMIProximityMapClick(object sender, EventArgs e)
        {
            FormProximity Pr = new FormProximity(City);
            Pr.Show();
        }

        private void CtrlTSMIGraph_Click(object sender, EventArgs e)
        {
            FormGraph Gr = new FormGraph(Agency.AgencyGetSummary());
            Gr.Show();
        }

        private void CtrlTSMIDrop_Click(object sender, EventArgs e)
        {
            if (CtrlTimer.Enabled)
                CtrlButTimerPause.Text = "Продолжить";
            CtrlTimer.Enabled = false;
        }

        private void CtrlTimer_Tick(object sender, EventArgs e)
        {
            CtrlLblDate.Text = "Дата: " + Date.DateGetAsString();
            Queue.QueueAddRand(CtrlTBQueueQuantity.Value, CtrlTBQueueIntense.Value);
            CtrlTxbOrders.Text = Queue.ToString();
            if (!Strategy.StrategyAction())
            {
                CtrlTimer.Stop();
                MessageBox.Show("Агентство " + Agency.AgencyGetData().Item1 + " было ликвидировано по причине банкротства.", "Беда!");
                Agency.AgencyDestroy();
                Strategy.DeleteStrategy();
            }
            Date.DateNewDay();
            drawers.Draw();
        }

        private void CtrlButTimerStartClick(object sender, EventArgs e)
        {
            CtrlButTimerStart.Enabled = false;
            CtrlTSMIGraph.Enabled = true;
            TextStorageClass.ParseTextData();
            CtrlTimer.Enabled = true;
            CtrlButTimerPause.Enabled = true;
        }
        private void CtrlButTimerPauseClick(object sender, EventArgs e)
        {
            CtrlTimer.Enabled = !CtrlTimer.Enabled;
            if (CtrlTimer.Enabled)
                CtrlButTimerPause.Text = "Пауза";
            else
                CtrlButTimerPause.Text = "Продолжить";
        }

        private void CtrlTBSpeed_Scroll(object sender, EventArgs e)
        {
            CtrlTimer.Interval = CtrlTBSpeed.Value * 20;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            drawers.Draw();
        }

        /// <summary>
        /// Костыль для Windows XP.
        /// </summary>
        private void CtrlPicBxMap_Click(object sender, EventArgs e)
        {
            CtrlPicBxMap.Focus();
        }
    }
}
