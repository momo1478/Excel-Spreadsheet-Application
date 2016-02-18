using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using SS;
using Dependencies;
using System.Text.RegularExpressions;

namespace SS
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
        /// Setup a new dependency graph for variable lookup and circular dependency checks. 
        /// </summary>
        DependencyGraph dg;

        /// <summary>
        /// Contructs an empty spreadsheet, with an empty dependency graph.
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            name = name?.ToUpper();                                 //"ignore case"

            if (ReferenceEquals(name, null) || !isValidName(name)) //null or invalid check
            {
                throw new InvalidNameException();
            }

            Cell cell;                                             //will be null if cannot find appropriate cell.
            cells.TryGetValue(name ,out cell);

            return cell?.contents ?? default(string); 
        }
        
        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (string cellName in cells.Keys)                              //Enumerate through cell names.
            {
                if (Convert.ToString(cells[cellName].contents) != string.Empty)  //If a cell's contents contains an empty string, then it is considered empty.
                {
                    yield return cellName;
                }
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            name = name?.ToUpper();
            if (ReferenceEquals(name, null) || !isValidName(name)) //null or invalid check
            {
                throw new InvalidNameException();
            }
            Cell cellWithName;                                     //null if name isn't found in spreadsheet.

            if (cells.TryGetValue(name, out cellWithName))
            {
                cellWithName.contents = number;                    //set cell's contents to number.
            }
            else
            {
                cells.Add(name, new Cell(number));         //or create a cell if it isn't in cells.
            }

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// If formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            name = name?.ToUpper();

            if (!isValidName(name))
                throw new InvalidNameException();

            Cell cellWithName;                                     //null if name isn't found in spreadsheet.

            DependencyGraph dgBackup = this.dg;                    //save the dependency graph in case the new Formula causes a CircularException.

            if (cells.TryGetValue(name, out cellWithName))
            {
                //performs in case the current cell's contents wasn't already a formula.
                HashSet<String> cellWithNameVariables = cellWithName.contents is Formula ? (HashSet<string>)((Formula)cellWithName.contents).GetVariables() : new HashSet<string>();
                foreach (string oldCellNames in cellWithNameVariables)  //remove old cell dependencies
                {         
                    dg.RemoveDependency(name, oldCellNames.ToUpper());
                }
                cellWithName.contents = formula;                       //set cell's contents to number.
                foreach (string newCellNames in formula.GetVariables())//add new cell dependencies
                {
                    dg.AddDependency(name, newCellNames.ToUpper());
                }

                try                                 //Test for CircularException.
                {
                    GetCellsToRecalculate(name);
                }
                catch (CircularException)           //If so replace new contents with the old one, restore old DG, and throw CircularException.
                {
                    cells[name].contents = cellWithName.contents;
                    this.dg = new DependencyGraph(dgBackup);
                    throw new CircularException();
                }
            }
            else
            {
                cellWithName = new Cell(formula);                        //assign for helper method.

                cells.Add(name, cellWithName);                           //or create a cell if it isn't in cells
                foreach (string cellName in formula.GetVariables())      //add new cell dependencies
                {         
                      dg.AddDependency(name, cellName.ToUpper());
                }

                try                                 //Test for CircularException.
                {
                    GetCellsToRecalculate(name);
                }
                catch (CircularException)           //If so remove newly created cell, restore old DG, and throw CircularException.
                {
                    cells.Remove(name);
                    this.dg = new DependencyGraph(dgBackup);
                    throw new CircularException();
                }        
            }

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Helps SetCellContents(string name, Formula formula) determine if there is a circular dependency.
        /// If the new replacement cell causes a circular dependency then replaces the new cell with it's oldCellName
        /// and Dependencies.
        /// </summary>

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            name = name?.ToUpper();
            if (ReferenceEquals(name, null) || !isValidName(name)) //null or invalid check
            {
                throw new InvalidNameException();
            }
            Cell cellWithName;                                    //null if name isn't found in spreadsheet.

            if (cells.TryGetValue(name, out cellWithName))
            {
                cellWithName.contents = text;                     //set cell's contents to number.
            }
            else
            {
                cells.Add(name, new Cell(text));            //or create a cell if it isn't in cells.
            }

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        ///<summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if(ReferenceEquals(name, null))                                     //null check
                throw new ArgumentNullException("name is null");
            if (!isValidName(name))                                             //valid name check
                throw new InvalidNameException();

            foreach (var dependent in dg.GetDependents(name))
            {
                yield return dependent;
            }
        }

        /// <summary>
        /// Determines if a given string is a valid Cell name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool isValidName(string name)
        {
            return !ReferenceEquals(name , null) && Regex.Matches(name, "([A-Za-z]+[1-9]{1}[0-9]*)$").Count == 1 && Regex.Matches(name, "([A-Za-z]+[1-9]{1}[0-9]*)$")[0].Value.Equals(name);
            
        }
    }
}