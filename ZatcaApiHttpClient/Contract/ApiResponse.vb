Imports System.Net

Namespace ZatcaApiHttpClient.Contract
    Public Class ApiResponse(Of T)
        Public Property StatusCode As HttpStatusCode
        Public Property Data As T
        Public Property Message As String
        Public Property ErrorMessages As List(Of String)

        Public ReadOnly Property IsSuccess As Boolean
            Get
                Return ErrorMessages Is Nothing OrElse ErrorMessages?.Count = 0
            End Get
        End Property
    End Class

End Namespace