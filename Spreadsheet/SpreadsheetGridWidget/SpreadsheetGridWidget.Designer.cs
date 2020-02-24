﻿using System.Drawing;
using System.Windows.Forms;
/// <summary>
///   Author(s):       Joe Zachary
///                    Jim de St. Germain - Refactor/Documentation for ASP Core
///   Course:          CS 3500
///   Date:            2011 Sept   - Initial Grid Code
///                    2020 Spring - Refactor for ASP Core
///   
///   Partial:
/// 
///     The code for this class is divided into two files using C#'s partial ability.
///     The other half is in the SpreadsheetGridWidget.cs file.  This code was
///     (somewhat) automatically generated by the WYSIWYG design editor.  It should not
///     be directly modified.  Personalizations should be done in the other half of the partial.
/// 
///   Contents:
///   
///     See SpreadsheetGridWidget.cs for more information.
/// 
/// </summary>
namespace SpreadsheetGrid_Framework
{
    partial class SpreadsheetGridWidget
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hScroll = new System.Windows.Forms.HScrollBar();
            this.vScroll = new System.Windows.Forms.VScrollBar();
            this.drawingPanel = new SpreadsheetGrid_Framework.DrawingPanel(  );
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScroll.Location = new System.Drawing.Point(0, 483);
            this.hScroll.Name = "hScrollBar1";
            this.hScroll.Size = new System.Drawing.Size(483, 17);
            this.hScroll.TabIndex = 0;
            // 
            // vScrollBar1
            // 
            this.vScroll.Location = new System.Drawing.Point(483, 0);
            this.vScroll.Name = "vScrollBar1";
            this.vScroll.Size = new System.Drawing.Size(17, 483);
            this.vScroll.TabIndex = 1;
            // 
            // drawingPanel1
            // 
            this.drawingPanel.Location = new System.Drawing.Point(0, 0);
            this.drawingPanel.Name = "drawingPanel1";
            this.drawingPanel.Size = new System.Drawing.Size(483, 483);
            this.drawingPanel.TabIndex = 3;
            // 
            // SpreadsheetGridWidget
            // 
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Controls.Add(this.drawingPanel);
            this.Controls.Add(this.vScroll);
            this.Controls.Add(this.hScroll);
            this.Name = "SpreadsheetGridWidget";
            this.Size = new System.Drawing.Size(500, 500);
            this.ResumeLayout(false);

        }

        #endregion

        private HScrollBar hScroll;
        private VScrollBar vScroll;
        private DrawingPanel drawingPanel;
    }
}