﻿Class MainPage
    '=======================================================
    '待修改部分：
    '   加入错误捕捉
    '   自定义Xmx Xms
    '=======================================================
#Region "退出按钮"
    Private Sub Button_Exit_Click(sender As Object, e As RoutedEventArgs) Handles Button_Exit.Click
        Static closing As Boolean '防止多次点击反复运行动画
        If Not closing Then
            closing = True
            FadeOut(Windows.Application.Current.MainWindow， 500, True)
            Windows.Application.Current.Shutdown()
        End If
    End Sub
#End Region
#Region "启动按钮"
    Private Sub Button_Launch_Click(sender As Object, e As RoutedEventArgs) Handles Button_Launch.Click
        If GameList.Items.Count = 0 Then
            MsgBox("未找到游戏")
            Exit Sub
        ElseIf My.Settings.UserName = ""
            MsgBox("没有设置玩家名")
            Exit Sub
        End If
        Try
            LaunchHelper.Launch(MainWindow.GameJarPath(GameList.SelectedIndex),
                                AppDomain.CurrentDomain.BaseDirectory & ".minecraft\libraries",
                                My.Settings.UserName,
                                My.Settings.CustomXmx,
                                My.Settings.CustomXms)
            Windows.Application.Current.Shutdown()
        Catch ex As Exception
            MsgBox("在" & Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName & "." & Reflection.MethodBase.GetCurrentMethod.Name & "发生了" & vbCrLf & ex.ToString)
            End
        End Try

    End Sub
#End Region
#Region "将游戏载入列表"
    Private Sub GameList_Loaded(sender As Object, e As RoutedEventArgs) Handles GameList.Loaded
        Try
            For Each n In MainWindow.GameName
                GameList.Items.Add(n)
            Next
            GameList.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "访问Doubi Launcher主页"
    Private Sub Label_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Process.Start("http://www.dreamerstudio.net/doubilauncher")
    End Sub
#End Region
End Class
