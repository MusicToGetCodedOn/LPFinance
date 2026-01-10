namespace LPFinance;

public partial class AddBookingPage : ContentPage
{
    public AddBookingPage()
    {
        InitializeComponent();
        SollPicker.ItemsSource = DataService.AllAccounts;
        HabenPicker.ItemsSource = DataService.AllAccounts;
        JournalList.ItemsSource = DataService.AllBookings;
    }

    // --- ECHTZEIT-VALIDIERUNG ---

    private void OnAmountTextChanged(object sender, TextChangedEventArgs e)
    {
        ValidateAmount();
    }

    private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e)
    {
        ValidateDescription();
    }

    private void OnSollSelectedIndexChanged(object sender, EventArgs e)
    {
        ValidateSoll();
    }

    private void OnHabenSelectedIndexChanged(object sender, EventArgs e)
    {
        ValidateHaben();
    }

    // --- VALIDIERUNGSLOGIK ---

    private bool ValidateAmount()
    {
        bool isValid = double.TryParse(AmountEntry.Text, out double val) && val > 0;
        AmountBorder.Stroke = isValid ? Colors.Transparent : Colors.Red;
        AmountErrorLabel.IsVisible = !isValid;
        return isValid;
    }

    private bool ValidateDescription()
    {
        bool isValid = !string.IsNullOrWhiteSpace(DescriptionEntry.Text);
        DescBorder.Stroke = isValid ? Colors.Transparent : Colors.Red;
        DescErrorLabel.IsVisible = !isValid;
        return isValid;
    }

    private bool ValidateSoll()
    {
        bool isValid = SollPicker.SelectedIndex != -1;
        SollBorder.Stroke = isValid ? Colors.Transparent : Colors.Red;
        SollErrorLabel.IsVisible = !isValid;
        return isValid;
    }

    private bool ValidateHaben()
    {
        bool isValid = HabenPicker.SelectedIndex != -1;
        HabenBorder.Stroke = isValid ? Colors.Transparent : Colors.Red;
        HabenErrorLabel.IsVisible = !isValid;
        return isValid;
    }

    // --- SPEICHERN ---

    private async void OnSaveBookingClicked(object sender, EventArgs e)
    {
        // Alle Felder noch einmal prüfen
        bool a = ValidateAmount();
        bool b = ValidateDescription();
        bool c = ValidateSoll();
        bool d = ValidateHaben();

        if (!a || !b || !c || !d) return;

        var soll = SollPicker.SelectedItem as AccountItem;
        var haben = HabenPicker.SelectedItem as AccountItem;

        // Spezialregeln
        if (soll.Typ == "Ertrag") { await DisplayAlert("Fehler", "Ertrag gehört ins Haben!", "OK"); return; }
        if (haben.Typ == "Aufwand") { await DisplayAlert("Fehler", "Aufwand gehört ins Soll!", "OK"); return; }
        if (soll.Name == haben.Name) { await DisplayAlert("Fehler", "Soll und Haben dürfen nicht gleich sein!", "OK"); return; }

        DataService.AllBookings.Add(new BookingItem
        {
            Text = DescriptionEntry.Text,
            Amount = double.Parse(AmountEntry.Text),
            SollKonto = soll.Name,
            HabenKonto = haben.Name
        });

        ResetForm();
    }

    private void ResetForm()
    {
        // Events kurz deaktivieren, um keine Validierung beim Leeren auszulösen
        DescriptionEntry.Text = string.Empty;
        AmountEntry.Text = string.Empty;
        SollPicker.SelectedIndex = -1;
        HabenPicker.SelectedIndex = -1;

        // UI aufräumen
        AmountBorder.Stroke = Colors.Transparent;
        SollBorder.Stroke = Colors.Transparent;
        HabenBorder.Stroke = Colors.Transparent;
        DescBorder.Stroke = Colors.Transparent;

        AmountErrorLabel.IsVisible = false;
        SollErrorLabel.IsVisible = false;
        HabenErrorLabel.IsVisible = false;
        DescErrorLabel.IsVisible = false;
    }
}