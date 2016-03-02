'=======================================================
'待修改部分：
'   网络更新启动器
'   UI完善
'   JVM启动参数优化
'   MC启动参数配置
'   网络更新
'   进程检测
'   mod切换
'   asset切换
'   一键安装皮肤
'   java路径注册表获取
'   java.exe相对路径
'   java.exe智能搜索
'   智能一键启动
'   内存智能匹配
'   随机名字
'   个性定制
'=======================================================


Imports System.Windows.Media.Animation
Class MainWindow
#Region "动画效果"
    '窗口移动
    Private Sub MainWindow_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseDown
        If (e.LeftButton = MouseButtonState.Pressed) Then
            DragMove()
        End If
    End Sub



    '菜单动画
    Dim WithEvents MenuThicknessAnimtion As New ThicknessAnimation
    Dim MenuAnimtionAction As Boolean
    Private Sub PageLabel_MouseDown(sender As Object, e As RoutedEventArgs) Handles Label_MainPage.MouseDown, Label_SettingPage.MouseDown, Label_AboutPage.MouseDown '（写烂了 （¯﹃¯）
        If MenuAnimtionAction Then Exit Sub
        '滑动效果
        MenuThicknessAnimtion.From = Menu_Decoration.Margin           '起始值
        MenuThicknessAnimtion.To = New Thickness(sender.Margin.Left, Menu_Decoration.Margin.Top, Menu_Decoration.Margin.Right, Menu_Decoration.Margin.Bottom)        '结束值
        MenuThicknessAnimtion.Duration = TimeSpan.FromMilliseconds(300)         '动画持续时间
        MenuAnimtionAction = True
        Menu_Decoration.BeginAnimation(Rectangle.MarginProperty, MenuThicknessAnimtion) '开始动画
    End Sub
    Private Sub MenuAnimtionDone() Handles MenuThicknessAnimtion.Completed
        MenuAnimtionAction = False
    End Sub
    Private Sub Label_MainPage_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Window.Navigate(New Uri("MainPage.xaml", UriKind.Relative))
    End Sub
    Private Sub Label_SettingPage_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Window.Navigate(New Uri("SettingPage.xaml", UriKind.Relative))
    End Sub
    Private Sub Label_AboutPage_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Window.Navigate(New Uri("AboutPage.xaml", UriKind.Relative))
    End Sub


    Dim AllowDirectNavigation As Boolean
    Dim FirstNavigation As Boolean = True
    Private Sub Window_Navigating(sender As Object, e As NavigatingCancelEventArgs)
        If (Not FirstNavigation And Not AllowDirectNavigation) Then
            e.Cancel = True
            FadeOut(Window, 150, True)
            AllowDirectNavigation = True
            Window.NavigationService.Navigate(e.Uri)
            FadeIn(Window, 150, True)
        End If
        FirstNavigation = False
        AllowDirectNavigation = False
    End Sub
#End Region
    Public Shared GameJarPath() As String
    Public Shared GameName() As String
    '委托检查更新
    Delegate Sub DelegateUpdate()
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        '检查更新
        Dim update As New DelegateUpdate(AddressOf UpdateMdl.update)
        update.BeginInvoke(Nothing, Nothing)
        Try
            '查找游戏
            GameJarPath = IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory & ".minecraft\versions", "*.jar", IO.SearchOption.AllDirectories)
            ReDim GameName(GameJarPath.Length - 1)
            For i = 0 To GameJarPath.Length - 1
                GameName(i) = Split(GameJarPath(i), "\").Last
            Next
        Catch ex As Exception

        End Try
    End Sub
End Class
