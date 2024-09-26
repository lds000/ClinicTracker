﻿using System;
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
using HWND = System.IntPtr;

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

            foreach (var tmpwin in GetOpenWindows())
            {
                if (tmpwin.Value.Contains("eCW"))
                {
                    MessageBox.Show(tmpwin.Value);
                }
            } // end foreach (var tmpwin in GetOpenWindows()


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

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<HWND, string> GetOpenWindows()
        {
            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();

            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                if (hWnd == shellWindow)
                    return true;
                if (!IsWindowVisible(hWnd))
                    return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0)
                    return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);

                windows[hWnd] = builder.ToString();
                return true;

            }, 0);

            return windows;
        }

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);


        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();

    }
}