Imports System.Net
Imports Newtonsoft.Json
Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Contract
Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Models.Login

Namespace ZatcaApiHttpClient
    Public Class UserAuthenticationClient
        Private ReadOnly _client As WebClient

        Public Sub New()
            _client = New WebClient()
            _client.BaseAddress = "https://zatcaapi.avtax.net/"
        End Sub

        Public Function Login(ByVal command As LoginCommand) As ApiResponse(Of LoginSuccessResponse)
            Dim apiResponse As New ApiResponse(Of LoginSuccessResponse)()

            Try

                _client.Headers(HttpRequestHeader.ContentType) = "application/json"
                Dim responseString As String = _client.UploadString("api/UserAuthentication/Login", JsonConvert.SerializeObject(command))
                apiResponse.Data = JsonConvert.DeserializeObject(Of LoginSuccessResponse)(responseString)
                apiResponse.Message = "Login successful."
                apiResponse.StatusCode = HttpStatusCode.OK
            Catch ex As WebException
                If ex.Status = WebExceptionStatus.ProtocolError AndAlso ex.Response IsNot Nothing Then
                    Dim resp = DirectCast(ex.Response, HttpWebResponse)
                    apiResponse.StatusCode = resp.StatusCode
                    apiResponse.Message = ex.Message
                    apiResponse.ErrorMessages = New List(Of String) From {ex.Message}
                Else
                    apiResponse.Message = $"An error occurred: {ex.Message}"
                    apiResponse.ErrorMessages = New List(Of String) From {ex.Message}
                End If
            End Try

            Return apiResponse
        End Function
    End Class
End Namespace
