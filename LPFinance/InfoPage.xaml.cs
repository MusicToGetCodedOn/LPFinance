namespace LPFinance;

public partial class InfoPage : ContentPage
{
    public InfoPage()
    {
        InitializeComponent();
    }

    // Toggle-Logik für FAQ 1
    private void ToggleFaq1(object sender, EventArgs e)
    {
        FaqAnswer1.IsVisible = !FaqAnswer1.IsVisible;
    }

    // Toggle-Logik für FAQ 2
    private void ToggleFaq2(object sender, EventArgs e)
    {
        FaqAnswer2.IsVisible = !FaqAnswer2.IsVisible;
    }

    // Link-Logik
    private async void OnLinkTapped(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://www.google.ch"); // Platzhalter für deine Webseite
    }
}