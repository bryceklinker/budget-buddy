using System.Diagnostics;
using System.IO;

namespace BudgetBuddy.Api.Bootstrap
{
    public static class BaseDirectory
    {
        public static string GetBaseDirectory()
        {
            if (Debugger.IsAttached)
                return Directory.GetCurrentDirectory();

            var exePath = Process.GetCurrentProcess().MainModule.FileName;
            return Path.GetDirectoryName(exePath);
        }
    }
}
