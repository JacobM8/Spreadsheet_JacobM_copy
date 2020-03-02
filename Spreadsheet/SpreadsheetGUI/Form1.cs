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
using System.Text.RegularExpressions;
using System.IO;
using System.Security;

namespace SpreadsheetGrid_Core
{
    public partial class Form1 : Form
    {
        Spreadsheet spreadsheet = new Spreadsheet(s => true, s => s.ToUpper(), "six");
        bool backgroundChanged = false;
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
        /// Creates a new empty spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same thread as the other forms.
            Spreadsheet_Window.getAppContext().RunForm(new Form1());
        }

        /// <summary>
        /// If the user selects the close on the drop down form button and the file is not saved, 
        /// the method is called and the user is asked whether they would like to save the file or not
        /// save the file. If the user selects yes, they are directed to the file explorer
        /// to save the file. If the user selects no, the program terminates and closes the 
        /// window. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // check if the spreadsheet has been saved
            if (spreadsheet.Changed == true)
            {
                // on the action of closing the window prompt the user to save
                switch (MessageBox.Show(this, "Do you want save your project?", "Save before Closing", MessageBoxButtons.YesNo))
                {
                    case DialogResult.No:
                        Close();
                        break;
                    case DialogResult.Yes:
                        saveContent(sender,e);
                        break;
                }
            }
            else
            {
                Close();
            }
        }
        private void saveContent(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "sprd files (*.sprd)|*.sprd| All files(*.*)|*.*";
            saveFile.FilterIndex = 1;
            saveFile.RestoreDirectory = true;
            // prompt user to save file they created
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                spreadsheet.Save(saveFile.FileName);
            }
        }
        /// <summary>
        /// If the user selects the "X" button and the file is not saved, the method is 
        /// called and the user is asked whether they would like to save the file or not
        /// save the file. If the user selects yes, they are directed to the file explorer
        /// to save the file. If the user selects no, the program terminates and closes the 
        /// window. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // check if the spreadsheet has been saved
            if (spreadsheet.Changed == true)
            {
                // on the action of closing the window prompt the user to save
                base.OnFormClosing(e);
                switch (MessageBox.Show(this, "Do you want save your project?", "Save before Closing", MessageBoxButtons.YesNo))
                {
                    case DialogResult.No:
                        _ = e.CloseReason;
                        break;
                    case DialogResult.Yes:
                        SaveFileDialog saveFile = new SaveFileDialog();
                        saveFile.Filter = "sprd files (*.sprd)|*.sprd| All files(*.*)|*.*";
                        saveFile.FilterIndex = 1;
                        saveFile.RestoreDirectory = true;
                        // prompt user to save file they created
                        if (saveFile.ShowDialog() == DialogResult.OK)
                        {
                            spreadsheet.Save(saveFile.FileName);
                        }
                        e.Cancel = true;
                        break;
                }
            }
        }
        /// <summary>
        /// Once the user selects save in the drop down menu. File Explorer is
        /// then opened and the user can select what type of files they would like to see
        /// then enter a name for the files and the file is store in their directory of choice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveContent(sender, e);
        }
        /// <summary>
        /// Opens a previously saved file in file explorer. In file explorer it
        /// allows the users to determine what type of files they want to see. 
        /// The user can see either all types of files or only .sprd. After the user
        /// selects the file the cells are populated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create a file that is .sprd and allow user to choose files displayed
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "sprd files (*.sprd)|*.sprd| All files(*.*)|*.*";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filepath = openFile.FileName;
                    //open file and loop through all the cells that need to be populated
                    using (Stream sr = openFile.OpenFile())
                    {
                        grid_widget.Clear();
                        spreadsheet = new Spreadsheet(filepath, s => true, s => s.ToUpper(), "six");
                        foreach (string name in spreadsheet.GetNamesOfAllNonemptyCells())
                        {
                            // turn into row column from name
                            GetCellLocation(name, out int row, out int col);
                            // get cellvalue and put into the grid 
                            grid_widget.SetValue(col, row, spreadsheet.GetCellValue(name).ToString());
                        }
                    }
                }
                // throw a security error if the user selects an invalid file
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        /// <summary>
        /// returns the column and row of the given cell. Name must be A-Z(upper or lower case) and 1-99
        /// </summary>
        /// <param name="name"> cell location (ex. A1) </param>
        /// <param name="col"> column number </param>
        /// <param name="row"> row number </param>
        private void GetCellLocation(string name, out int col, out int row)
        {
            // convert name to char array
            char[] rowInfo = name.ToCharArray();
            // onlyRow will get the row letter
            char onlyRow = rowInfo[0];
            // set column to the numbers remaining in rowInfo
            string column = "";
            for (int i = 1; i < rowInfo.Length; i++)
            {
                column += rowInfo[i];
            }
            // convert to ints
            int.TryParse(column, out col);
            // ascii value used get row number
            row = char.ToUpper(rowInfo[0]) - 64;
            // decrement once because grid doesn't start at A)
            col--;
            row--;
        }
        /// <summary>
        /// Tells the user how to navigate the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "- Select the desired cell by clicking on it to view the contents and cell name\n" +
                "\n- To enter a new value type inside of the cell contents box and press enter to save \n" +
                "\n **Warning your text will not save unless you press enter**\n" +
                "\n- To calculate a formula begin with an \"=\" equals sign then enter the rest of the formula\n" +
                "\n- If cells are arranged in a circular dependency you will be notified of the error\n" +
                "\n- If you divide by zero, the value will appear as a formula error\n" +
                "\n- All other formatting errors will prompt an error message, describing what error occurred\n" +
                "\n- The Font Color dropdown menu allows the user to choose the color for the text inside the textboxes.\n" +
                "\n- Dark Mode allows the user switch to a dark themed window."
                , "Help Menu",
                    MessageBoxButtons.OK);
        }
        /// <summary>
        ///     If the Enter/Return key is hit updates the CellContentsTextBox and CellValueTextBox
        /// <exception cref="CircularException"> 
        ///   If formula is created with a circular dependency, throw a CircularException
        /// </exception>
        /// <exception cref="FormatException"> 
        ///   If invalid formula is entered, throw a FormatException.
        /// </exception>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> </param>
        private void CellContentsTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // if KeyEventArgs is the Enter/Return key update CellContentsTextBox and CellValueTextBox
                if (e.KeyCode == Keys.Enter)
                {
                    
                    if (sender is TextBox)
                    {
                        UpdateContentsTextBoxOnKeyDown();
                        UpdateValueTextBoxOnKeyDown();
                    }
                }
            }
            // show a message if an exception is thrown
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        private void UpdateContentsTextBoxOnKeyDown()
        {
            // get cell name
            string cellLocation = GetCellName();
            // get row and col location
            grid_widget.GetSelection(out int col, out int row);
            // setContentsOfCell in our spreadsheet
            spreadsheet.SetContentsOfCell(cellLocation, CellContentsTextBox.Text);
            // set cell with cell value
            grid_widget.SetValue(col, row, spreadsheet.GetCellValue(cellLocation).ToString());
        }

        /// <summary>
        /// When the grid_widget is clicked on update the ReadOnly SelectedCellTextBox and CellValueTextBox, and editable CellContentTextBox
        /// </summary>
        /// <param name="ss"></param>
        private void grid_widget_Click(SpreadsheetGridWidget ss)
        {
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
            CellContentsTextBox.Text = spreadsheet.GetCellContents(GetCellName()).ToString();
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
            // append colLocation and add 1 to row and set to cellLocation
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
        private void DarkMode_Enter(object sender, EventArgs e)
        {
            if (backgroundChanged == false)
            {
                MainControlArea.BackColor = System.Drawing.Color.Black;
                tableLayoutForSelectedCellValueContents.BackColor = System.Drawing.Color.Black;
                tableLayoutPanel1.BackColor = System.Drawing.Color.Black;
                MainControlArea.ForeColor = System.Drawing.Color.Green;
                menuStrip.BackColor = System.Drawing.Color.Black;
                grid_widget.BackColor = System.Drawing.Color.Gray;
                grid_widget.ForeColor = System.Drawing.Color.Green;
                HelpMenu.ForeColor = System.Drawing.Color.Green;
                HelpMenu.BackColor = System.Drawing.Color.Black;
                Dark.ForeColor = System.Drawing.Color.Green;
                Dark.BackColor = System.Drawing.Color.Black;
                fileToolStripMenuItem.ForeColor = System.Drawing.Color.Green;
                fileToolStripMenuItem.BackColor = System.Drawing.Color.Black;
                CellValueLabel.ForeColor = System.Drawing.Color.Green;
                CellContentsLabel.ForeColor = System.Drawing.Color.Green; 
                SelectedCellLabel.ForeColor = System.Drawing.Color.Green;
                grid_widget.ForeColor = SystemColors.ActiveCaption;
                backgroundChanged = true;
            }
            else
            {
                MainControlArea.BackColor = System.Drawing.Color.DarkSeaGreen;
                grid_widget.BackColor = System.Drawing.SystemColors.MenuHighlight;
                MainMenuStrip.BackColor = System.Drawing.Color.WhiteSmoke;
                HelpMenu.ForeColor = System.Drawing.Color.Black;
                CellValueLabel.ForeColor = System.Drawing.Color.Black;
                CellContentsLabel.ForeColor = System.Drawing.Color.Black;
                SelectedCellLabel.ForeColor = System.Drawing.Color.Black;
                grid_widget.ForeColor = System.Drawing.Color.Black;
                Dark.ForeColor = System.Drawing.Color.Black;
                HelpMenu.ForeColor = System.Drawing.Color.Black;
                HelpMenu.BackColor = System.Drawing.Color.WhiteSmoke;
                Dark.ForeColor = System.Drawing.Color.Black;
                Dark.BackColor = System.Drawing.Color.WhiteSmoke;
                fileToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
                fileToolStripMenuItem.BackColor = System.Drawing.Color.White;
                tableLayoutPanel1.BackColor = System.Drawing.Color.White;
                tableLayoutForSelectedCellValueContents.BackColor = System.Drawing.Color.DarkSeaGreen; ;
                backgroundChanged = false;
            }
        }
        /// <summary>
        /// Changes color of the SelectedCellTextBox, CellValueTextBox, and CellContentsTextBox to black.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CellContentsTextBox.ForeColor = Color.Black;
            CellValueTextBox.ForeColor = Color.Black;
            SelectedCellTextBox.ForeColor = Color.Black;
        }
        /// <summary>
        /// Changes color of the SelectedCellTextBox, CellValueTextBox, and CellContentsTextBox to red.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // need to set BackColor on read only text boxes before you can set the ForeColor
            CellContentsTextBox.ForeColor = Color.Red;
            CellValueTextBox.BackColor = Color.White;
            CellValueTextBox.ForeColor = Color.Red;
            SelectedCellTextBox.BackColor = Color.White;
            SelectedCellTextBox.ForeColor = Color.Red;
        }
        /// <summary>
        /// Changes color of the SelectedCellTextBox, CellValueTextBox, and CellContentsTextBox to blue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // need to set BackColor on read only text boxes before you can set the ForeColor
            CellContentsTextBox.ForeColor = Color.Blue;
            CellValueTextBox.BackColor = Color.White;
            CellValueTextBox.ForeColor = Color.Blue;
            SelectedCellTextBox.BackColor = Color.White;
            SelectedCellTextBox.ForeColor = Color.Blue;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }
    }
}