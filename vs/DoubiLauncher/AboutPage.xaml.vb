Class AboutPage
    Private Sub label1_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Process.Start("http://lensual.dreamerstudio.net/blog")
    End Sub

    Private Sub label2_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Process.Start("http://www.dreamerstudio.net/doubilauncher")
    End Sub

    Private Sub label3_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Process.Start("http://github.com/Lensual/DoubiLauncher")
    End Sub

    Private Sub label6_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles label6.MouseDown
        Clipboard.SetData(DataFormats.Text, "370619976")
        MsgBox("复制成功喵")
    End Sub
End Class
