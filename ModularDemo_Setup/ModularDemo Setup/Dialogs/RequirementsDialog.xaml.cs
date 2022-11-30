using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using WixSharp;
using WixSharp.UI.Forms;
using WixSharp.UI.WPF;

namespace ModularDemo_Setup
{
    /// <summary>
    /// The standard WelcomeDialog.
    /// <para>Follows the design of the canonical Caliburn.Micro View (MVVM).</para>
    /// <para>See https://caliburnmicro.com/documentation/cheat-sheet</para>
    /// </summary>
    /// <seealso cref="WixSharp.UI.WPF.WpfDialog" />
    /// <seealso cref="WixSharp.IWpfDialog" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class RequirementsDialog : WpfDialog, IWpfDialog, IDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WelcomeDialog" /> class.
        /// </summary>
        public RequirementsDialog()
        {
            InitializeComponent();
            List<string> matches = new List<string>();
            CheckList(ref matches);
            listView.ItemsSource = matches;
        }

        /// <summary>
        /// This method is invoked by WixSHarp runtime when the custom dialog content is internally fully initialized.
        /// This is a convenient place to do further initialization activities (e.g. localization).
        /// </summary>
        public void Init()
        {
            ViewModelBinder.Bind(new RequirementDialogModel { Host = ManagedFormHost }, this, null);
        }

        public void CheckList(ref List<string> matches)
        {
            string registry_key = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

            Microsoft.Win32.RegistryKey sub_key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registry_key);



            foreach (var skname in sub_key.GetSubKeyNames())
            {

                Microsoft.Win32.RegistryKey productKey = sub_key.OpenSubKey(skname);
                if (productKey != null)
                {
                    string programName = Convert.ToString(productKey.GetValue("DisplayName"));

                    if (programName.Contains("Microsoft .NET Runtime"))
                    {
                        matches.Add(programName);
                    }
                }

            }

        }
    }

    /// <summary>
    /// ViewModel for standard WelcomeDialog.
    /// <para>Follows the design of the canonical Caliburn.Micro ViewModel (MVVM).</para>
    /// <para>See https://caliburnmicro.com/documentation/cheat-sheet</para>
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Screen" />
    internal class RequirementDialogModel : Caliburn.Micro.Screen
    {
        public ManagedForm Host;
        ISession session => Host?.Runtime.Session;
        IManagedUIShell shell => Host?.Shell;
        public List<string> matches = new List<string>();

        public BitmapImage Banner => session?.GetResourceBitmap("WixUI_Bmp_Dialog").ToImageSource();

        

        public bool CanGoNext() {
            return false;
        }

        public void GoPrev()
            => shell?.GoPrev();

        public void GoNext()
            => shell?.GoNext();

        public void Cancel()
            => shell?.Cancel();

    }
}