namespace LPFinance;

public partial class AddBookingPage : ContentPage
{
    public AddBookingPage()
    {
        InitializeComponent();
        SollPicker.ItemsSource = DataService.AllAccounts;
        HabenPicker.ItemsSource = DataService.AllAccounts;
        JournalList.ItemsSource = DataService.AllBookings;
        BookingDatePicker.Date = DateTime.Now;
    }

    
    private void OnAmountTextChanged(object sender, TextChangedEventArgs e) => ValidateAmount();
    private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e) => ValidateDescription();
    private void OnSollSelectedIndexChanged(object sender, EventArgs e) => ValidateSoll();
    private void OnHabenSelectedIndexChanged(object sender, EventArgs e) => ValidateHaben();
    private void OnDateSelected(object sender, DateChangedEventArgs e) => ValidateDate();

    
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

    private bool ValidateDate()
    {
       
        bool isValid = BookingDatePicker.Date <= DateTime.Now;
        DateBorder.Stroke = isValid ? Colors.Transparent : Colors.Red;
        DateErrorLabel.IsVisible = !isValid;
        return isValid;
    }

    private async void OnSaveBookingClicked(object sender, EventArgs e)
    {
        if (!ValidateAmount() || !ValidateDescription() || !ValidateSoll() || !ValidateHaben() || !ValidateDate())
            return;

        var soll = SollPicker.SelectedItem as AccountItem;
        var haben = HabenPicker.SelectedItem as AccountItem;

        
        if (soll.Typ == "Ertrag") { await DisplayAlert("Fehler", "Ertrag gehört ins Haben!", "OK"); return; }
        if (haben.Typ == "Aufwand") { await DisplayAlert("Fehler", "Aufwand gehört ins Soll!", "OK"); return; }
        if (soll.Name == haben.Name) { await DisplayAlert("Fehler", "Soll und Haben dürfen nicht gleich sein!", "OK"); return; }

        DataService.AllBookings.Add(new BookingItem
        {
            Text = DescriptionEntry.Text,
            Amount = double.Parse(AmountEntry.Text),
            SollKonto = soll.Name,
            HabenKonto = haben.Name,
            Datum = BookingDatePicker.Date,
            IsUrgent = UrgentSwitch.IsToggled
        });

        ResetForm();
    }

    private void ResetForm()
    {
        DescriptionEntry.Text = string.Empty;
        AmountEntry.Text = string.Empty;
        SollPicker.SelectedIndex = -1;
        HabenPicker.SelectedIndex = -1;
        BookingDatePicker.Date = DateTime.Now;
        UrgentSwitch.IsToggled = false;

        
        AmountBorder.Stroke = SollBorder.Stroke = HabenBorder.Stroke = DescBorder.Stroke = DateBorder.Stroke = Colors.Transparent;
        AmountErrorLabel.IsVisible = SollErrorLabel.IsVisible = HabenErrorLabel.IsVisible = DescErrorLabel.IsVisible = DateErrorLabel.IsVisible = false;
    }
}