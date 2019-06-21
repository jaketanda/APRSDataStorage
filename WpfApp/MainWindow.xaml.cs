using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Library;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        Excel excel;             // excel instance
        private string fileName; // name of the file
        private System.Timers.Timer MinuteTimer; // timer for reloading data
        private int row;         // excel row
        private string callsign; // callsign

        public MainWindow()
        {
            InitializeComponent();
            APIHelper.InitializeClient();

            //placeholder file name
            fileName = "null";
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                lblGenerating.Content = "Generating!";
            });
            // start the row at 1.
            row = 1;
            // set both filename and callsign to the inputted callsign.
            callsign = txtCallsign.Text;
            fileName = txtCallsign.Text;

            // if minutetimer exists, stop it
            if (MinuteTimer != null)
            {
                MinuteTimer.Stop();
                MinuteTimer.Dispose();
                MinuteTimer = null;
            }
            // if the excel file exists, end it.
            if (excel != null)
            {
                excel.SaveAs(@"" + fileName);
                excel.Close();
                excel = null;
            }

            excel = new Excel(); // create new instance of excel
            excel.CreateNewFile(); // create new excel file
            AddHeaders(); // add the headers to each category in the excel file
            SetTimer();  // start the timer 
        }

        // Start the timer for one minute
        private void SetTimer()
        {
            RecordData();
            MinuteTimer = new System.Timers.Timer(60000);
            MinuteTimer.Elapsed += RecordData; // every time the timer reaches 0, record the data at that time
            MinuteTimer.AutoReset = true;
            MinuteTimer.Enabled = true;
        }

        // two separate recorddata functions, one for the start, and one for every time the timer hits 0.
        private void RecordData()
        {
            labelRefresh();
            LoadData();
            excel.SaveAs(@"" + fileName);
        }
        private void RecordData(object sender, ElapsedEventArgs e)
        {
            labelRefresh();
            LoadData();
            excel.Save();
        }

        private async void LoadData()
        {
            DataModel.Rootobject rootDataInfo = new DataModel.Rootobject();

            // load location data
            rootDataInfo = await DataProcessor.LoadData(callsign, "loc");

            // check for errors
            if (rootDataInfo.result == "fail")
            {
                System.Windows.MessageBox.Show("Error loading callsign");
                MinuteTimer.Stop();
                MinuteTimer.Dispose();
                MinuteTimer = null;
                return;
            }

            // check if the entries are empty (no data for location stuff)
            if (rootDataInfo.entries.Length > 0 && excel != null)
            {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(rootDataInfo.entries[0].lasttime)).ToLocalTime();

                // print location data
                this.Dispatcher.Invoke(() =>
                {
                    lblLat.Content = rootDataInfo.entries[0].lat;
                    lblLng.Content = rootDataInfo.entries[0].lng;
                    lblAlt.Content = rootDataInfo.entries[0].altitude;
                    lblCom.Content = rootDataInfo.entries[0].comment;
                    lblTime.Content = dtDateTime.ToString();
                });

                // store location data in excel file
                excel.WriteToCell(row, 0, callsign);
                excel.WriteToCell(row, 1, dtDateTime.ToString());
                excel.WriteToCell(row, 2, rootDataInfo.entries[0].lat);
                excel.WriteToCell(row, 3, rootDataInfo.entries[0].lng);
                excel.WriteToCell(row, 4, rootDataInfo.entries[0].altitude);
                excel.WriteToCell(row, 10, rootDataInfo.entries[0].comment);
            }

            // load weather data

            rootDataInfo = await DataProcessor.LoadData(callsign, "wx");

            // check for errors
            if (rootDataInfo.result == "fail")
            {
                System.Windows.MessageBox.Show("Error loading callsign");
                MinuteTimer.Stop();
                MinuteTimer.Dispose();
                MinuteTimer = null;
                return;
            }

            // check if the entries are empty (no data for weather stuff)
            if (rootDataInfo.entries.Length > 0 && excel != null)
            {
                // print weather data
                this.Dispatcher.Invoke(() =>
                {
                    lblTemp.Content = rootDataInfo.entries[0].temp;
                    lblHum.Content = rootDataInfo.entries[0].humidity;
                    lblPres.Content = rootDataInfo.entries[0].pressure;
                    lblWindS.Content = rootDataInfo.entries[0].wind_speed;
                    lblWindD.Content = rootDataInfo.entries[0].wind_direction;
                });

                // store weather data in excel file
                excel.WriteToCell(row, 5, rootDataInfo.entries[0].temp);
                excel.WriteToCell(row, 6, rootDataInfo.entries[0].humidity);
                excel.WriteToCell(row, 7, rootDataInfo.entries[0].pressure);
                excel.WriteToCell(row, 8, rootDataInfo.entries[0].wind_speed);
                excel.WriteToCell(row, 9, rootDataInfo.entries[0].wind_direction);
            }

            row++;
        }

        // stop everything when the stop button is pressed.
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                lblGenerating.Content = "Not generating data...";
            });

            if (excel != null)
            {
                excel.Save();
                excel.Close();
                excel = null;
            }

            if (MinuteTimer != null)
            {
                MinuteTimer.Stop();
                MinuteTimer.Dispose();
                MinuteTimer = null;
            }
        }

        // headers for the categories in the excel file
        private void AddHeaders()
        {
            excel.WriteToCell(0, 0, "Callsign");
            excel.WriteToCell(0, 1, "Time");
            excel.WriteToCell(0, 2, "Latitude (deg)");
            excel.WriteToCell(0, 3, "Longitude (deg)");
            excel.WriteToCell(0, 4, "Altitude (m)");
            excel.WriteToCell(0, 5, "Temperature (C)");
            excel.WriteToCell(0, 6, "Humidity (%)");
            excel.WriteToCell(0, 7, "Pressure (mBars)");
            excel.WriteToCell(0, 8, "Wind Speed (m/s)");
            excel.WriteToCell(0, 9, "Wind Direction (deg)");
            excel.WriteToCell(0, 10, "Comment");
        }

        // refreshes the label on the main window
        private void labelRefresh()
        {
            this.Dispatcher.Invoke(() =>
            {
                lblAlt.Content = "";
                lblCom.Content = "";
                lblHum.Content = "";
                lblLat.Content = "";
                lblLng.Content = "";
                lblPres.Content = "";
                lblTemp.Content = "";
                lblWindD.Content = "";
                lblWindS.Content = "";
            });
        }
    }
}
