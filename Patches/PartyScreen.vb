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
    Shared isInPartyScreen = False

    <HarmonyPatch("AddLayer")>
    Public Shared Sub Postfix(ByRef __instance As ScreenBase)
        If __instance.GetType() Is GetType(GauntletPartyScreen) AndAlso
            searchOverlay Is Nothing Then
            Dim partyScreen = CType(__instance, GauntletPartyScreen)
            searchOverlay = New GauntletLayer(110)
            Dim traverser = Traverse.Create(partyScreen)
            Dim pvm = traverser.Field(Of PartyVM)("_dataSource").Value
            Dim pstate = traverser.Field(Of PartyState)("_partyState").Value
            searchVm = New SearchPartyViewModel(pvm, pstate.PartyScreenLogic)
            searchOverlay.LoadMovie("View_FindEveryWhere_Troop", searchVm)
            searchOverlay.InputRestrictions.SetInputRestrictions(True, InputUsageMask.All)
            partyScreen.AddLayer(searchOverlay)
            isInPartyScreen = True
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
            isInPartyScreen = False
        End If
    End Sub

    <HarmonyPatch("OnFrameTick")>
    Public Shared Sub Prefix(ByRef __instance As ScreenBase,
                            ByRef dt As Single)
        If __instance.GetType() Is GetType(GauntletPartyScreen) AndAlso
            searchOverlay IsNot Nothing Then
            If isInPartyScreen Then
                If (Input.IsKeyDown(InputKey.LeftControl) Or
                    Input.IsKeyDown(InputKey.RightControl)) AndAlso
                   (Input.IsKeyDown(InputKey.LeftShift) Or
                    Input.IsKeyDown(InputKey.RightShift)) AndAlso
                    Input.IsKeyPressed(InputKey.F) Then
                    Print("cntrl sht f pressed")
                    SearchPartyViewModel.Instance.FindLeftPane()
                ElseIf _
                   (Input.IsKeyDown(InputKey.LeftControl) Or
                    Input.IsKeyDown(InputKey.RightControl)) AndAlso
                    Input.IsKeyPressed(InputKey.F) Then
                    SearchPartyViewModel.Instance.FindRightPane()
                    Print("cntrl  f pressed")
                End If
            End If
        End If
    End Sub


End Class
