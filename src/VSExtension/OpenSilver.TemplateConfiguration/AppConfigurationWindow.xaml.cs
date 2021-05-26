using System;
using System.Windows;

namespace OpenSilver.TemplateConfiguration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AppConfigurationWindow : Window
    {
        public string OpenSilverBuildType
        {
            get
            {
                switch (BuildTypeComboBox.SelectedIndex)
                {
                    case 0:
                        return "stable";
                    case 1:
                        return "workinprogress";
                    default:
                        throw new InvalidOperationException("Error retrieving selected OpenSilver build type");
                }
            }
        }

        public string BlazorVersion
        {
            get
            {
                switch (BlazorVersionComboBox.SelectedIndex)
                {
                    case 0:
                        return "netcore3.1";
                    case 1:
                        return "net5";
                    case 2:
                        return "net6";
                    default:
                        throw new InvalidOperationException("Error retrieving selected blazor version");
                }
            }
        }

        public AppConfigurationWindow()
        {
            InitializeComponent();
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
