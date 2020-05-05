Imports System.Collections.Generic
Imports System.Linq
Imports SandBox.GauntletUI
Imports TaleWorlds.CampaignSystem
Imports TaleWorlds.CampaignSystem.ViewModelCollection
Imports TaleWorlds.Library

Public Class SearchPartyViewModel
    Inherits ViewModel
    Const leftSide As Integer = PartyScreenLogic.PartyRosterSide.Left
    Const rightSide As Integer = PartyScreenLogic.PartyRosterSide.Right
    Dim searchTermLeft As String
    Dim searchTermRight As String
    Dim bLeftVisible As Boolean = False
    Dim bRightVisible As Boolean = False
    Dim partyCharsViewM As MBBindingList(Of PartyCharacterVM)
    Dim partylogic As PartyScreenLogic
    Dim originalPartyList As List(Of PartyCharacterVM)
    Shared instance As SearchPartyViewModel
    Public Sub New(pvm As PartyVM,
                   psl As PartyScreenLogic,
                   parentScreen As GauntletPartyScreen)
        partyCharsViewM = pvm.MainPartyTroops
        partylogic = psl
        AddHandler psl.AfterReset, AddressOf AfterReset
        AddHandler psl.Update, AddressOf UpdateLabel
        originalPartyList = pvm.MainPartyTroops.ToList()
        instance = Me
    End Sub

    Public Sub AfterReset(logic As PartyScreenLogic)
        Print("search pvm has been reset")
    End Sub

    Public Sub UpdateLabel(command As PartyScreenLogic.PartyCommand)

    End Sub

    Public Overloads Sub OnFinalize()

    End Sub

    Public Overrides Sub RefreshValues()
        MyBase.RefreshValues()

    End Sub

    Public Sub HideShowSearch(leftOrRight As Integer)

    End Sub
    Public Shared Sub Reset()
        If instance Is Nothing Then Exit Sub
        instance.partyCharsViewM.Clear()
        For Each x In instance.originalPartyList
            instance.partyCharsViewM.Add(x)
        Next
    End Sub
    Public Shared Sub Filter(keyword As String)
        If instance Is Nothing Then Exit Sub
        If keyword Is "" Then
            Reset()
            Exit Sub
        End If
        instance.partyCharsViewM.Clear()
        'can't use list compreshension here :(
        'instance.partyCharsViewM = instance.originalPartyList.Where(Function(x) x.Troop.Character.Name.Contains(keyword))
        'good ol loop
        For Each x In instance.originalPartyList
            If x.Troop.Character.Name.Contains(keyword) Then
                instance.partyCharsViewM.Add(x)
            End If
        Next
    End Sub

    <DataSourceProperty>
    Public Property SearchLeft As String
        Get
            Return searchTermLeft
        End Get
        Set(ByVal value As String)
            If value <> searchTermLeft Then
                searchTermLeft = value
                OnPropertyChanged(NameOf(SearchLeft))
                'Print("value left changed " + value)
            End If
        End Set
    End Property
    <DataSourceProperty>
    Public Property SearchRight As String
        Get
            Return searchTermRight
        End Get
        Set(ByVal value As String)
            If value <> searchTermRight Then
                searchTermRight = value
                OnPropertyChanged(NameOf(SearchRight))
                'Print("value right changed " + value)
                Filter(value)
            End If
        End Set
    End Property
    <DataSourceProperty>
    Public Property LeftVisible As Boolean
        Get
            Return bLeftVisible
        End Get
        Set(ByVal value As Boolean)
            If value <> bLeftVisible Then
                bLeftVisible = value
                OnPropertyChanged(NameOf(LeftVisible))
            End If
        End Set
    End Property
    <DataSourceProperty>
    Public Property RightVisible As Boolean
        Get
            Return bRightVisible
        End Get
        Set(ByVal value As Boolean)
            If value <> bRightVisible Then
                bRightVisible = value
                OnPropertyChanged(NameOf(RightVisible))
            End If
        End Set
    End Property
End Class
