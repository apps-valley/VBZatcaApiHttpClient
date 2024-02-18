Imports System.IO
Imports Newtonsoft.Json
Imports ZatcaApiHttpClient.ZatcaApiHttpClient
Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Enums
Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Models.Invoices
Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Models.Login

Namespace ZatcaApiLibrary
    Friend Class Program
        Public Shared Sub Main(ByVal args As String())
            ' Fetches a new access token.
            ' The access token must be cached and used in every request. It is valid for 10 hours.
            ' Before the 10 hours end, a new access token must be generated.
            Console.WriteLine("__________________________Fetches a new access token___________________________")
            Dim apiResponse = New UserAuthenticationClient().Login(New LoginCommand() With {
                .ClientId = "YourClientId",
                .ClientSecret = "YourClientSecret"
            })

            ' If the login was not successful, terminate the program.
            If Not apiResponse.IsSuccess Then
                Console.WriteLine(String.Join(Environment.NewLine, apiResponse.ErrorMessages))
                Return
            End If
            ' Print the access token to the console.
            Console.WriteLine(apiResponse.Data.access_token)

            Using InvocieClient As New InvoiceClient(apiResponse.Data.access_token)
                Console.WriteLine("__________________________SubmitInvoice___________________________")
                Dim submitInvoiceResponse = InvocieClient.SubmitInvoice(New SubmitInvoiceCommand With {
           .DocumentId = 8001,
           .DocumentType = InvoiceKind.TaxInvoice,
           .Currency = Currency.SAR,
           .DocumentIssueDateTime = DateTime.Now, ' .ToKsaDateTime() method is not defined in this code
           .SupplyDate = DateTime.Now, ' .ToKsaDateTime() method is not defined in this code
           .Buyer = New Buyer With {
               .BuyerName = "Taj Al Mulook General Trading LLC",
               .BuyerVatId = "310285784400003",
               .BuyerAddress = New PartyPostalAddress With {
                   .CityName = "الرياض",
                   .CitySubdivisionName = "الرياض",
                   .Country = "SA",
                   .BuildingNumber = "1112",
                   .PostalZone = "12345",
                   .StreetName = "شارع الرياض"
               }
           },
           .ReferenceId = Nothing,
           .PaymentMeans = Nothing,
           .DocumentLineItems = New List(Of DocumentLineItem) From {
               New DocumentLineItem With {
                   .LineItemName = "POLYETHYLENE HDPE HHM TR - 131",
                   .LineItemPrice = 10,
                   .LineItemQty = 1D,
                   .DiscountOnLineItem = 0,
                   .TaxCategoryCode = TaxCategories.S,
                   .TaxReasonDescription = Nothing,
                   .VatRateOnLineItem = 15
               }
           },
           .InvoiceIndicator = InvoiceIndicator.None
       }).Result


                If Not submitInvoiceResponse.IsSuccess Then
                    Console.WriteLine(String.Join(Environment.NewLine, submitInvoiceResponse.ErrorMessages))
                    Return
                End If
                Console.WriteLine(JsonConvert.SerializeObject(submitInvoiceResponse.Data))

                Console.WriteLine("__________________________GetInvoiceStatus___________________________")
                Dim getInvoiceStatusResponse = InvocieClient.GetInvoiceStatus(submitInvoiceResponse.Data.UUID).Result
                If Not getInvoiceStatusResponse.IsSuccess Then
                    Console.WriteLine(String.Join(Environment.NewLine, getInvoiceStatusResponse.ErrorMessages))
                    Return
                End If
                Console.WriteLine(JsonConvert.SerializeObject(getInvoiceStatusResponse.Data))

                Console.WriteLine("__________________________PrintA3Invoice___________________________")
                Dim getPdfA3InvoiceResponse = InvocieClient.PrintA3Invoice(submitInvoiceResponse.Data.UUID).Result
                If Not getPdfA3InvoiceResponse.IsSuccess Then
                    Console.WriteLine(String.Join(Environment.NewLine, getPdfA3InvoiceResponse.ErrorMessages))
                    Return
                End If
                Dim pdfA3ByteArray = getPdfA3InvoiceResponse.Data
                Dim fileName = Path.GetRandomFileName() & ".pdf"
                File.WriteAllBytes(fileName, pdfA3ByteArray)
                Console.WriteLine($"PDF/A3 invoice saved as {fileName}")
            End Using

        End Sub
    End Class
End Namespace
