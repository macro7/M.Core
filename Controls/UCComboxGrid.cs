﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class UCComboxGrid.
    /// Implements the <see cref="M.Core.Controls.UCCombox" />
    /// </summary>
    /// <seealso cref="M.Core.Controls.UCCombox" />
    public partial class UCComboxGrid : UCCombox
    {

        /// <summary>
        /// The m row type
        /// </summary>
        private Type m_rowType = typeof(UCDataGridViewRow);
        /// <summary>
        /// 表格行类型
        /// </summary>
        /// <value>The type of the grid row.</value>
        [Description("表格行类型"), Category("自定义")]
        public Type GridRowType
        {
            get { return m_rowType; }
            set
            {
                m_rowType = value;
            }
        }
        /// <summary>
        /// The int width
        /// </summary>
        int intWidth = 0;

        /// <summary>
        /// The m columns
        /// </summary>
        private List<DataGridViewColumnEntity> m_columns = null;
        /// <summary>
        /// 表格列
        /// </summary>
        /// <value>The grid columns.</value>
        [Description("表格列"), Category("自定义")]
        public List<DataGridViewColumnEntity> GridColumns
        {
            get { return m_columns; }
            set
            {
                m_columns = value;
                if (value != null)
                {
                    intWidth = value.Sum(p => p.WidthType == SizeType.Absolute ? p.Width : (p.Width < 80 ? 80 : p.Width));
                }
            }
        }
        /// <summary>
        /// The m data source
        /// </summary>
        private List<object> m_dataSource = null;
        /// <summary>
        /// 表格数据源
        /// </summary>
        /// <value>The grid data source.</value>
        [Description("表格数据源"), Category("自定义")]
        public List<object> GridDataSource
        {
            get { return m_dataSource; }
            set { m_dataSource = value; }
        }

        /// <summary>
        /// The m text field
        /// </summary>
        private string m_textField;
        /// <summary>
        /// 显示值字段名称
        /// </summary>
        /// <value>The text field.</value>
        [Description("显示值字段名称"), Category("自定义")]
        public string TextField
        {
            get { return m_textField; }
            set
            {
                m_textField = value;
                SetText();
            }
        }
        /// <summary>
        /// 控件样式
        /// </summary>
        /// <value>The box style.</value>
        [Obsolete("不再可用的属性")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        private new ComboBoxStyle BoxStyle
        {
            get;
            set;
        }
        /// <summary>
        /// The select source
        /// </summary>
        private object selectSource = null;
        /// <summary>
        /// 选中的数据源
        /// </summary>
        /// <value>The select source.</value>
        [Description("选中的数据源"), Category("自定义")]
        public object SelectSource
        {
            get { return selectSource; }
            set
            {
                selectSource = value;
                SetText();
                if (SelectedChangedEvent != null)
                {
                    SelectedChangedEvent(value, null);
                }
            }
        }


        /// <summary>
        /// 选中数据源改变事件
        /// </summary>
        [Description("选中数据源改变事件"), Category("自定义")]
        public new event EventHandler SelectedChangedEvent;
        /// <summary>
        /// Initializes a new instance of the <see cref="UCComboxGrid" /> class.
        /// </summary>
        public UCComboxGrid()
        {
            InitializeComponent();
        }
        /// <summary>
        /// The m uc panel
        /// </summary>
        UCComboxGridPanel m_ucPanel = null;
        /// <summary>
        /// The FRM anchor
        /// </summary>
        Forms.FrmAnchor _frmAnchor;
        /// <summary>
        /// Handles the MouseDown event of the click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        protected override void click_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_columns == null || m_columns.Count <= 0)
            {
                return;
            }

            if (m_ucPanel == null)
            {
                var p = Parent.PointToScreen(Location);
                int intScreenHeight = Screen.PrimaryScreen.Bounds.Height;
                int intHeight = Math.Max(p.Y, intScreenHeight - p.Y - Height);
                intHeight -= 100;
                m_ucPanel = new UCComboxGridPanel();
                m_ucPanel.ItemClick += m_ucPanel_ItemClick;
                m_ucPanel.Height = intHeight;
                m_ucPanel.Width = intWidth;
                m_ucPanel.Columns = m_columns;
                m_ucPanel.RowType = m_rowType;
                if (m_dataSource != null && m_dataSource.Count > 0)
                {
                    int _intHeight = Math.Min(110 + m_dataSource.Count * 36, m_ucPanel.Height);
                    if (_intHeight <= 0)
                    {
                        _intHeight = 100;
                    }

                    m_ucPanel.Height = _intHeight;
                }
            }
            m_ucPanel.DataSource = m_dataSource;
            if (_frmAnchor == null || _frmAnchor.IsDisposed || _frmAnchor.Visible == false)
            {
                _frmAnchor = new Forms.FrmAnchor(this, m_ucPanel, isNotFocus: false);
                _frmAnchor.Show(FindForm());
            }

        }

        /// <summary>
        /// Handles the ItemClick event of the m_ucPanel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewEventArgs" /> instance containing the event data.</param>
        void m_ucPanel_ItemClick(object sender, DataGridViewEventArgs e)
        {
            _frmAnchor.Hide();
            SelectSource = sender;
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        private void SetText()
        {
            if (!string.IsNullOrEmpty(m_textField) && selectSource != null)
            {
                var pro = selectSource.GetType().GetProperty(m_textField);
                if (pro != null)
                {
                    TextValue = pro.GetValue(selectSource, null)?.ToString();
                }
            }
        }
    }
}