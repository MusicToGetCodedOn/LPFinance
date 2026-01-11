namespace LPFinance;

public partial class InfoPage : ContentPage
{
    public InfoPage()
    {
        InitializeComponent();
    }

    // FAQ Toggle Logik
    private void ToggleFaq1(object sender, EventArgs e) => FaqAnswer1.IsVisible = !FaqAnswer1.IsVisible;
    private void ToggleFaq2(object sender, EventArgs e) => FaqAnswer2.IsVisible = !FaqAnswer2.IsVisible;
    private void ToggleFaq3(object sender, EventArgs e) => FaqAnswer3.IsVisible = !FaqAnswer3.IsVisible;

    // Externer Link (Webseite)
    private async void OnLinkTapped(object sender, EventArgs e)
    {
        await Launcher.Default.OpenAsync("https://www.google.ch");
    }

    // E-Mail Handler (Support)
    private async void OnEmailTapped(object sender, EventArgs e)
    {
        if (Email.Default.IsComposeSupported)
        {
            var message = new EmailMessage
            {
                Subject = "Supportanfrage LPFinance",
                Body = "Guten Tag, ich habe eine Frage zu...",
                To = new List<string> { "support@lpfinance.ch" }
            };
            await Email.Default.ComposeAsync(message);
        }
    }
}