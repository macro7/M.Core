﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class UCListItemExt.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [ToolboxItem(false)]
    public partial class UCListItemExt : UserControl
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Description("标题"), Category("自定义")]
        public string Title
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        /// <summary>
        /// Gets or sets the title2.
        /// </summary>
        /// <value>The title2.</value>
        [Description("副标题"), Category("自定义")]
        public string Title2
        {
            get { return label3.Text; }
            set
            {
                label3.Text = value;
                label3.Visible = !string.IsNullOrEmpty(value);
                var g = label3.CreateGraphics();
                var size = g.MeasureString(value, label3.Font);
                label3.Width = (int)size.Width + 10;
            }
        }

        /// <summary>
        /// Gets or sets the title font.
        /// </summary>
        /// <value>The title font.</value>
        [Description("标题字体"), Category("自定义")]
        public Font TitleFont
        {
            get { return label1.Font; }
            set
            {
                label1.Font = value;
            }
        }

        /// <summary>
        /// Gets or sets the title2 font.
        /// </summary>
        /// <value>The title2 font.</value>
        [Description("副标题字体"), Category("自定义")]
        public Font Title2Font
        {
            get { return label3.Font; }
            set
            {
                label3.Font = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the item back.
        /// </summary>
        /// <value>The color of the item back.</value>
        [Description("背景色"), Category("自定义")]
        public Color ItemBackColor
        {
            get { return BackColor; }
            set
            {
                BackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the item fore.
        /// </summary>
        /// <value>The color of the item fore.</value>
        [Description("标题文本色"), Category("自定义")]
        public Color ItemForeColor
        {
            get { return label1.ForeColor; }
            set { label1.ForeColor = value; }
        }

        /// <summary>
        /// Gets or sets the item fore color2.
        /// </summary>
        /// <value>The item fore color2.</value>
        [Description("副标题文本色"), Category("自定义")]
        public Color ItemForeColor2
        {
            get { return label3.ForeColor; }
            set { label3.ForeColor = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show more BTN].
        /// </summary>
        /// <value><c>true</c> if [show more BTN]; otherwise, <c>false</c>.</value>
        [Description("是否显示右侧更多箭头"), Category("自定义")]
        public bool ShowMoreBtn
        {
            get { return label2.Visible; }
            set { label2.Visible = value; ; }
        }

        /// <summary>
        /// Occurs when [item click].
        /// </summary>
        [Description("项选中事件"), Category("自定义")]
        public event EventHandler ItemClick;

        /// <summary>
        /// 数据源
        /// </summary>
        /// <value>The data source.</value>
        public ListEntity DataSource { get; private set; }

        [Description("分割线颜色"), Category("自定义")]
        public Color SplitColor
        {
            get { return splitLine_H1.BackColor; }
            set { splitLine_H1.BackColor = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCListItemExt" /> class.
        /// </summary>
        public UCListItemExt()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// Handles the MouseDown event of the item control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void item_MouseDown(object sender, MouseEventArgs e)
        {
            if (ItemClick != null)
            {
                ItemClick(this, e);
            }
        }

        #region 设置数据
        /// <summary>
        /// </summary>
        /// <param name="data">data</param>
        public void SetData(ListEntity data)
        {
            Title = data.Title;
            Title2 = data.Title2;
            ShowMoreBtn = data.ShowMoreBtn;
            DataSource = data;
        }
        #endregion
    }
}
