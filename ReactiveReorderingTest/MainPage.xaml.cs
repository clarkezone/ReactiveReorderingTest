using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using ReactiveReorderingTest.DataModel;
using ReactiveReorderingTest.Services;
using ReactiveReorderingTest.ViewModels;
using Realms;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ReactiveReorderingTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPageViewmodel ViewModel { get; set; }

        public MainPage()
        {
            ViewModel = new MainPageViewmodel();

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
        }

        private void ListView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            ViewModel.EndReorder();
        }

        private void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            ViewModel.BeginReorder();
        }
    }
}
