using Afdb.ClientConnection.Application.Common.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Afdb.ClientConnection.Infrastructure.Services;

public class PdfGenerationService : IPdfGenerationService
{
    public byte[] GenerateAuthorizationForm(
        string firstName,
        string lastName,
        string email,
        string functionName,
        List<string> projects,
        string language)
    {
        var isFrench = language.Equals("fr", StringComparison.OrdinalIgnoreCase);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);

                page.Content().Column(column =>
                {
                    column.Spacing(15);

                    column.Item().AlignCenter().Text(text =>
                    {
                        text.Span(isFrench
                            ? "FORMULAIRE D'AUTORISATION POUR PROJET(S) SUPPLÉMENTAIRE(S)"
                            : "AUTHORIZATION FORM FOR ADDITIONAL PROJECT(S)")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Orange.Darken2);
                    });

                    column.Item().AlignCenter().Text(text =>
                    {
                        text.Span(isFrench
                            ? "SYSTÈME CONNECTION CLIENT DE LA BAD"
                            : "AFDB CLIENT CONNECTION SYSTEM")
                            .FontSize(12)
                            .Bold()
                            .FontColor(Colors.Orange.Darken2);
                    });

                    column.Item().PaddingTop(10).LineHorizontal(2).LineColor(Colors.Grey.Darken2);

                    column.Item().PaddingTop(20).Text(text =>
                    {
                        text.Span(isFrench ? "Je soussigné(e) " : "I, the undersigned ");
                        text.Span("(1) ").FontSize(9).Superscript();
                        text.Span(".............................................."+
                            "......................................................"+
                            "...........................").FontColor(Colors.Grey.Medium);
                    });

                    column.Item().PaddingTop(5).Text(text =>
                    {
                        text.Span(isFrench ? "de " : "of ");
                        text.Span("(2) ").FontSize(9).Superscript();
                        text.Span(".........................................."+
                            "................................................."+
                            ".........................................................").FontColor(Colors.Grey.Medium);
                    });

                    column.Item().PaddingTop(10).Text(text =>
                    {
                        text.Span(isFrench ? "certifie que" : "certify that").Bold();
                    });

                    column.Item().PaddingTop(15).Text(text =>
                    {
                        text.Span(isFrench ? "M / Mme " : "Mr / Ms ");
                        text.Span($"{firstName} {lastName}").Bold().FontSize(11);
                    });

                    column.Item().PaddingTop(5).Text(text =>
                    {
                        text.Span(isFrench ? "de " : "of ");
                        text.Span("(3) ").FontSize(9).Superscript();
                        text.Span("........................................."+
                            "................................................."+
                            "..........................................................").FontColor(Colors.Grey.Medium);
                    });

                    column.Item().PaddingTop(5).Text(text =>
                    {
                        text.Span(isFrench ? "dont l'adresse email est le suivant : " : "whose email address is: ");
                        text.Span(email).Bold().FontSize(11);
                    });

                    column.Item().PaddingTop(5).Text(text =>
                    {
                        text.Span(isFrench ? "est autorisé(e) à accéder au système Connection Client en tant que " :
                            "is authorized to access the Client Connection system as ");
                        text.Span(functionName).Bold().FontSize(11);
                    });

                    column.Item().PaddingTop(15).Text(text =>
                    {
                        text.Span(isFrench ? "pour le (les) projet(s) supplémentaires suivants " :
                            "for the following additional project(s) ");
                        text.Span("(4)").FontSize(9).Superscript();
                        text.Span(":");
                    });

                    if (projects != null && projects.Any())
                    {
                        foreach (var project in projects)
                        {
                            column.Item().PaddingLeft(20).Text(project).FontSize(10);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            column.Item().PaddingTop(3).Text("..............................................."+
                                "................................................."+
                                "...............................").FontColor(Colors.Grey.Medium);
                        }
                    }

                    column.Item().PaddingTop(30).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(isFrench ? "SIGNATURE(S) :" : "SIGNATURE(S):").Bold();
                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignRight().Text(isFrench ? "DATE :" : "DATE:").Bold();
                        });
                    });

                    column.Item().PaddingTop(50).LineHorizontal(1).LineColor(Colors.Grey.Darken2);

                    column.Item().PaddingTop(10).Text(text =>
                    {
                        text.Span("(1)").FontSize(9);
                        text.Span(isFrench
                            ? " : Nom et titre de l'autorité qui signe le formulaire."
                            : " : Name and title of the authority signing the form.")
                            .FontSize(8)
                            .Italic();
                    });

                    column.Item().PaddingTop(3).Text(text =>
                    {
                        text.Span("(2)").FontSize(9);
                        text.Span(isFrench
                            ? " : Nom de la structure ou de l'entité du signataire autorisé."
                            : " : Name of the structure or entity of the authorized signatory.")
                            .FontSize(8)
                            .Italic();
                    });

                    column.Item().PaddingTop(3).Text(text =>
                    {
                        text.Span("(3)").FontSize(9);
                        text.Span(isFrench
                            ? " : Emprunteur ou l'agence d'exécution ou du garant et nom du projet enregistré dans le fichier de la BAD."
                            : " : Borrower or executing agency or guarantor and project name registered in AfDB files.")
                            .FontSize(8)
                            .Italic();
                    });

                    column.Item().PaddingTop(3).Text(text =>
                    {
                        text.Span("(4)").FontSize(9);
                        text.Span(isFrench
                            ? " : Référence du projet et nom du projet enregistré dans le fichier de la BAD."
                            : " : Project reference and project name registered in AfDB files.")
                            .FontSize(8)
                            .Italic();
                    });
                });
            });
        });

        return document.GeneratePdf();
    }
}