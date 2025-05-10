using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigrin
{
    public class AppSettings
    {
        private static string DbName = "tigrin.db";
        private static string DbDirectory = FileSystem.AppDataDirectory;
        public static string DbPath = Path.Combine(DbDirectory, DbName);
    }
}
