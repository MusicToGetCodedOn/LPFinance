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
            // Schließt das Menü und navigiert zur Info-Seite
            Shell.Current.FlyoutIsPresented = false;
            await Navigation.PushAsync(new InfoPage());
        }
    }
}
