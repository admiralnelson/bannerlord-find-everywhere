Imports System.Collections.Generic
Imports System.Linq
Imports SandBox.GauntletUI
Imports TaleWorlds.CampaignSystem
Imports TaleWorlds.CampaignSystem.ViewModelCollection
Imports TaleWorlds.Library
Imports System.Diagnostics
Imports TaleWorlds.Core.ViewModelCollection
Imports TaleWorlds.MountAndBlade
Imports TaleWorlds.Core
Imports System

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
    Dim missionCtx As Mission
    Dim gameCtx As Game

    Public Shared ReadOnly Property Instance As SearchPartyViewModel
        Get
            Return mInstance
        End Get
    End Property
    Public Sub New(pvm As PartyVM,
                   psl As PartyScreenLogic,
                   gctx As Game)
        partyScreenController = psl
        partyViewModel = pvm
        missionCtx = Mission.Current
        gameCtx = gctx

        partyCharsViewM = pvm.MainPartyTroops
        partyCharsPrisonerViewM = pvm.MainPartyPrisoners
        otherPartyCharsViewM = pvm.OtherPartyTroops
        otherPartyCharsPrisonerViewM = pvm.OtherPartyPrisoners

        originalPartyList = pvm.MainPartyTroops.ToList()
        originalPartyPrisonerList = pvm.MainPartyPrisoners.ToList()

        originalOtherPartyList = pvm.OtherPartyTroops.ToList()
        originalOtherPartyPrisonerList = pvm.OtherPartyPrisoners.ToList()

        mInstance = Me
    End Sub

    Private Sub ResetPartyList(partyScreenLogic As PartyScreenLogic)
        SearchLeft = ""
        SearchRight = ""
    End Sub


#Region "Left Side"
    Public Sub FindLeftPane()
        LeftVisible = Not LeftVisible
        'Print($"find left clicked state {LeftVisible}")
    End Sub

    Public Sub FilterLeft(keyword As String)
        If keyword Is Nothing Then
            Exit Sub
        End If
        Dim useCivilianOutfit = False
        Dim reset = New PartyVM(gameCtx, partyScreenController, "Nothing")
        originalOtherPartyList.Clear()
        originalOtherPartyPrisonerList.Clear()
        otherPartyCharsViewM.Clear()
        otherPartyCharsPrisonerViewM.Clear()
        If keyword IsNot "" Then
            originalOtherPartyList = reset.OtherPartyTroops.Where(Function(x) x.Troop.Character.ToString().ToLower().Contains(keyword.ToLower())).ToList()
            originalOtherPartyPrisonerList = reset.OtherPartyPrisoners.Where(Function(x) x.Troop.Character.ToString().ToLower().Contains(keyword.ToLower())).ToList()
        Else
            originalOtherPartyList = reset.OtherPartyTroops.ToList()
            originalOtherPartyPrisonerList = reset.OtherPartyPrisoners.ToList()
        End If
        For Each x In originalOtherPartyList
            otherPartyCharsViewM.Add(x)
        Next
        For Each x In originalOtherPartyPrisonerList
            otherPartyCharsPrisonerViewM.Add(x)
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
    Dim mTooltipSearchLeftBtn As New HintViewModel("Search on the left side. (Ctrl+Shift+F)")
    <DataSourceProperty>
    Public Property TooltipSearchLeftBtn As HintViewModel
        Get
            Return mTooltipSearchLeftBtn
        End Get
        Set(ByVal value As HintViewModel)
            mTooltipSearchLeftBtn = value
        End Set
    End Property
#End Region

#Region "Right Side"

    Public Sub FindRightPane()
        RightVisible = Not RightVisible
        'Print($"find right clicked state {RightVisible}")
    End Sub

    Public Sub FilterRight(keyword As String)
        If keyword Is Nothing Then
            Exit Sub
        End If
        Dim useCivilianOutfit = False
        Dim reset = New PartyVM(gameCtx, partyScreenController, "Nothing")
        originalPartyList.Clear()
        originalPartyPrisonerList.Clear()
        partyCharsViewM.Clear()
        partyCharsPrisonerViewM.Clear()
        If keyword IsNot "" Then
            originalPartyList = reset.MainPartyTroops.Where(Function(x) x.Troop.Character.ToString().ToLower().Contains(keyword.ToLower())).ToList()
            originalPartyPrisonerList = reset.MainPartyPrisoners.Where(Function(x) x.Troop.Character.ToString().ToLower().Contains(keyword.ToLower())).ToList()
        Else
            originalPartyList = reset.MainPartyTroops.ToList()
            originalPartyPrisonerList = reset.MainPartyPrisoners.ToList()
        End If
        For Each x In originalPartyList
            partyCharsViewM.Add(x)
        Next
        For Each x In originalPartyPrisonerList
            partyCharsPrisonerViewM.Add(x)
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
    Dim mTooltipSearchRightBtn As New HintViewModel("Search on the right side. (Ctrl+F)")
    <DataSourceProperty>
    Public Property TooltipSearchRightBtn As HintViewModel
        Get
            Return mTooltipSearchRightBtn
        End Get
        Set(ByVal value As HintViewModel)
            mTooltipSearchRightBtn = value
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
