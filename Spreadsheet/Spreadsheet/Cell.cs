using System;
using SpreadsheetUtilities;
using System.Collections.Generic;
using System.Text;

namespace SS
{
    public class Cell
    {
        // contents getter will be used in Spreadsheet GetCellContent methods
        public object contents { get; set; }
        
        /// <summary>
        /// String constructor sets the public contents to the content parameter passed in the constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public Cell(string name, object content)
        {
            this.contents = content;
        }

        // don't need bottom two when first constructor has "object" passed in as second parameter
/*
        /// <summary>
        /// Double constructor sets the public contents to the content parameter passed in the constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public Cell (string name, double content)
        {
            this.contents = content;
        }

        /// <summary>
        /// Formula constructor sets the public contents to the content parameter passed in the constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public Cell (string name, Formula content)
        {
            this.contents = content;
        }
*/
    }
}
