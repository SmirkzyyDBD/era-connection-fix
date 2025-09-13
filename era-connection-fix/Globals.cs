using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace era_connection_fix
{
    public static class Globals
    {
        public static readonly string SelfDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Era-Connection-Fix";
        public static void EnsureSelfDataFolderExists()
        {
            if (Directory.Exists(SelfDataFolder) == false)
                Directory.CreateDirectory(SelfDataFolder);
        }

        public static string[] hostNames =
        {
            "erafn.dev"
        };

        public static void EraLogoFunc()
        {
            string EraLogo = @"
  ______             ______ _   _ 
 |  ____|           |  ____| \ | |
 | |__   _ __ __ _  | |__  |  \| |
 |  __| | '__/ _` | |  __| | . ` |
 | |____| | | (_| | | |    | |\  |
 |______|_|  \__,_| |_|    |_| \_|                
   
     https://discord.gg/erafn
";
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(EraLogo);
            Console.ResetColor();
        }
    }
}
