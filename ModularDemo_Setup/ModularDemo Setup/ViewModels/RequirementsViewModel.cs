using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularDemo_Setup.ViewModel
{
    internal class RequirementsViewModel
    {


        public bool CheckProgramInstalled(string programToCheck)
        {
            string registry_key = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

            Microsoft.Win32.RegistryKey sub_key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registry_key);



            foreach (var skname in sub_key.GetSubKeyNames())
            {

                Microsoft.Win32.RegistryKey productKey = sub_key.OpenSubKey(skname);
                if (productKey != null)
                {
                    string programName = Convert.ToString(productKey.GetValue("DisplayName"));

                    if (programName.Contains(programToCheck))
                    {
                        return true;
                    }
                }

            }

            return false;

        }

    }
    
}
