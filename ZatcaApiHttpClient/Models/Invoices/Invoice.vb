Imports System.ComponentModel
Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Enums

Namespace ZatcaApiHttpClient.Models.Invoices
    Public Class Invoice
        Public Sub New()
            DocumentIssueDateTime = DateTime.Now
        End Sub

        <DefaultValue(InvoiceKind.SimplifiedTaxInvoice)>
        Public Property DocumentType As InvoiceKind

        <DefaultValue(InvoiceIndicator.None)>
        Public Property InvoiceIndicator As InvoiceIndicator

        <DefaultValue(Currency.SAR)>
        Public Property Currency As Currency

        Public Property DocumentIssueDateTime As DateTime

        Public Property SupplyDate As DateTime?

        Public Property Buyer As Buyer

        Public Property DocumentLineItems As List(Of DocumentLineItem)
        Public Property ReferenceId As String
        Public Property DocumentId As Integer
        Public Property DiscountOnDocumentTotal As Double?

        Public Property PaymentMeans As String
    End Class
End Namespace

