﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class UCBtnsGroup.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class UCButtonGroup : UserControl
    {
        /// <summary>
        /// 选中改变事件
        /// </summary>
        [Description("选中改变事件"), Category("自定义")]
        public event EventHandler SelectedItemChanged;
        /// <summary>
        /// The m data source
        /// </summary>
        private Dictionary<string, string> m_dataSource = new Dictionary<string, string>();
        /// <summary>
        /// 数据源
        /// </summary>
        /// <value>The data source.</value>
        [Description("数据源"), Category("自定义")]
        public Dictionary<string, string> DataSource
        {
            get { return m_dataSource; }
            set
            {
                m_dataSource = value;
                Reload();
            }
        }

        /// <summary>
        /// The m select item
        /// </summary>
        private List<string> m_selectItem = new List<string>();
        /// <summary>
        /// 选中项
        /// </summary>
        /// <value>The select item.</value>
        [Description("选中项"), Category("自定义")]
        public List<string> SelectItem
        {
            get { return m_selectItem; }
            set
            {
                m_selectItem = value;
                if (m_selectItem == null)
                {
                    m_selectItem = new List<string>();
                }

                SetSelected();
            }
        }

        /// <summary>
        /// The m is multiple
        /// </summary>
        private bool m_isMultiple = false;
        /// <summary>
        /// 是否多选
        /// </summary>
        /// <value><c>true</c> if this instance is multiple; otherwise, <c>false</c>.</value>
        [Description("是否多选"), Category("自定义")]
        public bool IsMultiple
        {
            get { return m_isMultiple; }
            set { m_isMultiple = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCButtonGroup" /> class.
        /// </summary>
        public UCButtonGroup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        private void Reload()
        {
            try
            {
                ControlHelper.FreezeControl(flowLayoutPanel1, true);
                flowLayoutPanel1.Controls.Clear();
                if (DataSource != null)
                {
                    foreach (var item in DataSource)
                    {
                        UCButton btn = new UCButton();
                        btn.BackColor = System.Drawing.Color.Transparent;
                        btn.BtnBackColor = System.Drawing.Color.White;
                        btn.BtnFont = new System.Drawing.Font("微软雅黑", 10F);
                        btn.BtnForeColor = System.Drawing.Color.Gray;
                        btn.BtnText = item.Value;
                        btn.ConerRadius = 5;
                        btn.Cursor = System.Windows.Forms.Cursors.Hand;
                        btn.FillColor = System.Drawing.Color.White;
                        btn.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
                        btn.IsRadius = true;
                        btn.IsShowRect = true;
                        btn.IsShowTips = false;
                        btn.Location = new System.Drawing.Point(5, 5);
                        btn.Margin = new System.Windows.Forms.Padding(5);
                        btn.Name = item.Key;
                        btn.RectColor = System.Drawing.Color.FromArgb(224, 224, 224);
                        btn.RectWidth = 1;
                        btn.Size = new System.Drawing.Size(72, 38);
                        btn.TabStop = false;
                        btn.BtnClick += btn_BtnClick;
                        flowLayoutPanel1.Controls.Add(btn);
                    }
                }
            }
            finally
            {
                ControlHelper.FreezeControl(flowLayoutPanel1, false);
            }
            SetSelected();
        }

        /// <summary>
        /// Handles the BtnClick event of the btn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void btn_BtnClick(object sender, EventArgs e)
        {
            var btn = sender as UCButton;
            if (m_selectItem.Contains(btn.Name))
            {
                btn.RectColor = System.Drawing.Color.FromArgb(224, 224, 224);
                m_selectItem.Remove(btn.Name);
            }
            else
            {
                if (!m_isMultiple)
                {
                    foreach (var item in m_selectItem)
                    {
                        var lst = flowLayoutPanel1.Controls.Find(item, false);
                        if (lst.Length == 1)
                        {
                            var _btn = lst[0] as UCButton;
                            _btn.RectColor = System.Drawing.Color.FromArgb(224, 224, 224);
                        }
                    }
                    m_selectItem.Clear();
                }
                btn.RectColor = System.Drawing.Color.FromArgb(255, 77, 59);
                m_selectItem.Add(btn.Name);
            }
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, e);
            }
        }

        /// <summary>
        /// Sets the selected.
        /// </summary>
        private void SetSelected()
        {
            if (m_selectItem != null && m_selectItem.Count > 0 && DataSource != null && DataSource.Count > 0)
            {
                try
                {
                    ControlHelper.FreezeControl(flowLayoutPanel1, true);
                    if (m_isMultiple)
                    {
                        foreach (var item in m_selectItem)
                        {
                            var lst = flowLayoutPanel1.Controls.Find(item, false);
                            if (lst.Length == 1)
                            {
                                var btn = lst[0] as UCButton;
                                btn.RectColor = System.Drawing.Color.FromArgb(255, 77, 59);
                            }
                        }
                    }
                    else
                    {
                        UCButton btn = null;
                        foreach (var item in m_selectItem)
                        {
                            var lst = flowLayoutPanel1.Controls.Find(item, false);
                            if (lst.Length == 1)
                            {
                                btn = lst[0] as UCButton;
                                break;
                            }
                        }
                        if (btn != null)
                        {
                            m_selectItem = new List<string>() { btn.Name };
                            btn.RectColor = System.Drawing.Color.FromArgb(255, 77, 59);
                        }
                    }
                }
                finally
                {
                    ControlHelper.FreezeControl(flowLayoutPanel1, false);
                }
            }
        }
    }
}
