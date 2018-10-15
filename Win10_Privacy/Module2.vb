Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.Win32
Module Module2
#Region "Host file entries -- 100% -- OBSOLETE"
    Public Function h_file_entries() As String
        h_file_entries = Nothing

        'check if can be opened
        Dim s As String = Nothing
        Dim u As StreamReader
        u = My.Computer.FileSystem.OpenTextFileReader("C:\Windows\System32\drivers\etc\hosts", System.Text.Encoding.ASCII)

        s = u.ReadToEnd

        u.Close()


        If s.Contains("Microsoft Corp") = True Then
            h_file_entries = "open;"
        Else
            h_file_entries = "failopen;"
        End If


        If s.Contains("# Win10 - Privacy") = True Then
            h_file_entries = "already;"
        End If
        '###

        'Add entries
        Dim b As StreamWriter
        b = My.Computer.FileSystem.OpenTextFileWriter("C:\Windows\System32\drivers\etc\hosts", True, System.Text.Encoding.ASCII)

        b.WriteLine("")
        b.WriteLine("# " & myappfullname)
        b.Write(My.Resources.strings_of_hosts)
        b.WriteLine("")
        b.WriteLine("# End of " & myappfullname)
        b.WriteLine("")
        b.Close()

        h_file_entries = "success;"

        Return h_file_entries
    End Function

    Public Function remove_h_file_entries() As String
        remove_h_file_entries = Nothing
        'check if can be opened
        Dim s As String = Nothing
        Dim u As StreamReader
        u = My.Computer.FileSystem.OpenTextFileReader("C:\Windows\System32\drivers\etc\hosts", System.Text.Encoding.ASCII)

        s = u.ReadToEnd

        u.Close()


        If s.Contains("# Win10 - Privacy") = False Then
            remove_h_file_entries = "notfound"
            Exit Function
        End If

        s = s.Substring(0, s.IndexOf("# Win10 - Privacy"))

        Dim b As StreamWriter
        b = My.Computer.FileSystem.OpenTextFileWriter("C:\Windows\System32\drivers\etc\hosts", False, System.Text.Encoding.ASCII)

        b.Write(s)
        b.Close()
        remove_h_file_entries = "success"
        Return remove_h_file_entries
    End Function
#End Region

#Region "Remove Explorer Folders -- 100% -- OBSOLETE"
    Private myKeys As New ArrayList
    Private is_populate As Boolean = False
    Private Sub populate_array()
        If Not is_populate Then
            is_populate = True
            'Keys
            myKeys.Add("{088e3905-0323-4b02-9826-5d99428e115f};Downloads") 'Downloads
            myKeys.Add("{24ad3ad4-a569-4530-98e1-ab02f9417aa8};Pictures") 'Pictures
            myKeys.Add("{3dfdf296-dbec-4fb4-81d1-6a3438bcf4de};Music") 'Music
            myKeys.Add("{d3162b92-9365-467a-956b-92703aca08af};Desktop") 'Desktop
            myKeys.Add("{f86fa3ab-70d2-4fc7-9c99-fcbf05467f3a};Videos") 'Videos
            myKeys.Add("{B4BFCC3A-DB2C-424C-B029-7FE99A87C641};Desktop") 'Desktop
            '###
        End If
    End Sub
    Public Function remove_explorer_folders() As String
        Dim old_path As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace"
        Dim new_path As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions"

        remove_explorer_folders = Nothing
        populate_array()
        Dim output As String = Nothing


        '32-Bit Registry
        Dim RegTyp32 As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
        Dim Key As RegistryKey = RegTyp32.OpenSubKey(new_path, False, Security.AccessControl.RegistryRights.FullControl)


        For Each i In Key.GetSubKeyNames
            For Each obj In myKeys
                'getKeyname
                Dim l() As String = obj.ToString.Split(";")
                '###

                If l(0) = i.ToString Then
                    RegTyp32.DeleteSubKey(new_path & l(0), False)
                    output += l(1) & " has been found and removed from 32-bit Registry!" & vbCrLf
                End If
            Next
        Next

        '64-Bit Registry
        Dim RegTyp64 As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
        Dim Key64 As RegistryKey = RegTyp64.OpenSubKey(new_path, False, Security.AccessControl.RegistryRights.FullControl)


        For Each i In Key64.GetSubKeyNames
            For Each obj In myKeys
                'getKeyname
                Dim l() As String = obj.ToString.Split(";")
                '###

                If l(0) = i.ToString Then
                    RegTyp64.DeleteSubKey(new_path & l(0), False)
                    output += l(1) & " has been found and removed from 64-bit Registry!" & vbCrLf
                End If
            Next
        Next

        If Not IsNothing(output) Then
            MsgBox(output & vbCrLf & "Restarting explorer now...")

            'Restarting explorer.exe
            Dim exp_proc() As Process = Nothing


            exp_proc = Process.GetProcessesByName("explorer")

            For Each obj In exp_proc
                obj.Kill()
                obj.WaitForExit()
            Next
        Else
            MsgBox("Nothing found! - Seems to be already removed")
        End If

        Return remove_explorer_folders
    End Function

    Public Function restore_explorer_folders() As String
        restore_explorer_folders = Nothing

        populate_array()
        Dim output As String = Nothing

        '32-Bit Registry
        Dim RegTyp32 As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)

        For Each i In myKeys
            'getKeyname
            Dim l() As String = i.ToString.Split(";")
            '###

            RegTyp32.CreateSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\" & l(0))

            output += l(1) & " has been added to 32-bit Registry!" & vbCrLf
        Next

        '64-Bit Registry
        Dim RegTyp64 As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)

        For Each i In myKeys
            'getKeyname
            Dim l() As String = i.ToString.Split(";")
            '###

            RegTyp64.CreateSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace\" & l(0))

            output += l(1) & " has been added to 64-bit Registry!" & vbCrLf
        Next

        If Not IsNothing(output) Then
            MsgBox(output & vbCrLf & "Restarting explorer now...")

            'Restarting explorer.exe
            Dim exp_proc() As Process = Nothing


            exp_proc = Process.GetProcessesByName("explorer")

            For Each obj In exp_proc
                obj.Kill()
                obj.WaitForExit()
            Next
        Else
            MsgBox("Nothing found! - Seems to be already removed")
        End If
    End Function
