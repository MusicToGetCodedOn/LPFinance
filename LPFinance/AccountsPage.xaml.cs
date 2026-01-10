namespace LPFinance;

public partial class AccountsPage : ContentPage
{
    public AccountsPage()
    {
        InitializeComponent();
        AccountsListView.ItemsSource = DataService.AllAccounts;
    }

    // --- ECHTZEIT-EVENTS ---

    private void OnNameTextChanged(object sender, TextChangedEventArgs e)
    {
        ValidateName();
    }

    private void OnTypeSelectedIndexChanged(object sender, EventArgs e)
    {
        ValidateType();
    }

    // --- VALIDIERUNGSLOGIK ---

    private bool ValidateName()
    {
        bool isValid = !string.IsNullOrWhiteSpace(AccountNameEntry.Text);
        NameBorder.Stroke = isValid ? Colors.Transparent : Colors.Red;
        NameErrorLabel.IsVisible = !isValid;
        return isValid;
    }

    private bool ValidateType()
    {
        bool isValid = AccountTypePicker.SelectedIndex != -1;
        TypeBorder.Stroke = isValid ? Colors.Transparent : Colors.Red;
        TypeErrorLabel.IsVisible = !isValid;
        return isValid;
    }

    // --- SPEICHERN ---

    private async void OnSaveAccountClicked(object sender, EventArgs e)
    {
        // Beide Felder prüfen
        bool isNameValid = ValidateName();
        bool isTypeValid = ValidateType();

        if (!isNameValid || !isTypeValid) return;

        // Zum zentralen Service hinzufügen
        DataService.AllAccounts.Add(new AccountItem
        {
            Name = AccountNameEntry.Text,
            Typ = AccountTypePicker.SelectedItem.ToString()
        });

        // Formular zurücksetzen
        ResetForm();
    }

    private void ResetForm()
    {
        // Inhalt leeren
        AccountNameEntry.Text = string.Empty;
        AccountTypePicker.SelectedIndex = -1;

        // Validierungs-UI zurücksetzen
        NameBorder.Stroke = Colors.Black;
        TypeBorder.Stroke = Colors.Black;
        NameErrorLabel.IsVisible = false;
        TypeErrorLabel.IsVisible = false;
    }
}