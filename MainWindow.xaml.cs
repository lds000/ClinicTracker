using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoIt;

namespace ClinicTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set up the view model
            ClinicStatusReportVM vm = new ClinicStatusReportVM();

            //load vm.ClinicScheduleRawString with the contents of the file
            vm.ClinicScheduleRawString = File.ReadAllText("SampleScreenCaptureText.txt");
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_GetData(object sender, RoutedEventArgs e)
        {
            string str = AutoIt.AutoItX.WinGetText("eCW");
           MessageBox.Show(str);
        }

    }
}