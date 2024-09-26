using System;
using System.IO;
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
            // capture the window content text from the screen starting with window title "eCW"

            // get the window handle of the eCW window
            IntPtr eCWWindowHandle = FindWindow(null, "eCW");

            // check if the window handle is valid
            if (eCWWindowHandle != IntPtr.Zero)
            {
                // get the window content text
                StringBuilder sb = new StringBuilder(256);
                GetWindowText(eCWWindowHandle, sb, sb.Capacity);
                string eCWWindowContentText = sb.ToString();

                // process the window content text
                // ...
            }
            else
            {
                // handle the case when the window is not found
                MessageBox.Show("eCW window not found.");
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    }
}