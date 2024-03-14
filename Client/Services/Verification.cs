using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client.Services
{
    public class Verification
    {
        public static int InputInt(string query)
        {
            int element;
            Console.Write(query);
            bool result = int.TryParse(Console.ReadLine(), out element);
            if (!result)
            {
                while (!result)
                {
                    Console.WriteLine("Error. Enter again");
                    result = int.TryParse(Console.ReadLine(), out element);
                }
            }
            return element;
        }

        public static double InputDouble(string query)
        {
            double element;
            Console.WriteLine(query);
            bool result = double.TryParse(Console.ReadLine(), out element);

            if (!result)
            {
                while (!result)
                {
                    Console.WriteLine("Error. Enter again");
                    result = double.TryParse(Console.ReadLine(), out element);
                }
            }
            return element;
        }

        public static string InputString(string query)
        {
            Console.WriteLine(query);
            string str = Console.ReadLine();
            bool isInvalid = Regex.IsMatch(str, @"^[\p{L}\p{N}: ]+$");
            while (!isInvalid)
            {
                Console.WriteLine("Incorrect Input, Please Enter again !");
                str = Console.ReadLine();
                isInvalid = Regex.IsMatch(str, @"^[\p{L}\p{N}: ]+$");
            }
            return str;
        }
        //@"^[а-яА-Я0-9: ]+$"
    }
}
