Public Class CoverLabel
    'Public Property Text As String
    '    Get
    '        Return Me.Content
    '    End Get
    '    Set(value As String)
    '        Me.Content = value
    '    End Set
    'End Property

    '按钮动画
    'Private Sub Label_MouseEnter(sender As Object, e As MouseEventArgs) Handles Me.MouseEnter
    '    '#FF039AE3 边框颜色
    '    Dim ca As New Animation.ColorAnimation
    '    ca.From = Color.FromArgb(&H0, &H3, &H9A, &HE3)
    '    ca.To = Color.FromArgb(&HFF, &H3, &H9A, &HE3)
    '    ca.Duration = TimeSpan.FromSeconds(0.3)
    '    sender.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ca)
    'End Sub
    'Private Sub Label_MouseLeave(sender As Object, e As MouseEventArgs) Handles Me.MouseLeave
    '    Dim ca As New Animation.ColorAnimation
    '    ca.From = Color.FromArgb("&h" & Mid(sender.BorderBrush.ToString, 2, 2), &H3, &H9A, &HE3) '垃圾语句
    '    ca.To = Color.FromArgb(&H0, &H3, &H9A, &HE3)
    '    ca.Duration = TimeSpan.FromSeconds(0.3)
    '    sender.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, ca)
    'End Sub

End Class
