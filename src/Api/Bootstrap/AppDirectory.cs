using System.Diagnostics;
using System.IO;

namespace BudgetBuddy.Api.Bootstrap
{
    public static class AppDirectory
    {
        public static string Root
        {
            get
            {
                if (Debugger.IsAttached)
                    return Directory.GetCurrentDirectory();

                var exePath = Process.GetCurrentProcess().MainModule.FileName;
                var directory = Path.GetDirectoryName(exePath);
                return directory;
            }
        }
    }
}
