Imports HarmonyLib
Imports SandBox.GauntletUI
Imports SandBox.GauntletUI.Missions
Imports TaleWorlds.CampaignSystem
Imports TaleWorlds.CampaignSystem.ViewModelCollection
Imports TaleWorlds.Engine.GauntletUI
Imports TaleWorlds.Engine.Screens
Imports TaleWorlds.InputSystem
Imports TaleWorlds.Library
Imports TaleWorlds.MountAndBlade.View.Missions
Imports TaleWorlds.MountAndBlade.View.Screen

<HarmonyPatch(GetType(MissionGauntletBarterView))>
Public Class BarterScreen
    Friend Shared searchOverlay As GauntletLayer
    Friend Shared searchVm As SearchBarterViewModel
    Friend Shared mIsInBarterScreeen = False

    <HarmonyPatch("OnBarterBegin")>
    Public Shared Sub Postfix(ByRef __instance As MissionGauntletBarterView,
                              args As BarterData)
        If searchOverlay Is Nothing Then
            Dim traverser = Traverse.Create(__instance)
            Dim bvm = traverser.Field(Of SPBarterVM)("_dataSource").Value
            Dim bstate = traverser.Field(Of BarterManager)("_barter").Value
            searchOverlay = New GauntletLayer(110)
            searchVm = New SearchBarterViewModel(bvm, bstate)
            searchOverlay.LoadMovie("View_FindEveryWhere_Barter", searchVm)
            searchOverlay.InputRestrictions.SetInputRestrictions(True, InputUsageMask.All)
            Dim layers = __instance.MissionScreen
            layers.AddLayer(searchOverlay)
            mIsInBarterScreeen = True
        End If
    End Sub
    'Never called from patch. WHY?
    <HarmonyPatch("OnBarterClosed")>
    Public Shared Sub Prefix(ByRef __instance As MissionGauntletBarterView)
        If searchOverlay IsNot Nothing Then
            Dim layers = __instance.MissionScreen
            layers.RemoveLayer(searchOverlay)
            searchVm.OnFinalize()
            searchVm = Nothing
            searchOverlay = Nothing
            mIsInBarterScreeen = False
        End If
    End Sub

    <HarmonyPatch(GetType(MissionView))>
    Public Class BarterScreenKeyboard
        <HarmonyPatch("OnMissionScreenTick")>
        Public Shared Sub Prefix(ByRef __instance As ScreenBase,
                                ByRef dt As Single)
            If mIsInBarterScreeen Then
                If mIsInBarterScreeen Then
                    If (Input.IsKeyDown(InputKey.LeftControl) Or
                        Input.IsKeyDown(InputKey.RightControl)) AndAlso
                       (Input.IsKeyDown(InputKey.LeftShift) Or
                        Input.IsKeyDown(InputKey.RightShift)) AndAlso
                        Input.IsKeyPressed(InputKey.F) Then
                        'Print("cntrl sht f pressed")
                        SearchBarterViewModel.Instance.FindLeftPane()
                    ElseIf _
                       (Input.IsKeyDown(InputKey.LeftControl) Or
                        Input.IsKeyDown(InputKey.RightControl)) AndAlso
                        Input.IsKeyPressed(InputKey.F) Then
                        SearchBarterViewModel.Instance.FindRightPane()
                        'Print("cntrl  f pressed")
                    End If
                End If
            End If
        End Sub
    End Class
End Class
