using Dalamud.Bindings.ImGui;
using System.Numerics;
using ECommons.ImGuiMethods;
using BFE.Ui.SettingsWindow;
using BFE.Windows;

namespace BFE.Ui.SettingWindow;

internal class SettingsWindow : PositionedWindow
{
    public SettingsWindow(): base($"{PluginInfo.DisplayName} Settings ###BFESettingsWindow")
    {
        SizeConstraints = new()
        {
            MinimumSize = new(620, 500),
            MaximumSize = new(1180, 980)
        };
        P.windowSystem.AddWindow(this);
    }

    public void Dispose() {}

    public override void Draw()
    {
        ImGuiEx.EzTabBar("Bunnies Settings Tabs",
                        ("General Settings", GeneralSettings.Draw,null, true),
                        ("AutoRetainer Settings", AutoReatinerSettings.Draw, null, true)
                        );
        FinalizePendingWindowPlacement();
    }
}
