Module Module1
#Region "My Vars v0.1"
    Public myappname As String = "Win10 - Privacy"
    Public myappversnum As Integer = 1500
    Public myappvers As String = "v" & (myappversnum / 1000).ToString.Replace(",", ".")
    Public myappfullname As String = myappname & " " & myappvers
    '


    'Copyright Stuff
    Public Function current_copyright(ByVal fromyear As String, Optional ByVal Copyright_Icon As Boolean = True, Optional ByVal After_Copyright_Text As String = "") As String
        Dim s As String = Date.Today.Year.ToString
        Dim p As String = ""
        Dim c As String = ""
        Select Case Copyright_Icon
            Case True
                c = " © "
            Case False
                c = " (c) "
        End Select
        p = fromyear & "-" & s & c & After_Copyright_Text
        Return p
    End Function
#End Region

#Region "Styling"
    Public myBackgroundColor As Color = ColorTranslator.FromHtml("#303040")
#End Region
End Module
