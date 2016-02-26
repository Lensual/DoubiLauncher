Imports System.IO
Imports System.Text
Module UpdateMdl
    Dim updateUrl As String = "http://update.dreamerstudio.net/update/DoubiLauncher/" '更新地址
    Dim ClientVersion As String '游戏客户端版本号
#Region "更新"
    Public Sub update()
        On Error Resume Next
        '检查本地是否存在doubicustom.cfg
        If IO.File.Exists(".\doubicustom.cfg") Then
            Dim str_customcfg As String
            str_customcfg = IO.File.ReadAllText(".\doubicustom.cfg")
            '使用定制地址
            Dim tmp() As String
            tmp = Split(str_customcfg, "updateUrl=")
            updateUrl = Split(tmp(1), vbCrLf)(0)
            '取得游戏客户端版本号
            tmp = Split(str_customcfg, "ClientVersion=")
            ClientVersion = Split(tmp(1), vbCrLf)(0)

        End If

        '向更新服务器发送本地信息 询问下一步操作
        Dim response As String
        response = cURL(updateUrl & "?Command=WhatShouldIdo" &
             "&Build=" & Reflection.Assembly.GetExecutingAssembly.GetName.Version.Build &
             "&ClientVersion=" & ClientVersion &
             "&exeName=" & My.Application.Info.AssemblyName & ".exe")
        Dim result As String = runscript(response)
        If Not IsNothing(result) Then
            MsgBox(result, MsgBoxStyle.Exclamation)
        End If

        '==============0.2.0.1删除===================
        ''获取最后版本
        'Dim LastestVersion As String = cURL(updateUrl & "?Command=GetLastestVersion&Version=Debug")
        'If IsNothing(LastestVersion) Then Exit Sub
        ''最后版本与当前版本不同
        'If LastestVersion <> Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString Then
        '    '询问是否更新
        '    If MsgBox("发现新版本" & LastestVersion & vbCrLf & "是否更新", MsgBoxStyle.YesNo + MsgBoxStyle.Information) = vbYes Then
        '        '下载文件
        '        My.Computer.Network.DownloadFile(updateUrl & "?Command=Download&Version=" & LastestVersion, AppDomain.CurrentDomain.BaseDirectory & "Doubi Launcher V" & LastestVersion & ".exe", "", "", True, 100, True)
        '        '运行文件
        '        Shell("Doubi Launcher V" & LastestVersion & ".exe")
        '        End
        '    End If
        'End If
    End Sub
#End Region
#Region "升级脚本解析"
    Public Function runscript(script As String) As String
        '去掉回车换行
        script = script.Replace(vbCrLf, "")
        '分离出每个语句行
        Dim cmdline() As String
        cmdline = Split(script, ");")

        '按顺序执行命令循环
        For i = 0 To cmdline.Length - 1
            '分离命令和参数
            Dim tmp() As String = Split(cmdline(i), "(")
            Dim cmd As String = tmp(0)
            Dim parameter() As String
            If Not tmp.Length = 1 Then '如果有参数
                parameter = Split(tmp(1), ",") '分割参数
                For Each p In parameter '转义
                    Replace(p, "%0A", vbCrLf,,, CompareMethod.Text)
                    Replace(p, "%25", "%",,, CompareMethod.Text)
                    Replace(p, "%28", "(",,, CompareMethod.Text)
                    Replace(p, "%29", ")",,, CompareMethod.Text)
                    Replace(p, "%3B", ";",,, CompareMethod.Text)
                    Replace(p, "%2C", ",",,, CompareMethod.Text)
                Next
            End If
            Try
