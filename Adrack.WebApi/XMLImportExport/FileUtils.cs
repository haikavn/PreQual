using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Adrack.WebApi.XMLImportExport
{
    // to add large files handling
    public class FileUtils
    {
        public static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public static void CreateEmptyFile(string filename)
        {
            File.Create(filename).Dispose();
        }

        public static void CreateEmptyFile(string path, string filename)
        {
            File.Create(Path.Combine(path, filename)).Dispose();
        }

        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static void CreateDirectory(string path, string childpath)
        {
            Directory.CreateDirectory(Path.Combine(path, childpath));
        }
    }
}
