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
  public partial class _FormCity : Form
  {
    private bool moving = false;
    private bool drawing = false;
    private bool loaded = true;
    private MouseEventArgs e0;
    private CityRedactorLogicsClass CityCreationKit;

    public _FormCity()
    {
      InitializeComponent();
      MouseWheel += new MouseEventHandler(_ctrlPicBxMap_MouseScroll);
    }
    private void _ctrlPicBxMap_MouseScroll(object sender, MouseEventArgs e)
    {
      if (CityCreationKit != null)
        CityCreationKit.ZoomImage(e.X, e.Y, e.Delta);
    }

    private void _ctrlButMark_Click(object sender, EventArgs e)
    {
      MainUnitProcessor.CityCreate(_ctrlTxbCityName.Text, (int)(_ctrlNumCityHeight.Value),
        (int)(_ctrlNumCityWidth.Value));

      CityCreationKit = new CityRedactorLogicsClass(_ctrlPicBxCity);
      _ctrlButSave.Enabled = true;
    }

    private void _ctrlPicBxCity_MouseDown(object sender, MouseEventArgs e)
    {
      if (CityCreationKit != null)
      {
        switch (e.Button)
        {
          case MouseButtons.Left:
            if (drawing)
            {
              CityCreationKit.AddElementToMatrix(e.X, e.Y, 
                (int)(_ctrlNumHouseWidth.Value), (int)(_ctrlNumHouseHeigth.Value));
              drawing = false;
            }
            break;
          case MouseButtons.Right:
            moving = true;
            e0 = e;
            break;
          case MouseButtons.Middle:
            CityCreationKit.SetNormalZoom();
            break;
        }
      }
    }
    private void _ctrlPicBxCity_MouseUp(object sender, MouseEventArgs e)
    {
      moving = false;
    }
    private void _ctrlPicBxCity_MouseMove(object sender, MouseEventArgs e)
    {
      if (CityCreationKit != null)
      {
        if (moving)
        {
          CityCreationKit.MoveImage(e.X, e.Y, e0.X, e0.Y);
          e0 = e;
        }
        if (drawing)
          CityCreationKit.DrawCurrentObject(e.X, e.Y, (int)_ctrlNumHouseWidth.Value, (int)_ctrlNumHouseHeigth.Value);
      }
    }

    private void _ctrlButHouse_Click(object sender, EventArgs e)
    {
      drawing = true;
    }
    private void _ctrlReset_Click(object sender, EventArgs e)
    {
      CityCreationKit.DestroyCreator();
    }
    private void _ctrlButReady_Click(object sender, EventArgs e)
    {
      CityCreationKit.DestroyCreator();
      CityCreationKit = null;
      loaded = false;
      Close();
    }

    private void _FormCity_Load(object sender, EventArgs e)
    {
      if (MainUnitProcessor.CityIsPresent())
      {
        if (!loaded)
        {
          CityCreationKit = new CityRedactorLogicsClass(_ctrlPicBxCity);
          loaded = true;
        }
        _ctrlButSave.Enabled = true;
      }
    }

    private void _ctrlButSave_Click(object sender, EventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      if (sfd.ShowDialog() == DialogResult.OK)
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName))
        {
          sw.Write(CityCreationKit.Save());
          sw.Close();
        } 
    }

    private void _ctrlButLoad_Click(object sender, EventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      if (ofd.ShowDialog() == DialogResult.OK)
        using (System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName))
        {
          CityCreationKit = new CityRedactorLogicsClass(_ctrlPicBxCity, sr.ReadToEnd());
            _ctrlButSave.Enabled = true;
          sr.Close();
        }
    }
  }
}