using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LicenseManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();

            var manager = new LicenseManager(currentDirectory);

            manager.RemoveLicenseFromFiles();
            manager.AddLicenseToFiles();
        }
    }
}
