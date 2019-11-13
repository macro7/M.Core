using System;
using System.Drawing;

namespace M.Core.Controls
{
    /// <summary>
    /// Class DataGridViewColumnEntity.
    /// </summary>
    public class DataGridViewColumnEntity
    {
        /// <summary>
        /// Gets or sets the head text.
        /// </summary>
        /// <value>The head text.</value>
        public string HeadText { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the type of the width.
        /// </summary>
        /// <value>The type of the width.</value>
        public System.Windows.Forms.SizeType WidthType { get; set; }
        /// <summary>
        /// Gets or sets the data field.
        /// </summary>
        /// <value>The data field.</value>
        public string DataField { get; set; }
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public Func<object, string> Format { get; set; }
        /// <summary>
        /// Gets or sets the text align.
        /// </summary>
        /// <value>The text align.</value>
        public ContentAlignment TextAlign { get; set; } = ContentAlignment.MiddleCenter;
    }
}
