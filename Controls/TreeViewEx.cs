using M.Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class TreeViewEx.
    /// Implements the <see cref="TreeView" />
    /// </summary>
    /// <seealso cref="TreeView" />
    public partial class TreeViewEx : TreeView
    {

        /// <summary>
        /// The ws vscroll
        /// </summary>
        private const int WS_VSCROLL = 2097152;

        /// <summary>
        /// The GWL style
        /// </summary>
        private const int GWL_STYLE = -16;

        /// <summary>
        /// The LST tips
        /// </summary>
        private Dictionary<string, string> _lstTips = new Dictionary<string, string>();

        /// <summary>
        /// The node height
        /// </summary>
        private int _nodeHeight = 50;

        /// <summary>
        /// The tree font size
        /// </summary>
        private SizeF treeFontSize = SizeF.Empty;

        /// <summary>
        /// The BLN has v bar
        /// </summary>
        private bool blnHasVBar = false;

        /// <summary>
        /// Gets or sets the LST tips.
        /// </summary>
        /// <value>The LST tips.</value>
        public Dictionary<string, string> LstTips
        {
            get
            {
                return _lstTips;
            }
            set
            {
                _lstTips = value;
            }
        }

        /// <summary>
        /// Gets or sets the tip font.
        /// </summary>
        /// <value>The tip font.</value>
        [Category("自定义属性"), Description("角标文字字体")]
        public Font TipFont { get; set; } = new Font("Arial Unicode MS", 12f);

        /// <summary>
        /// Gets or sets the tip image.
        /// </summary>
        /// <value>The tip image.</value>
        [Category("自定义属性"), Description("是否显示角标")]
        public Image TipImage { get; set; } = Resources.tips;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show tip.
        /// </summary>
        /// <value><c>true</c> if this instance is show tip; otherwise, <c>false</c>.</value>
        [Category("自定义属性"), Description("是否显示角标")]
        public bool IsShowTip { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show by custom model.
        /// </summary>
        /// <value><c>true</c> if this instance is show by custom model; otherwise, <c>false</c>.</value>
        [Category("自定义属性"), Description("使用自定义模式")]
        public bool IsShowByCustomModel { get; set; } = true;

        /// <summary>
        /// Gets or sets the height of the node.
        /// </summary>
        /// <value>The height of the node.</value>
        [Category("自定义属性"), Description("节点高度（IsShowByCustomModel=true时生效）")]
        public int NodeHeight
        {
            get
            {
                return _nodeHeight;
            }
            set
            {
                _nodeHeight = value;
                base.ItemHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets the node down pic.
        /// </summary>
        /// <value>The node down pic.</value>
        [Category("自定义属性"), Description("下翻图标（IsShowByCustomModel=true时生效）")]
        public Image NodeDownPic { get; set; } = Resources.list_add;

        /// <summary>
        /// Gets or sets the node up pic.
        /// </summary>
        /// <value>The node up pic.</value>
        [Category("自定义属性"), Description("上翻图标（IsShowByCustomModel=true时生效）")]
        public Image NodeUpPic { get; set; } = Resources.list_subtract;

        /// <summary>
        /// Gets or sets the color of the node background.
        /// </summary>
        /// <value>The color of the node background.</value>
        [Category("自定义属性"), Description("节点背景颜色（IsShowByCustomModel=true时生效）")]
        public Color NodeBackgroundColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the color of the node fore.
        /// </summary>
        /// <value>The color of the node fore.</value>
        [Category("自定义属性"), Description("节点字体颜色（IsShowByCustomModel=true时生效）")]
        public Color NodeForeColor { get; set; } = Color.FromArgb(62, 62, 62);

        /// <summary>
        /// Gets or sets a value indicating whether [node is show split line].
        /// </summary>
        /// <value><c>true</c> if [node is show split line]; otherwise, <c>false</c>.</value>
        [Category("自定义属性"), Description("节点是否显示分割线（IsShowByCustomModel=true时生效）")]
        public bool NodeIsShowSplitLine { get; set; } = false;

        /// <summary>
        /// Gets or sets the color of the node split line.
        /// </summary>
        /// <value>The color of the node split line.</value>
        [Category("自定义属性"), Description("节点分割线颜色（IsShowByCustomModel=true时生效）")]
        public Color NodeSplitLineColor { get; set; } = Color.FromArgb(232, 232, 232);

        /// <summary>
        /// Gets or sets the color of the node selected.
        /// </summary>
        /// <value>The color of the node selected.</value>
        [Category("自定义属性"), Description("选中节点背景颜色（IsShowByCustomModel=true时生效）")]
        public Color NodeSelectedColor { get; set; } = Color.FromArgb(255, 77, 59);

        /// <summary>
        /// Gets or sets the color of the node selected fore.
        /// </summary>
        /// <value>The color of the node selected fore.</value>
        [Category("自定义属性"), Description("选中节点字体颜色（IsShowByCustomModel=true时生效）")]
        public Color NodeSelectedForeColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets a value indicating whether [parent node can select].
        /// </summary>
        /// <value><c>true</c> if [parent node can select]; otherwise, <c>false</c>.</value>
        [Category("自定义属性"), Description("父节点是否可选中")]
        public bool ParentNodeCanSelect { get; set; } = true;
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewEx" /> class.
        /// </summary>
        public TreeViewEx()
        {
            base.HideSelection = false;
            base.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            base.DrawNode += new DrawTreeNodeEventHandler(treeview_DrawNode);
            base.NodeMouseClick += new TreeNodeMouseClickEventHandler(TreeViewEx_NodeMouseClick);
            base.SizeChanged += new EventHandler(TreeViewEx_SizeChanged);
            base.AfterSelect += new TreeViewEventHandler(TreeViewEx_AfterSelect);
            base.FullRowSelect = true;
            base.ShowLines = false;
            base.ShowPlusMinus = false;
            base.ShowRootLines = false;
            BackColor = Color.White;
            BorderStyle = System.Windows.Forms.BorderStyle.None;
            DoubleBuffered = true;
        }
        /// <summary>
        /// 重写 <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" />。
        /// </summary>
        /// <param name="m">要处理的 Windows<see cref="T:System.Windows.Forms.Message" />。</param>
        protected override void WndProc(ref Message m)
        {

            if (m.Msg == 0x0014) // 禁掉清除背景消息WM_ERASEBKGND
            {
                return;
            }

            base.WndProc(ref m);

        }
        /// <summary>
        /// Handles the AfterSelect event of the TreeViewEx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TreeViewEventArgs" /> instance containing the event data.</param>
        private void TreeViewEx_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node != null)
                {
                    if (!ParentNodeCanSelect)
                    {
                        if (e.Node.Nodes.Count > 0)
                        {
                            e.Node.Expand();
                            base.SelectedNode = e.Node.Nodes[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Handles the SizeChanged event of the TreeViewEx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void TreeViewEx_SizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Handles the NodeMouseClick event of the TreeViewEx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TreeNodeMouseClickEventArgs" /> instance containing the event data.</param>
        private void TreeViewEx_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node != null)
                {
                    if (e.Node.Nodes.Count > 0)
                    {
                        if (e.Node.IsExpanded)
                        {
                            e.Node.Collapse();
                        }
                        else
                        {
                            e.Node.Expand();
                        }
                    }
                    if (base.SelectedNode != null)
                    {
                        if (base.SelectedNode == e.Node && e.Node.IsExpanded)
                        {
                            if (!ParentNodeCanSelect)
                            {
                                if (e.Node.Nodes.Count > 0)
                                {
                                    base.SelectedNode = e.Node.Nodes[0];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Handles the DrawNode event of the treeview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DrawTreeNodeEventArgs" /> instance containing the event data.</param>
        private void treeview_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            try
            {

                if (e.Node == null || !IsShowByCustomModel || (e.Node.Bounds.Width <= 0 && e.Node.Bounds.Height <= 0 && e.Node.Bounds.X <= 0 && e.Node.Bounds.Y <= 0))
                {
                    e.DrawDefault = true;
                }
                else
                {
                    e.Graphics.SetGDIHigh();
                    if (base.Nodes.IndexOf(e.Node) == 0)
                    {
                        blnHasVBar = IsVerticalScrollBarVisible();
                    }
                    Font font = e.Node.NodeFont;
                    if (font == null)
                    {
                        font = ((TreeView)sender).Font;
                    }
                    if (treeFontSize == SizeF.Empty)
                    {
                        treeFontSize = GetFontSize(font, e.Graphics);
                    }
                    bool flag = false;
                    int intLeft = 0;
                    if (CheckBoxes)
                    {
                        intLeft = 20;
                    }
                    int num = 0;
                    if (base.ImageList != null && base.ImageList.Images.Count > 0 && e.Node.ImageIndex >= 0 && e.Node.ImageIndex < base.ImageList.Images.Count)
                    {
                        flag = true;
                        num = (e.Bounds.Height - base.ImageList.ImageSize.Height) / 2;
                        intLeft += base.ImageList.ImageSize.Width;
                    }

                    intLeft += e.Node.Level * Indent;

                    if ((e.State == TreeNodeStates.Selected || e.State == TreeNodeStates.Focused || e.State == (TreeNodeStates.Focused | TreeNodeStates.Selected)) && (ParentNodeCanSelect || e.Node.Nodes.Count <= 0))
                    {
                        e.Graphics.FillRectangle(new SolidBrush(NodeSelectedColor), new Rectangle(new Point(0, e.Node.Bounds.Y), new Size(base.Width, e.Node.Bounds.Height)));
                        e.Graphics.DrawString(e.Node.Text, font, new SolidBrush(NodeSelectedForeColor), (float)e.Bounds.X + intLeft, e.Bounds.Y + (_nodeHeight - treeFontSize.Height) / 2f);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(NodeBackgroundColor), new Rectangle(new Point(0, e.Node.Bounds.Y), new Size(base.Width, e.Node.Bounds.Height)));
                        e.Graphics.DrawString(e.Node.Text, font, new SolidBrush(NodeForeColor), (float)e.Bounds.X + intLeft, e.Bounds.Y + (_nodeHeight - treeFontSize.Height) / 2f);
                    }
                    if (CheckBoxes)
                    {
                        Rectangle rectCheck = new Rectangle(e.Bounds.X + 3 + e.Node.Level * Indent, e.Bounds.Y + (e.Bounds.Height - 16) / 2, 16, 16);
                        GraphicsPath pathCheck = rectCheck.CreateRoundedRectanglePath(3);
                        e.Graphics.FillPath(new SolidBrush(Color.FromArgb(247, 247, 247)), pathCheck);
                        if (e.Node.Checked)
                        {
                            e.Graphics.DrawLines(new Pen(new SolidBrush(NodeSelectedColor), 2), new Point[]
                            {
                                new Point(rectCheck.Left+2,rectCheck.Top+8),
                                new Point(rectCheck.Left+6,rectCheck.Top+12),
                                new Point(rectCheck.Right-4,rectCheck.Top+4)
                            });
                        }

                        e.Graphics.DrawPath(new Pen(new SolidBrush(Color.FromArgb(200, 200, 200))), pathCheck);
                    }
                    if (flag)
                    {
                        int num2 = e.Bounds.X - num - base.ImageList.ImageSize.Width;
                        if (num2 < 0)
                        {
                            num2 = 3;
                        }
                        e.Graphics.DrawImage(base.ImageList.Images[e.Node.ImageIndex], new Rectangle(new Point(num2 + intLeft - base.ImageList.ImageSize.Width, e.Bounds.Y + num), base.ImageList.ImageSize));
                    }
                    if (NodeIsShowSplitLine)
                    {
                        e.Graphics.DrawLine(new Pen(NodeSplitLineColor, 1f), new Point(0, e.Bounds.Y + _nodeHeight - 1), new Point(base.Width, e.Bounds.Y + _nodeHeight - 1));
                    }
                    bool flag2 = false;
                    if (e.Node.Nodes.Count > 0)
                    {
                        if (e.Node.IsExpanded && NodeUpPic != null)
                        {
                            e.Graphics.DrawImage(NodeUpPic, new Rectangle(base.Width - (blnHasVBar ? 50 : 30), e.Bounds.Y + (_nodeHeight - 20) / 2, 20, 20));
                        }
                        else if (NodeDownPic != null)
                        {
                            e.Graphics.DrawImage(NodeDownPic, new Rectangle(base.Width - (blnHasVBar ? 50 : 30), e.Bounds.Y + (_nodeHeight - 20) / 2, 20, 20));
                        }
                        flag2 = true;
                    }
                    if (IsShowTip && _lstTips.ContainsKey(e.Node.Name) && !string.IsNullOrWhiteSpace(_lstTips[e.Node.Name]))
                    {
                        int num3 = base.Width - (blnHasVBar ? 50 : 30) - (flag2 ? 20 : 0);
                        int num4 = e.Bounds.Y + (_nodeHeight - 20) / 2;
                        e.Graphics.DrawImage(TipImage, new Rectangle(num3, num4, 20, 20));
                        SizeF sizeF = e.Graphics.MeasureString(_lstTips[e.Node.Name], TipFont, 100, StringFormat.GenericTypographic);
                        e.Graphics.DrawString(_lstTips[e.Node.Name], TipFont, new SolidBrush(Color.White), num3 + 10 - sizeF.Width / 2f - 3f, num4 + 10 - sizeF.Height / 2f);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the size of the font.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="g">The g.</param>
        /// <returns>SizeF.</returns>
        private SizeF GetFontSize(Font font, Graphics g = null)
        {
            SizeF result;
            try
            {
                bool flag = false;
                if (g == null)
                {
                    g = base.CreateGraphics();
                    flag = true;
                }
                SizeF sizeF = g.MeasureString("a", font, 100, StringFormat.GenericTypographic);
                if (flag)
                {
                    g.Dispose();
                }
                result = sizeF;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Gets the window long.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="nIndex">Index of the n.</param>
        /// <returns>System.Int32.</returns>
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        /// <summary>
        /// Determines whether [is vertical scroll bar visible].
        /// </summary>
        /// <returns><c>true</c> if [is vertical scroll bar visible]; otherwise, <c>false</c>.</returns>
        private bool IsVerticalScrollBarVisible()
        {
            return base.IsHandleCreated && (TreeViewEx.GetWindowLong(base.Handle, -16) & 2097152) != 0;
        }
    }
}
