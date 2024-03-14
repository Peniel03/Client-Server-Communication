using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class Table
    {
        public delegate void OutputMessage(string str);
        public OutputMessage Message { get; set; }

        private string _tableHeader;
        private string[] _columnHeaders;
        private int[] _columnWidth;
        private string _fileName;

        private void OutputToTheConsole(string str)
        {
            Console.Write(str);
        }

        private void OutputToTheFile(string str)
        {
            using (StreamWriter sw = new StreamWriter(_fileName, true, System.Text.Encoding.Default))
            {
                sw.Write(str);
            }
        }

        public static void ClearFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
            {

            }
        }

        public Table(string tableHeader, string[] columnHeaders, int[] columnWidth, string fileName = "")
        {
            _tableHeader = tableHeader;
            _columnHeaders = columnHeaders;
            _columnWidth = columnWidth;
            _fileName = fileName;

            if (columnHeaders.Length != columnWidth.Length)
                throw new Exception("The number of elements in the arrays does not match");

            for (int i = 0; i < columnHeaders.Length; i++)
            {
                if (columnHeaders[i].Length > columnWidth[i])
                    throw new Exception("Column title width is greater than column width");
            }

            if (fileName == "")
            {
                Message = OutputToTheConsole;
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(fileName, true, System.Text.Encoding.Default))
                    {
                        Message = OutputToTheFile;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Hat()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Message(_tableHeader + "\n");
            Console.ForegroundColor = ConsoleColor.White;
            Message("╔");
            int count = 0;

            foreach (var i in _columnWidth)
            {
                Message(string.Join("", Enumerable.Repeat('═', i)));
                if (count != _columnWidth.Length - 1)
                {
                    Message("╦");
                }
                else
                {
                    Message("╗\n");
                }
                count++;
            }

            Message("║");
            int index = 0;

            foreach (var i in _columnWidth)
            {
                int quantity = _columnHeaders[index].Length;
                Message(string.Join("", Enumerable.Repeat(' ', i - quantity)));
                Message($"{_columnHeaders[index]}║");
                index++;
            }

            Message("\n");
            Message("╠");
            count = 0;

            foreach (var i in _columnWidth)
            {
                Message(string.Join("", Enumerable.Repeat('═', i)));
                if (count != _columnWidth.Length - 1)
                {
                    Message("╬");
                }
                else
                {
                    Message("╣\n");
                }
                count++;
            }
        }

        /// <summary>
        ///  bool output,  
        /// </summary>
        /// <param name="data"></param>
        public void Body(object[] data)
        {
            Message("║");
            int index = 0;
            int quantity;

            foreach (var i in _columnWidth)
            {
                if (data[index] is double @double)
                {
                    int number;
                    if (@double >= 0)
                    {
                        number = (int)Math.Ceiling(@double);
                    }
                    else
                    {
                        number = (int)Math.Floor(@double);
                    }
                    quantity = number.ToString().Length - 2;
                }
                else if (data[index] is string @string)
                {
                    quantity = @string.Length;
                }
                else if (data[index] is int @int)
                {
                    quantity = @int.ToString().Length;
                }
                else
                {
                    quantity = 0;
                }

                Message(string.Join("", Enumerable.Repeat(' ', i - quantity)));

                if (data[index] is int)
                {
                    Message($"{data[index]}║");
                }
                else
                {
                    Message($"{data[index]:f2}║");
                }

                index++;
            }

            Message("\n");
        }

        public void Bottom()
        {
            Message("╚");
            int count = 0;

            foreach (var i in _columnWidth)
            {
                Message(string.Join("", Enumerable.Repeat('═', i)));

                if (count != _columnWidth.Length - 1)
                {
                    Message("╩");
                }
                else
                {
                    Message("╝\n");
                }
                count++;
            }
        }
    }
}
