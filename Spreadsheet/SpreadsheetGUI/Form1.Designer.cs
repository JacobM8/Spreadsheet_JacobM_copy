using SpreadsheetGrid_Framework;
using System.ComponentModel;
using System.Windows.Forms;
/// <summary>
/// February 29, 2019
/// We, Jacob Morrison and James Gibb, certify that we wrote this code from scratch and did not copy it in part or whole from  
/// another source. Some of the code was provided to us but the Univeristy of Utah College Of Engineering
/// All references used in the completion of the assignment are cited in my README file. 
/// </summary>
namespace SpreadsheetGrid_Core
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Dark = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainControlArea = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutForSelectedCellValueContents = new System.Windows.Forms.TableLayoutPanel();
            this.CellValueTextBox = new System.Windows.Forms.TextBox();
            this.CellValueLabel = new System.Windows.Forms.Label();
            this.SelectedCellLabel = new System.Windows.Forms.Label();
            this.CellContentsTextBox = new System.Windows.Forms.TextBox();
            this.SelectedCellTextBox = new System.Windows.Forms.TextBox();
            this.CellContentsLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grid_widget = new SpreadsheetGrid_Framework.SpreadsheetGridWidget();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.MainControlArea.SuspendLayout();
            this.tableLayoutForSelectedCellValueContents.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            this.HelpMenu = new System.Windows.Forms.Button();
            this.fontColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker = new BackgroundWorker(); 
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();


            // 
            // menuStrip
            // 
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem, this.fontColorToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1684, 40);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(72, 38);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // MainControlArea
            // 
            this.MainControlArea.AutoSize = true;
            this.MainControlArea.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MainControlArea.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.MainControlArea.Controls.Add(this.tableLayoutForSelectedCellValueContents);
            this.MainControlArea.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MainControlArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainControlArea.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.MainControlArea.Location = new System.Drawing.Point(6, 6);
            this.MainControlArea.Margin = new System.Windows.Forms.Padding(6);
            this.MainControlArea.MinimumSize = new System.Drawing.Size(200, 192);
            this.MainControlArea.Name = "MainControlArea";
            this.MainControlArea.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MainControlArea.Size = new System.Drawing.Size(1672, 192);
            this.MainControlArea.TabIndex = 4;
            // 
            // tableLayoutForSelectedCellValueContents
            // 
            this.tableLayoutForSelectedCellValueContents.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutForSelectedCellValueContents.ColumnCount = 2;
            this.tableLayoutForSelectedCellValueContents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.85624F));
            this.tableLayoutForSelectedCellValueContents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.14376F));
            this.tableLayoutForSelectedCellValueContents.Controls.Add(this.CellValueTextBox, 1, 1);
            this.tableLayoutForSelectedCellValueContents.Controls.Add(this.CellValueLabel, 0, 1);
            this.tableLayoutForSelectedCellValueContents.Controls.Add(this.SelectedCellLabel, 0, 0);
            this.tableLayoutForSelectedCellValueContents.Controls.Add(this.CellContentsTextBox, 1, 2);
            this.tableLayoutForSelectedCellValueContents.Controls.Add(this.SelectedCellTextBox, 1, 0);
            this.tableLayoutForSelectedCellValueContents.Controls.Add(this.CellContentsLabel, 0, 2);
            this.tableLayoutForSelectedCellValueContents.Location = new System.Drawing.Point(3, 55);
            this.tableLayoutForSelectedCellValueContents.Name = "tableLayoutForSelectedCellValueContents";
            this.tableLayoutForSelectedCellValueContents.RowCount = 3;
            this.tableLayoutForSelectedCellValueContents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutForSelectedCellValueContents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutForSelectedCellValueContents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutForSelectedCellValueContents.Size = new System.Drawing.Size(946, 134);
            this.tableLayoutForSelectedCellValueContents.TabIndex = 0;
            // 
            // CellValueTextBox
            // 
            this.CellValueTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CellValueTextBox.Location = new System.Drawing.Point(157, 15);
            this.CellValueTextBox.Name = "CellValueTextBox";
            this.CellValueTextBox.Size = new System.Drawing.Size(250, 5);
            this.CellValueTextBox.TabIndex = 0;
            this.CellValueTextBox.ReadOnly = true;

            // 
            // CellValueLabel
            // 
            this.CellValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CellValueLabel.AutoSize = true;
            this.CellValueLabel.Location = new System.Drawing.Point(38, 55);
            this.CellValueLabel.Name = "CellValueLabel";
            this.CellValueLabel.Size = new System.Drawing.Size(110, 25);
            this.CellValueLabel.TabIndex = 1;
            this.CellValueLabel.Text = "Cell Value";
            // 
            // SelectedCellLabel
            // 
            this.SelectedCellLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SelectedCellLabel.AutoSize = true;
            this.SelectedCellLabel.Location = new System.Drawing.Point(9, 11);
            this.SelectedCellLabel.Name = "SelectedCellLabel";
            this.SelectedCellLabel.Size = new System.Drawing.Size(139, 25);
            this.SelectedCellLabel.TabIndex = 2;
            this.SelectedCellLabel.Text = "Selected Cell";
            // 
            // CellContentsTextBox
            // 
            this.CellContentsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CellContentsTextBox.Location = new System.Drawing.Point(157, 95);
            this.CellContentsTextBox.Name = "CellContentsTextBox";
            this.CellContentsTextBox.Size = new System.Drawing.Size(750, 31);
            this.CellContentsTextBox.TabIndex = 4;
            this.CellContentsTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CellContentsTextBox_KeyDown);
            // 
            // SelectedCellTextBox
            // 
            this.SelectedCellTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SelectedCellTextBox.Location = new System.Drawing.Point(157, 8);
            this.SelectedCellTextBox.Name = "SelectedCellTextBox";
            this.SelectedCellTextBox.Size = new System.Drawing.Size(250, 31);
            this.SelectedCellTextBox.TabIndex = 5;
            this.SelectedCellTextBox.ReadOnly = true;

            // 
            // CellContentsLabel
            // 
            this.CellContentsLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CellContentsLabel.AutoSize = true;
            this.CellContentsLabel.Location = new System.Drawing.Point(50, 98);
            this.CellContentsLabel.Name = "CellContentsLabel";
            this.CellContentsLabel.Size = new System.Drawing.Size(98, 25);
            this.CellContentsLabel.TabIndex = 3;
            this.CellContentsLabel.Text = "Contents";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.MainControlArea, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grid_widget, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 40);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 192F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1684, 960);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // grid_widget
            // 
            this.grid_widget.AutoSize = true;
            this.grid_widget.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.grid_widget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_widget.Location = new System.Drawing.Point(6, 198);
            this.grid_widget.Margin = new System.Windows.Forms.Padding(6);
            this.grid_widget.MaximumSize = new System.Drawing.Size(4200, 3846);
            this.grid_widget.Name = "grid_widget";
            this.grid_widget.Size = new System.Drawing.Size(1672, 756);
            this.grid_widget.TabIndex = 0;
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // SimpleSpreadsheetGUI
            // 
            this.Controls.Add(this.Dark);
            this.Controls.Add(this.HelpMenu);
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1684, 1000);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "SimpleSpreadsheetGUI";
            this.Text = "Sample GUI - Copy/Modify/Profit";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.MainControlArea.ResumeLayout(false);
            this.tableLayoutForSelectedCellValueContents.ResumeLayout(false);
            this.tableLayoutForSelectedCellValueContents.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
            // 
            // HelpMenu
            // 
            this.HelpMenu.BackColor = System.Drawing.SystemColors.MenuBar;
            this.HelpMenu.Location = new System.Drawing.Point(115, 0);
            this.HelpMenu.Name = "HelpMenu";
            this.HelpMenu.Size = new System.Drawing.Size(84, 28);
            this.HelpMenu.TabIndex = 7;
            this.HelpMenu.Text = "Help Menu";
            this.HelpMenu.UseVisualStyleBackColor = true;
            this.HelpMenu.Click += new System.EventHandler(this.HelpMenu_Click);
            // 
            // Dark
            // 
            this.Dark.Location = new System.Drawing.Point(199, 0);
            this.Dark.Name = "Dark";
            this.Dark.Size = new System.Drawing.Size(84, 28);
            this.Dark.TabIndex = 8;
            this.Dark.Text = "Dark Mode";
            this.Dark.UseVisualStyleBackColor = true;
            this.Dark.Click += new System.EventHandler(this.DarkMode_Enter);

            // 
            // fontColorToolStripMenuItem
            // 
            this.fontColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blackToolStripMenuItem,
            this.redToolStripMenuItem,
            this.blueToolStripMenuItem});
            this.fontColorToolStripMenuItem.Name = "fontColorToolStripMenuItem";
            this.fontColorToolStripMenuItem.Size = new System.Drawing.Size(129, 38);
            this.fontColorToolStripMenuItem.Text = "Font Color";
            // 
            // blackToolStripMenuItem
            // 
            this.blackToolStripMenuItem.Name = "blackToolStripMenuItem";
            this.blackToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.blackToolStripMenuItem.Text = "Black";
            this.blackToolStripMenuItem.Click += new System.EventHandler(this.blackToolStripMenuItem_Click);

            // 
            // redToolStripMenuItem
            // 
            this.redToolStripMenuItem.Name = "redToolStripMenuItem";
            this.redToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.redToolStripMenuItem.Text = "Red";
            this.redToolStripMenuItem.Click += new System.EventHandler(this.redToolStripMenuItem_Click);

            // 
            // blueToolStripMenuItem
            // 
            this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            this.blueToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.blueToolStripMenuItem.Text = "Blue";
            this.blueToolStripMenuItem.Click += new System.EventHandler(this.blueToolStripMenuItem_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);

        }



        private SpreadsheetGridWidget grid_widget;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;

        private FlowLayoutPanel MainControlArea;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutForSelectedCellValueContents;
        private TextBox CellValueTextBox;
        private Label CellValueLabel;
        private Label SelectedCellLabel;
        private Label CellContentsLabel;
        private TextBox CellContentsTextBox;
        private TextBox SelectedCellTextBox;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private Button HelpMenu;
        private Button Dark;
        private System.Windows.Forms.ToolStripMenuItem fontColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private BackgroundWorker backgroundWorker;
        #endregion
    }
}