#Region "命令Select"
                Select Case cmd
                    Case "echo"
                        'echo(内容);
                        MsgBox(parameter(0))
                    Case "ask"
                        'ask(问题,标识);
                        If MsgBox(parameter(0), MsgBoxStyle.YesNo + MsgBoxStyle.Information) = MsgBoxResult.Yes Then
                            Exit Select
                        Else
                            For j = 0 To cmdline.Length - 1 '寻找标识循环
                                If cmdline(j) = "sign(" & parameter(1) Then
                                    i = j - 1 '通过修改变量i实现跳转
                                    Exit Select
                                End If
                            Next
                            Throw New Exception("升级脚本错误 找不到跳转标识")
                        End If
                    Case "7z"
                        '7z(存放路径,文件路径);
                        IO.File.WriteAllBytes(".\7z.exe", My.Resources._7za)
                        Dim p As Process = Process.Start(".\7za.exe", "-mx9 a" & parameter(0) & " " & parameter(1))
                        p.WaitForExit()
                        IO.File.Delete(".\7z.exe")
                        If Not p.ExitCode = 0 Then Throw New Exception("压缩时发生错误 代码" & p.ExitCode)
                    Case "un7z"
                        'un7z(文件路径,存放路径);
                        IO.File.WriteAllBytes(".\7z.exe", My.Resources._7za)
                        Dim p As Process = Process.Start(".\7za.exe", "x " & parameter(0) & " -o" & parameter(1))
                        p.WaitForExit()
                        IO.File.Delete(".\7z.exe")
                        If Not p.ExitCode = 0 Then Throw New Exception("解压时发生错误 代码" & p.ExitCode)
                    Case "download"
                        'download(地址,存放路径);
                        '（阻塞的）
                        My.Computer.Network.DownloadFile(parameter(0), parameter(1), "", "", True, 100, True, FileIO.UICancelOption.ThrowException)
                    Case "start"
                        'start(路径,参数,等待);
                        Dim p As Process = Process.Start(parameter(0), parameter(1))
                        If parameter(2) = "true" Then ' 等待
                            p.WaitForExit()
                        End If
                    Case "rename"
                        'rename(旧名字路径,新名字路径);
                        If IO.File.Exists(parameter(0)) Then '判断源是文件还是文件夹
                            My.Computer.FileSystem.RenameFile(parameter(0), parameter(1))
                        ElseIf IO.Directory.Exists(parameter(0))
                            My.Computer.FileSystem.RenameDirectory(parameter(0), parameter(1))
                        Else
                            Throw New IO.FileNotFoundException("文件不存在")
                        End If
                    Case "copy"
                        'copy(源文件路径,目标路径);
                        If IO.File.Exists(parameter(0)) Then '判断源是文件还是文件夹
                            My.Computer.FileSystem.CopyFile(parameter(0), parameter(1), True)
                        ElseIf IO.Directory.Exists(parameter(0))
                            My.Computer.FileSystem.CopyDirectory(parameter(0), parameter(1), True)
                        Else
                            Throw New IO.FileNotFoundException("源文件或文件夹不存在")
                        End If
                    Case "move"
                        'move(源文件路径,目标路径);
                        If IO.File.Exists(parameter(0)) Then '判断源是文件还是文件夹
                            My.Computer.FileSystem.MoveFile(parameter(0), parameter(1), True)
                        ElseIf IO.Directory.Exists(parameter(0))
                            My.Computer.FileSystem.MoveDirectory(parameter(0), parameter(1), True)
                        Else
                            Throw New IO.FileNotFoundException("源文件或文件夹不存在")
                        End If
                    Case "mkdir"
                        'mkdir(文件夹路径);
                        IO.Directory.CreateDirectory(parameter(0))
                    Case "delete"
                        'delete(文件路径);
                        If IO.File.Exists(parameter(0)) Then '判断源是文件还是文件夹
                            IO.File.Delete(parameter(0))
                        ElseIf IO.Directory.Exists(parameter(0))
                            IO.Directory.Delete(parameter(0), True)
                        End If
                    Case "deleteme"
                        'deleteme();
                        Shell("cmd /c taskkill /F /PID " & Process.GetCurrentProcess.Id & " & choice /t 2 /d y /n >nul & del " & My.Application.Info.AssemblyName & ".exe", AppWinStyle.Hide)
                    Case "end"
                        'end();
                        Exit For
                    Case "goto"
                        'goto(标识);
                        For j = 0 To cmdline.Length - 1 '寻找标识循环
                            If cmdline(j) = "sign(" & parameter(0) Then
                                i = j - 1 '通过修改变量i实现跳转
                                Exit Select
                            End If
                        Next
                        Throw New Exception("升级脚本错误 找不到跳转标识")
                    Case "sign"
                        'sign(标识名称);
                    Case "upversion"
                        'upversion(新版本号);
                        Dim str As String
                        str = IO.File.ReadAllText(".\doubilauncher.cfg")
                        Dim stra() As String
                        stra = Split(str, vbCrLf) '分割语句行
                        Dim out As String = "" '最后结果
                        For Each str In stra '遍历语句行
                            If Mid(str, 1, 14) = "ClientVersion=" Then '如果这行是ClientVersion
                                str = "ClientVersion=" & parameter(0)
                            End If
                            out = out & str & vbCrLf
                        Next
                        IO.File.WriteAllText(".\doubilauncher.cfg", out) '将配置写回文件
                End Select
#End Region
            Catch ex As Exception
                Return "升级时发生错误 " & vbCrLf &
                       "脚本语句：" & cmdline(i) & ");" & vbCrLf &
                       ex.Message
                Exit For
            End Try
        Next
        Return Nothing
    End Function
#End Region
#Region "简易cURL封装"
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
#End Region

End Module
