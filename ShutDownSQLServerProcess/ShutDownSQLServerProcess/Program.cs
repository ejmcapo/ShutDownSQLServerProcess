using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;

namespace ShutDownSQLServerProcess
{
    class Program
    {
        static void Menu(ShutDownProcess shdp)
        {
            short option = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("////////////////////////////////////////////////////////////////");
                Console.WriteLine("//  Please select an option:                                  //");
                Console.WriteLine("//    1 -> Specify connection string.                         //");
                Console.WriteLine("//    2 -> Show sql server user processs.                     //");
                Console.WriteLine("//    3 -> Show all sql server process.                       //");
                Console.WriteLine("//    4 -> Kill a sql server user process.                    //");
                Console.WriteLine("//    5 -> Kill all sql server user process.                  //");
                Console.WriteLine("//    6 -> Stop all sql server process.                       //");
                Console.WriteLine("//    7 -> Exit.                                              //");
                Console.WriteLine("////////////////////////////////////////////////////////////////");

                try
                {
                    option = short.Parse(Console.ReadLine());
                    switch (option)
                    {
                        case 1:
                            Console.WriteLine("Enter server name: ");
                            shdp.ServerName = Console.ReadLine();
                            Console.WriteLine("Enter database name: ");
                            shdp.DatabaseName = Console.ReadLine();
                            shdp.UpdateConnectionString();
                            Console.ReadKey();
                            break;
                        case 2:
                            shdp.ShowSQLServerUserProcess();
                            Console.ReadKey();
                            break;
                        case 3:
                            shdp.ShowSQLServerAllProcess();
                            Console.ReadKey();
                            break;
                        case 4:
                            Console.WriteLine("Enter the psid process: ");
                            short psid = short.Parse(Console.ReadLine());
                            shdp.KillSQLServerUserProcess(psid);
                            Console.ReadKey();
                            break;
                        case 5:
                            shdp.KillSQLServerAllProcess();
                            Console.ReadKey();
                            break;
                        case 6:
                            shdp.StopSQLServerAllProcess();
                            Console.WriteLine("All database process was stopped");
                            Console.ReadKey();
                            break;
                        case 7:
                            Console.WriteLine("Come back soon. Bye");
                            Console.ReadKey();
                            option = 0;
                            break;
                        default:
                            Console.WriteLine("Wrong option");
                            Console.ReadKey();
                            option = -1;
                            break;
                    }

                    if (option == 0)
                        break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("You must enter an integer value between [1,5]");
                    Console.ReadKey();
                }
                catch (Exception)
                {
                    Console.WriteLine("Something was wrong with the input value");
                    Console.ReadKey();
                }
            }
        }

        static void Main(string[] args)
        {
            ShutDownProcess shdp = new ShutDownProcess();
            Menu(shdp);
        }
    }
}
