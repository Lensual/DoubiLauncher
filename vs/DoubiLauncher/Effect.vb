'把常用WPF动画封装好的模块
Imports System.Windows.Media.Animation
Imports System.Windows.Threading

Module Effect
    Public Sub Fade(Obj As Object, FadeFrom As Double, FadeTo As Double, Milliseconds As Long, Optional Wait As Boolean = False)
        Dim DA As New DoubleAnimation
        DA.From = FadeFrom
        DA.To = FadeTo
        DA.Duration = TimeSpan.FromMilliseconds(Milliseconds)
        Obj.BeginAnimation(UIElement.OpacityProperty, DA)
        If Wait Then
            Do Until Obj.Opacity = DA.To
                DoEvent()
            Loop
        End If
    End Sub
    Public Sub FadeIn(Obj As Object, Milliseconds As Long, Optional Wait As Boolean = False)
        Fade(Obj, 0, 1, Milliseconds, Wait)
    End Sub
    Public Sub FadeOut(Obj As Object, Milliseconds As Long, Optional Wait As Boolean = False)
        Fade(Obj, 1, 0, Milliseconds, Wait)
    End Sub


    '自制DoEvent
    Public Sub DoEvent()
        Dim Frame As New DispatcherFrame()
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, New DispatcherOperationCallback(AddressOf ExitFrame), Frame)
        Dispatcher.PushFrame(Frame)
    End Sub
    Public Function ExitFrame(f As Object) As Object
        CType(f, DispatcherFrame).Continue = False
        Return Nothing
    End Function
End Module
