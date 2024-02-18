Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Enums

Namespace ZatcaApiHttpClient.Models.Invoices
    Public NotInheritable Class GetInvoicesQuery
        Public Property InvoiceTypes As List(Of InvoiceKind)
        Public Property PageNumber As Integer
        Public Property PageSize As Integer

        Public Sub New(ByVal invoiceTypes As List(Of InvoiceKind), ByVal pageNumber As Integer, ByVal pageSize As Integer)
            Me.InvoiceTypes = invoiceTypes
            Me.PageNumber = pageNumber
            Me.PageSize = pageSize
        End Sub
    End Class

    Public Class InvoiceStatusResponse
        Public Property invoiceUUID As String
        Public Property InvoiceStatus As InvoiceStatus
        Public Property InvoiceReportingResponse As InvoiceReportingResponse
        Public Property InvoiceClearanceResponse As InvoiceClearanceResponse
    End Class

    Public Class InvoiceReportingResponse
        Public Property ValidationResults As Contract.ValidationResult
        Public Property ReportingStatus As InvoiceStatus
    End Class

    Public Class InvoiceClearanceResponse
        Public Property ValidationResults As Contract.ValidationResult
        Public Property ClearedInvoice As String
        Public Property ClearanceStatus As InvoiceStatus
    End Class

    Public Class GetInvoicesResponse
        Inherits Invoice

        Public Property InvoiceStatus As InvoiceStatusResponse
    End Class
End Namespace
