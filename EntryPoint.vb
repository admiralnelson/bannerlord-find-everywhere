Imports TaleWorlds.Core
Imports TaleWorlds.Engine.GauntletUI
Imports TaleWorlds.MountAndBlade

Namespace Global.FindEverywhere
    Public Class EntryPoint
        Inherits MBSubModuleBase

        Public Overrides Sub OnCampaignStart(game As Game, starterObject As Object)
            MyBase.OnCampaignStart(game, starterObject)
            UIResourceManager.UIResourceDepot.StartWatchingChangesInDepot()
        End Sub

        Protected Overrides Sub OnSubModuleLoad()
            MyBase.OnSubModuleLoad()
        End Sub

        Protected Overrides Sub OnApplicationTick(dt As Single)
            MyBase.OnApplicationTick(dt)
            UIResourceManager.UIResourceDepot.CheckForChanges()
        End Sub
    End Class
End Namespace