Imports System.ComponentModel

Public Class Frm_UninstallUselessApps

    Private Sub Frm_uninstall_useless_apps_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = myappfullname

        Dim applist As Object(,) = {
            {True, "BingFinance""BingNews"},
            {True, "BingSports"},
            {True, "BingWeather"},
            {True, "GetStarted"},
            {True, "MicrosoftOfficeHub"},
            {False, "MicrosoftSolitaireCollection"},
            {True, "Office.OneNote"},
            {True, "People"},
            {True, "SkypeApp"},
            {False, "Windows.Photos"},
            {True, "WindowsAlarms"},
            {False, "WindowsCalculator"},
            {False, "WindowsCamera"},
            {True, "windowscommunicationsapps"},
            {False, "WindowsMaps"},
            {True, "WindowsPhone"},
            {False, "WindowsSoundRecorder"},
            {True, "XboxApp"},
            {True, "ZuneMusic"},
            {True, "ZuneVideo"},
            {True, "3DBuilder"},
            {True, "Messaging"}
        }
        Dim checkpos_y As Integer = 12
        Dim checkpos_x As Integer = 12
        Dim count_boxes As Integer = 0

        'Get currently installed Apps
        Dim currentlyInstalled As String() = {}
        Using p As Process = New Process With {
                .StartInfo = New ProcessStartInfo With {
                    .FileName = "powershell.exe",
                    .Arguments = "Get-AppxPackage * | Format-Table Name -HideTableHeaders",
                    .CreateNoWindow = True,
                    .UseShellExecute = False,
                    .RedirectStandardOutput = True
                }
            }
            p.Start()
            While Not p.StandardOutput.EndOfStream
                Dim acc As String = p.StandardOutput.ReadLine.Replace(" ", "")
                If acc.Length >= 1 Then
                    Array.Resize(Of String)(currentlyInstalled, currentlyInstalled.Length + 1)
                    currentlyInstalled(currentlyInstalled.Length - 1) = acc
                End If
            End While
            p.WaitForExit(New TimeSpan(0, 0, 30).TotalMilliseconds)
        End Using
        '# ### #

        For i = 0 To applist.GetLength(0) - 1
            Dim inter As Integer = i
            Dim acc As String = currentlyInstalled.Where(Function(x) x.Contains(Convert.ToString(applist(inter, 1)))).FirstOrDefault
            If acc Is Nothing Then
                Continue For
            End If

            If count_boxes = 14 Then
                checkpos_x = 170
                checkpos_y = 12
            End If

            Dim p As New CheckBox
            With p
                .Text = Convert.ToString(applist(inter, 1))
                .Location = New Point(checkpos_x, checkpos_y)
                .Name = "chk_box_" & Convert.ToString(applist(inter, 1))
                .Checked = True
                .AutoSize = True
            End With
            Controls.Add(p)

            Controls.OfType(Of CheckBox).All(Function(x)
                                                 If x.Name.Contains(Convert.ToString(applist(inter, 1))) Then
                                                     x.Checked = Convert.ToBoolean(applist(inter, 0))
                                                 End If
                                                 Return True
                                             End Function)

            checkpos_y += 20
            count_boxes += 1
        Next

        'styling
        BackColor = myBackgroundColor

        For Each i In Controls
            If i.ToString.Contains("Button") Or i.name.ToString.Contains("chk_box_") Or i.name.ToString.ToLower.Contains("checkbox") Then
                i.backcolor = myBackgroundColor
                i.forecolor = Color.White
            End If
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For Each c In Controls
            c.enabled = False
        Next
        ProgressBar1.Visible = True
        ExecutePS()
    End Sub

    Private Async Sub ExecutePS()
        Dim sel_apps As New ArrayList

        For Each i In Controls
            If i.name.ToString.Contains("chk_box_") Then
                If i.checked = True Then
                    sel_apps.Add(i.text)
                End If
            End If
        Next

        Dim arg_text As String

        If Chk_AllUsers.Checked Then
            arg_text = "-allusers"
        Else
            arg_text = ""
        End If
        sel_apps.Sort()
        sel_apps.Reverse()

        Dim t As List(Of Task) = New List(Of Task)

        For Each i In sel_apps
            t.Add(New Task(Sub()
                               Using p As Process = New Process With {
                                   .StartInfo = New ProcessStartInfo With {
                                       .FileName = "powershell.exe",
                                       .Arguments = String.Format("Get-AppxPackage {0} *{1}* | Remove-AppxPackage", arg_text, i),
                                       .CreateNoWindow = False,
                                       .WindowStyle = ProcessWindowStyle.Normal
                                   }
                               }
                                   p.Start()
                                   p.WaitForExit(New TimeSpan(0, 0, 30).TotalMilliseconds)
                               End Using
                           End Sub))
        Next
        t.All(Function(x)
                  x.Start()
                  Return True
              End Function)
        Await Task.WhenAll(t)

        For Each c In Controls
            c.enabled = True
        Next
        ProgressBar1.Visible = False

        MsgBox("All done, have fun!")
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Controls.OfType(Of CheckBox).All(Function(x)
                                             If x.Name = "Chk_AllUsers" Then
                                                 Return True
                                             End If
                                             x.Checked = False
                                             Return True
                                         End Function)
    End Sub
End Class