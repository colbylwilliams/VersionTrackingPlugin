using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VersionTrackingTests
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            Plugin.VersionTracking.CrossVersionTracking.Current.Track();

            LabelVersion.Text = Plugin.VersionTracking.CrossVersionTracking.Current.CurrentVersion;
            ListViewVersions.ItemsSource = Plugin.VersionTracking.CrossVersionTracking.Current.VersionHistory;
		}
	}
}
