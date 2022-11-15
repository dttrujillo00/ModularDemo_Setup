using System;
using System.Windows.Forms;
using WixSharp;
using WixSharp.UI.WPF;

namespace ModularDemo_Setup
{
    internal class Program
    {
        static void Main()
        {

            var module1 = new Feature("Modulo 1");
            var module2 = new Feature("Modulo 2");
            var shell = new Feature("Shell");

            var project = new ManagedProject("ModularDemo Setup",
                              new Dir(@"%ProgramFiles%\EMSI FARMA\ModularDemo Setup",
                                  new Files(shell, @"Shell\*.*"),
                                  new Dir("Modules",
                                  new File(@"Files\Modules\ModularDemo.Core.dll"),
                                  new File(@"Files\Modules\ModularDemo.Core.pdb"),
                                  new Files(module1, @"Modulo1\*.*"),
                                  new Files(module2, @"Modulo2\*.*"),
                                   new ExeFileShortcut("Uninstall MyApp", "[System64Folder]msiexec.exe", "/x [ProductCode]"))));

            project.GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b");

            // project.ManagedUI = ManagedUI.DefaultWpf; // all stock UI dialogs

            //custom set of UI WPF dialogs
            project.ManagedUI = new ManagedUI();

            project.ManagedUI.InstallDialogs.Add<ModularDemo_Setup.WelcomeDialog>()
                                            .Add<ModularDemo_Setup.LicenceDialog>()
                                            .Add<ModularDemo_Setup.FeaturesDialog>()
                                            .Add<ModularDemo_Setup.InstallDirDialog>()
                                            .Add<ModularDemo_Setup.ProgressDialog>()
                                            .Add<ModularDemo_Setup.ExitDialog>();

            project.ManagedUI.ModifyDialogs.Add<ModularDemo_Setup.MaintenanceTypeDialog>()
                                           .Add<ModularDemo_Setup.FeaturesDialog>()
                                           .Add<ModularDemo_Setup.ProgressDialog>()
                                           .Add<ModularDemo_Setup.ExitDialog>();

            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            project.UILoaded += project_UIInit;
           // project.BeforeInstall += project_BeforeInstall;


            project.BuildMsi();
        }

       // static void project_BeforeInstall(SetupEventArgs e)
      //  {
       //     MessageBox.Show(e.Session.GetMainWindow(), e.ToString(), "BeforeInstall");
       // }

        static void project_UIInit(SetupEventArgs e)
        {
            MessageBox.Show(e.Session.GetMainWindow(), "Hello World! (CLR: v" + Environment.Version + ")", "Managed Setup - UIInit");

            //set custom installdir
            //This event is fired before Wix# ManagedUI loaded (disabled for demo purposes)
            //e.Session["INSTALLDIR"] = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\7-Zip")
            //                                              .GetValue("Path")
            //                                              .ToString();

  
        }

        string rutaEnElRegistro = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

        private string BuscarVersionInstalada(string displayName, string publisher)
        {
            Microsoft.Win32.RegistryKey clave = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(rutaEnElRegistro);
            foreach (var skname in clave.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey productKey = key.OpenSubKey(skname);
                if (productKey != null)
                {
                    string keyValue = Convert.ToString(productKey.GetValue("Publisher"));
                    if (!keyValue.Equals(publisher, StringComparison.OrdinalIgnoreCase))
                        continue;

                    string productName = Convert.ToString(productKey.GetValue("DisplayName"));
                    if (!productName.Equals(displayName, StringComparison.OrdinalIgnoreCase))
                        continue;

                    string displayVersion = Convert.ToString(productKey.GetValue("DisplayVersion"));

                    return displayVersion;
                }
            }

            return null;
        }
    }
}