Public Class MCJsonStruct 'MCJS数据结构定义类
#Region "启动参数"
    Public Class LaunchArguments
        Public minecraftArguments As String
        Public mainClass As String
        Public libraries() As libraries
    End Class
    Public Class libraries
        Public name As String
    End Class
#End Region
End Class
