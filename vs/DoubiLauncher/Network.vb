Imports System.IO
Imports System.Text
Module Network
    Dim updateUrl As String = "http://update.dreamerstudio.net/update/DoubiLauncher/" '更新地址

    '更新
    Public Sub update()
        On Error Resume Next
        '获取最后版本
        Dim LastestVersion As String = cURL(updateUrl & "?Command=GetLastestVersion&Version=Debug")
        If IsNothing(LastestVersion) Then Exit Sub
        '最后版本与当前版本不同
        If LastestVersion <> Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString Then
            '询问是否更新
            If MsgBox("发现新版本" & LastestVersion & vbCrLf & "是否更新", MsgBoxStyle.YesNo + MsgBoxStyle.Information) = vbYes Then
                '下载文件
                My.Computer.Network.DownloadFile(updateUrl & "?Command=Download&Version=" & LastestVersion, AppDomain.CurrentDomain.BaseDirectory & "Doubi Launcher V" & LastestVersion & ".exe", "", "", True, 100, True)
                '运行文件
                Shell("Doubi Launcher V" & LastestVersion & ".exe")
                End
            End If
        End If
    End Sub

    '简易cURL封装
    Public Function cURL(URL As String, Optional Method As String = "GET", Optional Data As String = Nothing) As String
        Dim httpReq As Net.HttpWebRequest = Net.WebRequest.Create(URL)
        httpReq.Method = Method
        If Method = "POST" Then
            Dim btData() As Byte = Encoding.UTF8.GetBytes(Data)
            httpReq.ContentType = "application/x-www-form-urlencoded"
            httpReq.ContentLength = btData.Length
            httpReq.GetRequestStream.Write(btData, 0, btData.Length)
        End If
        Dim httpRes As Net.HttpWebResponse = httpReq.GetResponse
        Dim reader As New StreamReader(httpRes.GetResponseStream)
        Return reader.ReadToEnd
    End Function
End Module
