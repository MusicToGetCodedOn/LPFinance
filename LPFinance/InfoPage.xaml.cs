namespace LPFinance;

public partial class InfoPage : ContentPage
{
    public InfoPage()
    {
        InitializeComponent();
    }

   
    private void ToggleFaq1(object sender, EventArgs e)
    {
        FaqAnswer1.IsVisible = !FaqAnswer1.IsVisible;
    }

    
    private void ToggleFaq2(object sender, EventArgs e)
    {
        FaqAnswer2.IsVisible = !FaqAnswer2.IsVisible;
    }

   
    private async void OnLinkTapped(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://www.google.ch"); 
    }
}