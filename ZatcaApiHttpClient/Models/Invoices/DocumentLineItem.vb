Imports System.ComponentModel
Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Enums

Namespace ZatcaApiHttpClient.Models.Invoices
    Public Class DocumentLineItem
        Public Property LineItemName As String

        <DefaultValue(250)>
        Public Property LineItemPrice As Decimal

        <DefaultValue(2)>
        Public Property LineItemQty As Decimal

        Public Property DiscountOnLineItem As Decimal

        Public Property TaxCategoryCode As TaxCategories

        <DefaultValue(TaxReasonCode.None)>
        Public Property TaxReasonCode As TaxReasonCode

        Public Property TaxReasonDescription As String

        Public Property ItemVatCategoryCodeReasonCode As String

        Public Property VatRateOnLineItem As Decimal
    End Class
End Namespace

