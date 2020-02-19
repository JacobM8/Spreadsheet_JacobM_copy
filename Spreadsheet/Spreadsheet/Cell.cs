/// <summary>
///     Author: Jacob Morrison
///     Date: 2/7/2020
///     This file is used to store the contents of a specific cell
///     I pledge that I did the work myself.
/// </summary>
using System;
using SpreadsheetUtilities;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SS
{
    public class Cell
    {
        // contents getter will be used in Spreadsheet GetCellContent methods
        public object contents { get; set; }
        public object value { get; set; }
        private string name;
        
        /// <summary>
        /// Object constructor sets the public contents to the content parameter passed in the constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public Cell(string _name, object content)
        {
            name = _name;
            this.contents = content;
        }

        /// <summary>
        /// Write XML for this Cell
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("cell");
            writer.WriteElementString("name", name);
           
            writer.WriteElementString("contents", contents.ToString());
            writer.WriteEndElement();
        }
    }
}
