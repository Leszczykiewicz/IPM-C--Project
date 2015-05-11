using XMLexample.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using System.Text;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace XMLexample
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class BasicPage1 : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private Waluta currency;
        public BasicPage1()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            startDatePicker.MinYear = new DateTimeOffset(new DateTime(2002, 05, 01));
            startDatePicker.MaxYear = new DateTimeOffset(DateTime.Today);
            endDatePicker.MinYear = new DateTimeOffset(new DateTime(2002, 01, 01));
            endDatePicker.MaxYear = new DateTimeOffset(DateTime.Today);

        }

        

     

       

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            currency = e.Parameter as Waluta;
            pageTitle.Text = "Historia kursu " + currency.KodWaluty;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
           
        }

        #endregion

        private void exit(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void goToNewPage(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void selection_changed(object sender, SelectionChangedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage), e.AddedItems[0]);
            }
        }

     

        private void close(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.Exit();
        }


        private  double ProccedWithXML(String xml_url, String kod_waluty)
        {
            XDocument loadedXML = XDocument.Load(xml_url);
           
            var data = from query in loadedXML.Descendants("pozycja")
                       select new Waluta
                       {
                           KursSredni = (string)query.Element("kurs_sredni"),
                           KodWaluty = (string) query.Element("kod_waluty")
                       };
            
            foreach(Waluta waluta in data)
            {
                if(waluta.KodWaluty.Equals(kod_waluty))
                {
                    StringBuilder S = new StringBuilder(waluta.KursSredni);
                    S.Replace(",", ".");
                    string s = S.ToString();
                    return Convert.ToDouble(s);
                }
            }

            return 0;

        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            List<String> list = new List<String>();
            String startDate = startDatePicker.Date.ToString("yyyy-MM-dd");
            String endDate = endDatePicker.Date.ToString("yyyy-MM-dd");
            String tmpStart = startDate.Substring(2, 2) + startDate.Substring(5, 2) + startDate.Substring(8, 2);
            String tmpEnd = endDate.Substring(2, 2) + endDate.Substring(5, 2) + endDate.Substring(8, 2);
            foreach (string ss in MainPage.splitted)
            {
                if (ss.Length==0 || !ss.Substring(0, 1).Equals("a"))
                    continue;
                if (Convert.ToInt32(ss.Substring(5, 6)) > Convert.ToInt32(tmpStart) && Convert.ToInt32(ss.Substring(5, 6)) < Convert.ToInt32(tmpEnd)) //a002z020103
                {
                    list.Add(ss);
                    System.Diagnostics.Debug.WriteLine(ss);
                   
                }
            }
           


          //  System.Diagnostics.Debug.WriteLine(MainPage.datesList.ElementAt(0));
          //  System.Diagnostics.Debug.WriteLine(startDatePicker.Date.ToString("yyyy-MM-dd"));
            List<DataToChart> listToChart = new List<DataToChart>(); 
            foreach (string l in list)
            {
                progressBar.Value += 100.0 / list.Capacity;
                System.Diagnostics.Debug.WriteLine(list.Capacity);
                System.Diagnostics.Debug.WriteLine("progress: "+progressBar.Value);
                DateTime date = new DateTime(Convert.ToInt32("20" + l.Substring(5, 2)), Convert.ToInt32(l.Substring(7, 2)), Convert.ToInt32(l.Substring(9, 2)));
                double value =  ProccedWithXML(@"http://www.nbp.pl/kursy/xml/" + l + @".xml", currency.KodWaluty);
         //       System.Diagnostics.Debug.WriteLine("wartosc: " +value);
                listToChart.Add(new DataToChart(date,value));

            }
        
         //   listToChart.Add(new DataToChart(DateTime.Today, 8.7 ));
         //   listToChart.Add(new DataToChart(new DateTime(2015,5,5), 8.9 ));
         //   listToChart.Add(new DataToChart(new DateTime(2015,5,6), 10.6 ));
            (LineChart.Series[0] as LineSeries).ItemsSource = listToChart;
        }

      
     
    }
}
