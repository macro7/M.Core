﻿using System;
using System.Windows.Forms;

namespace M.Core.Forms
{
    /// <summary>
    /// Class FrmWaiting.
    /// Implements the <see cref="FrmBase" />
    /// </summary>
    /// <seealso cref="FrmBase" />
    public partial class FrmWaiting : FrmBase
    {
        /// <summary>
        /// Gets or sets the MSG.
        /// </summary>
        /// <value>The MSG.</value>
        public string Msg { get { return label2.Text; } set { label2.Text = value; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="FrmWaiting" /> class.
        /// </summary>
        public FrmWaiting()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Tick event of the timer1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label1.ImageIndex == imageList1.Images.Count - 1)
            {
                label1.ImageIndex = 0;
            }
            else
            {
                label1.ImageIndex++;
            }
        }

        /// <summary>
        /// Handles the VisibleChanged event of the FrmWaiting control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FrmWaiting_VisibleChanged(object sender, EventArgs e)
        {
            //this.timer1.Enabled = this.Visible;
        }

        /// <summary>
        /// Does the escape.
        /// </summary>
        protected override void DoEsc()
        {

        }

        /// <summary>
        /// Handles the Tick event of the timer2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            base.Opacity = 1.0;
            timer2.Enabled = false;
        }

        /// <summary>
        /// Shows the form.
        /// </summary>
        /// <param name="intSleep">The int sleep.</param>
        public void ShowForm(int intSleep = 1)
        {
            base.Opacity = 0.0;
            if (intSleep <= 0)
            {
                intSleep = 1;
            }
            base.Show();
            timer2.Interval = intSleep;
            timer2.Enabled = true;
        }
    }
}
