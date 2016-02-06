
'启动器类
Public Class LaunchHelper
    '查找JAR包

    '=======================================================
    '待修改部分：
    '   Launcher：
    '       加入其它JVM参数
    '       加入正版登陆
    '       重定向控制台IO
    '       通过读取JSON获取启动参数(重要
    '=======================================================
    Public Function FindJAR(LibrariesPath) As String '传入搜索路径 返回分号分割的所有JAR包路径
        Dim Files() As String
        Files = IO.Directory.GetFiles(LibrariesPath, "*.jar", IO.SearchOption.AllDirectories)
        'Format Array to String
        For s As Integer = 0 To Files.Length - 1
            FindJAR = FindJAR & Files(s) & ";"
        Next
        FindJAR = Mid(FindJAR, 1, Len(FindJAR) - 1) '去掉最后一个分号并返回
    End Function

    Public Function MakeCP(LibrariesPath As String, VersionsPath As String) As String '传入Libraries路径和Versions路径 返回JVM启动的参数CP
        MakeCP = FindJAR(LibrariesPath) & ";" & VersionsPath
    End Function
    Public Sub Launcher(cp As String, UserName As String, Optional Xmx As Integer = 1024, Optional Xms As Integer = 1024) '此函数较多处待修改
        'cp 加载的包, 分号隔开
        'Xmx 设置JVM最大可用内存, 不含单位(m)
        'Xms 设置JVM最初始内存, 不含单位(m)
        'UserName 玩家名称
        Dim p As New Process
        With p
            With .StartInfo
                '使用net.minecraft.launchwrapper.Launch类启动将启用mod
                If My.Settings.UseFML Then
                    'FML启动参数==============================================================
                    .Arguments = "-Xmx" & Xmx & "m -Xms" & Xms & "m " &
                   "-Dfml.ignoreInvalidMinecraftCertificates=true " & '忽略无效的MC证书
                   "-Dfml.ignorePatchDiscrepancies=true " & '忽略补丁差异
                   "-Djava.library.path=" & Chr(34) & ".minecraft\natives" & Chr(34) & " " & 'Natives路径
                   "-cp " & Chr(34) & cp & Chr(34) &
                   "   net.minecraft.launchwrapper.Launch " & '启动主类
                       "--username " & UserName & " " & '玩家名称
                       "--session ${auth_session} " & '正版登陆session
                       "--version 1.6.4 " & '版本
                       "--gameDir .minecraft " & '.minecraft路径
                       "--assetsDir .minecraft\assets " &'assets路径
                       "--uuid ${auth_uuid} " &
                       "--accessToken ${auth_access_token} " &
                       "--tweakClass cpw.mods.fml.common.launcher.FMLTweaker"
                Else
                    '原版启动参数==================================================================
                    .Arguments = "-Xmx" & Xmx & "m -Xms" & Xms & "m " &
                    "-Dfml.ignoreInvalidMinecraftCertificates=true " & '忽略无效的MC证书
                    "-Dfml.ignorePatchDiscrepancies=true " & '忽略补丁差异
                    "-Djava.library.path=" & Chr(34) & ".minecraft\natives" & Chr(34) & " " & 'Natives路径
                    "-cp " & Chr(34) & cp & Chr(34) &
                    "   net.minecraft.client.main.Main " & '启动主类
                        "--username " & UserName & " " & '玩家名称
                        "--session ${auth_session} " & '正版登陆session
                        "--version 1.6.4 " & '版本
                        "--gameDir .minecraft " & '.minecraft路径
                        "--assetsDir .minecraft\assets " & 'assets路径
                        "--uuid ${auth_uuid} " &
                        "--accessToken ${auth_access_token}  "
                End If
                .FileName = My.Settings.CustomJavaPath '自定义java路径
                .WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory '设定工作目录为当前目录

                ''控制台输出重定向 功能未完成
                '.CreateNoWindow = True
                .UseShellExecute = False '不使用系统外壳启动
                '.RedirectStandardInput = True
                '.RedirectStandardOutput = True
                '.RedirectStandardError = True

            End With
            .Start() '启动游戏
        End With


    End Sub
End Class
