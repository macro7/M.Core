using System.ComponentModel;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class UCPagerControl.
    /// Implements the <see cref="M.Core.Controls.UCPagerControlBase" />
    /// </summary>
    /// <seealso cref="M.Core.Controls.UCPagerControlBase" />
    [ToolboxItem(true)]
    public partial class UCPagerControl : UCPagerControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UCPagerControl" /> class.
        /// </summary>
        public UCPagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseDown event of the panel1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            PreviousPage();
        }

        /// <summary>
        /// Handles the MouseDown event of the panel2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            NextPage();
        }

        /// <summary>
        /// Handles the MouseDown event of the panel3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            FirstPage();
        }

        /// <summary>
        /// Handles the MouseDown event of the panel4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            EndPage();
        }

        /// <summary>
        /// Shows the BTN.
        /// </summary>
        /// <param name="blnLeftBtn">if set to <c>true</c> [BLN left BTN].</param>
        /// <param name="blnRightBtn">if set to <c>true</c> [BLN right BTN].</param>
        protected override void ShowBtn(bool blnLeftBtn, bool blnRightBtn)
        {
            panel1.Visible = panel3.Visible = blnLeftBtn;
            panel2.Visible = panel4.Visible = blnRightBtn;
        }
    }
}
