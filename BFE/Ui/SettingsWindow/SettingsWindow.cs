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
        var appearanceRoot = ImGui.GetID("");
        bool tabsOpen;
        using (MaterialText.PushLineHeight(UiText.T("General Settings"), UiText.T("AutoRetainer Settings"), UiText.T("Window appearance")))
            tabsOpen = ImGui.BeginTabBar("Bunnies Settings Tabs", ImGuiTabBarFlags.FittingPolicyScroll);
        if (tabsOpen)
        {
            if (UiGui.TabItem("General Settings")) { GeneralSettings.Draw(); ImGui.EndTabItem(); }
            if (UiGui.TabItem("AutoRetainer Settings")) { AutoReatinerSettings.Draw(); ImGui.EndTabItem(); }
            using (var appearance = MaterialTabs.Item(UiText.T("Window appearance") + "###WindowAppearance", ImGuiTabItemFlags.NoPushId))
                if (appearance.Visible)
                {
                    ImGuiP.PushOverrideID(appearanceRoot);
                    try { P.appearance.DrawWindowAppearanceSettings(); }
                    finally { ImGui.PopID(); }
                }
            ImGui.EndTabBar();
        }
        FinalizePendingWindowPlacement();
    }
}
