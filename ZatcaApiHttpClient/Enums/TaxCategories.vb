Imports System.ComponentModel

Namespace ZatcaApiHttpClient.Enums
    Public Enum TaxCategories
        None = 0
        <Description("التوريدات المعفاة")>
        E
        <Description("التوريدات الخاضة للضريبة")>
        S
        <Description("التوريدات الخاضعة لنسبة الصفر")>
        Z
        <Description("التوريدات الخاضعة للضريبة")>
        O
    End Enum
End Namespace