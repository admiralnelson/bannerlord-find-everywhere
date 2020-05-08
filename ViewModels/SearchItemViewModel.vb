Imports System.Collections.Generic
Imports System.Linq
Imports SandBox.GauntletUI
Imports TaleWorlds.CampaignSystem
Imports TaleWorlds.CampaignSystem.ViewModelCollection
Imports TaleWorlds.Library
Imports System.Diagnostics
Imports TaleWorlds.Core.ViewModelCollection
Imports System
Imports HarmonyLib
Imports TaleWorlds.Core
Imports System.Reflection
Imports TaleWorlds.MountAndBlade

Public Class SearchItemViewModel
    Inherits ViewModel

    Dim searchTermLeft As String
    Dim searchTermRight As String
    Dim bLeftVisible As Boolean = False
    Dim bRightVisible As Boolean = False

    Friend Shared partyItemViewM As MBBindingList(Of SPItemVM)
    Friend Shared traderItemViewM As MBBindingList(Of SPItemVM)

    Shared partyItemList As List(Of SPItemVM)
    Shared traderItemList As List(Of SPItemVM)

    Shared mInstance As SearchItemViewModel
    Dim inventoryController As InventoryLogic
    Dim itemViewModel As SPInventoryVM
    Dim showRightSearchPanel = False
    Dim showLeftSearchPanel = False
    Dim hasBeenReset = False
    Dim missionCtx As Mission

    Public Shared ReadOnly Property Instance As SearchItemViewModel
        Get
            Return mInstance
        End Get
    End Property
    Public Sub New(ivm As SPInventoryVM,
                   ivl As InventoryLogic)
        inventoryController = ivl
        itemViewModel = ivm
        missionCtx = Mission.Current

        partyItemViewM = ivm.RightItemListVM
        traderItemViewM = ivm.LeftItemListVM

        partyItemList = itemViewModel.RightItemListVM.ToList()
        traderItemList = itemViewModel.LeftItemListVM.ToList()
        UpdateReset(Nothing)

        AddHandler ivl.AfterReset, AddressOf UpdateReset
        AddHandler ivl.AfterTransfer, AddressOf UpdateItemList
        mInstance = Me
    End Sub

    Private Sub UpdateItemList(inventoryLogic As InventoryLogic, results As List(Of TransferCommandResult))

    End Sub

    Private Sub UpdateReset(ivl As InventoryLogic)
        SearchLeft = ""
        SearchRight = ""
        'FilterRight(SearchRight)
        'FilterLeft(SearchLeft)
        'Print("changes in party state")        

    End Sub

    Private Function GetItemUsageFlag(item As WeaponComponentData) As ItemObject.ItemUsageSetFlags
        If String.IsNullOrEmpty(item.ItemUsage) Then
            Return MBItem.GetItemUsageSetFlags(item.ItemUsage)
        End If
        Return 0
    End Function
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
        If missionCtx IsNot Nothing Then
            useCivilianOutfit = missionCtx IsNot Nothing AndAlso missionCtx.DoesMissionRequireCivilianEquipment
        End If
        Dim reset = New SPInventoryVM(inventoryController, useCivilianOutfit,
                                      New Func(Of WeaponComponentData, ItemObject.ItemUsageSetFlags)(AddressOf GetItemUsageFlag),
                                      "Nothing")
        traderItemList.Clear()
        traderItemViewM.Clear()
        If keyword IsNot "" Then
            traderItemList = reset.LeftItemListVM.Where(Function(x) x.ItemDescription.ToLower().Contains(keyword.ToLower())).ToList()
        Else
            traderItemList = reset.LeftItemListVM.ToList()
        End If
        For Each x In traderItemList
            traderItemViewM.Add(x)
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
        If missionCtx IsNot Nothing Then
            useCivilianOutfit = missionCtx IsNot Nothing AndAlso missionCtx.DoesMissionRequireCivilianEquipment
        End If
        Dim reset = New SPInventoryVM(inventoryController, useCivilianOutfit,
                                      New Func(Of WeaponComponentData, ItemObject.ItemUsageSetFlags)(AddressOf GetItemUsageFlag),
                                      "Nothing")
        partyItemList.Clear()
        partyItemViewM.Clear()
        If keyword IsNot "" Then
            partyItemList = reset.RightItemListVM.Where(Function(x) x.ItemDescription.ToLower().Contains(keyword.ToLower())).ToList()
        Else
            partyItemList = reset.RightItemListVM.ToList()
        End If
        For Each x In partyItemList
            partyItemViewM.Add(x)
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
            Return 550
        End Get
        Set(value As Single)
            OnPropertyChanged(NameOf(IconMargin))
        End Set
    End Property
End Class
