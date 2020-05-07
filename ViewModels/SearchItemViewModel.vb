Imports System.Collections.Generic
Imports System.Linq
Imports SandBox.GauntletUI
Imports TaleWorlds.CampaignSystem
Imports TaleWorlds.CampaignSystem.ViewModelCollection
Imports TaleWorlds.Library
Imports System.Diagnostics
Imports TaleWorlds.Core.ViewModelCollection
Imports System

Public Class SearchItemViewModel
    Inherits ViewModel

    Dim searchTermLeft As String
    Dim searchTermRight As String
    Dim bLeftVisible As Boolean = False
    Dim bRightVisible As Boolean = False

    Dim partyItemViewM As MBBindingList(Of SPItemVM)
    Dim traderItemViewM As MBBindingList(Of SPItemVM)

    Dim originalPartyItemList As List(Of SPItemVM)
    Dim originalTraderItemList As List(Of SPItemVM)

    Shared mInstance As SearchItemViewModel
    Dim inventoryController As InventoryLogic
    Dim itemViewModel As SPInventoryVM
    Dim showRightSearchPanel = False
    Dim showLeftSearchPanel = False

    Public Shared ReadOnly Property Instance As SearchItemViewModel
        Get
            Return mInstance
        End Get
    End Property
    Public Sub New(ivm As SPInventoryVM,
                   ivl As InventoryLogic)
        inventoryController = ivl
        itemViewModel = ivm


        partyItemViewM = ivm.RightItemListVM
        traderItemViewM = ivm.LeftItemListVM

        UpdateItemList(Nothing, Nothing)

        AddHandler ivl.AfterTransfer, AddressOf UpdateItemList
        mInstance = Me
    End Sub

    Private Sub UpdateItemList(inventoryLogic As InventoryLogic, results As List(Of TransferCommandResult))
        Print("changes in party state")

        originalPartyItemList = itemViewModel.RightItemListVM.Where(Function(x) x.ItemCount > 0).ToList()
        originalTraderItemList = itemViewModel.LeftItemListVM.Where(Function(x) x.ItemCount > 0).ToList()
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
        'partyItemViewM = originalPartyItemList.Where(Function(x) x.Troop.Character.Name.Contains(keyword))
        'good ol loop
        For Each x In originalTraderItemList
            If x.ItemDescription.ToLower().Contains(keyword.ToLower()) And x.ItemCount > 0 Then
                traderItemViewM.Add(x)
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
        For Each x In originalPartyItemList
            If x.ItemDescription.ToLower().Contains(keyword.ToLower()) And x.ItemCount > 0 Then
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
