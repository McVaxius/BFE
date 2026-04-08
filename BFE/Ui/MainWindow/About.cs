using Dalamud.Bindings.ImGui;
using System;
using System.Diagnostics;

namespace BFE.Ui.MainWindow
{
    internal class About
    {
        public static void Draw() 
        {
            ImGui.TextWrapped(PluginInfo.Summary);
            ImGui.Spacing();
            ImGui.BulletText($"{PluginInfo.Command}");
            ImGui.BulletText($"{PluginInfo.Command} settings");
            ImGui.BulletText($"{PluginInfo.Command} pyros");
            ImGui.BulletText($"{PluginInfo.Command} stop");
            ImGui.BulletText($"{PluginInfo.Command} ws");
            ImGui.BulletText($"{PluginInfo.Command} j");
            ImGui.BulletText($"{PluginInfo.LegacyCommand} (legacy alias)");
            ImGui.Spacing();

            if (ImGui.SmallButton("Ko-fi"))
                Process.Start(new ProcessStartInfo { FileName = PluginInfo.SupportUrl, UseShellExecute = true });

            ImGui.SameLine();
            if (ImGui.SmallButton("Discord"))
                Process.Start(new ProcessStartInfo { FileName = PluginInfo.DiscordUrl, UseShellExecute = true });

            ImGui.SameLine();
            if (ImGui.SmallButton("Copy Icon Guide Link"))
                ImGui.SetClipboardText(PluginInfo.IconGuideUrl);

            ImGui.Spacing();
            ImGui.TextDisabled(PluginInfo.DiscordFeedbackNote);
        }
    }
}
