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


    Shared mInstance As SearchBarterViewModel
    Dim inventoryController As BarterManager
    Dim itemViewModel As SPBarterVM
    Dim showRightSearchPanel = False
    Dim showLeftSearchPanel = False

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
        partyOtherViewM = bvm.RightOtherList
        partyOfferViewM = bvm.RightOfferList

        traderItemViewM = bvm.LeftItemList
        traderPrisonerViewM = bvm.LeftPrisonerList
        traderFiefViewM = bvm.LeftFiefList
        traderDiplomaticViewM = bvm.LeftDiplomaticList
        traderOtherViewM = bvm.LeftOtherList
        traderOfferViewM = bvm.LeftOfferList


        UpdateItem(Nothing, Nothing)

        mInstance = Me
        bm.OnTransfer = AddressOf UpdateItem

    End Sub

    Private Sub UpdateItem(barter As Barterable, transferable As Boolean)
        Print("changes in party state")

        originalPartyItemList = itemViewModel.RightItemList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalPartyDiplomaticList = itemViewModel.RightDiplomaticList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalPartyFiefList = itemViewModel.RightFiefList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalPartyPrisonerList = itemViewModel.RightPrisonerList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalTraderOtherList = itemViewModel.RightOtherList.Where(Function(x) x.TotalItemCount > 0).ToList()

        originalTraderItemList = itemViewModel.LeftItemList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalTraderDiplomaticList = itemViewModel.LeftDiplomaticList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalTraderFiefList = itemViewModel.LeftFiefList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalTraderPrisonerList = itemViewModel.LeftPrisonerList.Where(Function(x) x.TotalItemCount > 0).ToList()
        originalTraderOtherList = itemViewModel.LeftOtherList.Where(Function(x) x.TotalItemCount > 0).ToList()



    End Sub



#Region "Left Side"
    Public Sub FindLeftPane()
        LeftVisible = Not LeftVisible
        'Print($"find left clicked state {LeftVisible}")
    End Sub
    Public Sub ResetLeft()
        traderItemViewM.Clear()
        For Each x In originalTraderItemList
            traderItemViewM.Add(x)
        Next
    End Sub
    Public Sub FilterLeft(keyword As String)
        If keyword Is "" Then
            ResetLeft()
            Exit Sub
        End If
        traderItemViewM.Clear()

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
        For Each x In originalTraderOtherList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                traderOtherViewM.Add(x)
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
        partyItemViewM.Clear()
        For Each x In originalPartyItemList
            partyItemViewM.Add(x)
        Next
    End Sub

    Public Sub FilterRight(keyword As String)
        If keyword Is "" Then
            ResetRight()
            Exit Sub
        End If
        partyItemViewM.Clear()
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
                partyItemViewM.Add(x)
            End If
        Next
        For Each x In originalPartyFiefList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                partyItemViewM.Add(x)
            End If
        Next
        For Each x In originalPartyPrisonerList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                partyItemViewM.Add(x)
            End If
        Next
        For Each x In originalPartyOtherList
            If x.ItemLbl.ToLower().Contains(keyword.ToLower()) And x.TotalItemCount > 0 Then
                partyItemViewM.Add(x)
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
            Return 550
        End Get
        Set(value As Single)
            OnPropertyChanged(NameOf(IconMargin))
        End Set
    End Property


End Class
