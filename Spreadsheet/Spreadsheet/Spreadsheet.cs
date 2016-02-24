using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using SS;
using Dependencies;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.Schema;

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
        /// isValid Regex for the current Spreadsheet instance
        /// </summary>
        Regex isValid;

        /// <summary>
        /// Determines if the spreadsheethas been modified since it was created or saved.
        /// Turns False : on created or save.
        /// Turns True : on modification.
        /// </summary>
        bool hasChanged;

        //Added PS6
        //TODO: implement hasChanged everywhere.
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return hasChanged;
            }

            protected set
            {
                hasChanged = value;
            }
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();

            hasChanged = false;

            this.isValid = new Regex(".*?");
        }

        //Added in PS6
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        public Spreadsheet(Regex IsValid)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();

            hasChanged = false;

            this.isValid = IsValid;
        }

        //Added in PS6
        //TODO : Spreadsheet(TextReader source)
        /// <summary>
        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format
        ///specification.  If there's a problem reading source, throws an IOException
        ///If the contents of source are not consistent with the schema in Spreadsheet.xsd,
        ///throws a SpreadsheetReadException.If there is an invalid cell name, or a
        ///duplicate cell name, or an invalid formula in the source, throws a SpreadsheetReadException.
        ///If there's a Formula that causes a circular dependency, throws a SpreadsheetReadException.
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        /// </summary>
        /// <param name="source"></param>
        public Spreadsheet(TextReader source) : this()
        {
            // Create the XmlSchemaSet class.  Anything with the namespace "urn:states-schema" will
            // be validated against states3.xsd.
            XmlSchemaSet sc = new XmlSchemaSet();

            // NOTE: To read states3.xsd this way, it must be stored in the same folder with the
            // executable.  To arrange this, I set the "Copy to Output Directory" propery of states3.xsd to
            // "Copy If Newer", which will copy states3.xsd as part of each build (if it has changed
            // since the last build).
            sc.Add("urn:spreadsheet-schema", "Spreadsheet.xsd");

            // Configure validation.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidationCallback;

            try
            {
                using (XmlReader reader = XmlReader.Create(source))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    reader.MoveToFirstAttribute();
                                    isValid = new Regex(reader.GetAttribute(0));
                                    break;

                                case "cell":
                                    reader.MoveToFirstAttribute();
                                    string cellNameAttribute = reader.Value;
                                    reader.MoveToNextAttribute();
                                    string cellContentsAttribute = reader.Value;
                                    SetContentsOfCell(cellNameAttribute, cellContentsAttribute);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        // Display any validation errors.
        private static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            Console.WriteLine(" *** Validation Error: {0}", e.Message);
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

            object obj = (cell?.contents) ?? "";
            return obj;
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
        protected override ISet<string> SetCellContents(string name, double number)
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
                cellWithName.value = number;
            }
            else
            {
                cells.Add(name, new Cell(number, number));                 //or create a cell if it isn't in cells.
            }

            foreach (string cellName in GetCellsToRecalculate(name))
            {
                if (cells[cellName].contents is Formula)
                {
                    try
                    {
                        cells[cellName].value = ((Formula)cells[cellName].contents).Evaluate(lookup);
                    }
                    catch (UndefinedVariableException)
                    {
                        cells[cellName].value = new FormulaError();
                    }

                }
            }

            hasChanged = true;
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            name = name?.ToUpper();

            if (ReferenceEquals(formula, null))
                throw new ArgumentNullException();

            if (!isValidName(name))
                throw new InvalidNameException();

            Cell cellWithName;                                     //null if name isn't found in spreadsheet.
            

            DependencyGraph dgBackup = this.dg;                    //save the dependency graph in case the new Formula causes a CircularException.

            if (cells.TryGetValue(name, out cellWithName))
            {
                //performs in case the current cell's contents wasn't already a formula.
                HashSet<String> cellWithNameVariables = cellWithName.contents is Formula ? (HashSet<string>)((Formula)cellWithName.contents).GetVariables() : new HashSet<string>();
                object oldContents = cellWithName.contents;             //save old contents in case of circular exception.

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
                    hasChanged = true;
                }
                catch (CircularException)           //If so replace new contents with the old one, restore old DG, and throw CircularException.
                {
                    cells[name].contents = oldContents;
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
                    hasChanged = true;
                }
                catch (CircularException)           //If so remove newly created cell, restore old DG, and throw CircularException.
                {
                    cells.Remove(name);
                    this.dg = new DependencyGraph(dgBackup);
                    throw new CircularException();
                }        
            }

            foreach (string cellName in GetCellsToRecalculate(name))
            {
                if (cells[cellName].contents is Formula)
                {
                    try
                    {
                        cells[cellName].value = ((Formula)cells[cellName].contents).Evaluate(lookup);
                    }
                    catch(UndefinedVariableException)
                    {
                        cells[cellName].value = new FormulaError();
                    }
                    
                }
            }

            hasChanged = true;
            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Helper method for SetCellContents that allows the method to find out the values of variables.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        private double lookup(string variable)
        {
            if (cells[variable].value is double)
            {
                return (double)cells[variable].value;
            }
            else
            {
                throw new UndefinedVariableException(variable + "is undefined or is not a double.");
            }
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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            name = name?.ToUpper();
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException();
            }

            if (ReferenceEquals(name, null) || !isValidName(name)) //null or invalid check
            {
                throw new InvalidNameException();
            }
            Cell cellWithName;                                    //null if name isn't found in spreadsheet.

            if (cells.TryGetValue(name, out cellWithName))
            {
                cellWithName.contents = text;                     //set cell's contents to number.
                cellWithName.value = text;
            }
            else
            {
                cells.Add(name, new Cell(text,text));            //or create a cell if it isn't in cells.
            }

            foreach (string cellName in GetCellsToRecalculate(name))
            {
                if (cells[cellName].contents is Formula)
                {
                    try
                    {
                        cells[cellName].value = ((Formula)cells[cellName].contents).Evaluate(lookup);
                    }
                    catch (UndefinedVariableException)
                    {
                        cells[cellName].value = new FormulaError();
                    }

                }
            }

            hasChanged = true;
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

            foreach (var dependee in dg.GetDependees(name))
            {
                yield return dependee;
            }
        }

        //HACK : isValid.IsMatch() could need fixing.
        /// <summary>
        /// Determines if a given string is a valid Cell name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool isValidName(string name)
        {
            return !ReferenceEquals(name , null) && Regex.Matches(name, "([A-Za-z]+[1-9]{1}[0-9]*)$").Count == 1
                   && Regex.Matches(name, "([A-Za-z]+[1-9]{1}[0-9]*)$")[0].Value.Equals(name)
                   && isValid.IsMatch(name); 
        }

        //TODO: Save(TextWriter dest)
        //Added in PS6
        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the isvalid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            if (!hasChanged)
                return;
            //"../../MySpreadsheet.xml"
            try
            {
                using (XmlWriter writer = XmlWriter.Create(dest))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("", "spreadsheet", "urn:spreadsheet-schema");
                    writer.WriteAttributeString("IsValid=", isValid.ToString());

                    foreach (KeyValuePair<string, Cell> cell in cells)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteAttributeString("name", cell.Key);
                        if (cell.Value.contents is Formula)
                        {
                            writer.WriteAttributeString("contents", "=" + cell.Value.contents.ToString());
                        }
                        else if (cell.Value.contents is double)
                        {
                            writer.WriteAttributeString("contents", ((double)cell.Value.contents).ToString());
                        }
                        else
                        {
                            writer.WriteAttributeString("contents", ((string)cell.Value.contents).ToString());
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception)
            {
                throw new IOException("Exception occured within Save.");
            }
            hasChanged = false;

        }

        //TODO: GetCellValue(string name)
        //Added in PS6
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if (name == null || !isValidName(name))
            {
                throw new InvalidNameException();
            }
            return cells[name].value;
        }

        //Added in PS6
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || !isValidName(name))
            {
                throw new InvalidNameException();
            }

            double doubleContent;
            if (Double.TryParse(content, out doubleContent)) //Set contents to a double
            {
                return SetCellContents(name, doubleContent);
            }

            else if (content.StartsWith("="))                //Set contents to a formula.
            {
                Formula formulaContent = new Formula(content.Substring(1), s => s.ToUpper(), s => isValidName(s) == true);
                return SetCellContents(name, formulaContent);
            }

            else                                             //Set contents to the given string.
            {
                return SetCellContents(name, content);
            }
        }
    }
}