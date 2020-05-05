Imports TaleWorlds.Core

Module Util
    Public Sub Print(s As String)
        InformationManager.DisplayMessage(New InformationMessage(s))
    End Sub
End Module
