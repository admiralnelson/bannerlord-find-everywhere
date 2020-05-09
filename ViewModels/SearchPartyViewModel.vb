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
Imports HarmonyLib
Imports TaleWorlds.CampaignSystem.PartyScreenLogic
Imports Microsoft.VisualBasic

Public Class SearchPartyViewModel
    Inherits ViewModel

    Shared isModified = False

    Dim searchTermLeft As String
    Dim searchTermRight As String
    Dim bLeftVisible As Boolean = False
    Dim bRightVisible As Boolean = False

    Friend Shared partyCharsViewM, partyCharsPrisonerViewM As MBBindingList(Of PartyCharacterVM)
    Friend Shared otherPartyCharsViewM, otherPartyCharsPrisonerViewM As MBBindingList(Of PartyCharacterVM)

    Dim originalPartyList, originalPartyPrisonerList As List(Of PartyCharacterVM)
    Dim originalOtherPartyList, originalOtherPartyPrisonerList As List(Of PartyCharacterVM)

    Friend Shared partyScreenController As PartyScreenLogic
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
        isModified = False
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
        AddHandler psl.Update, AddressOf UpdatePartyList
        AddHandler psl.AfterReset, AddressOf ResetPartyList
    End Sub

    Private Sub UpdatePartyList(command As PartyCommand)
        Print("changes detected")
        originalPartyList.AddRange(partyCharsViewM)
        originalPartyList = originalPartyList.Distinct().ToList()

        originalPartyPrisonerList.AddRange(partyCharsPrisonerViewM)
        originalPartyPrisonerList = originalPartyPrisonerList.Distinct().ToList()

        originalOtherPartyList.AddRange(otherPartyCharsViewM)
        originalOtherPartyList = originalOtherPartyList.Distinct().ToList()

        originalOtherPartyPrisonerList.AddRange(otherPartyCharsPrisonerViewM)
        originalOtherPartyPrisonerList = originalOtherPartyPrisonerList.Distinct().ToList()

        'originalPartyList = partyViewModel.MainPartyTroops.Where(Function(x) x.Number > 0).ToList()
        'originalPartyPrisonerList = partyViewModel.MainPartyPrisoners.Where(Function(x) x.Number > 0).ToList()
        'originalOtherPartyList = partyViewModel.OtherPartyTroops.Where(Function(x) x.Number > 0).ToList()
        'originalOtherPartyPrisonerList = partyViewModel.OtherPartyPrisoners.Where(Function(x) x.Number > 0).ToList()
    End Sub

    Private Sub ResetPartyList(partyScreenLogic As PartyScreenLogic)
        SearchLeft = ""
        SearchRight = ""
        isModified = False
    End Sub


#Region "Left Side"
    Public Sub FindLeftPane()
        LeftVisible = Not LeftVisible
        If Not LeftVisible Then SearchLeft = ""
        If (SearchLeft = "" Or SearchLeft Is Nothing) And
           (SearchRight = "" Or SearchRight Is Nothing) Then
            isModified = False
        End If
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
        isModified = Not ((SearchLeft = "" Or SearchLeft Is Nothing) And (SearchRight = "" Or SearchRight Is Nothing))
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
        isModified = True
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
        If Not RightVisible Then SearchRight = ""
        If (SearchLeft = "" Or SearchLeft Is Nothing) And
           (SearchRight = "" Or SearchRight Is Nothing) Then
            isModified = False
        End If
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
        isModified = Not ((SearchLeft = "" Or SearchLeft Is Nothing) And (SearchRight = "" Or SearchRight Is Nothing))
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
        isModified = True
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


    '<HarmonyPatch(GetType(PartyVM))>
    'Public Class PatchButtonMoveAllParty
    '    Shared currentChar As PartyCharacterVM
    '
    '    <HarmonyPatch("ExecuteTransferAllMainTroops")>
    '    <HarmonyPatch("ExecuteTransferAllOtherTroops")>
    '    <HarmonyPatch("ExecuteTransferAllMainPrisoners")>
    '    <HarmonyPatch("ExecuteTransferAllOtherPrisoners")>
    '    Public Shared Function Prefix(ByRef __instance As PartyVM) As Boolean
    '        If isModified Then
    '            MessageBoxBL("Warning", "You can't transfer all troops with filters on!" & vbLf &
    '                                      "Clear right & left search term and try again " & vbLf &
    '                                      "(we're working for proper fix, see: https://tinyurl.com/y8s54s6h)")
    '            Return False
    '        End If
    '        Return True
    '    End Function
    'End Class

End Class
