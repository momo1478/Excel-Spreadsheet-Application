using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet
{
    class Cell
    {

        /// <summary>
        /// Contents of Cell. Can be a string, a double, or a FormulaError.
        /// </summary>
        internal object contents;

        /// <summary>
        /// Value of Cell. Can be a string, a double, or a FormulaError.
        /// </summary>
        internal object value;

        /// <summary>
        /// Constructs a cell with a given contents, and value.
        /// </summary>
        /// <param name="iName"></param>
        /// <param name="iContents"></param>
        /// <param name="iValue"></param>
        internal Cell(object iContents = null, object iValue = null)
        {
            contents = iContents;
            value = iValue;
        }
    }
}
