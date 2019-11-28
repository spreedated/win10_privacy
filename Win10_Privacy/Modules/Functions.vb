Imports System.IO
Imports Microsoft.Win32

Module Functions

#Region "Remove Explorer Folders -- 100% -- 25.10.2017"

    Private ReadOnly myKeysn As New ArrayList
    Private is_populaten As Boolean = False
    Const new_path As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions"

    Private Sub Populate_arrayn()
        If Not is_populaten Then
            is_populaten = True
            'Keys
            myKeysn.Add("{7d83ee9b-2244-4e70-b1f5-5393042af1e4};Downloads") 'Downloads
            myKeysn.Add("{0ddd015d-b06c-45d5-8c4c-f59713854639};Pictures") 'Pictures
            myKeysn.Add("{a0c69a99-21c8-4671-8703-7934162fcf1d};Music") 'Music
            myKeysn.Add("{B4BFCC3A-DB2C-424C-B029-7FE99A87C641};Desktop") 'Desktop
            myKeysn.Add("{35286a68-3c57-41a1-bbb1-0eae73d76c95};Videos") 'Videos
            myKeysn.Add("{f42ee2d3-909f-4907-8871-4c22fc0bf756};Documents") 'Documents
            myKeysn.Add("{31C0DD25-9439-4F12-BF41-7FF4EDA38722};3D-Objects") '3D-Objects
            '###
        End If
    End Sub

    Public Sub Remove_explorer_folders_new(ByVal Optional restore As Boolean = False)
        'Ask user to confirm
        Dim user_choice As String
        If restore Then
            user_choice = "Show"
        Else
            user_choice = "Hide"
        End If
        If MsgBox("Are you sure to " & user_choice & " items?" & vbCrLf & vbCrLf & "explorer.exe will be restarted!", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Exit Sub
        End If
        '###

        Populate_arrayn()
        Dim output As String = Nothing

        '32-Bit Registry
        Using RegTyp32 As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
            Dim Key As RegistryKey = RegTyp32.OpenSubKey(new_path, True)

            For Each i In Key.GetSubKeyNames
                For Each obj In myKeysn
                    'getKeyname
                    Dim l() As String = obj.ToString.Split(";")
                    '###

                    If l(0) = i.ToString Then
                        RegTyp32.CreateSubKey(new_path & "\" & l(0) & "\PropertyBag")
                        Dim acc = RegTyp32.OpenSubKey(new_path & "\" & l(0) & "\PropertyBag", True)
                        acc.SetValue("ThisPCPolicy", user_choice)
                        output += l(1) & " has been set to " & user_choice & " in 32-bit Registry!" & vbCrLf
                        acc.Close()
                    End If
                Next
            Next
        End Using

        '64-Bit Registry
        Using RegTyp64 As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
            Dim Key64 As RegistryKey = RegTyp64.OpenSubKey(new_path, True)

            For Each i In Key64.GetSubKeyNames
                For Each obj In myKeysn
                    'getKeyname
                    Dim l() As String = obj.ToString.Split(";")
                    '###

                    If l(0) = i.ToString Then
                        RegTyp64.CreateSubKey(new_path & "\" & l(0) & "\PropertyBag")
                        Dim acc = RegTyp64.OpenSubKey(new_path & "\" & l(0) & "\PropertyBag", True)
                        acc.SetValue("ThisPCPolicy", user_choice)
                        output += l(1) & " has been found and set to " & user_choice & " in 64-bit Registry!" & vbCrLf
                        acc.Close()
                    End If
                Next
            Next
        End Using

        If Not IsNothing(output) Then
            MsgBox(output & vbCrLf & "Restarting explorer now...")

            'Restarting explorer.exe
            Dim exp_proc() As Process = Process.GetProcessesByName("explorer")

            For Each obj In exp_proc
                obj.Kill()
                obj.WaitForExit()
            Next
        Else
            MsgBox("Nothing found! - Seems to be already removed")
        End If
    End Sub

#End Region

    Class Hostfile
        Private ReadOnly path_to_hostfile As String = "C:\\Windows\\System32\\drivers\\etc\\hosts"

        Private Function Get_hostfile() As String
            Get_hostfile = Nothing
            If File.Exists(path_to_hostfile) = True Then
                Try
                    Get_hostfile = File.ReadAllText(path_to_hostfile, System.Text.Encoding.ASCII)
                Catch ex As Exception
                End Try
            End If
            Return Get_hostfile
        End Function

        Public Function Delete_entries()
            Dim output As String = ""

            If File.Exists(path_to_hostfile & ".bak") Then
                File.Delete(path_to_hostfile)
                File.Move(path_to_hostfile & ".bak", path_to_hostfile)
                output &= "Backup file found and restored!"
            Else
                output &= "No Backup file found ;(" & vbCrLf &
                    "Nothing changed."
            End If

            Return output
        End Function

        Public Function Add_entries(ByVal parameters As String, ByVal Optional make_backup As Boolean = True)
            Dim s As String = Get_hostfile() & vbCrLf & vbCrLf
            Dim acc As String = Nothing
            Dim to_add As New ArrayList
            Dim output As String = ""

            If parameters.Contains("telemetry") Then
                For Each line In My.Resources.strings_of_hosts.Split(vbCr)
                    to_add.Add(line)
                Next
            End If

            If parameters.Contains("skype") Then
                For Each line In My.Resources.strings_of_hosts_skype.Split(vbCr)
                    to_add.Add(line)
                Next
            End If

            Dim count_added_items As Short = 0

            For Each item In to_add
                If s.Contains(item) = False Then
                    s = s & vbCr & item
                    count_added_items += 1
                End If
            Next
            output &= count_added_items.ToString & " entries were made!" & vbCrLf

            If count_added_items = 0 Then
                Return "Nothing changed, all entries were already found."
            Else
                If make_backup Then
                    'do backup
                    File.Copy(path_to_hostfile, path_to_hostfile & ".bak", True)
                    output &= "Backup file named ""hosts.bak"" created." & vbCrLf
                End If
                File.WriteAllText(path_to_hostfile, s)
            End If

            Return output
        End Function

    End Class

End Module