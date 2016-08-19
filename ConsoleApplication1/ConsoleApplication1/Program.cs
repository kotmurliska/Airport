using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 130;

            var menu = new Menu();

            var key = menu.DrawMenu();

            menu.Execute(key);

        }
    }

    
}