#End Region

#Region "Remove Explorer Folders -- 100% -- 25.10.2017"
    Private myKeysn As New ArrayList
    Private is_populaten As Boolean = False
    Dim new_path As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions"
    Private Sub populate_arrayn()
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
    Public Sub remove_explorer_folders_new(ByVal Optional restore As Boolean = False)
        'Ask user to confirm
        Dim user_choice As String = Nothing
        If restore Then
            user_choice = "Show"
        Else
            user_choice = "Hide"
        End If
        If MsgBox("Are you sure to " & user_choice & " items?" & vbCrLf & vbCrLf & "explorer.exe will be restarted!", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Exit Sub
        End If
        '###

        populate_arrayn()
        Dim output As String = Nothing


        '32-Bit Registry
        Dim RegTyp32 As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
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

        '64-Bit Registry
        Dim RegTyp64 As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
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

        If Not IsNothing(output) Then
            MsgBox(output & vbCrLf & "Restarting explorer now...")

            'Restarting explorer.exe
            Dim exp_proc() As Process = Nothing

            exp_proc = Process.GetProcessesByName("explorer")

            For Each obj In exp_proc
                obj.Kill()
                obj.WaitForExit()
            Next

        Else
            MsgBox("Nothing found! - Seems to be already removed")
        End If
    End Sub
#End Region


#Region "Remove OneDrive -- 0%"
    '%SYSTEMROOT%\SYSWOW64\ONEDRIVESETUP.EXE uninstall

    'HKEY_CLASSES_ROOT\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\ShellFolder
    'Attributes  REG_DWORD 0

    'HKEY_CLASSES_ROOT\Wow6432Node\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\ShellFolder
    'Attributes  REG_DWORD 0

    Public Sub remove_onedrive()
        'uninstall process
        Dim p As New Process
        Dim p_set As New ProcessStartInfo
        With p_set
            .FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\SYSWOW64\ONEDRIVESETUP.EXE"
            .Arguments = "uninstall"
            .UseShellExecute = True
            .CreateNoWindow = True
        End With
        With p
            .StartInfo = p_set
            .Start()
            .WaitForExit(10000)
        End With

        'remove folders
        If Directory.Exists("C:\OneDriveTemp") Then
            Directory.Delete("C:\OneDriveTemp")
        End If
        If Directory.Exists("C:\OneDriveTemp") Then
            Directory.Delete("C:\OneDriveTemp")
        End If


        'run powershell_stuff
        RunSpace("New-PSDrive -PSProvider registry -Root HKEY_CLASSES_ROOT -Name HKCR; New-ItemProperty -path ""HKCR:\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\ShellFolder"" -Name ""Attributes"" -Value ""0"" -PropertyType DWORD -Force")
        RunSpace("New-PSDrive -PSProvider registry -Root HKEY_CLASSES_ROOT -Name HKCR; New-ItemProperty -path ""HKCR:\Wow6432Node\CLSID\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\ShellFolder"" -Name ""Attributes"" -Value ""0"" -PropertyType DWORD -Force")




        MsgBox("Restarting explorer now...")
        'Restarting explorer.exe
        Dim exp_proc() As Process = Nothing
        exp_proc = Process.GetProcessesByName("explorer")

        For Each obj In exp_proc
            obj.Kill()
            obj.WaitForExit()
        Next

    End Sub
#End Region

    Class hostfile
        Private path_to_hostfile As String = "C:\\Windows\\System32\\drivers\\etc\\hosts"
        Private host_file_text As String = Nothing
        Private Function get_hostfile() As String
            get_hostfile = Nothing
            If File.Exists(path_to_hostfile) = True Then
                Try
                    get_hostfile = File.ReadAllText(path_to_hostfile, System.Text.Encoding.ASCII)
                Catch ex As Exception
                End Try
            End If
            Return get_hostfile
        End Function
        Public Function delete_entries()
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
        Public Function add_entries(ByVal parameters As String, ByVal Optional make_backup As Boolean = True)
            Dim s As String = get_hostfile() & vbCrLf & vbCrLf
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
