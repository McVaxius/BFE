using Dalamud.Bindings.ImGui;
using System.Numerics;
using ECommons.ImGuiMethods;
using BFE.Ui.SettingsWindow;
using BFE.Windows;
using AethertekUI;

namespace BFE.Ui.SettingWindow;

internal class SettingsWindow : PositionedWindow
{
    public SettingsWindow(): base($"{PluginInfo.DisplayName} Settings ###BFESettingsWindow")
    {
        Flags |= ImGuiWindowFlags.HorizontalScrollbar;
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
        WindowMotion.DrawChrome();
        UiGui.Title($"{PluginInfo.DisplayName} Settings", UiText.T("BFE Settings"));
        P.appearance.DrawWindowAppearanceSettings();
        ImGui.Separator();
        bool tabsOpen;
        using (MaterialText.PushLineHeight(UiText.T("General Settings"), UiText.T("AutoRetainer Settings")))
            tabsOpen = ImGui.BeginTabBar("Bunnies Settings Tabs");
        if (tabsOpen)
        {
            if (UiGui.TabItem("General Settings")) { GeneralSettings.Draw(); ImGui.EndTabItem(); }
            if (UiGui.TabItem("AutoRetainer Settings")) { AutoReatinerSettings.Draw(); ImGui.EndTabItem(); }
            ImGui.EndTabBar();
        }
        FinalizePendingWindowPlacement();
    }
}
