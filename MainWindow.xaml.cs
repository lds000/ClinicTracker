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
using System.Diagnostics;
using System.Threading;

namespace ClinicTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ClinicStatusReportVM _ClinicStatusReportVM;

        public MainWindow()
        {
            InitializeComponent();

            // Set up the view model
            _ClinicStatusReportVM = new ClinicStatusReportVM();

            //load vm.ClinicScheduleRawString with the contents of the file
            _ClinicStatusReportVM.ClinicScheduleRawString = File.ReadAllText("SampleScreenCaptureText.txt");
            this.DataContext = _ClinicStatusReportVM;
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

            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {

                    if (process.MainWindowTitle.Contains("eCW"))
                    {
                        string strTitle = process.MainWindowTitle;
                        AutoIt.AutoItX.WinActivate(strTitle);
                        int iColorCheck = AutoIt.AutoItX.PixelGetColor(60, 150);
                        if (iColorCheck != 15333631)
                        {
                            MessageBox.Show("Please make sure eCW has the schedule pulled up.");
                            return;
                        }

                        var tmpPos = AutoIt.AutoItX.MouseGetPos();
                        AutoIt.AutoItX.MouseClick("LEFT", 60, 150, 1, 0);
                        AutoIt.AutoItX.Send("^a^c");
                        AutoIt.AutoItX.MouseMove(tmpPos.X,tmpPos.Y, 0);
                        Thread.Sleep(500);
                        AutoIt.AutoItX.MouseClick("LEFT", 60, 150, 1, 0);
                        AutoIt.AutoItX.MouseMove(tmpPos.X, tmpPos.Y, 0);
                        _ClinicStatusReportVM.ClinicScheduleRawString = AutoIt.AutoItX.ClipGet();
                    }
                }
            }
        }

    }
}