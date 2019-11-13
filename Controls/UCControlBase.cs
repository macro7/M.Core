﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class UCControlBase.
    /// Implements the <see cref="UserControl" />
    /// Implements the <see cref="IContainerControl" />
    /// </summary>
    /// <seealso cref="UserControl" />
    /// <seealso cref="IContainerControl" />
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(System.ComponentModel.Design.IDesigner))]
    public partial class UCControlBase : UserControl, IContainerControl
    {
        /// <summary>
        /// The is radius
        /// </summary>
        private bool _isRadius = false;

        /// <summary>
        /// The corner radius
        /// </summary>
        private int _cornerRadius = 24;


        /// <summary>
        /// The is show rect
        /// </summary>
        private bool _isShowRect = false;

        /// <summary>
        /// The rect color
        /// </summary>
        private Color _rectColor = Color.FromArgb(220, 220, 220);

        /// <summary>
        /// The rect width
        /// </summary>
        private int _rectWidth = 1;

        /// <summary>
        /// The fill color
        /// </summary>
        private Color _fillColor = Color.Transparent;
        /// <summary>
        /// 是否圆角
        /// </summary>
        /// <value><c>true</c> if this instance is radius; otherwise, <c>false</c>.</value>
        [Description("是否圆角"), Category("自定义")]
        public virtual bool IsRadius
        {
            get
            {
                return _isRadius;
            }
            set
            {
                _isRadius = value;
                Refresh();
            }
        }
        /// <summary>
        /// 圆角角度
        /// </summary>
        /// <value>The coner radius.</value>
        [Description("圆角角度"), Category("自定义")]
        public virtual int ConerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                _cornerRadius = Math.Max(value, 1);
                Refresh();
            }
        }

        /// <summary>
        /// 是否显示边框
        /// </summary>
        /// <value><c>true</c> if this instance is show rect; otherwise, <c>false</c>.</value>
        [Description("是否显示边框"), Category("自定义")]
        public virtual bool IsShowRect
        {
            get
            {
                return _isShowRect;
            }
            set
            {
                _isShowRect = value;
                Refresh();
            }
        }
        /// <summary>
        /// 边框颜色
        /// </summary>
        /// <value>The color of the rect.</value>
        [Description("边框颜色"), Category("自定义")]
        public virtual Color RectColor
        {
            get
            {
                return _rectColor;
            }
            set
            {
                _rectColor = value;
                Refresh();
            }
        }
        /// <summary>
        /// 边框宽度
        /// </summary>
        /// <value>The width of the rect.</value>
        [Description("边框宽度"), Category("自定义")]
        public virtual int RectWidth
        {
            get
            {
                return _rectWidth;
            }
            set
            {
                _rectWidth = value;
                Refresh();
            }
        }
        /// <summary>
        /// 当使用边框时填充颜色，当值为背景色或透明色或空值则不填充
        /// </summary>
        /// <value>The color of the fill.</value>
        [Description("当使用边框时填充颜色，当值为背景色或透明色或空值则不填充"), Category("自定义")]
        public virtual Color FillColor
        {
            get
            {
                return _fillColor;
            }
            set
            {
                _fillColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCControlBase" /> class.
        /// </summary>
        public UCControlBase()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// 引发 <see cref="E:System.Windows.Forms.Control.Paint" /> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Forms.PaintEventArgs" />。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Visible)
            {
                if (_isRadius)
                {
                    SetWindowRegion();
                }

                GraphicsPath graphicsPath = new GraphicsPath();
                if (_isShowRect || (_fillColor != Color.Empty && _fillColor != Color.Transparent && _fillColor != BackColor))
                {
                    Rectangle clientRectangle = base.ClientRectangle;
                    if (_isRadius)
                    {
                        graphicsPath.AddArc(0, 0, _cornerRadius, _cornerRadius, 180f, 90f);
                        graphicsPath.AddArc(clientRectangle.Width - _cornerRadius - 1, 0, _cornerRadius, _cornerRadius, 270f, 90f);
                        graphicsPath.AddArc(clientRectangle.Width - _cornerRadius - 1, clientRectangle.Height - _cornerRadius - 1, _cornerRadius, _cornerRadius, 0f, 90f);
                        graphicsPath.AddArc(0, clientRectangle.Height - _cornerRadius - 1, _cornerRadius, _cornerRadius, 90f, 90f);
                        graphicsPath.CloseFigure();
                    }
                    else
                    {
                        graphicsPath.AddRectangle(clientRectangle);
                    }
                }
                e.Graphics.SetGDIHigh();
                if (_fillColor != Color.Empty && _fillColor != Color.Transparent && _fillColor != BackColor)
                {
                    e.Graphics.FillPath(new SolidBrush(_fillColor), graphicsPath);
                }

                if (_isShowRect)
                {
                    Color rectColor = _rectColor;
                    Pen pen = new Pen(rectColor, _rectWidth);
                    e.Graphics.DrawPath(pen, graphicsPath);
                }
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Sets the window region.
        /// </summary>
        private void SetWindowRegion()
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(-1, -1, base.Width + 1, base.Height);
            path = GetRoundedRectPath(rect, _cornerRadius);
            base.Region = new Region(path);
        }

        /// <summary>
        /// Gets the rounded rect path.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>GraphicsPath.</returns>
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            Rectangle rect2 = new Rectangle(rect.Location, new Size(radius, radius));
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(rect2, 180f, 90f);//左上角
            rect2.X = rect.Right - radius;
            graphicsPath.AddArc(rect2, 270f, 90f);//右上角
            rect2.Y = rect.Bottom - radius;
            rect2.Width += 1;
            rect2.Height += 1;
            graphicsPath.AddArc(rect2, 360f, 90f);//右下角           
            rect2.X = rect.Left;
            graphicsPath.AddArc(rect2, 90f, 90f);//左下角
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        /// <summary>
        /// WNDs the proc.
        /// </summary>
        /// <param name="m">要处理的 Windows <see cref="T:System.Windows.Forms.Message" />。</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg != 20)
            {
                base.WndProc(ref m);
            }
        }
    }
}
