Class SettingPage
    Private Sub Page_Unloaded(sender As Object, e As RoutedEventArgs)
        My.Settings.Save() '退出时保存
    End Sub

End Class