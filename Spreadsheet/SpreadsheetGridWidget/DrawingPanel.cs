using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpreadsheetGrid_Framework
{
    /// <summary>
    /// The panel where the spreadsheet grid is drawn.  It keeps track of the
    /// current selection as well as what is supposed to be drawn in each cell.
    /// </summary>
    public partial class DrawingPanel : UserControl
    {

        // Columns and rows are numbered beginning with 0.  This is the coordinate
        // of the selected cell.
        private int _selectedCol;
        private int _selectedRow;

        // Coordinate of cell in upper-left corner of display
        private int _firstColumn = 0;
        private int _firstRow = 0;

        // The strings contained by the spreadsheet
        private Dictionary<SpreadsheetGridWidget.Address, String> _values;

        // The containing panel
        private SpreadsheetGridWidget _ssp;

        public DrawingPanel() : this(null) { }

        public DrawingPanel(SpreadsheetGridWidget ss = null)
        {
            InitializeComponent();

            DoubleBuffered = true;
            _values = new Dictionary<SpreadsheetGridWidget.Address, String>();
            _ssp = ss;
        }

        public void SetGrid(SpreadsheetGridWidget ss)
        {
            _ssp = ss;
        }

        private bool InvalidAddress(int col, int row)
        {
            return col < 0 || row < 0 || col >= SpreadsheetGridWidget.COL_COUNT || row >= SpreadsheetGridWidget.ROW_COUNT;
        }

        public void Clear()
        {
            _values.Clear();
            Invalidate();
        }

        public bool SetValue(int col, int row, string c)
        {
            if (InvalidAddress(col, row))
            {
                return false;
            }

            SpreadsheetGridWidget.Address a = new SpreadsheetGridWidget.Address(col, row);
            if (c == null || c == "")
            {
                _values.Remove(a);
            }
            else
            {
                _values[a] = c;
            }
            Invalidate();
            return true;
        }

        public bool GetValue(int col, int row, out string c)
        {
            if (InvalidAddress(col, row))
            {
                c = null;
                return false;
            }
            if (!_values.TryGetValue(new SpreadsheetGridWidget.Address(col, row), out c))
            {
                c = "";
            }
            return true;
        }

        public bool SetSelection(int col, int row)
        {
            if (InvalidAddress(col, row))
            {
                return false;
            }
            _selectedCol = col;
            _selectedRow = row;
            Invalidate();
            return true;
        }

        public void GetSelection(out int col, out int row)
        {
            col = _selectedCol;
            row = _selectedRow;
        }

        public void HandleHScroll(Object sender, ScrollEventArgs args)
        {
            Console.WriteLine(args.NewValue);
            _firstColumn = args.NewValue;
            Invalidate();
        }

        public void HandleVScroll(Object sender, ScrollEventArgs args)
        {
            _firstRow = args.NewValue;
            Invalidate();
        }

        /// <summary>
        /// This is the "heart" of the program. It generates the look and feel of multiple textboxes, but
        /// does so by just drawing lines and text on the screen!
        /// </summary>
        /// <param name="e"> The "Graphics" object used to paint the contents on the screen </param>
        protected override void OnPaint(PaintEventArgs e)
        {

            // Clip based on what needs to be refreshed.
            Region clip = new Region(e.ClipRectangle);
            e.Graphics.Clip = clip;

            // Color the background of the data area white
            e.Graphics.FillRectangle(
                new SolidBrush(Color.White),
                SpreadsheetGridWidget.LABEL_COL_WIDTH,
                SpreadsheetGridWidget.LABEL_ROW_HEIGHT,
                (SpreadsheetGridWidget.COL_COUNT - _firstColumn) * SpreadsheetGridWidget.DATA_COL_WIDTH,
                (SpreadsheetGridWidget.ROW_COUNT - _firstRow) * SpreadsheetGridWidget.DATA_ROW_HEIGHT);

            // Pen, brush, and fonts to use
            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush);
            Font regularFont = Font;
            Font boldFont = new Font(regularFont, FontStyle.Bold);

            // Draw the column lines
            int bottom = SpreadsheetGridWidget.LABEL_ROW_HEIGHT + (SpreadsheetGridWidget.ROW_COUNT - _firstRow) * SpreadsheetGridWidget.DATA_ROW_HEIGHT;
            e.Graphics.DrawLine(pen, new Point(0, 0), new Point(0, bottom));
            for (int x = 0; x <= (SpreadsheetGridWidget.COL_COUNT - _firstColumn); x++)
            {
                e.Graphics.DrawLine(
                    pen,
                    new Point(SpreadsheetGridWidget.LABEL_COL_WIDTH + x * SpreadsheetGridWidget.DATA_COL_WIDTH, 0),
                    new Point(SpreadsheetGridWidget.LABEL_COL_WIDTH + x * SpreadsheetGridWidget.DATA_COL_WIDTH, bottom));
            }

            // Draw the column labels
            for (int x = 0; x < SpreadsheetGridWidget.COL_COUNT - _firstColumn; x++)
            {
                Font f = (_selectedCol - _firstColumn == x) ? boldFont : Font;
                DrawColumnLabel(e.Graphics, x, f);
            }

            // Draw the row lines
            int right = SpreadsheetGridWidget.LABEL_COL_WIDTH + (SpreadsheetGridWidget.COL_COUNT - _firstColumn) * SpreadsheetGridWidget.DATA_COL_WIDTH;
            e.Graphics.DrawLine(pen, new Point(0, 0), new Point(right, 0));
            for (int y = 0; y <= SpreadsheetGridWidget.ROW_COUNT - _firstRow; y++)
            {
                e.Graphics.DrawLine(
                    pen,
                    new Point(0, SpreadsheetGridWidget.LABEL_ROW_HEIGHT + y * SpreadsheetGridWidget.DATA_ROW_HEIGHT),
                    new Point(right, SpreadsheetGridWidget.LABEL_ROW_HEIGHT + y * SpreadsheetGridWidget.DATA_ROW_HEIGHT));
            }

            // Draw the row labels
            for (int y = 0; y < (SpreadsheetGridWidget.ROW_COUNT - _firstRow); y++)
            {
                Font f = (_selectedRow - _firstRow == y) ? boldFont : Font;
                DrawRowLabel(e.Graphics, y, f);
            }

            // Highlight the selection, if it is visible
            if ((_selectedCol - _firstColumn >= 0) && (_selectedRow - _firstRow >= 0))
            {
                e.Graphics.DrawRectangle(
                    pen,
                    new Rectangle(SpreadsheetGridWidget.LABEL_COL_WIDTH + (_selectedCol - _firstColumn) * SpreadsheetGridWidget.DATA_COL_WIDTH + 1,
                                  SpreadsheetGridWidget.LABEL_ROW_HEIGHT + (_selectedRow - _firstRow) * SpreadsheetGridWidget.DATA_ROW_HEIGHT + 1,
                                  SpreadsheetGridWidget.DATA_COL_WIDTH - 2,
                                  SpreadsheetGridWidget.DATA_ROW_HEIGHT - 2));
            }

            // Draw the text
            foreach (KeyValuePair<SpreadsheetGridWidget.Address, String> address in _values)
            {
                String text = address.Value;
                int x = address.Key.Col - _firstColumn;
                int y = address.Key.Row - _firstRow;
                float height = e.Graphics.MeasureString(text, regularFont).Height;
                float width = e.Graphics.MeasureString(text, regularFont).Width;
                if (x >= 0 && y >= 0)
                {
                    Region cellClip = new Region(new Rectangle(SpreadsheetGridWidget.LABEL_COL_WIDTH + x * SpreadsheetGridWidget.DATA_COL_WIDTH + SpreadsheetGridWidget.PADDING,
                                                               SpreadsheetGridWidget.LABEL_ROW_HEIGHT + y * SpreadsheetGridWidget.DATA_ROW_HEIGHT,
                                                               SpreadsheetGridWidget.DATA_COL_WIDTH - 2 * SpreadsheetGridWidget.PADDING,
                                                               SpreadsheetGridWidget.DATA_ROW_HEIGHT));
                    cellClip.Intersect(clip);
                    e.Graphics.Clip = cellClip;
                    e.Graphics.DrawString(
                        text,
                        regularFont,
                        brush,
                        SpreadsheetGridWidget.LABEL_COL_WIDTH + x * SpreadsheetGridWidget.DATA_COL_WIDTH + SpreadsheetGridWidget.PADDING,
                        SpreadsheetGridWidget.LABEL_ROW_HEIGHT + y * SpreadsheetGridWidget.DATA_ROW_HEIGHT + (SpreadsheetGridWidget.DATA_ROW_HEIGHT - height) / 2);
                }
            }
        }

        /// <summary>
        /// Draws a column label.  The columns are indexed beginning with zero.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="f"></param>
        private void DrawColumnLabel(Graphics g, int x, Font f)
        {
            String label = ((char)('A' + x + _firstColumn)).ToString();
            float height = g.MeasureString(label, f).Height;
            float width = g.MeasureString(label, f).Width;
            g.DrawString(
                  label,
                  f,
                  new SolidBrush(Color.Black),
                  SpreadsheetGridWidget.LABEL_COL_WIDTH + x * SpreadsheetGridWidget.DATA_COL_WIDTH + (SpreadsheetGridWidget.DATA_COL_WIDTH - width) / 2,
                  (SpreadsheetGridWidget.LABEL_ROW_HEIGHT - height) / 2);
        }

        /// <summary>
        /// Draws a row label.  The rows are indexed beginning with zero.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="y"></param>
        /// <param name="f"></param>
        private void DrawRowLabel(Graphics g, int y, Font f)
        {
            String label = (y + 1 + _firstRow).ToString();
            float height = g.MeasureString(label, f).Height;
            float width = g.MeasureString(label, f).Width;
            g.DrawString(
                label,
                f,
                new SolidBrush(Color.Black),
                SpreadsheetGridWidget.LABEL_COL_WIDTH - width - SpreadsheetGridWidget.PADDING,
                SpreadsheetGridWidget.LABEL_ROW_HEIGHT + y * SpreadsheetGridWidget.DATA_ROW_HEIGHT + (SpreadsheetGridWidget.DATA_ROW_HEIGHT - height) / 2);
        }

        /// <summary>
        /// Determines which cell, if any, was clicked.  Generates a SelectionChanged event.  All of
        /// the indexes are zero based.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnClick(e);
            int x = (e.X - SpreadsheetGridWidget.LABEL_COL_WIDTH) / SpreadsheetGridWidget.DATA_COL_WIDTH;
            int y = (e.Y - SpreadsheetGridWidget.LABEL_ROW_HEIGHT) / SpreadsheetGridWidget.DATA_ROW_HEIGHT;
            if (e.X > SpreadsheetGridWidget.LABEL_COL_WIDTH && e.Y > SpreadsheetGridWidget.LABEL_ROW_HEIGHT && (x + _firstColumn < SpreadsheetGridWidget.COL_COUNT) && (y + _firstRow < SpreadsheetGridWidget.ROW_COUNT))
            {
                _selectedCol = x + _firstColumn;
                _selectedRow = y + _firstRow;
                if (_ssp != null)
                {
                    _ssp.SetSelection(_selectedCol, _selectedRow);
                }
                //if (_ssp.SelectionChanged != null)
                //{
                //    _ssp.SelectionChanged(_ssp);
                //} 
            }
            Invalidate();
        }

        private void DrawingPanel_Load(object sender, EventArgs e)
        {

        }
    }

}
