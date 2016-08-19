using System;
using System.IO;
using System.Linq;

namespace CreateProjectFTPFolder
{
    internal class Project
    {
        private string ProjectNumber;
        public string OriginalPath { get; set; }
        public string GDrivePath { get; set; }
        public string ProjectYear { get; set; }
        public string ProjectNumberWithName { get; set; }

        public Project(string projectNumber)
        {
            ProjectNumber = projectNumber;
            ProjectYear = GetProjectYear(ProjectNumber);
            OriginalPath = GetOriginalPath(ProjectNumber);
            ProjectNumberWithName = GetProjectNumberWithName(OriginalPath);
            GDrivePath = SetGDrivePath(ProjectNumberWithName);
        }

        private string SetGDrivePath(string projectNumberWithName)
        {
            return string.Format("C:\\test\\20{0}\\{1}\\", ProjectYear, projectNumberWithName); // Sets the path for the new folder
        }

        private string GetProjectNumberWithName(string originalPath)
        {
            return originalPath.Substring(originalPath.LastIndexOf("\\") + 1); // Returns the project number and name from the original path
        }

        private string GetProjectCode(string projectNumber)
        {
            return projectNumber.Substring(2); // Returns the last three digits of the project number
        }

        private string GetProjectYear(string projectNumber)
        {
            return projectNumber.Substring(0, 2); // Returns the firs two digits of the project number
        }

        private string GetOriginalPath(string projectNumber)
        {
            var path = string.Format("P:\\20{0}\\", ProjectYear); // Sets the folder where to look into
            var searchPattern = string.Format("{0}_*", ProjectNumber); // Sets what folder to look for

            var result = (from dir in
                             Directory.EnumerateDirectories(path, searchPattern)
                         select dir);

            if (result.Count() == 0)
            {
                throw new DirectoryNotFoundException();
            }

            var found = result.FirstOrDefault().ToString();

            return found; // Returns the whole path
        }
    }
}