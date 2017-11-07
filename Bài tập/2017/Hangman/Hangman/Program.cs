using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            HangmanImage.PrintImage(0); Console.WriteLine();
            HangmanImage.PrintImage(6);
            Console.ReadKey();
        }
    }
}
