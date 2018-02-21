using LoSa.Land.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoSa.Land.Tables
{
    public class Form6Zem
    {
        //public List<RowForm6Zem> Rows { get; set; }
        public List<RowForm6Zem> FirstSection { get; set; }
        public List<RowForm6Zem> SecondSection { get; set; }
        public List<ColForm6Zem> Cols { get; set; }
        public List<BlockColForm6Zem> Blocks { get; set; }
        

        public Form6Zem()
        {
            //this.Rows = new List<RowForm6Zem>();
            //this.Cols = new List<ColForm6Zem>();
            this.FirstSection = new List<RowForm6Zem>();
            this.SecondSection = new List<RowForm6Zem>();
            this.Blocks = new List<BlockColForm6Zem>();
        }

    }

    public class RowForm6Zem
    {
        public NumberRow Number { get; set; }
        public int Code { get; set; }
        public List<int> JoinRow { get; set; }
        public string Description { get; set; }

        public RowForm6Zem()
        {
            this.JoinRow = new List<int>();
        }

        public RowForm6Zem(int code, NumberRow number, List<int> joinRow, string description)
        {
            this.Code = code;
            this.Number = number;
            this.JoinRow = joinRow;
            this.Description = description;
        }
        
    }

    public class NumberRow
    {
        List<int> value = new List<int>(3);

        public string Value
        {
            get 
            {
                string strRez = null;
                for (int i = 0; i < 4; i++)
                {
                    
                    if (this.value[i] > 0 && i == 0)
                    {
                        strRez = value[i].ToString();
                    }
                    else if (this.value[i] > 0 && i > 0)
                    {
                        strRez += "." + value[i].ToString();
                    }
                }
                return strRez; 
            }
            set 
            { 
                string[] mValue = value.Split( new char[] { '.' }, 4);
                for (int i = 0; i < 4; i++)
                {
                    if (i < mValue.Length)
                    {
                        this.value[i] = Convert.ToInt32(mValue[i]);
                    }
                    else 
                    {
                        this.value[i] = 0;
                    }
                }
            }
        }

        public NumberRow(string value)
        {
            this.Value = value;
        }
    }

    public class ColForm6Zem
    {
        public int Number { get; set; }
        public string CodeKVZU { get; set; }
        public string Name { get; set; }
        public List<int> JoinCol { get; set; }
        public string Description { get; set; }

        public ColForm6Zem()
        {
            this.JoinCol = new List<int>();
        }

        public ColForm6Zem( int number,
                            //string codeKVZU,
                            string name,
                            List<int> joinCol,
                            string description)
        {
            this.Number = number;
            //this.CodeKVZU = codeKVZU;
            this.Name = Name;
            this.JoinCol = joinCol;
            this.Description = description;
        }
    }

    public class BlockColForm6Zem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> NumberCols { get; set; }
        public List<BlockColForm6Zem> Blocks { get; set; }
        public int NumberLinesTitle { get; set; }

        public BlockColForm6Zem()
        {
            this.NumberCols = new List<int>();
            this.Blocks = new List<BlockColForm6Zem>();
            this.NumberLinesTitle = 1;
        }
    }
}
