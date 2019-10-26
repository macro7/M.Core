﻿using M.Core.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class UCDataGridViewRow.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// Implements the <see cref="M.Core.Controls.IDataGridViewRow" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="M.Core.Controls.IDataGridViewRow" />
    [ToolboxItem(false)]
    public partial class UCDataGridViewRow : UserControl, IDataGridViewRow
    {

        #region 属性
        /// <summary>
        /// CheckBox选中事件
        /// </summary>
        public event DataGridViewEventHandler CheckBoxChangeEvent;
        /// <summary>
        /// Occurs when [row custom event].
        /// </summary>
        public event DataGridViewRowCustomEventHandler RowCustomEvent;
        /// <summary>
        /// 点击单元格事件
        /// </summary>
        public event DataGridViewEventHandler CellClick;

        /// <summary>
        /// 数据源改变事件
        /// </summary>
        public event DataGridViewEventHandler SourceChanged;

        /// <summary>
        /// 列参数，用于创建列数和宽度
        /// </summary>
        /// <value>The columns.</value>
        public List<DataGridViewColumnEntity> Columns
        {
            get;
            set;
        }

        /// <summary>
        /// 数据源
        /// </summary>
        /// <value>The data source.</value>
        public object DataSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show CheckBox.
        /// </summary>
        /// <value><c>true</c> if this instance is show CheckBox; otherwise, <c>false</c>.</value>
        public bool IsShowCheckBox
        {
            get;
            set;
        }
        /// <summary>
        /// The m is checked
        /// </summary>
        private bool m_isChecked;
        /// <summary>
        /// 是否选中
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked
        {
            get
            {
                return m_isChecked;
            }

            set
            {
                if (m_isChecked != value)
                {
                    m_isChecked = value;
                    (panCells.Controls.Find("check", false)[0] as UCCheckBox).Checked = value;
                }
            }
        }
        /// <summary>
        /// The m row height
        /// </summary>
        int m_rowHeight = 40;
        /// <summary>
        /// 行高
        /// </summary>
        /// <value>The height of the row.</value>
        public int RowHeight
        {
            get
            {
                return m_rowHeight;
            }
            set
            {
                m_rowHeight = value;
                Height = value;
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UCDataGridViewRow" /> class.
        /// </summary>
        public UCDataGridViewRow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定数据到Cell
        /// </summary>
        /// <returns>返回true则表示已处理过，否则将进行默认绑定（通常只针对有Text值的控件）</returns>
        public void BindingCellData()
        {
            if (DataSource != null)
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    DataGridViewColumnEntity com = Columns[i];
                    var cs = panCells.Controls.Find("lbl_" + com.DataField, false);
                    if (cs != null && cs.Length > 0)
                    {
                        var pro = DataSource.GetType().GetProperty(com.DataField);
                        if (pro != null)
                        {
                            var value = pro.GetValue(DataSource, null);
                            if (com.Format != null)
                            {
                                cs[0].Text = com.Format(value);
                            }
                            else
                            {
                                cs[0].Text = value?.ToString();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the Item control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        void Item_MouseDown(object sender, MouseEventArgs e)
        {
            if (CellClick != null)
            {
                CellClick(this, new DataGridViewEventArgs()
                {
                    CellControl = this,
                    CellIndex = (sender as Control).Tag.ToInt()
                });
            }
        }

        /// <summary>
        /// 设置选中状态，通常为设置颜色即可
        /// </summary>
        /// <param name="blnSelected">是否选中</param>
        public void SetSelect(bool blnSelected)
        {
            if (blnSelected)
            {
                BackColor = Color.FromArgb(255, 247, 245);
            }
            else
            {
                BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// Reloads the cells.
        /// </summary>
        public void ReloadCells()
        {
            try
            {
                ControlHelper.FreezeControl(this, true);
                panCells.Controls.Clear();
                panCells.ColumnStyles.Clear();

                int intColumnsCount = Columns.Count();
                if (Columns != null && intColumnsCount > 0)
                {
                    if (IsShowCheckBox)
                    {
                        intColumnsCount++;
                    }
                    panCells.ColumnCount = intColumnsCount;
                    for (int i = 0; i < intColumnsCount; i++)
                    {
                        Control c = null;
                        if (i == 0 && IsShowCheckBox)
                        {
                            panCells.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 30F));

                            UCCheckBox box = new UCCheckBox();
                            box.Name = "check";
                            box.TextValue = "";
                            box.Size = new Size(30, 30);
                            box.Dock = DockStyle.Fill;
                            box.CheckedChangeEvent += (a, b) =>
                            {
                                IsChecked = box.Checked;
                                if (CheckBoxChangeEvent != null)
                                {
                                    CheckBoxChangeEvent(a, new DataGridViewEventArgs()
                                    {
                                        CellControl = box,
                                        CellIndex = 0
                                    });
                                }
                            };
                            c = box;
                        }
                        else
                        {
                            var item = Columns[i - (IsShowCheckBox ? 1 : 0)];
                            panCells.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(item.WidthType, item.Width));

                            Label lbl = new Label();
                            lbl.Tag = i - (IsShowCheckBox ? 1 : 0);
                            lbl.Name = "lbl_" + item.DataField;
                            lbl.Font = new Font("微软雅黑", 12);
                            lbl.ForeColor = Color.Black;
                            lbl.AutoSize = false;
                            lbl.Dock = DockStyle.Fill;
                            lbl.TextAlign = item.TextAlign;
                            lbl.MouseDown += (a, b) =>
                            {
                                Item_MouseDown(a, b);
                            };
                            c = lbl;
                        }
                        panCells.Controls.Add(c, i, 0);
                    }

                }
            }
            finally
            {
                ControlHelper.FreezeControl(this, false);
            }
        }

    }
}