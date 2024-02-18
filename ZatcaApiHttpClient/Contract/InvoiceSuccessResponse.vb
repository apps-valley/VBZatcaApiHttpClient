Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Enums

Namespace ZatcaApiHttpClient.Contract
    Public Class InvoiceSuccessResponse
        Public Property UUID As String
        Public Property Status As InvoiceStatus
        Public Property ValidationResults As ValidationResult
        Public Property QrCodeImageUrl As String
        Public Property QrCodeString As String
        Public Property XmlVersionUrl As String
    End Class
End Namespace