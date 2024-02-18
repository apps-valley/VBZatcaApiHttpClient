Namespace ZatcaApiHttpClient.Contract
    Public Class ValidationResult
        Public Property infoMessages As List(Of ValidationMessage)
        Public Property warningMessages As List(Of ValidationMessage)
        Public Property errorMessages As List(Of ValidationMessage)
        Public Property status As String
    End Class
End Namespace