using System.Collections.ObjectModel;

namespace LPFinance
{
    // Diese Klassen dürfen NUR HIER stehen!
    public class AccountItem
    {
        public string Name { get; set; }
        public string Typ { get; set; } // Aktiv, Passiv, Aufwand, Ertrag
    }

    public class BookingItem
    {
        public string Text { get; set; }
        public double Amount { get; set; }
        public string SollKonto { get; set; }
        public string HabenKonto { get; set; }
        public DateTime Datum { get; set; } 
        public bool IsUrgent { get; set; } 

        public string DisplayAccounts => $"Soll: {SollKonto} → Haben: {HabenKonto}";
        public string DisplayAmount => $"CHF {Amount:N2}";
        public string DisplayDate => Datum.ToString("dd.MM.yyyy"); 
    }
    public static class DataService
    {
        
        public static ObservableCollection<AccountItem> AllAccounts { get; } = new ObservableCollection<AccountItem>
        {
            new AccountItem { Name = "Kasse", Typ = "Aktiv" },
            new AccountItem { Name = "Bank", Typ = "Aktiv" },
            new AccountItem { Name = "Mobiliar", Typ = "Aktiv" },
            new AccountItem { Name = "Fahrzeuge", Typ = "Aktiv" },
            new AccountItem { Name = "FLL", Typ = "Aktiv" },
            new AccountItem { Name = "VLL", Typ = "Passiv" },
            new AccountItem { Name = "Eigenkapital", Typ = "Passiv" },
            new AccountItem { Name = "Fremdkapital", Typ = "Passiv" },
            new AccountItem { Name = "Dienstleistungsertrag", Typ = "Ertrag" },
            new AccountItem { Name = "Warenertrag", Typ = "Ertrag" },
            new AccountItem { Name = "Warenaufwand", Typ = "Aufwand" },
            new AccountItem { Name = "Lohnaufwand", Typ = "Aufwand" },
            new AccountItem { Name = "Raumaufwand", Typ = "Aufwand" },
        };

        public static ObservableCollection<BookingItem> AllBookings { get; } = new ObservableCollection<BookingItem>();
    }
}