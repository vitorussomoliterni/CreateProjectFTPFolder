﻿using System;
using System.Collections.Generic;
using System.IO;
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
                    Console.WriteLine("Invalid project number. The project number must consist in 5 digits.\n");
                }

                else
                {
                    try
                    {
                        var project = new Project(projectNumber);

                        if (ConfirmResult(project))
                        {
                            CreateNewFolder(project);
                            Console.WriteLine("Press any key to exit.");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            if (!CheckIfExit())
                            {
                                break;
                            }
                        }

                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        Console.WriteLine("Error: no folder found on the P drive for that project number.");
                        Console.WriteLine("Please try again.\n");
                    }
                }
            }
        }

        private static void CreateNewFolder(Project project)
        {
            try
            {
                Directory.CreateDirectory("C:\\test\\" + project.ProjectNumberWithName);

                var SourcePath = @"F:\template\FTP";
                var DestinationPath = @"C:\test\" + project.ProjectNumberWithName;

                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);

                Console.WriteLine("\nSuccess.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message, ex.Message);
            }
        }

        private static bool CheckIfExit()
        {
            while (true)
            {
                Console.Write("Would you like to enter a new project? (Y/N) ");
                var response = Console.ReadLine();

                while (true)
                {
                    if (response.ToLower().Equals("y"))
                    {
                        return true;
                    }
                    else if (response.ToLower().Equals("n"))
                    {
                        return false;
                    }
                }
            }
        }

        private static bool ConfirmResult(Project project)
        {
            Console.WriteLine("\nThe project you requested is {0}\n", project.ProjectNumberWithName);
            Console.WriteLine("This new folder will be created:  {0}\n", project.GDrivePath);
            Console.Write("Are you sure you want to proceed? (Y/N) ");

            var response = Console.ReadLine();

            while (true)
            {
                if (response.ToLower().Equals("y"))
                {
                    return true;
                }
                else if (response.ToLower().Equals("n"))
                {
                    break;
                }
            }

            return false;
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
