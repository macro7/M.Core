using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class UCBtnFillet.
    /// Implements the <see cref="M.Core.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="M.Core.Controls.UCControlBase" />
    [DefaultEvent("BtnClick")]
    public partial class UCButtonFillet : UCControlBase
    {
        /// <summary>
        /// 按钮点击事件
        /// </summary>
        [Description("按钮点击事件"), Category("自定义")]
        public event EventHandler BtnClick;
        /// <summary>
        /// 按钮图片
        /// </summary>
        /// <value>The BTN image.</value>
        [Description("按钮图片"), Category("自定义")]
        public Image BtnImage
        {
            get
            {
                return lbl.Image;
            }
            set
            {
                lbl.Image = value;
            }
        }
        /// <summary>
        /// 按钮文字
        /// </summary>
        /// <value>The BTN text.</value>
        [Description("按钮文字"), Category("自定义")]
        public string BtnText
        {
            get { return lbl.Text; }
            set
            {
                lbl.Text = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCButtonFillet" /> class.
        /// </summary>
        public UCButtonFillet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseDown event of the lbl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void lbl_MouseDown(object sender, MouseEventArgs e)
        {
            if (BtnClick != null)
            {
                BtnClick(this, e);
            }
        }
    }
}
