using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace M.Core.Controls
{

    /// <summary>
    /// Class UCVScrollbar.
    /// Implements the <see cref="M.Core.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="M.Core.Controls.UCControlBase" />
    [Designer(typeof(ScrollbarControlDesigner))]
    [DefaultEvent("Scroll")]
    public class UCVScrollbar : UCControlBase
    {
        /// <summary>
        /// The mo large change
        /// </summary>
        protected int moLargeChange = 10;
        /// <summary>
        /// The mo small change
        /// </summary>
        protected int moSmallChange = 1;
        /// <summary>
        /// The mo minimum
        /// </summary>
        protected int moMinimum = 0;
        /// <summary>
        /// The mo maximum
        /// </summary>
        protected int moMaximum = 100;
        /// <summary>
        /// The mo value
        /// </summary>
        protected int moValue = 0;
        /// <summary>
        /// The n click point
        /// </summary>
        private int nClickPoint;
        /// <summary>
        /// The mo thumb top
        /// </summary>
        protected int moThumbTop = 0;
        /// <summary>
        /// The mo automatic size
        /// </summary>
        protected bool moAutoSize = false;
        /// <summary>
        /// The mo thumb down
        /// </summary>
        private bool moThumbMouseDown = false;
        /// <summary>
        /// The mo thumb dragging
        /// </summary>
        private bool moThumbMouseDragging = false;
        /// <summary>
        /// Occurs when [scroll].
        /// </summary>
        public new event EventHandler Scroll = null;
        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event EventHandler ValueChanged = null;

        /// <summary>
        /// The BTN height
        /// </summary>
        private int btnHeight = 18;
        /// <summary>
        /// The m int thumb minimum height
        /// </summary>
        private int m_intThumbMinHeight = 15;

        /// <summary>
        /// Gets or sets the height of the BTN.
        /// </summary>
        /// <value>The height of the BTN.</value>
        public int BtnHeight
        {
            get { return btnHeight; }
            set { btnHeight = value; }
        }
        /// <summary>
        /// Gets or sets the large change.
        /// </summary>
        /// <value>The large change.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("LargeChange")]
        public int LargeChange
        {
            get { return moLargeChange; }
            set
            {
                moLargeChange = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the small change.
        /// </summary>
        /// <value>The small change.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("SmallChange")]
        public int SmallChange
        {
            get { return moSmallChange; }
            set
            {
                moSmallChange = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("Minimum")]
        public int Minimum
        {
            get { return moMinimum; }
            set
            {
                moMinimum = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("Maximum")]
        public int Maximum
        {
            get { return moMaximum; }
            set
            {
                moMaximum = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("Value")]
        public int Value
        {
            get { return moValue; }
            set
            {
                moValue = value;

                int nTrackHeight = (Height - btnHeight * 2);
                float fThumbHeight = (LargeChange / (float)Maximum) * nTrackHeight;
                int nThumbHeight = (int)fThumbHeight;

                if (nThumbHeight > nTrackHeight)
                {
                    nThumbHeight = nTrackHeight;
                    fThumbHeight = nTrackHeight;
                }
                if (nThumbHeight < m_intThumbMinHeight)
                {
                    nThumbHeight = m_intThumbMinHeight;
                    fThumbHeight = m_intThumbMinHeight;
                }

                //figure out value
                int nPixelRange = nTrackHeight - nThumbHeight;
                int nRealRange = (Maximum - Minimum) - LargeChange;
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = moValue / (float)nRealRange;

                }

                float fTop = fPerc * nPixelRange;
                moThumbTop = (int)fTop;


                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic size].
        /// </summary>
        /// <value><c>true</c> if [automatic size]; otherwise, <c>false</c>.</value>
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
                if (base.AutoSize)
                {
                    Width = 15;
                }
            }
        }

        /// <summary>
        /// The thumb color
        /// </summary>
        private Color thumbColor = Color.FromArgb(255, 77, 58);

        /// <summary>
        /// Gets or sets the color of the thumb.
        /// </summary>
        /// <value>The color of the thumb.</value>
        public Color ThumbColor
        {
            get { return thumbColor; }
            set { thumbColor = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCVScrollbar"/> class.
        /// </summary>
        public UCVScrollbar()
        {
            InitializeComponent();
            ConerRadius = 2;
            FillColor = Color.FromArgb(239, 239, 239);
            IsShowRect = false;
            IsRadius = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // UCVScrollbar
            // 
            MinimumSize = new System.Drawing.Size(10, 0);
            Name = "UCVScrollbar";
            Size = new System.Drawing.Size(18, 150);
            MouseDown += new System.Windows.Forms.MouseEventHandler(CustomScrollbar_MouseDown);
            MouseMove += new System.Windows.Forms.MouseEventHandler(CustomScrollbar_MouseMove);
            MouseUp += new System.Windows.Forms.MouseEventHandler(CustomScrollbar_MouseUp);
            ResumeLayout(false);

        }


        /// <summary>
        /// 引发 <see cref="E:System.Windows.Forms.Control.Paint" /> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Forms.PaintEventArgs" />。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SetGDIHigh();

            //draw thumb
            int nTrackHeight = (Height - btnHeight * 2);
            float fThumbHeight = (LargeChange / (float)Maximum) * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < m_intThumbMinHeight)
            {
                nThumbHeight = m_intThumbMinHeight;
                fThumbHeight = m_intThumbMinHeight;
            }
            int nTop = moThumbTop;
            nTop += btnHeight;
            e.Graphics.FillPath(new SolidBrush(thumbColor), new Rectangle(1, nTop, Width - 3, nThumbHeight).CreateRoundedRectanglePath(ConerRadius));

            ControlHelper.PaintTriangle(e.Graphics, new SolidBrush(thumbColor), new Point(Width / 2, btnHeight - Math.Min(5, Width / 2)), Math.Min(5, Width / 2), GraphDirection.Upward);
            ControlHelper.PaintTriangle(e.Graphics, new SolidBrush(thumbColor), new Point(Width / 2, Height - (btnHeight - Math.Min(5, Width / 2))), Math.Min(5, Width / 2), GraphDirection.Downward);

        }



        /// <summary>
        /// Handles the MouseDown event of the CustomScrollbar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
        {
            Point ptPoint = PointToClient(Cursor.Position);
            int nTrackHeight = (Height - btnHeight * 2);
            float fThumbHeight = (LargeChange / (float)Maximum) * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < m_intThumbMinHeight)
            {
                nThumbHeight = m_intThumbMinHeight;
                fThumbHeight = m_intThumbMinHeight;
            }

            int nTop = moThumbTop;
            nTop += btnHeight;


            Rectangle thumbrect = new Rectangle(new Point(1, nTop), new Size(Width - 2, nThumbHeight));
            if (thumbrect.Contains(ptPoint))
            {

                //hit the thumb
                nClickPoint = (ptPoint.Y - nTop);
                //MessageBox.Show(Convert.ToString((ptPoint.Y - nTop)));
                moThumbMouseDown = true;
            }
            else
            {
                Rectangle uparrowrect = new Rectangle(new Point(1, 0), new Size(Width, btnHeight));
                if (uparrowrect.Contains(ptPoint))
                {

                    int nRealRange = (Maximum - Minimum) - LargeChange;
                    int nPixelRange = (nTrackHeight - nThumbHeight);
                    if (nRealRange > 0)
                    {
                        if (nPixelRange > 0)
                        {
                            if ((moThumbTop - SmallChange) < 0)
                            {
                                moThumbTop = 0;
                            }
                            else
                            {
                                moThumbTop -= SmallChange;
                            }

                            //figure out value
                            float fPerc = moThumbTop / (float)nPixelRange;
                            float fValue = fPerc * (Maximum - LargeChange);

                            moValue = (int)fValue;

                            if (ValueChanged != null)
                            {
                                ValueChanged(this, new EventArgs());
                            }

                            if (Scroll != null)
                            {
                                Scroll(this, new EventArgs());
                            }

                            Invalidate();
                        }
                    }
                }
                else
                {
                    Rectangle downarrowrect = new Rectangle(new Point(1, btnHeight + nTrackHeight), new Size(Width, btnHeight));
                    if (downarrowrect.Contains(ptPoint))
                    {
                        int nRealRange = (Maximum - Minimum) - LargeChange;
                        int nPixelRange = (nTrackHeight - nThumbHeight);
                        if (nRealRange > 0)
                        {
                            if (nPixelRange > 0)
                            {
                                if ((moThumbTop + SmallChange) > nPixelRange)
                                {
                                    moThumbTop = nPixelRange;
                                }
                                else
                                {
                                    moThumbTop += SmallChange;
                                }

                                //figure out value
                                float fPerc = moThumbTop / (float)nPixelRange;
                                float fValue = fPerc * (Maximum - LargeChange);

                                moValue = (int)fValue;

                                if (ValueChanged != null)
                                {
                                    ValueChanged(this, new EventArgs());
                                }

                                if (Scroll != null)
                                {
                                    Scroll(this, new EventArgs());
                                }

                                Invalidate();
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Handles the MouseUp event of the CustomScrollbar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
        {
            moThumbMouseDown = false;
            moThumbMouseDragging = false;
        }

        /// <summary>
        /// Moves the thumb.
        /// </summary>
        /// <param name="y">The y.</param>
        private void MoveThumb(int y)
        {
            int nRealRange = Maximum - Minimum;
            int nTrackHeight = (Height - btnHeight * 2);
            float fThumbHeight = (LargeChange / (float)Maximum) * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < m_intThumbMinHeight)
            {
                nThumbHeight = m_intThumbMinHeight;
                fThumbHeight = m_intThumbMinHeight;
            }

            int nSpot = nClickPoint;

            int nPixelRange = (nTrackHeight - nThumbHeight);
            if (moThumbMouseDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    int nNewThumbTop = y - (btnHeight + nSpot);

                    if (nNewThumbTop < 0)
                    {
                        moThumbTop = nNewThumbTop = 0;
                    }
                    else if (nNewThumbTop > nPixelRange)
                    {
                        moThumbTop = nNewThumbTop = nPixelRange;
                    }
                    else
                    {
                        moThumbTop = y - (btnHeight + nSpot);
                    }


                    float fPerc = moThumbTop / (float)nPixelRange;
                    float fValue = fPerc * (Maximum - LargeChange);
                    moValue = (int)fValue;

                    Application.DoEvents();

                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the CustomScrollbar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!moThumbMouseDown)
            {
                return;
            }

            if (moThumbMouseDown == true)
            {
                moThumbMouseDragging = true;
            }

            if (moThumbMouseDragging)
            {
                MoveThumb(e.Y);
            }

            if (ValueChanged != null)
            {
                ValueChanged(this, new EventArgs());
            }

            if (Scroll != null)
            {
                Scroll(this, new EventArgs());
            }
        }

    }
}