/// <summary>
///   Author(s):       Joe Zachary
///                    Jim de St. Germain - Refactor/Documentation for ASP Core
///   Course:          CS 3500
///   Date:            2011 Sept   - Initial Grid Code
///                    2020 Spring - Refactor for ASP Core
///
///   
///   Partial:
/// 
///     The code for this class is divided into two files using C#'s partial ability.
///     The other half is in the SpreadsheetGridWidget.Designer.cs file.  The Designer
///     code is generally created automatically by the WYSIWYG editor.
/// 
///   Contents:
///   
///   This class represents a GRID that looks like (behaves as) a spreadsheet interface.
///   
///   This could have been done by creating a giant 2D array of textbox widgets, but the
///   overhead and inefficiency is too great.  Therefore, a "drawing panel" which simply
///   draws lines and text such that it looks like a bunch of textboxes is used.
///   
///   This code determines the location (based on scrolling) for the upper left cell
///   and then draws a "picture" for the displayable "image" of the spreadsheet.
/// 
/// </summary>

using System;
using System.Drawing;
using System.Windows.Forms;


namespace SpreadsheetGrid_Framework
{
    /// <summary>
    /// The type of delegate used to register for SelectionChanged events
    /// </summary>
    /// <param name="sender"></param>
    public delegate void SelectionChangedHandler(SpreadsheetGridWidget sender);

    /// <summary>
    /// A panel that displays a spreadsheet with 26 columns (labeled A-Z) and 99 rows
    /// (labeled 1-99).  Each cell on the grid can display a non-editable string.  One 
    /// of the cells is always selected (and highlighted).  When the selection changes, a 
    /// SelectionChanged event is fired.  Clients can register to be notified of
    /// such events.
    /// 
    /// None of the cells are editable.  They are for display purposes only.
    /// </summary>
    public partial class SpreadsheetGridWidget : UserControl
    {
        /// <summary>
        /// The event used to send notifications of a selection change
        /// </summary>
        public event SelectionChangedHandler SelectionChanged;

        // These constants control the layout of the spreadsheet grid.  The height and
        // width measurements are in pixels.
        internal const int DATA_COL_WIDTH = 80;
        internal const int DATA_ROW_HEIGHT = 20;
        internal const int LABEL_COL_WIDTH = 30;
        internal const int LABEL_ROW_HEIGHT = 30;
        internal const int PADDING = 2;
        internal const int SCROLLBAR_WIDTH = 20;
        internal const int COL_COUNT = 26;
        internal const int ROW_COUNT = 99;


        /// <summary>
        /// Creates the Spreadsheet Grid Widget.  In this case, an empty one
        /// with no values.  Code similar to this can be created via the GUI WYSIWYG designer
        /// but in this case, the code was manually constructed.
        /// 
        /// The InitializeComponent calls the auto-generated code
        /// </summary>

        public SpreadsheetGridWidget()
        {
            InitializeComponent();

            // after the base initialization, we have to do some tweaks
            // that are not available from the WYSIWYG designer

            this.drawingPanel.SetGrid(this); // associate the grid panel with "this" 
            this.hScroll.Scroll += drawingPanel.HandleHScroll;
            this.vScroll.Scroll += drawingPanel.HandleVScroll;

            this.hScroll.Maximum = COL_COUNT;
            this.hScroll.SmallChange = 1;
            this.vScroll.Maximum = ROW_COUNT + 1;
            this.vScroll.SmallChange = 1;

        }


        /// <summary>
        /// Clears the display.
        /// </summary>

        public void Clear()
        {
            drawingPanel.Clear();
        }


        /// <summary>
        /// If the zero-based column and row are in range, sets the value of that
        /// cell and returns true.  Otherwise, returns false.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        public bool SetValue(int col, int row, string value)
        {
            return drawingPanel.SetValue(col, row, value);
        }


        /// <summary>
        /// If the zero-based column and row are in range, assigns the value
        /// of that cell to the out parameter and returns true.  Otherwise,
        /// returns false.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        public bool GetValue(int col, int row, out string value)
        {
            return drawingPanel.GetValue(col, row, out value);
        }


        /// <summary>
        /// If the zero-based column and row are in range, uses them to set
        /// the current selection and returns true.  Otherwise, returns false.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="changed"> whether to call the event handler </param>
        /// <returns></returns>

        public bool SetSelection(int col, int row, bool changed = true)
        {
            var temp = drawingPanel.SetSelection(col, row);

            if (changed)
            {
                SelectionChanged(this);
            }
            return temp;
        }


        /// <summary>
        /// Assigns the column and row of the current selection to the
        /// out parameters.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>

        public void GetSelection(out int col, out int row)
        {
            drawingPanel.GetSelection(out col, out row);
        }


        /// <summary>
        /// When the SpreadsheetPanel is resized, we set the size and locations of the three
        /// components that make it up.
        /// </summary>
        /// <param name="eventargs"></param>

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            try
            {
                if (FindForm() == null || FindForm().WindowState != FormWindowState.Minimized)
                {
                    if (Height < 1)
                    {
                        drawingPanel.Size = new Size(100, 100);
                        return;
                    }
                    drawingPanel.Size = new Size(Width - SCROLLBAR_WIDTH, Height - SCROLLBAR_WIDTH);
                    vScroll.Location = new Point(Width - SCROLLBAR_WIDTH, 0);
                    vScroll.Size = new Size(SCROLLBAR_WIDTH, Height - SCROLLBAR_WIDTH);
                    vScroll.LargeChange = (Height - SCROLLBAR_WIDTH) / DATA_ROW_HEIGHT;
                    hScroll.Location = new Point(0, Height - SCROLLBAR_WIDTH);
                    hScroll.Size = new Size(Width - SCROLLBAR_WIDTH, SCROLLBAR_WIDTH);
                    hScroll.LargeChange = (Width - SCROLLBAR_WIDTH) / DATA_COL_WIDTH;
                }
            }
            catch (Exception)
            {
                ; // no action taken 
            }
        }



        /// <summary>
        /// Used internally to keep track of cell addresses
        /// </summary>
        internal class Address
        {

            public int Col { get; set; }
            public int Row { get; set; }

            public Address(int c, int r)
            {
                Col = c;
                Row = r;
            }

            public override int GetHashCode()
            {
                return Col.GetHashCode() ^ Row.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if ((obj == null) || !(obj is Address))
                {
                    return false;
                }
                Address a = (Address)obj;
                return Col == a.Col && Row == a.Row;
            }
        }

       
    }

  
}