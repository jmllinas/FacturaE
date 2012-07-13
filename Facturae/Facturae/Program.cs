using System;
using System.Security.Cryptography.X509Certificates;
using nFacturae.Extensions;
using System.Web;

namespace nFacturae
{
    public class Program
    {
        static void Main(string[] args)
        {
            Facturae            eInvoice	= new Facturae();
            X509Certificate2    cert        = new X509Certificate2(@"Certificates/usr0052.p12", "usr0052");
            SignedFacturae      signed      = null;
            bool                isValid    = false;

            // Create a new facturae invoice & sign it
            signed = eInvoice
                .Parties()
                    .Seller()
                        .SetIdentification("00001")
                        .TaxIdentification()
                            .IsResidentInSpain()
                            .SetIdentificationNumber("555888555")
                            .Party()
                        .IsIndividual()
                            .SetName("JOHN")
                            .SetFirstSurname("DOE")
                            .SetAddress("8585 FIRST STREET")
                            .SetProvince("MADRID")
                            .SetTown("MADRID")
                            .SetPostCode("99900")
                            .SetCountryCode(CountryType.ESP)
                            .Party()
                        .Parties()
                    .Buyer()
                        .SetIdentification("00002")
                        .TaxIdentification()
                            .IsResidentInSpain()
                            .SetIdentificationNumber("555888555")
                            .Party()
                        .IsIndividual()
                            .SetName("JOHN")
                            .SetFirstSurname("DOE")
                            .SetAddress("8585 FIRST STREET")
                            .SetProvince("MADRID")
                            .SetTown("MADRID")
                            .SetPostCode("99900")
                            .SetCountryCode(CountryType.ESP)
                            .Party()
                    .Parties()
                .Facturae()
                    .CreateInvoice()
                        .SetCurrency(CurrencyCodeType.EUR)
                        .SetExchangeRate(1, DateTime.Now)
                        .SetTaxCurrency(CurrencyCodeType.EUR)
                        .SetLanguage(LanguageCodeType.es)
                        .SetPlaceOfIssue(String.Empty, "00000")
                        .IsOriginal()
                        .IsComplete()
                        .SetInvoiceSeries("0")
                        .SetInvoiceNumber("1000")
                        .Items()
                            .AddInvoiceItem("XX", "XX")
                                .GiveQuantity(1)
                                .GiveUnitPriceWithoutTax(100.01)
                                .GiveDiscount(10.01)
                                .GiveTax(18.00)
                                .CalculateTotals()
                            .AddInvoiceItem("XXX", "XXX")
                                .GiveQuantity(1)
                                .GiveUnitPriceWithoutTax(100.01)
                                .GiveDiscount(10.01)
                                .GiveTax(18.00)
                                .CalculateTotals()
                        .CalculateTotals()
                .CalculateTotals()
                .Sign(cert);

            // Check signature
            isValid = signed.WriteToFile(@"Sample.xml")
                            .CheckSignature();

            System.Console.WriteLine(isValid);
            System.Console.ReadLine();
        }
    }
}

