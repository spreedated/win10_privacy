Imports System.Reflection

Module AppProperties

#Region "My Vars v1.0"

    Public myappname As String = "Win10 - Privacy"
    'Public myappfullnameString As String = myappname & " " & myappvers
    '

    'Copyright Stuff
    Public Function Current_copyright(ByVal fromyear As String, Optional ByVal Copyright_Icon As Boolean = True, Optional ByVal After_Copyright_Text As String = "") As String
        Dim s As String = Date.Today.Year.ToString
        Dim c As String = ""
        Select Case Copyright_Icon
            Case True
                c = " © "
            Case False
                c = " (c) "
        End Select
        Dim p As String = fromyear & "-" & s & c & After_Copyright_Text
        Return p
    End Function

    Public Function MyAppFullName() As String
        Return Assembly.GetExecutingAssembly().GetName().Name & " " & CreateVersion()
    End Function

    Private Function CreateVersion() As String
        Dim acc As String = Convert.ToString(Assembly.GetExecutingAssembly().GetName().Version.Major)
        acc &= Convert.ToString(Assembly.GetExecutingAssembly().GetName().Version.Minor)
        acc &= Convert.ToString(If(Assembly.GetExecutingAssembly().GetName().Version.Build > 0, Assembly.GetExecutingAssembly().GetName().Version.Build, ""))
        acc &= Convert.ToString(If(Assembly.GetExecutingAssembly().GetName().Version.Revision > 0, Assembly.GetExecutingAssembly().GetName().Version.Revision, ""))

        Dim output As String = Nothing
        For i = 0 To acc.Length - 1
            output &= acc(i) & "."
        Next
        output = output.TrimEnd(".")

        Return output
    End Function

#End Region

#Region "Styling"

    Public myBackgroundColor As Color = ColorTranslator.FromHtml("#303040")

#End Region

End Module