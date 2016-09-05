using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WardriveAnalyse
{






        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
    public partial class MainWindow : Window
    {
        private static List<Sample> allSamples;
        private static List<Sample> WPA2Samples;
        private static List<Sample> WEPSamples;
        private static List<Sample> WPASamples;
        private static List<Sample> OpenSamples;




        public MainWindow()
        {
            InitializeComponent();
        }

        protected void loadXml()
        {
            var xml = new System.Xml.XmlDocument();
            var fs = new System.IO.FileStream("wardrive.xml", System.IO.FileMode.Open);
            xml.Load(fs);
            allSamples = new List<Sample>();
            WPA2Samples = new List<Sample>();
            WEPSamples = new List<Sample>();
            WPASamples = new List<Sample>();
            OpenSamples = new List<Sample>();
            string[] stringSeparators = new string[] { "<br/>" };
            string[] descrSplit;
     
            var list = xml.GetElementsByTagName("Placemark");
            for (var i = 0; i < list.Count; i++)
            {
                var tempSample = new Sample();
                var descr = xml.GetElementsByTagName("description")[i].InnerText;

                descrSplit = descr.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                tempSample.ssid = Regex.Replace(descrSplit[0], @"<[^>]+>|&nbsp;", "").Trim();
                tempSample.ssid = tempSample.ssid.Replace("SSID: ", "");
                tempSample.bssid = Regex.Replace(descrSplit[1], @"<[^>]+>|&nbsp;", "").Trim();
                tempSample.bssid = tempSample.bssid.Replace("BSSID: ", "");
                tempSample.crypt = Regex.Replace(descrSplit[2], @"<[^>]+>|&nbsp;", "").Trim();
                tempSample.crypt = tempSample.crypt.Replace("Crypt: ", "");
                if (tempSample.crypt == "WPA2") { WPA2Samples.Add(tempSample); }
                else if (tempSample.crypt == "WpaPsk") { WPASamples.Add(tempSample); }
                else if (tempSample.crypt == "Wep") { WEPSamples.Add(tempSample); }
                else if (tempSample.crypt == "Open") { OpenSamples.Add(tempSample); }
                tempSample.channel = Regex.Replace(descrSplit[3], @"<[^>]+>|&nbsp;", "").Trim();
                tempSample.channel = tempSample.channel.Replace("Channel: ", "");
                tempSample.level = Regex.Replace(descrSplit[4], @"<[^>]+>|&nbsp;", "").Trim();
                tempSample.level = tempSample.level.Replace("Level: ", "");
                tempSample.lastupdate = Regex.Replace(descrSplit[5], @"<[^>]+>|&nbsp;", "").Trim();
                tempSample.lastupdate = tempSample.lastupdate.Replace("Last Update: ", "");
                tempSample.style = xml.GetElementsByTagName("styleUrl")[i].InnerText;
                tempSample.coord = xml.GetElementsByTagName("coordinates")[i].InnerText;
                allSamples.Add(tempSample);
            }
            textBlock1.Text = ""+list.Count+" samples";
            textBlock1.Text += "\nWPA2: " + WPA2Samples.Count + " samples ("+WPA2Samples.Count/(list.Count/100)+"%)";
            textBlock1.Text += "\nWPA: " + WPASamples.Count + " samples (" + WPASamples.Count / (list.Count / 100) + "%)";
            textBlock1.Text += "\nWEP: " + WEPSamples.Count + " samples (" + WEPSamples.Count / (list.Count / 100) + "%)";
            textBlock1.Text += "\nOpen: " + OpenSamples.Count + " samples (" + OpenSamples.Count / (list.Count / 100) + "%)";
            fs.Close();
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            loadXml();
            listBox1.ItemsSource = allSamples;
        }
    }
}
