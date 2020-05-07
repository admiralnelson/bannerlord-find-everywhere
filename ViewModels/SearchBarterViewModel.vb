Imports System.Collections.Generic
Imports System.Linq
Imports SandBox.GauntletUI
Imports TaleWorlds.CampaignSystem
Imports TaleWorlds.CampaignSystem.ViewModelCollection
Imports TaleWorlds.Library
Imports System.Diagnostics
Imports TaleWorlds.Core.ViewModelCollection
Imports System
Imports TaleWorlds.CampaignSystem.BarterManager
Imports HarmonyLib

Public Class SearchBarterViewModel
    Inherits ViewModel

    Dim searchTermLeft As String
    Dim searchTermRight As String
    Dim bLeftVisible As Boolean = False
    Dim bRightVisible As Boolean = False

    Dim partyItemViewM,
        partyPrisonerViewM,
        partyFiefViewM,
        partyDiplomaticViewM,
        partyOtherViewM,
        partyOfferViewM As MBBindingList(Of BarterItemVM)

    Dim traderItemViewM,
        traderPrisonerViewM,
        traderFiefViewM,
        traderDiplomaticViewM,
        traderOtherViewM,
        traderOfferViewM As MBBindingList(Of BarterItemVM)

    Dim originalPartyItemList,
        originalPartyPrisonerList,
        originalPartyFiefList,
        originalPartyDiplomaticList,
        originalPartyOtherList As List(Of BarterItemVM)


    Dim originalTraderItemList,
        originalTraderPrisonerList,
        originalTraderFiefList,
        originalTraderDiplomaticList,
        originalTraderOtherList As List(Of BarterItemVM)


    Friend Shared mInstance As SearchBarterViewModel
    Dim inventoryController As BarterManager
    Dim itemViewModel As SPBarterVM
    Dim showRightSearchPanel = False
    Dim showLeftSearchPanel = False
    Dim ready = False

    Public Shared ReadOnly Property Instance As SearchBarterViewModel
        Get
            Return mInstance
        End Get
    End Property
    Public Sub New(bvm As SPBarterVM,
                   bm As BarterManager)
        inventoryController = bm
        itemViewModel = bvm


        partyItemViewM = bvm.RightItemList
        partyPrisonerViewM = bvm.RightPrisonerList
        partyFiefViewM = bvm.RightFiefList
        partyDiplomaticViewM = bvm.RightDiplomaticList
        'partyOtherViewM = bvm.RightOtherList        

        traderItemViewM = bvm.LeftItemList
        traderPrisonerViewM = bvm.LeftPrisonerList
        traderFiefViewM = bvm.LeftFiefList
        traderDiplomaticViewM = bvm.LeftDiplomaticList
        'traderOtherViewM = bvm.LeftOtherList

        originalPartyItemList = itemViewModel.RightItemList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalPartyDiplomaticList = itemViewModel.RightDiplomaticList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalPartyFiefList = itemViewModel.RightFiefList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalPartyPrisonerList = itemViewModel.RightPrisonerList.Where(Function(x) x.TotalItemCount > 0).ToList()
        'originalTraderOtherList = itemViewModel.RightOtherList.Where(Function(x) x.TotalItemCount > 0).ToList()

        originalTraderItemList = itemViewModel.LeftItemList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalTraderDiplomaticList = itemViewModel.LeftDiplomaticList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalTraderFiefList = itemViewModel.LeftFiefList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalTraderPrisonerList = itemViewModel.LeftPrisonerList.Where(Function(x) x.TotalItemCount > 0).ToList()
        'originalTraderOtherList = itemViewModel.LeftOtherList.Where(Function(x) x.TotalItemCount > 0).ToList()   

        mInstance = Me
        bm.OnTransfer = AddressOf UpdateItem
        UpdateItem(Nothing, Nothing)
        ready = True
    End Sub

    Private Sub UpdateItem(barter As Barterable, Optional transferable As Boolean = False)
        'Print("changes in party state")
        If barter IsNot Nothing Then
            itemViewModel.OnTransferItem(barter, transferable)
        End If

    End Sub



