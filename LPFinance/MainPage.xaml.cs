using System.Linq;

namespace LPFinance
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Verknüpfe die Liste im Dashboard mit dem zentralen Speicher
            RecentBookingsList.ItemsSource = DataService.AllBookings;
        }

        // Diese Methode läuft jedes Mal, wenn du die Seite öffnest
        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateDashboardTotals();
        }

        private void UpdateDashboardTotals()
        {
            double totalIncome = 0;
            double totalExpense = 0;

            foreach (var booking in DataService.AllBookings)
            {
                // Wir suchen den Typ des Haben-Kontos (für Einnahmen)
                var habenKonto = DataService.AllAccounts.FirstOrDefault(a => a.Name == booking.HabenKonto);
                if (habenKonto != null && habenKonto.Typ == "Ertrag")
                {
                    totalIncome += booking.Amount;
                }

                // Wir suchen den Typ des Soll-Kontos (für Ausgaben)
                var sollKonto = DataService.AllAccounts.FirstOrDefault(a => a.Name == booking.SollKonto);
                if (sollKonto != null && sollKonto.Typ == "Aufwand")
                {
                    totalExpense += booking.Amount;
                }
            }

            // UI Texte aktualisieren
            IncomeLabel.Text = $"CHF {totalIncome:N2}";
            ExpenseLabel.Text = $"CHF {totalExpense:N2}";
        }
    }
}