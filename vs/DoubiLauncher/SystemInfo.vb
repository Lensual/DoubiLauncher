Imports Microsoft.Win32.Registry

'用于获取系统信息
Module SystemInfo
    Public Function GetJavaPath() As String
        Dim v As String
        If IsWOW64() Then
            v = GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "CurrentVersion", "")
            Return GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment\" + v, "JavaHome", "") + "\bin"
        Else
            v = GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "CurrentVersion", "")
            Return GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment\" + v, "JavaHome", "") + "\bin"
        End If
    End Function

    Public Function IsWOW64() As Boolean
        If Dir(Environ("windir") + "\SysWOW64") <> "" Then
            Return True
        Else
            Return False
        End If
    End Function
End Module