#Region "Left Side"
    Public Sub FindLeftPane()
        LeftVisible = Not LeftVisible
        'Print($"find left clicked state {LeftVisible}")
    End Sub
    Public Sub ResetLeft()
        traderItemViewM.Clear()
        traderDiplomaticViewM.Clear()
        traderFiefViewM.Clear()
        traderPrisonerViewM.Clear()
        For Each x In originalTraderItemList
            traderItemViewM.Add(x)
        Next
        For Each x In originalTraderDiplomaticList
            traderDiplomaticViewM.Add(x)
        Next
        For Each x In originalTraderFiefList
            traderFiefViewM.Add(x)
        Next
        For Each x In originalTraderPrisonerList
            traderPrisonerViewM.Add(x)
        Next
    End Sub
    Public Sub FilterLeft(keyword As String)
        If keyword Is "" Then
            ResetLeft()
            Exit Sub
        End If
        traderItemViewM.Clear()
        traderDiplomaticViewM.Clear()
        traderFiefViewM.Clear()
        traderPrisonerViewM.Clear()
        'traderOtherViewM.Clear()

        'can't use list compreshension here :(
        'partyItemViewM = originalTraderItemList.Where(Function(x) x.Troop.Character.Name.Contains(keyword))
        'good ol loop
        For Each x In originalTraderItemList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                traderItemViewM.Add(x)
            End If
        Next
        For Each x In originalTraderDiplomaticList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                traderDiplomaticViewM.Add(x)
            End If
        Next
        For Each x In originalTraderFiefList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                traderFiefViewM.Add(x)
            End If
        Next
        For Each x In originalTraderPrisonerList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                traderPrisonerViewM.Add(x)
            End If
        Next
        'For Each x In originalTraderOtherList
        '    If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
        '        traderOtherViewM.Add(x)
        '    End If
        'Next
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
    Public Sub ResetRight()
        partyItemViewM.Clear()
        partyDiplomaticViewM.Clear()
        partyFiefViewM.Clear()
        partyPrisonerViewM.Clear()
        For Each x In originalPartyItemList
            partyItemViewM.Add(x)
        Next
        For Each x In originalPartyDiplomaticList
            partyDiplomaticViewM.Add(x)
        Next
        For Each x In originalPartyFiefList
            partyFiefViewM.Add(x)
        Next
        For Each x In originalPartyPrisonerList
            partyPrisonerViewM.Add(x)
        Next
    End Sub

    Public Sub FilterRight(keyword As String)
        If keyword Is "" Then
            ResetRight()
            Exit Sub
        End If
        partyItemViewM.Clear()
        partyDiplomaticViewM.Clear()
        partyFiefViewM.Clear()
        partyPrisonerViewM.Clear()
        'partyOtherViewM.Clear()
        'can't use list compreshension here :(
        'partyItemViewM = originalPartyItemList.Where(Function(x) x.Troop.Character.Name.Contains(keyword))
        'good ol loop
        'good ol loop
        For Each x In originalPartyItemList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                partyItemViewM.Add(x)
            End If
        Next
        For Each x In originalPartyDiplomaticList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                partyDiplomaticViewM.Add(x)
            End If
        Next
        For Each x In originalPartyFiefList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                partyFiefViewM.Add(x)
            End If
        Next
        For Each x In originalPartyPrisonerList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                partyPrisonerViewM.Add(x)
            End If
        Next
        'For Each x In originalPartyOtherList
        '    If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
        '        partyOtherViewM.Add(x)
        '    End If
        'Next
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
            Return 550
        End Get
        Set(value As Single)
            OnPropertyChanged(NameOf(IconMargin))
        End Set
    End Property

    <HarmonyPatch(GetType(SPBarterVM))>
    Public Class SearchBarterAux
        <HarmonyPatch("ExecuteReset")>
        Public Shared Sub Postfix()
            If mInstance IsNot Nothing Then
                mInstance.UpdateItem(Nothing)
            End If
        End Sub
    End Class
End Class
