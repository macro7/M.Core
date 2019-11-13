using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace M.Core.Controls
{
    /// <summary>
    /// Class TextBoxEx.
    /// Implements the <see cref="System.Windows.Forms.TextBox" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.TextBox" />
    public partial class TextBoxEx : TextBox
    {
        /// <summary>
        /// The BLN focus
        /// </summary>
        private bool blnFocus = false;

        /// <summary>
        /// The prompt text
        /// </summary>
        private string _promptText = string.Empty;

        /// <summary>
        /// The prompt font
        /// </summary>
        private Font _promptFont = new Font("微软雅黑", 15f, FontStyle.Regular, GraphicsUnit.Pixel);

        /// <summary>
        /// The prompt color
        /// </summary>
        private Color _promptColor = Color.Gray;

        /// <summary>
        /// My rectangle
        /// </summary>
        private Rectangle _myRectangle = Rectangle.FromLTRB(1, 3, 1000, 3);

        /// <summary>
        /// The input type
        /// </summary>
        private TextInputType _inputType = TextInputType.NotControl;

        /// <summary>
        /// The regex pattern
        /// </summary>
        private string _regexPattern = "";

        /// <summary>
        /// The m string old value
        /// </summary>
        private string m_strOldValue = string.Empty;

        /// <summary>
        /// The maximum value
        /// </summary>
        private decimal _maxValue = 1000000m;

        /// <summary>
        /// The minimum value
        /// </summary>
        private decimal _minValue = -1000000m;

        /// <summary>
        /// The decimal length
        /// </summary>
        private int _decLength = 2;

        /// <summary>
        /// 水印文字
        /// </summary>
        /// <value>The prompt text.</value>
        [Description("水印文字"), Category("自定义")]
        public string PromptText
        {
            get
            {
                return _promptText;
            }
            set
            {
                _promptText = value;
                OnPaint(null);
            }
        }

        /// <summary>
        /// Gets or sets the prompt font.
        /// </summary>
        /// <value>The prompt font.</value>
        [Description("水印字体"), Category("自定义")]
        public Font PromptFont
        {
            get
            {
                return _promptFont;
            }
            set
            {
                _promptFont = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the prompt.
        /// </summary>
        /// <value>The color of the prompt.</value>
        [Description("水印颜色"), Category("自定义")]
        public Color PromptColor
        {
            get
            {
                return _promptColor;
            }
            set
            {
                _promptColor = value;
            }
        }

        /// <summary>
        /// Gets or sets my rectangle.
        /// </summary>
        /// <value>My rectangle.</value>
        public Rectangle MyRectangle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the old text.
        /// </summary>
        /// <value>The old text.</value>
        public string OldText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the input.
        /// </summary>
        /// <value>The type of the input.</value>
        [Description("获取或设置一个值，该值指示文本框中的文本输入类型。")]
        public TextInputType InputType
        {
            get
            {
                return _inputType;
            }
            set
            {
                _inputType = value;
                if (value != TextInputType.NotControl)
                {
                    TextChanged -= new EventHandler(TextBoxEx_TextChanged);
                    TextChanged += new EventHandler(TextBoxEx_TextChanged);
                }
                else
                {
                    TextChanged -= new EventHandler(TextBoxEx_TextChanged);
                }
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示当输入类型InputType=Regex时，使用的正则表达式。
        /// </summary>
        /// <value>The regex pattern.</value>
        [Description("获取或设置一个值，该值指示当输入类型InputType=Regex时，使用的正则表达式。")]
        public string RegexPattern
        {
            get
            {
                return _regexPattern;
            }
            set
            {
                _regexPattern = value;
            }
        }
        /// <summary>
        /// 当InputType为数字类型时，能输入的最大值
        /// </summary>
        /// <value>The maximum value.</value>
        [Description("当InputType为数字类型时，能输入的最大值。")]
        public decimal MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }
        /// <summary>
        /// 当InputType为数字类型时，能输入的最小值
        /// </summary>
        /// <value>The minimum value.</value>
        [Description("当InputType为数字类型时，能输入的最小值。")]
        public decimal MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }
        /// <summary>
        /// 当InputType为数字类型时，能输入的最小值
        /// </summary>
        /// <value>The length of the decimal.</value>
        [Description("当InputType为数字类型时，小数位数。")]
        public int DecLength
        {
            get
            {
                return _decLength;
            }
            set
            {
                _decLength = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxEx" /> class.
        /// </summary>
        public TextBoxEx()
        {
            InitializeComponent();
            base.GotFocus += new EventHandler(TextBoxEx_GotFocus);
            base.MouseUp += new MouseEventHandler(TextBoxEx_MouseUp);
            base.KeyPress += TextBoxEx_KeyPress;
        }

        /// <summary>
        /// Handles the KeyPress event of the TextBoxEx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs" /> instance containing the event data.</param>
        void TextBoxEx_KeyPress(object sender, KeyPressEventArgs e)
        {
            //以下代码  取消按下回车或esc的“叮”声
            if (e.KeyChar == System.Convert.ToChar(13) || e.KeyChar == System.Convert.ToChar(27))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the TextBoxEx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void TextBoxEx_MouseUp(object sender, MouseEventArgs e)
        {
            if (blnFocus)
            {
                base.SelectAll();
                blnFocus = false;
            }
        }

        /// <summary>
        /// Handles the GotFocus event of the TextBoxEx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void TextBoxEx_GotFocus(object sender, EventArgs e)
        {
            blnFocus = true;
            base.SelectAll();
        }

        /// <summary>
        /// Handles the TextChanged event of the TextBoxEx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void TextBoxEx_TextChanged(object sender, EventArgs e)
        {
            if (Text == "")
            {
                m_strOldValue = Text;
            }
            else if (m_strOldValue != Text)
            {
                if (!ControlHelper.CheckInputType(Text, _inputType, _maxValue, _minValue, _decLength, _regexPattern))
                {
                    int num = base.SelectionStart;
                    if (m_strOldValue.Length < Text.Length)
                    {
                        num--;
                    }
                    else
                    {
                        num++;
                    }
                    base.TextChanged -= new EventHandler(TextBoxEx_TextChanged);
                    Text = m_strOldValue;
                    base.TextChanged += new EventHandler(TextBoxEx_TextChanged);
                    if (num < 0)
                    {
                        num = 0;
                    }
                    base.SelectionStart = num;
                }
                else
                {
                    m_strOldValue = Text;
                }
            }
        }

        /// <summary>
        /// 引发 <see cref="E:System.Windows.Forms.Control.Paint" /> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Forms.PaintEventArgs" />。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(_promptText))
            {
                if (e == null)
                {
                    using (Graphics graphics = Graphics.FromHwnd(base.Handle))
                    {
                        if (Text.Length == 0 && !string.IsNullOrEmpty(PromptText))
                        {
                            TextFormatFlags textFormatFlags = TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter;
                            if (RightToLeft == RightToLeft.Yes)
                            {
                                textFormatFlags |= (TextFormatFlags.Right | TextFormatFlags.RightToLeft);
                            }
                            TextRenderer.DrawText(graphics, PromptText, _promptFont, base.ClientRectangle, _promptColor, textFormatFlags);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 处理 Windows 消息。
        /// </summary>
        /// <param name="m">一个 Windows 消息对象。</param>
        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    if (m.Msg == 15 || m.Msg == 7 || m.Msg == 8)
        //    {
        //        //this.OnPaint(null);
        //        Invalidate();
        //    }
        //}

        /// <summary>
        /// Handles the <see cref="E:TextChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            base.Invalidate();
        }
    }
}
