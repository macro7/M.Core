﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class UCKeyBorderNum.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class UCKeyBorderNum : UserControl
    {
        /// <summary>
        /// The use custom event
        /// </summary>
        private bool useCustomEvent = false;
        /// <summary>
        /// 是否使用自定义的事件来接收按键，当为true时将不再向系统发送按键请求
        /// </summary>
        /// <value><c>true</c> if [use custom event]; otherwise, <c>false</c>.</value>
        [Description("是否使用自定义的事件来接收按键，当为true时将不再向系统发送按键请求"), Category("自定义")]
        public bool UseCustomEvent
        {
            get { return useCustomEvent; }
            set { useCustomEvent = value; }
        }
        /// <summary>
        /// Occurs when [number click].
        /// </summary>
        [Description("数字点击事件"), Category("自定义")]
        public event EventHandler NumClick;
        /// <summary>
        /// Occurs when [backspace click].
        /// </summary>
        [Description("删除点击事件"), Category("自定义")]
        public event EventHandler BackspaceClick;
        /// <summary>
        /// Occurs when [enter click].
        /// </summary>
        [Description("回车点击事件"), Category("自定义")]
        public event EventHandler EnterClick;
        /// <summary>
        /// Initializes a new instance of the <see cref="UCKeyBorderNum" /> class.
        /// </summary>
        public UCKeyBorderNum()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseDown event of the Num control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void Num_MouseDown(object sender, MouseEventArgs e)
        {
            if (NumClick != null)
            {
                NumClick(sender, e);
            }
            if (useCustomEvent)
            {
                return;
            }

            Label lbl = sender as Label;
            SendKeys.Send(lbl.Tag.ToString());
        }

        /// <summary>
        /// Handles the MouseDown event of the Backspace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void Backspace_MouseDown(object sender, MouseEventArgs e)
        {
            if (BackspaceClick != null)
            {
                BackspaceClick(sender, e);
            }
            if (useCustomEvent)
            {
                return;
            }

            Label lbl = sender as Label;
            SendKeys.Send("{BACKSPACE}");
        }

        /// <summary>
        /// Handles the MouseDown event of the Enter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void Enter_MouseDown(object sender, MouseEventArgs e)
        {
            if (EnterClick != null)
            {
                EnterClick(sender, e);
            }
            if (useCustomEvent)
            {
                return;
            }

            SendKeys.Send("{ENTER}");
        }
    }
}
