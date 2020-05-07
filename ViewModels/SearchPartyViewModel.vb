Imports System.Collections.Generic
Imports System.Linq
Imports SandBox.GauntletUI
Imports TaleWorlds.CampaignSystem
Imports TaleWorlds.CampaignSystem.ViewModelCollection
Imports TaleWorlds.Library
Imports System.Diagnostics

Public Class SearchPartyViewModel
    Inherits ViewModel

    Dim searchTermLeft As String
    Dim searchTermRight As String
    Dim bLeftVisible As Boolean = False
    Dim bRightVisible As Boolean = False
    Dim partyCharsViewM, partyCharsPrisonerViewM As MBBindingList(Of PartyCharacterVM)
    Dim otherPartyCharsViewM, otherPartyCharsPrisonerViewM As MBBindingList(Of PartyCharacterVM)

    Dim originalPartyList, originalPartyPrisonerList As List(Of PartyCharacterVM)
    Dim originalOtherPartyList, originalOtherPartyPrisonerList As List(Of PartyCharacterVM)

    Dim partyScreenController As PartyScreenLogic
    Dim partyViewModel As PartyVM

    Shared mInstance As SearchPartyViewModel
    Dim showRightSearchPanel = False
    Dim showLeftSearchPanel = False

    Public Shared ReadOnly Property Instance As SearchPartyViewModel
        Get
            Return mInstance
        End Get
    End Property
    Public Sub New(pvm As PartyVM,
                   psl As PartyScreenLogic)
        partyScreenController = psl
        partyViewModel = pvm

        partyCharsViewM = pvm.MainPartyTroops
        partyCharsPrisonerViewM = pvm.MainPartyPrisoners
        otherPartyCharsViewM = pvm.OtherPartyPrisoners
        otherPartyCharsPrisonerViewM = pvm.OtherPartyPrisoners

        UpdatePartyList(Nothing)

        AddHandler psl.Update, AddressOf UpdatePartyList
        mInstance = Me
    End Sub

    Public Sub UpdatePartyList(cmd As PartyScreenLogic.PartyCommand)
        Print("changes in party state")

        originalPartyList = partyViewModel.MainPartyTroops.Where(Function(x) x.Number > 0).ToList()
        originalPartyPrisonerList = partyViewModel.MainPartyPrisoners.Where(Function(x) x.Number > 0).ToList()
        originalOtherPartyList = partyViewModel.OtherPartyTroops.Where(Function(x) x.Number > 0).ToList()
        originalOtherPartyPrisonerList = partyViewModel.OtherPartyPrisoners.Where(Function(x) x.Number > 0).ToList()
    End Sub

    Public Sub UpdateLabel(command As PartyScreenLogic.PartyCommand)

    End Sub

#Region "Left Side"
    Public Sub FindLeftPane()
        LeftVisible = Not LeftVisible
        'Print($"find left clicked state {LeftVisible}")
    End Sub
    Public Sub ResetLeft()
        otherPartyCharsViewM.Clear()
        otherPartyCharsPrisonerViewM.Clear()
        For Each x In originalOtherPartyList
            otherPartyCharsViewM.Add(x)
        Next
        For Each x In originalOtherPartyPrisonerList
            otherPartyCharsPrisonerViewM.Add(x)
        Next
    End Sub
    Public Sub FilterLeft(keyword As String)
        If keyword Is "" Then
            ResetLeft()
            Exit Sub
        End If
        otherPartyCharsViewM.Clear()
        otherPartyCharsPrisonerViewM.Clear()
        'can't use list compreshension here :(
        'partyCharsViewM = originalPartyList.Where(Function(x) x.Troop.Character.Name.Contains(keyword))
        'good ol loop
        For Each x In originalOtherPartyList
            If x.Troop.Character.ToString().ToLower().Contains(keyword.ToLower()) And x.Troop.Number > 0 Then
                otherPartyCharsViewM.Add(x)
            End If
        Next
        For Each x In originalOtherPartyPrisonerList
            If x.Troop.Character.ToString().ToLower().Contains(keyword.ToLower()) And x.Troop.Number > 0 Then
                otherPartyCharsPrisonerViewM.Add(x)
            End If
        Next
    End Sub
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
    Public Property SearchLeft As String
        Get
            Return searchTermLeft
        End Get
        Set(ByVal value As String)
            If value <> searchTermLeft Then
                searchTermLeft = value
                OnPropertyChanged(NameOf(SearchLeft))
                'Print("value left changed " + value)
                FilterLeft(value)
            End If
        End Set
    End Property
#End Region

#Region "Right Side"

    Public Sub FindRightPane()
        RightVisible = Not RightVisible
        'Print($"find right clicked state {RightVisible}")
    End Sub
    Public Sub ResetRight()
        partyCharsViewM.Clear()
        partyCharsPrisonerViewM.Clear()
        For Each x In originalPartyList
            partyCharsViewM.Add(x)
        Next
        For Each x In originalPartyPrisonerList
            partyCharsPrisonerViewM.Add(x)
        Next
    End Sub

    Public Sub FilterRight(keyword As String)
        If keyword Is "" Then
            ResetRight()
            Exit Sub
        End If
        partyCharsViewM.Clear()
        partyCharsPrisonerViewM.Clear()
        'can't use list compreshension here :(
        'partyCharsViewM = originalPartyList.Where(Function(x) x.Troop.Character.Name.Contains(keyword))
        'good ol loop
        For Each x In originalPartyList
            If x.Troop.Character.ToString().ToLower().Contains(keyword.ToLower()) And x.Troop.Number > 0 Then
                partyCharsViewM.Add(x)
            End If
        Next
        For Each x In originalPartyPrisonerList
            If x.Troop.Character.ToString().ToLower().Contains(keyword.ToLower()) And x.Troop.Number > 0 Then
                partyCharsPrisonerViewM.Add(x)
            End If
        Next
    End Sub

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
                FilterRight(value)
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
#End Region

    <DataSourceProperty>
    Public Property IconMargin As Single
        Get
            Dim partyenhancementsLoaded = HarmonyLib.Harmony.HasAnyPatches("top.hirtol.patch.partyenhancements")
            If Not partyenhancementsLoaded Then
                Return 600
            End If
            Return 550
        End Get
        Set(value As Single)
            OnPropertyChanged(NameOf(IconMargin))
        End Set
    End Property


End Class
