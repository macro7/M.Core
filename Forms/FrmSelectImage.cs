﻿using M.Core.Controls;
using M.Core.Extensions;
using M.Core.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace M.Core.Forms
{
    public partial class FrmSelectImage : Form
    {
        public Image SelectImage { get; set; }
        public FrmSelectImage()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "错误");
            }
        }

        private void tabControlExt1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlExt1.SelectedIndex == 0)
            {
                ActiveControl = flowLayoutPanel1;
            }
            else
            {
                ActiveControl = flowLayoutPanel2;
            }
        }

        private void FrmSelectImage_Load(object sender, EventArgs e)
        {
            string[] nameList = System.Enum.GetNames(typeof(FontIcons));
            var lst = nameList.ToList();
            lst.Sort();
            foreach (var item in lst)
            {
                FontIcons icon = (FontIcons)Enum.Parse(typeof(FontIcons), item);
                Label lbl = new Label();
                lbl.AutoSize = false;
                lbl.Size = new System.Drawing.Size(300, 35);
                lbl.ForeColor = Color.FromArgb(255, 77, 59);
                lbl.TextAlign = ContentAlignment.MiddleLeft;
                lbl.Margin = new System.Windows.Forms.Padding(5);
                lbl.DoubleClick += lbl_DoubleClick;
                string s = char.ConvertFromUtf32((int)icon);
                lbl.Text = "       " + item;
                lbl.Image = FontImages.GetImage(icon, 32, Color.FromArgb(255, 77, 59));
                lbl.ImageAlign = ContentAlignment.MiddleLeft;
                lbl.Font = new Font("微软雅黑", 12);
                lbl.Tag = icon;
                if (item.StartsWith("A_"))
                {
                    flowLayoutPanel1.Controls.Add(lbl);
                }
                else
                {
                    flowLayoutPanel2.Controls.Add(lbl);
                }
            }
            ActiveControl = flowLayoutPanel1;
        }

        void lbl_DoubleClick(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            FontIcons icon = (FontIcons)lbl.Tag;
            int intSize = ucTextBoxEx1.InputText.ToInt().Value;
            if (intSize <= 0)
            {
                intSize = 32;
            }

            SelectImage = FontImages.GetImage(icon, intSize, txtForeColor.BackColor, txtBackcolor.BackColor == Color.White ? Color.Empty : txtBackcolor.BackColor);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }


        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            ColorDialog ColorForm = new ColorDialog();
            ColorForm.AnyColor = true;
            List<int> lstCustomColors = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                lstCustomColors.Add(ColorTranslator.ToOle(ControlHelper.Colors[i]));
            }
            ColorForm.CustomColors = lstCustomColors.ToArray();
            if (ColorForm.ShowDialog() == DialogResult.OK)
            {
                Color GetColor = ColorForm.Color;
                txt.BackColor = GetColor;
                txt.Text = GetColor.R + "," + GetColor.G + "," + GetColor.B;
            }
        }
    }
}
