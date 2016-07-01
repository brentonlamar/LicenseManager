using System;
using System.Linq;

namespace LicenseManager
{
    public class LicenseManager
    {
        public string LicenseToAdd { get; set; }
        public string LicenseToRemove { get; set; }
        public bool HasLicenseToRemove => !string.IsNullOrWhiteSpace(LicenseToRemove);

        private string _currentDirectory;
        private const string FILE_FILTER_SEARCH_PATTERN = "*.cs";
        private readonly string ASSEMBLY_INFO =  "assemblyinfo.cs";
        public LicenseManager(string currentDirectory)
        {
            _currentDirectory = currentDirectory;
            var licenseToRemoveFileName = System.IO.Path.Combine(_currentDirectory, "LicenseToRemove");
            var licenseToAddFileName = System.IO.Path.Combine(_currentDirectory, "License");

            if (!System.IO.File.Exists(licenseToAddFileName))
            {
                throw new ArgumentException($"The file {licenseToAddFileName} must exist to add a license to your source files");
            }
            LicenseToAdd = System.IO.File.ReadAllText(licenseToAddFileName);

            if (System.IO.File.Exists(licenseToRemoveFileName))
            {
                LicenseToRemove = System.IO.File.ReadAllText(licenseToRemoveFileName);
            }            
        }

        public void RemoveLicenseFromFiles()
        {
            foreach (var item in System.IO.Directory.GetFiles(_currentDirectory, FILE_FILTER_SEARCH_PATTERN, System.IO.SearchOption.AllDirectories)
                                                    .Where(f => !f.ToLower().Contains(ASSEMBLY_INFO)))
            {
                var file = System.IO.File.ReadAllText(item);
                if (this.HasLicenseToRemove & file.Contains(this.LicenseToRemove))
                {
                    Console.WriteLine($"Removing license from {item}");
                    var fileWithoutHeader = $"{file.Remove(0, this.LicenseToRemove.Count())}";
                    System.IO.File.WriteAllText(item, fileWithoutHeader);
                }                
            }
        }

        public void AddLicenseToFiles()
        {
            foreach (var item in System.IO.Directory.GetFiles(_currentDirectory, FILE_FILTER_SEARCH_PATTERN, System.IO.SearchOption.AllDirectories)
                                                    .Where(f => !f.ToLower().Contains(ASSEMBLY_INFO)))
            {
                var file = System.IO.File.ReadAllText(item);
                if (!file.Contains(this.LicenseToAdd))
                {
                    Console.WriteLine($"Adding license to {item}");
                    var fileWithHeader = $"{this.LicenseToAdd}{file}";
                    System.IO.File.WriteAllText(item, fileWithHeader);
                }
            }            
        }
    }
}