using System;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace CreateProjectFTPFolder
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ShowMenu();
        }

        private static void ShowMenu()
        {
            Console.WriteLine("This program will create a template folder for a new project in the FTP drive.\n");

            while (true)
            {
                Console.Write("Enter the project number (type 'exit' to exit): ");
                var userInput = Console.ReadLine(); // Get input from user
                var projectNumber = userInput.Trim().ToLower(); // Trim input and make it lower case

                if (projectNumber.Equals("exit"))
                {
                    break;
                }

                if (!CheckInput(projectNumber)) // Check if the user input is not a proper project number
                {
                    Console.WriteLine("Invalid project number. The project number must be 5 digits long and contain no letter nor special characters.\n");
                }

                else
                {
                    try
                    {
                        var project = new Project(projectNumber);

                        if (ConfirmResult(project))
                        {
                            CreateNewFolder(project);
                            if (!CheckIfExit())
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (!CheckIfExit())
                            {
                                break;
                            }
                        }

                    }
                    catch (DirectoryNotFoundException)
                    {
                        Console.WriteLine("Error: no folder found on the P drive for the project {0}.", projectNumber);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("While trying to work on the project {0} this error occurred: {1}.", projectNumber, ex.Message);
                    }
                }
            }
        }

        private static void CreateNewFolder(Project project)
        {
            try
            {
                var SourcePath = @"F:\template\FTP";

                if (Directory.Exists(project.GDrivePath)) // Throws an exception if the destination folder already exists
                {
                    throw new DirectoryAlreadyExistsExceptions("A directory for this project already exists on the FTP drive.");
                }

                Directory.CreateDirectory(project.GDrivePath);

                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, project.GDrivePath));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(SourcePath, project.GDrivePath), true);

                Console.WriteLine("\nSuccess.\n");

                Clipboard.SetText(project.ProjectNumberWithName);

                Console.WriteLine("{0} copied to the clipboard.\n", project.ProjectNumberWithName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
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

    [Serializable]
    internal class DirectoryAlreadyExistsExceptions : IOException
    {
        public DirectoryAlreadyExistsExceptions()
        {
        }

        public DirectoryAlreadyExistsExceptions(string message) : base(message)
        {
        }

        public DirectoryAlreadyExistsExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DirectoryAlreadyExistsExceptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
