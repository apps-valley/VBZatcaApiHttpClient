Imports System.ComponentModel.DataAnnotations

Namespace ZatcaApiHttpClient.Models.Invoices
    Public Class PartyPostalAddress
        <StringLength(127, ErrorMessage:="عنوان المشتري – الشارع يجب أن يحتوي على حرف واحد على الأقل ولا يزيد عن 127 حرف.")>
        Public Property StreetName As String

        Public Property AdditionalStreetName As String

        Public Property BuildingNumber As String

        Public Property PlotIdentification As String

        Public Property CitySubdivisionName As String

        Public Property CityName As String

        Public Property PostalZone As String

        Public Property CountrySubentity As String

        Public Property Country As String
    End Class
End Namespace