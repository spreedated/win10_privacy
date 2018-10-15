Imports System.ComponentModel

Public Class frm_uninstall_useless_apps
    Private Sub frm_uninstall_useless_apps_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = myappfullname

        Dim applist As Array = {"BingFinance", "BingNews", "BingSports", "BingWeather", "GetStarted", "MicrosoftOfficeHub", "MicrosoftSolitaireCollection", "Office.OneNote", "People", "SkypeApp", "Windows.Photos", "WindowsAlarms", "WindowsCalculator", "WindowsCamera", "windowscommunicationsapps", "WindowsMaps", "WindowsPhone", "WindowsSoundRecorder", "XboxApp", "ZuneMusic", "ZuneVideo", "3DBuilder", "Messaging"}
        Dim checkpos_y As Integer = 12
        Dim checkpos_x As Integer = 12
        Dim count_boxes As Integer = 0

        For Each app In applist
            If count_boxes = 14 Then
                checkpos_x = 170
                checkpos_y = 12
            End If

            Dim p As New CheckBox
            With p
                .Text = app
                .Location = New Point(checkpos_x, checkpos_y)
                .Name = "chk_box_" & app
                .Checked = True
                .AutoSize = True
            End With
            Controls.Add(p)

            For Each i In Controls
                If i.text.ToString.Contains("Calc") Or i.text.ToString.Contains("SoundRecorder") Or i.text.ToString.Contains("Camera") Then
                    i.checked = False
                End If
            Next

            checkpos_y += 20
            count_boxes += 1
            Debug.Print("created box: " & count_boxes)
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
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim sel_apps As New ArrayList

        For Each i In Controls
            If i.name.ToString.Contains("chk_box_") Then
                If i.checked = True Then
                    sel_apps.Add(i.text)
                End If
            End If
        Next

        Dim arg_text As String = ""
        If CheckBox1.Checked Then
            arg_text = " -allusers"
        Else
            arg_text = ""
        End If
        sel_apps.Sort()
        sel_apps.Reverse()



        For Each i In sel_apps
            RunSpace("Get-AppxPackage" & arg_text & " *" & i & "* | Remove-AppxPackage")
        Next
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        For Each c In Controls
            c.enabled = True
        Next
        ProgressBar1.Visible = False

        MsgBox("All done, have fun!")
        Close()
    End Sub
End Class