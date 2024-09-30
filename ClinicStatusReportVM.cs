using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ClinicTracker
{
    class ClinicStatusReportVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _clinicName = string.Empty;
        /// <summary>
        /// The name of the clinic.
        /// </summary>
        public string ClinicName
        {
            get
            {
                return _clinicName;
            }
            set
            {
                _clinicName = value;
                OnPropertyChanged(nameof(ClinicName));
            }
        }

        /// <summary>
        /// DateTime Clinic Date that was captured from the screen capture.
        /// </summary>

        private DateTime _clinicDate = DateTime.Now;
        public DateTime ClinicDate
        {
            get
            {
                return _clinicDate;
            }
            set
            {
                _clinicDate = value;
                OnPropertyChanged(nameof(ClinicDate));
            }
        }

        /// <summary>
        /// How many hours the clinic has been open (after 8:00 AM) today.
        /// </summary>
        public double HoursOpen
        {
            get
            {
                //double that represents the hours elapsed since 8:00 AM today
                return (DateTime.Now - DateTime.Parse("8:00 AM")).TotalHours;
            }
        }

        /// <summary>
        /// String that contains the PatientsSeen, PatientsWaiting, and PatientsScheduled properties.  It is updated when any of those properties are updated.
        /// </summary>

        public string ClinicStatus
        {
            get
            {
                return $"{PatientsSeen} ({PatientsWaiting} Waiting {PatientsScheduled} Online Scheduled)";
            }
        }

        private int _patientsSeen = 0;
        /// <summary>
        /// How many patients have been seen today.
        /// </summary>
        public int PatientsSeen
        {
            get
            {
                return _patientsSeen;
            }
            set
            {
                _patientsSeen = value;
                OnPropertyChanged(nameof(PatientsSeen));
                OnPropertyChanged(nameof(ScheduledAndSeenCount));
                OnPropertyChanged(nameof(ClinicStatus));
            }
        }

        private int _patientsWaiting = 0;
        /// <summary>
        /// How many patients are waiting to be seen.
        /// </summary>
        public int PatientsWaiting
        {
            get
            {
                return _patientsWaiting;
            }
            set
            {
                _patientsWaiting = value;
                OnPropertyChanged(nameof(PatientsWaiting));
                OnPropertyChanged(nameof(ScheduledAndSeenCount));
                OnPropertyChanged(nameof(ClinicStatus));
            }
        }

        /// <summary>
        /// property that returns the sum of PatientsSeen and PatientsWaiting and is updated when either of those properties are updated.
        /// </summary>

        private int _scheduledAndSeenCount = 0;
        public int ScheduledAndSeenCount
        {
            get
            {
                return _scheduledAndSeenCount;
            }
            set
            {
                _scheduledAndSeenCount = value;
                OnPropertyChanged(nameof(ScheduledAndSeenCount));
            }
        }

        private int _patientsScheduled = 0;
        /// <summary>
        /// How many patients are scheduled to be seen today.
        /// </summary>
        public int PatientsScheduled
        {
            get
            {
                return _patientsScheduled;
            }
            set
            {
                _patientsScheduled = value;
                OnPropertyChanged(nameof(PatientsScheduled));
                OnPropertyChanged(nameof(ClinicStatus));
            }
        }

        private int _nonVisitPatientsSeen = 0;
        /// <summary>
        /// How many non-Visit patients have been seen today.
        /// </summary>
        public int NonVisitPatientsSeen
        {
            get
            {
                return _nonVisitPatientsSeen;
            }
            set
            {
                _nonVisitPatientsSeen = value;
                OnPropertyChanged(nameof(NonVisitPatientsSeen));
            }
        }

        /// <summary>
        /// How many Drug Screen patients have been seen today.
        /// </summary>

        private int _drugScreenVisitCount = 0;
        public int DrugScreenVisitCount
        {
            get
            {
                return _drugScreenVisitCount;
            }
            set
            {
                _drugScreenVisitCount = value;
                OnPropertyChanged(nameof(DrugScreenVisitCount));
            }
        }

        private int _vaxVisitCount = 0;
        public int VaxVisitCount
        {
            get
            {
                return _vaxVisitCount;
            }
            set
            {
                _vaxVisitCount = value;
                OnPropertyChanged(nameof(VaxVisitCount));
            }
        }

        /// <summary>
        /// Property that returns the a string of a decimal number representing the number of hours that have passed since 8:00 this morning
        /// </summary>
        public string HoursOpenString
        {
            get
            {
                return HoursOpen.ToString("0.0");
            }
        }


        private int[] visitHistogramData = new int[12];
        private int[] VisitHistogramData
        {
            get
            {
                return visitHistogramData;
            }
            set
            {
                visitHistogramData = value;
                OnPropertyChanged(nameof(VisitHistogramData));
            }
        }

        private int[] scheduledHistogramData = new int[12];
        private int[] ScheduledHistogramData
        {
            get
            {
                return scheduledHistogramData;
            }
            set
            {
                scheduledHistogramData = value;
                OnPropertyChanged(nameof(ScheduledHistogramData));
            }
        }


        private int _patientsSeenLast2Hours = 0;
        /// <summary>
        /// How many patients have been seen the last 2 hours.
        /// </summary>
        public int PatientsSeenLast2Hours
        {
            get
            {
                return _patientsSeenLast2Hours;
            }
            set
            {
                _patientsSeenLast2Hours = value;
                OnPropertyChanged(nameof(PatientsSeenLast2Hours));
            }
        }

        /// <summary>
        /// How many patients have been seen the last 2 hours.
        /// </summary>
        public int ProjectedNumber
        {
            get
            {
                if (HoursOpen < 1)
                    return 0;
                var tmpcount = PatientsSeen * 12 / HoursOpen;
                return (int)Math.Round(tmpcount);
            }
        }

        private string _clinicScheduleRawString = string.Empty;
        /// <summary>
        /// A string that contains the raw text from a screen capture of the clinic schedule for today.
        /// </summary>
        public string ClinicScheduleRawString
        {
            get
            {
                return _clinicScheduleRawString;
            }
            set
            {
                _clinicScheduleRawString = value;
                PatientsWaiting = 0;
                PatientsSeen = 0;
                NonVisitPatientsSeen = 0;
                DrugScreenVisitCount = 0;
                PatientsSeenLast2Hours = 0;
                PatientsScheduled = 0;
                VaxVisitCount = 0;
                VisitHistogramData = new int[12];
                ScheduledHistogramData = new int[12];


                // Parse the raw string to get the clinic name, date, hours open, patients seen, patients waiting, patients scheduled, non-visit patients seen, and patients seen last 2 hours.
                using (StringReader reader = new StringReader(_clinicScheduleRawString))
                {
                    string strLine;
                    while ((strLine = reader.ReadLine()) != null)
                    {
                        // Get the clinic visit date.
                        if (strLine == " Office Visits")
                        {
                            reader.ReadLine();
                            string dateLine = reader.ReadLine();
                            if (!string.IsNullOrEmpty(dateLine))
                            {
                                ClinicDate = DateTime.Parse(dateLine);
                            }
                        }
                        if (strLine == "Facility")
                        {
                            ClinicName = reader.ReadLine() ?? string.Empty;
                        }

                        string sep = "\t";
                        string[] splitContent = strLine.Split(sep.ToCharArray());

                        /// if the line has 11 columns, it is a visit line
                        if (splitContent.Length == 11)
                        {
                            /// if the first column contains "NON", it is a non-visit line
                            if (splitContent[0] == "NON")
                            /// if the 6th column contains "OH DS", it is also a drug screen
                            {
                                if (splitContent[5].ToLower() == "oh ds")
                                    DrugScreenVisitCount++;
                                else
                                {
                                    NonVisitPatientsSeen++;
                                }

                            }


                            if (splitContent[0] == "VAX")

                                VaxVisitCount++;


                            /// if the first column contains "UC", it is a UC visit line
                            if (splitContent[0].StartsWith("UC"))
                            {
                                if (splitContent[8] == "ARR")
                                {
                                    PatientsWaiting++;
                                }
                                DateTime timeSeen = DateTime.Parse(splitContent[1]);
                                TimeSpan tAfter8 = timeSeen - DateTime.Parse("8:00 AM");
                                if (splitContent[8] == "PEN")
                                {
                                    for (int i = 0; i <= 11; i++)
                                    {
                                        if ((tAfter8.TotalMinutes >= i * 60) && tAfter8.TotalMinutes < (i + 1) * 60)
                                        {
                                            Console.WriteLine(splitContent[1] + " added to " + i + "\n");
                                            ScheduledHistogramData[i]++;
                                        }
                                    }
                                    PatientsScheduled++;
                                }
                                if (splitContent[8] == "CHK" || splitContent[8] == "ARR")
                                {

                                    if (splitContent[8] == "CHK")
                                    {
                                        PatientsSeen++;
                                    }
                                    TimeSpan t = DateTime.Now - timeSeen;
                                    if (t.TotalMinutes <= 120)
                                        PatientsSeenLast2Hours++;
                                    if (tAfter8.TotalMinutes < 0)
                                        VisitHistogramData[0]++;
                                    for (int i = 0; i <= 11; i++)
                                    {
                                        if ((tAfter8.TotalMinutes >= i * 60) && tAfter8.TotalMinutes < (i + 1) * 60)
                                        {
                                            Console.WriteLine(splitContent[1] + " added to " + i + "\n");
                                            VisitHistogramData[i]++;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                OnPropertyChanged(nameof(HoursOpenString));
                OnPropertyChanged(nameof(ClinicScheduleRawString));
                OnPropertyChanged(nameof(HistoPanel));
                OnPropertyChanged(nameof(ProjectedNumber));
            }
        }

        public StackPanel HistoPanel
        {
            get
            {
                StackPanel spHisto = new StackPanel();
                spHisto.Orientation = Orientation.Horizontal;
                for (int i = 0; i <= 11; i++)
                {
                    Rectangle myRect = new Rectangle();
                    myRect.Stroke = System.Windows.Media.Brushes.Black;
                    myRect.Fill = System.Windows.Media.Brushes.Lime;
                    if (VisitHistogramData[i] >= 4)
                        myRect.Fill = System.Windows.Media.Brushes.Yellow;
                    if (VisitHistogramData[i] >= 7)
                        myRect.Fill = System.Windows.Media.Brushes.Red;
                    myRect.HorizontalAlignment = HorizontalAlignment.Left;
                    myRect.VerticalAlignment = VerticalAlignment.Bottom;
                    myRect.Height = 10 * VisitHistogramData[i];
                    myRect.Width = 50;
                    myRect.RadiusX = 5;
                    myRect.RadiusY = 5;

                    Rectangle myRect2 = new Rectangle();
                    myRect2.Stroke = System.Windows.Media.Brushes.Black;
                    myRect2.Fill = System.Windows.Media.Brushes.Gray;
                    myRect2.HorizontalAlignment = HorizontalAlignment.Left;
                    myRect2.VerticalAlignment = VerticalAlignment.Bottom;
                    myRect2.Height = 10 * ScheduledHistogramData[i];
                    myRect2.Width = 50;
                    myRect2.RadiusX = 5;
                    myRect2.RadiusY = 5;


                    StackPanel tmpSP = new StackPanel();
                    tmpSP.Orientation = Orientation.Vertical;
                    tmpSP.VerticalAlignment = VerticalAlignment.Bottom;
                    tmpSP.HorizontalAlignment = HorizontalAlignment.Center;

                    string lampm = "AM";

                    StackPanel sptimelbl = new StackPanel();

                    Label l = new Label();
                    l.Padding = new Thickness(0);
                    l.Margin = new Thickness(0);
                    int tmpt = i + 8;
                    if (tmpt > 12)
                    {
                        lampm = "PM";
                        tmpt = tmpt - 12;
                    }
                    l.Content = tmpt;
                    l.Background = System.Windows.Media.Brushes.Black;
                    l.Foreground = System.Windows.Media.Brushes.White;
                    l.HorizontalAlignment = HorizontalAlignment.Left;

                    l.FontSize = 20;

                    Label l2 = new Label();
                    l2.Margin = new Thickness(0);
                    l2.Padding = new Thickness(0);
                    l2.Content = lampm;
                    l2.Background = System.Windows.Media.Brushes.Black;
                    l2.Foreground = System.Windows.Media.Brushes.White;
                    l2.HorizontalAlignment = HorizontalAlignment.Right;

                    l2.FontSize = 10;


                    sptimelbl.Orientation = Orientation.Horizontal;
                    sptimelbl.HorizontalAlignment = HorizontalAlignment.Center;
                    sptimelbl.Children.Add(l);
                    sptimelbl.Children.Add(l2);
                    sptimelbl.Margin = new Thickness(0);

                    tmpSP.Children.Add(myRect2);
                    tmpSP.Children.Add(myRect);
                    tmpSP.Children.Add(sptimelbl);

                    spHisto.Children.Add(tmpSP);

                }
                return spHisto;

            }
        }
    }
}
