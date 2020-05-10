Imports System
Imports TaleWorlds.Core
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Runtime.CompilerServices
Module Util
    Public Sub Print(s As String)
        InformationManager.DisplayMessage(New InformationMessage(s))
    End Sub

    Public Sub MessageBoxBL(title As String, msg As String, Optional callbackOK As Action = Nothing, Optional callbackAbrt As Action = Nothing)
        Dim isYesNo = callbackAbrt IsNot Nothing
        If callbackOK Is Nothing Then
            callbackOK = Sub()

                         End Sub
        End If
        If callbackAbrt Is Nothing Then
            callbackAbrt = Sub()

                           End Sub
        End If
        InformationManager.ShowInquiry(New InquiryData(title, msg, True, isYesNo, "Accept", "Cancel", callbackOK, callbackAbrt))
    End Sub
End Module

