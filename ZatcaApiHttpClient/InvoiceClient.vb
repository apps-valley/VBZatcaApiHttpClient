Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports Newtonsoft.Json
Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Contract
Imports ZatcaApiHttpClient.ZatcaApiHttpClient.Models.Invoices

Namespace ZatcaApiHttpClient
    Public Class InvoiceClient
        Implements IDisposable

        Private ReadOnly _client As HttpClient

        Public Sub New(ByVal accessToken As String)
            _client = New HttpClient With {
        .BaseAddress = New Uri("https://localhost:7181/")
    }
            _client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", accessToken)
            _client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        End Sub


        Public Async Function SubmitInvoice(ByVal command As SubmitInvoiceCommand) As Task(Of ApiResponse(Of InvoiceSuccessResponse))
            ' Send a POST request to the "SubmitInvoice" endpoint with the invoice details.
            Dim jsonContent = New StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json")
            Dim response = Await _client.PostAsync("api/Invoice/SubmitInvoice", jsonContent)


            ' Initialize a new ApiResponse object with the status code of the HTTP response.
            Dim apiResponse As New ApiResponse(Of InvoiceSuccessResponse) With {
        .StatusCode = response.StatusCode
    }

            ' Read the content of the HTTP response as a string.
            Dim responseString As String = If(Await response.Content.ReadAsStringAsync(), String.Empty)

            ' Handle the HTTP response based on its status code.
            Select Case response.StatusCode
                Case HttpStatusCode.OK
                    ' Deserialize the response content into a InvoiceSuccessResponse object.
                    apiResponse.Data = JsonConvert.DeserializeObject(Of InvoiceSuccessResponse)(responseString)

                    ' Set the message of the ApiResponse to indicate a successful submission.
                    apiResponse.Message = "Invoice Submitted to Zatca"

                    ' If the invoice status is NOT_REPORTED or NOT_CLEARED, set the errors of the ApiResponse.
                    If apiResponse.Data.Status = Enums.InvoiceStatus.NOT_REPORTED OrElse apiResponse.Data.Status = Enums.InvoiceStatus.NOT_CLEARED Then
                        apiResponse.ErrorMessages = apiResponse.Data.ValidationResults.errorMessages.Select(Function(a) a.message).ToList()
                    End If


                Case HttpStatusCode.BadRequest
                    ' Deserialize the response content into an ApiErrorResponse object.
                    Dim errorResponse = JsonConvert.DeserializeObject(Of ApiErrorResponse)(responseString)

                    ' Set the errors of the ApiResponse to the errors from the ApiErrorResponse.
                    apiResponse.ErrorMessages = errorResponse.Errors

                Case HttpStatusCode.NotFound, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized
                    ' For these status codes, set the message and errors of the ApiResponse to the content of the HTTP response.
                    apiResponse.Message = responseString
                    apiResponse.ErrorMessages = New List(Of String)({responseString})

                Case Else
                    ' For all other status codes, an unexpected error occurred.
                    ' Set the message and errors of the ApiResponse to indicate an error.
                    apiResponse.Message = $"An unexpected error occurred: {response.ReasonPhrase}"
                    apiResponse.ErrorMessages = New List(Of String)({$"An unexpected error occurred: {response.ReasonPhrase}"})
            End Select

            ' Return the ApiResponse object.
            Return apiResponse
        End Function


        Public Async Function GetInvoiceStatus(ByVal UUID As String) As Task(Of ApiResponse(Of InvoiceStatusResponse))
            ' Send a GET request to the "GetInvoiceStatus" endpoint with the UUID of the invoice.
            Dim response = Await _client.GetAsync($"api/Invoice/GetInvoiceStatus/{UUID}/status")

            ' Initialize a new ApiResponse object with the status code of the HTTP response.
            Dim apiResponse As New ApiResponse(Of InvoiceStatusResponse) With {
            .StatusCode = response.StatusCode
        }

            ' Read the content of the HTTP response as a string.
            Dim responseString As String = If(Await response.Content.ReadAsStringAsync(), String.Empty)

            ' Handle the HTTP response based on its status code.
            Select Case response.StatusCode
                Case HttpStatusCode.OK
                    ' Deserialize the response content into an InvoiceStatusResponse object.
                    apiResponse.Data = JsonConvert.DeserializeObject(Of InvoiceStatusResponse)(responseString)

                    ' Set the message of the ApiResponse to indicate a successful retrieval of invoice status.
                    apiResponse.Message = "Invoice status retrieved successfully."

                Case HttpStatusCode.NotFound, HttpStatusCode.Unauthorized
                    ' For these status codes, set the message and errors of the ApiResponse to the content of the HTTP response.
                    apiResponse.Message = responseString
                    apiResponse.ErrorMessages = New List(Of String) From {responseString}

                Case Else
                    ' For all other status codes, an unexpected error occurred.
                    ' Set the message and errors of the ApiResponse to indicate an error.
                    apiResponse.Message = $"An unexpected error occurred: {response.ReasonPhrase}"
                    apiResponse.ErrorMessages = New List(Of String) From {$"An unexpected error occurred: {response.ReasonPhrase}"}
            End Select

            ' Return the ApiResponse object.
            Return apiResponse
        End Function

        Public Async Function PrintA3Invoice(ByVal UUID As String) As Task(Of ApiResponse(Of Byte()))
            ' Send a GET request to the "PrintInvoice" endpoint with the UUID of the invoice.
            Dim response = Await _client.GetAsync($"api/Invoice/PrintInvoice/{UUID}/PrintA3")

            ' Initialize a new ApiResponse object with the status code of the HTTP response.
            Dim apiResponse As New ApiResponse(Of Byte()) With {
            .StatusCode = response.StatusCode
        }

            ' Handle the HTTP response based on its status code.
            Select Case response.StatusCode
                Case HttpStatusCode.OK
                    ' Read the content of the HTTP response as a byte array.
                    apiResponse.Data = Await response.Content.ReadAsByteArrayAsync()

                    ' Set the message of the ApiResponse to indicate a successful retrieval of the invoice PDF/A3.
                    apiResponse.Message = "Invoice PDF/A3 retrieved successfully."

                Case HttpStatusCode.NotFound, HttpStatusCode.Unauthorized
                    ' Read the content of the HTTP response as a string.
                    Dim responseString As String = Await response.Content.ReadAsStringAsync()

                    ' For these status codes, set the message and errors of the ApiResponse to the content of the HTTP response.
                    apiResponse.Message = responseString
                    apiResponse.ErrorMessages = New List(Of String) From {responseString}

                Case Else
                    ' For all other status codes, an unexpected error occurred.
                    ' Set the message and errors of the ApiResponse to indicate an error.
                    apiResponse.Message = $"An unexpected error occurred: {response.ReasonPhrase}"
                    apiResponse.ErrorMessages = New List(Of String) From {$"An unexpected error occurred: {response.ReasonPhrase}"}
            End Select

            ' Return the ApiResponse object.
            Return apiResponse
        End Function

        Public Async Function GetInvoices(ByVal GetInvoicesQuery As GetInvoicesQuery) As Task(Of ApiResponse(Of List(Of GetInvoicesResponse)))
            ' Start building the URL.
            Dim url As New StringBuilder("api/Invoice/GetInvoices")

            ' Add the InvoiceTypes parameter if it's not null and has items.
            If GetInvoicesQuery.InvoiceTypes IsNot Nothing AndAlso GetInvoicesQuery.InvoiceTypes.Any() Then
                Dim invoiceTypesString As String = String.Join(",", GetInvoicesQuery.InvoiceTypes)
                url.Append($"?InvoiceTypes={invoiceTypesString}")
            End If

            ' Add the other parameters.
            url.Append($"&PageNumber={GetInvoicesQuery.PageNumber}&PageSize={GetInvoicesQuery.PageSize}")

            Dim response = Await _client.GetAsync(url.ToString())

            ' Initialize a new ApiResponse object with the status code of the HTTP response.
            Dim apiResponse As New ApiResponse(Of List(Of GetInvoicesResponse)) With {
            .StatusCode = response.StatusCode
        }

            ' Read the content of the HTTP response as a string.
            Dim responseString As String = If(Await response.Content.ReadAsStringAsync(), String.Empty)

            ' Handle the HTTP response based on its status code.
            Select Case response.StatusCode
                Case HttpStatusCode.OK
                    ' Deserialize the response content into a list of GetInvoicesResponse objects.
                    apiResponse.Data = JsonConvert.DeserializeObject(Of List(Of GetInvoicesResponse))(responseString)

                    ' Set the message of the ApiResponse to indicate a successful retrieval of invoices.
                    apiResponse.Message = "Invoices retrieved successfully."

                Case HttpStatusCode.Unauthorized, HttpStatusCode.BadRequest
                    ' For these status codes, set the message and errors of the ApiResponse to the content of the HTTP response.
                    apiResponse.Message = responseString
                    apiResponse.ErrorMessages = New List(Of String) From {responseString}

                Case Else
                    ' For all other status codes, an unexpected error occurred.
                    ' Set the message and errors of the ApiResponse to indicate an error.
                    apiResponse.Message = $"An unexpected error occurred: {response.ReasonPhrase}"
                    apiResponse.ErrorMessages = New List(Of String) From {$"An unexpected error occurred: {response.ReasonPhrase}"}
            End Select

            ' Return the ApiResponse object.
            Return apiResponse
        End Function


        Public Sub Dispose() Implements IDisposable.Dispose
            _client.Dispose()
        End Sub
    End Class
End Namespace
