using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using SS;

namespace Spreadsheet
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// "Infinite" list of cells that form the spreadsheet. 
        /// Key = name of Cell
        /// Value = Cell object.
        /// </summary>
        Dictionary<string, Cell> cells;

        /// <summary>
        /// Contructs an empty spreadsheet.
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (ReferenceEquals(name, null))
            {
                throw new InvalidNameException("name is null.");
            }

            Cell cell;

            cells.TryGetValue(name);
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if a given string is a valid Cell name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool isValidName(string name)
        {

        }
    }
}
