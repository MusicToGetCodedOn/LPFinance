using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


using QuestColors = QuestPDF.Helpers.Colors;
using MauiColors = Microsoft.Maui.Graphics.Colors;
using QuestContainer = QuestPDF.Infrastructure.IContainer;

namespace LPFinance;

public class AccountBalance
{
    public string Name { get; set; }
    public double Balance { get; set; }
}

public partial class OverviewPage : ContentPage
{
    public OverviewPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshData();
    }

    private void RefreshData()
    {
        var aktiv = new List<AccountBalance>();
        var passiv = new List<AccountBalance>();
        var aufwand = new List<AccountBalance>();
        var ertrag = new List<AccountBalance>();

        double totalAufwand = 0;
        double totalErtrag = 0;

        foreach (var acc in DataService.AllAccounts)
        {
            double balance = 0;
            foreach (var b in DataService.AllBookings)
            {
                if (b.SollKonto == acc.Name) balance += b.Amount;
                if (b.HabenKonto == acc.Name) balance -= b.Amount;
            }

            var item = new AccountBalance { Name = acc.Name, Balance = Math.Abs(balance) };

            switch (acc.Typ)
            {
                case "Aktiv": aktiv.Add(item); break;
                case "Passiv": passiv.Add(item); break;
                case "Aufwand":
                    aufwand.Add(item);
                    totalAufwand += Math.Abs(balance);
                    break;
                case "Ertrag":
                    ertrag.Add(item);
                    totalErtrag += Math.Abs(balance);
                    break;
            }
        }

        AktivList.ItemsSource = aktiv;
        PassivList.ItemsSource = passiv;
        AufwandList.ItemsSource = aufwand;
        ErtragList.ItemsSource = ertrag;

        double result = totalErtrag - totalAufwand;
        ResultValueLabel.Text = $"CHF {result:N2}";
        ResultTitleLabel.Text = result >= 0 ? "Gewinn" : "Verlust";

        
        ResultValueLabel.TextColor = result >= 0 ? MauiColors.White : MauiColors.LightCoral;
    }

    private async void OnExportPdfClicked(object sender, EventArgs e)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        try
        {
            string fileName = $"LPFinance_Abschluss_{DateTime.Now:yyyyMMdd}.pdf";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(QuestColors.White);

                    page.Header().Text("LPFinance - Abschlussbericht").FontSize(22).SemiBold().FontColor("#1E3A8A");

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Spacing(10);
                        col.Item().Text($"Erstellungsdatum: {DateTime.Now:f}").FontSize(10);
                        col.Item().LineHorizontal(1).LineColor("#1E3A8A");
                        col.Item().PaddingTop(10).Text("Übersicht aller Konten").FontSize(14).SemiBold();

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(100);
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                
                                header.Cell().Element(c => CellStyle(c)).Text("Konto");
                                header.Cell().Element(c => CellStyle(c)).AlignRight().Text("Typ");
                                header.Cell().Element(c => CellStyle(c)).AlignRight().Text("Saldo (CHF)");
                            });

                            foreach (var acc in DataService.AllAccounts)
                            {
                                double bal = 0;
                                foreach (var b in DataService.AllBookings)
                                {
                                    if (b.SollKonto == acc.Name) bal += b.Amount;
                                    if (b.HabenKonto == acc.Name) bal -= b.Amount;
                                }

                                table.Cell().BorderBottom(1).BorderColor(QuestColors.Grey.Lighten3).PaddingVertical(5).Text(acc.Name);
                                table.Cell().BorderBottom(1).BorderColor(QuestColors.Grey.Lighten3).PaddingVertical(5).AlignRight().Text(acc.Typ);
                                table.Cell().BorderBottom(1).BorderColor(QuestColors.Grey.Lighten3).PaddingVertical(5).AlignRight().Text($"{Math.Abs(bal):N2}");
                            }
                        });

                        col.Item().PaddingTop(20).AlignRight().Column(resCol =>
                        {
                            resCol.Item().Text(ResultValueLabel.Text).FontSize(20).SemiBold().FontColor("#1E3A8A");
                            resCol.Item().Text(ResultTitleLabel.Text).FontSize(12);
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Seite ");
                        x.CurrentPageNumber();
                    });
                });
            })
            .GeneratePdf(filePath);

            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Export Fehler", "Das PDF konnte nicht erstellt werden: " + ex.Message, "OK");
        }
    }

    private QuestContainer CellStyle(QuestContainer container)
    {
        return container.DefaultTextStyle(x => x.SemiBold())
                        .PaddingVertical(5)
                        .BorderBottom(1)
                        .BorderColor("#1E3A8A");
    }
}