Imports HarmonyLib
Imports SandBox.GauntletUI
Imports TaleWorlds.CampaignSystem
Imports TaleWorlds.CampaignSystem.ViewModelCollection
Imports TaleWorlds.Engine.GauntletUI
Imports TaleWorlds.Engine.Screens
Imports TaleWorlds.InputSystem
Imports TaleWorlds.Library

<HarmonyPatch(GetType(ScreenBase))>
Public Class ItemScreen
    Friend Shared searchOverlay As GauntletLayer
    Friend Shared searchVm As SearchItemViewModel
    Shared isInPartyScreen = False

    <HarmonyPatch("AddLayer")>
    Public Shared Sub Postfix(ByRef __instance As ScreenBase)
        If __instance.GetType() Is GetType(InventoryGauntletScreen) AndAlso
            searchOverlay Is Nothing Then
            Dim itemScreen = CType(__instance, InventoryGauntletScreen)
            searchOverlay = New GauntletLayer(110)
            Dim traverser = Traverse.Create(itemScreen)
            Dim ivm = traverser.Field(Of SPInventoryVM)("_dataSource").Value
            Dim istate = traverser.Field(Of InventoryState)("_inventoryState").Value
            searchVm = New SearchItemViewModel(ivm, istate.InventoryLogic)
            searchOverlay.LoadMovie("View_FindEveryWhere_Item", searchVm)
            searchOverlay.InputRestrictions.SetInputRestrictions(True, InputUsageMask.All)
            itemScreen.AddLayer(searchOverlay)
            isInPartyScreen = True
        End If
    End Sub
    'Never called from patch. WHY?
    <HarmonyPatch("RemoveLayer")>
    Public Shared Sub Prefix(ByRef __instance As ScreenBase,
                             ByRef layer As ScreenLayer)
        'If __instance.GetType() Is GetType(InventoryGauntletScreen) AndAlso
        '    searchOverlay IsNot Nothing Then
        '    __instance.RemoveLayer(searchOverlay)
        '    searchVm.OnFinalize()
        '    searchVm = Nothing
        '    searchOverlay = Nothing
        '    isInPartyScreen = False
        'End If
    End Sub

    <HarmonyPatch("OnFrameTick")>
    Public Shared Sub Prefix(ByRef __instance As ScreenBase,
                            ByRef dt As Single)
        If __instance.GetType() IsNot GetType(InventoryGauntletScreen) AndAlso
            searchOverlay IsNot Nothing Then
            __instance.RemoveLayer(searchOverlay)
            searchVm.OnFinalize()
            searchVm = Nothing
            searchOverlay = Nothing
            isInPartyScreen = False
        End If
        If __instance.GetType() Is GetType(InventoryGauntletScreen) AndAlso
            searchOverlay IsNot Nothing Then
            If isInPartyScreen Then
                If (Input.IsKeyDown(InputKey.LeftControl) Or
                    Input.IsKeyDown(InputKey.RightControl)) AndAlso
                   (Input.IsKeyDown(InputKey.LeftShift) Or
                    Input.IsKeyDown(InputKey.RightShift)) AndAlso
                    Input.IsKeyPressed(InputKey.F) Then
                    'Print("cntrl sht f pressed")
                    SearchItemViewModel.Instance.FindLeftPane()
                ElseIf _
                   (Input.IsKeyDown(InputKey.LeftControl) Or
                    Input.IsKeyDown(InputKey.RightControl)) AndAlso
                    Input.IsKeyPressed(InputKey.F) Then
                    SearchItemViewModel.Instance.FindRightPane()
                    'Print("cntrl  f pressed")
                End If
            End If
        End If
    End Sub


End Class
