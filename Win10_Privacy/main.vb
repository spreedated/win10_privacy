Imports System.IO
Imports Microsoft.Win32
Public Class main
    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = myappfullname

        ' Styling
        BackColor = myBackgroundColor

        For Each u In TabControl1.Controls
            u.backcolor = myBackgroundColor
            u.forecolor = Color.White
        Next

        GroupBox1.ForeColor = Color.White
        GroupBox2.ForeColor = Color.White
        GroupBox3.ForeColor = Color.White

        For Each i In Controls
            If i.ToString.Contains("Button") = True Then
                i.backcolor = myBackgroundColor
                i.forecolor = Color.White
            End If
        Next
        '###
        Label2.Text = current_copyright("2015", True, myappname)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If CheckBox1.Checked = False And CheckBox2.Checked = False Then
            MsgBox("Please select at least one option!")
            Exit Sub
        End If

        Dim w As New hostfile
        Dim para As String = Nothing

        If CheckBox1.Checked Then
            para += ",telemetry"
        End If
        If CheckBox2.Checked Then
            para += ",skype"
        End If

        If CheckBox3.Checked Then
            RichTextBox1.Text = w.add_entries(para, True)
        Else
            RichTextBox1.Text = w.add_entries(para, False)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim w As New hostfile
        RichTextBox1.Text = w.delete_entries
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        remove_explorer_folders_new()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        remove_explorer_folders_new(True)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        frm_uninstall_useless_apps.ShowDialog()
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=503249")
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(LinkLabel1.Text)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        remove_onedrive()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim i As New hostfile
        i.add_entries("telemetry")
    End Sub
End Class
