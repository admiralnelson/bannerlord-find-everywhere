Imports TaleWorlds.GauntletUI

Public Class SearchBox
    Inherits Widget

    Dim isEnchancedPartyLoaded = False
    Public Sub New(ctx As UIContext)
        MyBase.New(ctx)
        isEnchancedPartyLoaded = HarmonyLib.Harmony.HasAnyPatches("top.hirtol.patch.partyenhancements")
        ' If isEnchancedPartyLoaded Then
        '     SetState("TransitionVisible")
        ' Else
        '     SetState("TransitionVisibleFull")
        ' End If
        SetState("TransitionInvisible")
        AddHandler PropertyChanged, AddressOf OnEvent
        AddHandler EventFire, AddressOf OnEvent
    End Sub

    Private Sub OnEvent(widget As PropertyOwnerObject,
                        propName As String,
                        val As Object)
        'Print(propName)
        Select Case propName
            'Case "SuggestedWidth"
            '    Print($"suggested width changed {CInt(val)}")
            '    'MyBase.SetState("")
            'Case "IsVisible"
            '    Print($"visible {CBool(val)}")
            '    Dim value As Boolean = val
            '    If value Then
            '        SetState("TransitionVisible")
            '    Else
            '        SetState("TransitionInvisible")
            '    End If
            '    Print($"state is {CurrentState}")
            Case "IsCollapsed"
                'Print($"IsCollapsed {CBool(val)}")
                Dim value As Boolean = val
                If value Then
                    If isEnchancedPartyLoaded Then
                        SetState("TransitionVisible")
                    Else
                        SetState("TransitionVisibleFull")
                    End If
                Else
                    SetState("TransitionInvisible")
                End If
                'Print($"state is {CurrentState}")
            Case Else

        End Select
    End Sub

    Private mIsCollapsed As Boolean
    <Editor(False)>
    Public Property IsCollapsed As Boolean
        Get
            Return IsCollapsed
        End Get
        Set(ByVal value As Boolean)
            If mIsCollapsed <> value Then
                mIsCollapsed = value
                OnPropertyChanged(value, NameOf(IsCollapsed))
                'Print($"OnPropertyChanged IsCollapsed {value}")
                'RefreshState()
            End If
        End Set
    End Property
End Class
