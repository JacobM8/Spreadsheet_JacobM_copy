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
        public Cell(String name, String content)
        {
            this.contents = content;
        }

        /// <summary>
        /// Double constructor sets the public contents to the content parameter passed in the constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public Cell (String name, Double content)
        {
            this.contents = content;
        }

        /// <summary>
        /// Formula constructor sets the public contents to the content parameter passed in the constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public Cell (String name, Formula content)
        {
            this.contents = content;
        }

    }
}
