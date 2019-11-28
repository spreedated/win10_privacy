Public Class Frm_Main

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = myappfullname

        ' Styling
        BackColor = myBackgroundColor

        TabControl1.TabPages.OfType(Of TabPage).All(Function(y)
                                                        y.BackColor = myBackgroundColor
                                                        y.Controls.OfType(Of Control).All(Function(x)
                                                                                              x.BackColor = myBackgroundColor
                                                                                              x.ForeColor = Color.White
                                                                                              Return True
                                                                                          End Function)
                                                        Return True
                                                    End Function)

        Controls.OfType(Of Control).All(Function(x)
                                            If x.GetType Is GetType(GroupBox) Then
                                                x.ForeColor = Color.White
                                            End If
                                            If x.GetType Is GetType(Button) Then
                                                x.ForeColor = Color.White
                                                x.BackColor = myBackgroundColor
                                            End If
                                            Return True
                                        End Function)
        '###
        Label2.Text = Current_copyright("2015", True, "Markus Karl Wackermann")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If CheckBox1.Checked = False And CheckBox2.Checked = False Then
            MsgBox("Please select at least one option!")
            Exit Sub
        End If

        Dim w As New Hostfile
        Dim para As String = Nothing

        If CheckBox1.Checked Then
            para += ",telemetry"
        End If
        If CheckBox2.Checked Then
            para += ",skype"
        End If

        RichTextBox1.Text = w.Add_entries(para, CheckBox3.Checked)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim w As New Hostfile
        RichTextBox1.Text = w.Delete_entries
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Remove_explorer_folders_new()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Remove_explorer_folders_new(True)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Frm_UninstallUselessApps.ShowDialog()
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=503249")
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(LinkLabel1.Text)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Remove_onedrive()
    End Sub
End Class