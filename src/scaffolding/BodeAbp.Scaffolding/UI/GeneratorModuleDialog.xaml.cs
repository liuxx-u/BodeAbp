using BodeAbp.Scaffolding.UI.Base;
using System;


namespace BodeAbp.Scaffolding.UI
{
    /// <summary>
    /// Interaction logic for WebFormsScaffolderDialog.xaml
    /// </summary>
    internal partial class GeneratorModuleDialog : VSPlatformDialogWindow
    {
        public GeneratorModuleDialog(GeneratorModuleViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
            
            InitializeComponent();
            viewModel.Close += result => DialogResult = result;

            DataContext = viewModel;
        }
    }
}
