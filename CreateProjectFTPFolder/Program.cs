using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateProjectFTPFolder
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowMenu();
        }

        private static void ShowMenu()
        {
            Console.WriteLine("This program will create a template folder for a new project in the FTP drive.\n");

            while (true)
            {
                Console.Write("Enter the project number (type 'exit' to exit the program): ");
                var userInput = Console.ReadLine(); // Get input from user
                var projectNumber = userInput.Trim().ToLower(); // Trim input and make it lower case

                if (projectNumber.Equals("exit"))
                {
                    break;
                }

                if (!CheckInput(projectNumber)) // Check if the user input is not a proper project number
                {
                    Console.WriteLine("Invalid project number. The project number must consist in 6 digits.\n");
                }

                else
                {
                    var project = new Project(projectNumber);
                }
            }
        }

        private static bool CheckInput(string projectNumber)
        {
            int integer;
            if (projectNumber.Length != 5 || !int.TryParse(projectNumber, out integer))
            {
                return false;
            }

            return true;
        }
    }
}
