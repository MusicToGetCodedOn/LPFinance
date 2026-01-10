namespace LPFinance
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            
            Shell.Current.FlyoutIsPresented = false;
            await Navigation.PushAsync(new InfoPage());
        }
    }
}
