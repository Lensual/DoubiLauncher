Imports System.IO
Imports System.Runtime.Serialization.Json
Imports System.Text

Public Module JSONSerizer
#Region "序列化JSON"
    Public Function Serizer_Write(obj As Object) As String
        Dim MemStream As New MemoryStream()
        Dim ser As New DataContractJsonSerializer(obj.GetType())
        ser.WriteObject(MemStream, obj)
        MemStream.Position = 0
        Dim MemStreamRerder As New StreamReader(MemStream)
        Return MemStreamRerder.ReadToEnd()
    End Function
#End Region
#Region "反序列化JSON"
    Public Function Serizer_Read(JSON As String, JsonType As Type) As Object
        Dim MemStream As New MemoryStream(Encoding.UTF8.GetBytes(JSON))
        Dim ser As New DataContractJsonSerializer(JsonType)
        Return ser.ReadObject(MemStream)
    End Function
#End Region
End Module
