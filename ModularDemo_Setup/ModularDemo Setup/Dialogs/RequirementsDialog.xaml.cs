using Caliburn.Micro;
using ModularDemo_Setup.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using WixSharp;
using WixSharp.UI.Forms;
using WixSharp.UI.WPF;
using ModularDemo_Setup.NewFolder;

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
            //DataContext = new RequirementsViewModel();
            InitializeComponent();
        }

        /// <summary>
        /// This method is invoked by WixSHarp runtime when the custom dialog content is internally fully initialized.
        /// This is a convenient place to do further initialization activities (e.g. localization).
        /// </summary>
        public void Init()
        {
            ViewModelBinder.Bind(new RequirementDialogModel { Host = ManagedFormHost }, this, null);
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

        public BitmapImage Banner => session?.GetResourceBitmap("WixUI_Bmp_Dialog").ToImageSource();

        /// <summary>
        /// Listado de programas requeridos para la instalacion
        /// </summary>
        private BindableCollection<ItemProgram> _programList = new BindableCollection<ItemProgram>();

        public BindableCollection<ItemProgram> ProgramList
        {
            get { return _programList; }
            set { _programList = value; }
        }

        private string _programName;

        public string ProgramName
        {
            get { return _programName; }
            set { _programName = value; }
        }

        private bool _programInstalled;
        public bool ProgramInstalled
        {
            get { return _programInstalled; }
            set { 
                _programInstalled = value;
                NotifyOfPropertyChange(nameof(ProgramInstalled));
            }
        }

        /// <summary>
        /// Inicializando el listado de los programas requeridos
        /// </summary>
        /// <param name="view"></param>
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ProgramList.Add(new ItemProgram { Name = "Microsoft .NET Runtime", IsInstalled = false });
            ProgramList.Add(new ItemProgram { Name = "Microsoft Visual Studio", IsInstalled = false });
            ProgramList.Add(new ItemProgram { Name = "GitHub Desktop", IsInstalled = false });
            CheckProgramsInstalled();

            //ItemProgram x =new ItemProgram();
            //var type = x.GetType();
            //var property = type.GetProperties(System.Reflection.BindingFlags.Public);
            //property[0].
        }

        protected void CheckProgramsInstalled()
        {
            foreach (var item in ProgramList)
            {
                string registry_key = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
                Microsoft.Win32.RegistryKey sub_key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registry_key);

                foreach (var skname in sub_key.GetSubKeyNames())
                {

                    Microsoft.Win32.RegistryKey productKey = sub_key.OpenSubKey(skname);
                    if (productKey != null)
                    {
                        string programName = Convert.ToString(productKey.GetValue("DisplayName"));

                        if (programName.Contains(item.Name))
                        {
                            item.IsInstalled = true;
                        }
                    }

                }
            }
        }


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