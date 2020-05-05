Imports HarmonyLib
Imports SandBox.GauntletUI
Imports TaleWorlds.CampaignSystem
Imports TaleWorlds.CampaignSystem.ViewModelCollection
Imports TaleWorlds.Engine.GauntletUI
Imports TaleWorlds.Engine.Screens
Imports TaleWorlds.InputSystem
Imports TaleWorlds.Library

<HarmonyPatch(GetType(ScreenBase))>
Public Class PartyScreen
    Friend Shared searchOverlay As GauntletLayer
    Friend Shared searchVm As SearchPartyViewModel

    <HarmonyPatch("AddLayer")>
    Public Shared Sub Postfix(ByRef __instance As ScreenBase)
        If __instance.GetType() Is GetType(GauntletPartyScreen) AndAlso
            searchOverlay Is Nothing Then
            Dim partyScreen = CType(__instance, GauntletPartyScreen)
            searchOverlay = New GauntletLayer(100)
            Dim traverser = Traverse.Create(partyScreen)
            Dim pvm = traverser.Field(Of PartyVM)("_dataSource").Value
            Dim pstate = traverser.Field(Of PartyState)("_partyState").Value
            searchVm = New SearchPartyViewModel(pvm, pstate.PartyScreenLogic, partyScreen)
            searchOverlay.LoadMovie("View_FindEveryWhere", searchVm)
            searchOverlay.InputRestrictions.SetInputRestrictions(True, InputUsageMask.All)
            partyScreen.AddLayer(searchOverlay)
        End If
    End Sub

    <HarmonyPatch("RemoveLayer")>
    Public Shared Sub Prefix(ByRef __instance As ScreenBase,
                             ByRef layer As ScreenLayer)
        If __instance.GetType() Is GetType(GauntletPartyScreen) AndAlso
            searchOverlay IsNot Nothing AndAlso
            layer.Input.IsCategoryRegistered(HotKeyManager.GetCategory("PartyHotKeyCategory")) Then
            __instance.RemoveLayer(searchOverlay)
            searchVm.OnFinalize()
            searchVm = Nothing
            searchOverlay = Nothing
        End If
    End Sub


End Class
