﻿using SpreadsheetGrid_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;

namespace SpreadsheetGrid_Core
{
    public partial class Form1 : Form
    {
        Spreadsheet spreadsheet = new Spreadsheet();
        public Form1()
        {
            this.grid_widget = new SpreadsheetGridWidget();

            // Call the AutoGenerated code
            InitializeComponent();

            // Add event handler and select a start cell
            grid_widget.SelectionChanged += grid_widget_Click;
            // initial cell set to A1
            grid_widget.SetSelection(0, 0, false);
            // set initial value of SelectedCellTextBox to display "A1"
            SelectedCellTextBox.Text = "A1";

        }

        /// <summary>
        /// Given a spreadsheet, find the current selected cell and
        /// create a popup that contains the information from that cell
        /// </summary>
        /// <param name="ss"></param>
        private void DisplaySelection(SpreadsheetGridWidget ss)
        {
            int row, col;

            string value;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);
            // if cell is "" (empty) puts the current date and time when clicked on
            if (value == "")
            {
                ss.SetValue(col, row, DateTime.Now.ToLocalTime().ToString("T"));
                ss.GetValue(col, row, out value);
                MessageBox.Show("Selection: column " + col + " row " + row + " value " + value);
            }
        }

        // Deals with the New menu
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            Spreadsheet_Window.getAppContext().RunForm(new Form1());
        }

        // Deals with the Close menu
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Deals with Save menu
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Deals with Open menu
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void CellContentsTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter )
            {
                if (sender is TextBox)
                {
                    UpdateContentsTextBox();
                    UpdateValueTextBoxOnKeyDown();
                }
            }
        }

        // helper methods for CellContentsTextBox_KeyDown
        /// <summary>
        /// Updates CellValueTextBox, if a string is entered the text box is updated with a string, if a double is entered the text box is updated with a double,
        /// if a formula is is entered the formula is evaluated then the text box is updated with the result.
        /// </summary>
        private void UpdateValueTextBoxOnKeyDown()
        {
            // get cell name
            string cellLocation = GetCellName();
            // calculate value of cell and set it to CellValueTextBox
            CellValueTextBox.Text = spreadsheet.GetCellValue(cellLocation).ToString();
        }
        /// <summary>
        /// Updates CellContentsTextBox with entered text
        /// </summary>
        private void UpdateContentsTextBox()
        {
            // get cell name
            string cellLocation = GetCellName();
            // get row and col location
            grid_widget.GetSelection(out int col, out int row);
            // setContentsOfCell in our spreadsheet
            spreadsheet.SetContentsOfCell(cellLocation, CellContentsTextBox.Text);
            // set cell with same contents as CellContentsTextBox
            grid_widget.SetValue(col, row, CellContentsTextBox.Text);
        }

        private void SelectedCellLabel_Click_1(object sender, EventArgs e)
        {
        }

        private void CellValueLabel_Click(object sender, EventArgs e)
        {

        }

        private void CellContentsLabel_Click(object sender, EventArgs e)
        {

        }
        private void grid_widget_Click(SpreadsheetGridWidget ss)
        {
            // TODO need to update name, value, and contents textbox
            UpdateSelectedCellTextBox();
            UpdateContentsOnClick();
            UpdateValuesOnClick();
            

        }
        // Helper methods for grid_widget_Click
        /// <summary>
        /// Updates the SelectedCellTextBox with the appropriate cell name
        /// </summary>
        private void UpdateSelectedCellTextBox()
        {
            SelectedCellTextBox.Text = GetCellName();
        }
        
        /// <summary>
        /// Updates CellContentsTextBox with the contents when the cell is clicked on
        /// </summary>
        private void UpdateContentsOnClick()
        {
            grid_widget.GetSelection(out int col, out int row);
            grid_widget.GetValue(col, row, out string value);
            CellContentsTextBox.Text = value;
        }

        /// <summary>
        /// Gets the cell name by using the column and rows
        /// </summary>
        /// <returns></returns>
        private string GetCellName()
        {
            // get column and row location
            grid_widget.GetSelection(out int col, out int row);
            // use ascii value to convert column location to a letter
            char letter = (char)('A' + col);
            // set column location as a string instead of a char
            string colLocation = "" + letter;
            // append colLocation and and 1 to row and set to cellLocation
            row++;
            string cellLocation = colLocation + row.ToString();
            return cellLocation;
        }

        /// <summary>
        /// Updates CellValueTextBox with the contents when the cell is clicked on
        /// </summary>
        private void UpdateValuesOnClick()
        {
            // get cell name
            string cellLocation = GetCellName();
            // set ValueTextBox text with value
            CellValueTextBox.Text = spreadsheet.GetCellValue(cellLocation).ToString();
        }
    }
}
