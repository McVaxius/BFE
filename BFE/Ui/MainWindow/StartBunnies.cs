using Dalamud.Interface.Components;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using ECommons.ImGuiMethods;
using System.Numerics;
using static FFXIVClientStructs.FFXIV.Client.UI.RaptureAtkHistory.Delegates;
using BFE.Scheduler.Tasks;
using BFE.Scheduler;
using Dalamud.Interface.Colors;
using ECommons.Logging;
using static System.Net.WebRequestMethods;

namespace BFE.Ui.MainWindow
{
    internal class StartBunnies
    {
        public static bool IsRunning = false;
        private static string ButtonName = "Normal Raid";
        public static void Draw()
        {
            // Goes to Pagos (disabled for now)
            ImGui.BeginDisabled(true);
            if (ImGui.RadioButton("Pagos", C.zoneSelected == 0))
            {
                C.zoneSelected = 0;
                C.Save();
            }
            ImGui.EndDisabled();
            ImGuiComponents.HelpMarker("Disabled for now");

            // Goes to Pyros (WIP)
            if (ImGui.RadioButton("Pyros", C.zoneSelected == 1))
            {
                C.zoneSelected = 1;
                C.Save();
            }
            ImGuiComponents.HelpMarker("WIP");

            // Goes to Hydatos (disabled for now)
            ImGui.BeginDisabled(true);
            if (ImGui.RadioButton("Hydatos", C.zoneSelected == 2))
            {
                C.zoneSelected = 2;
                C.Save();
            }
            ImGui.EndDisabled();
            ImGuiComponents.HelpMarker("Disabled for now");

            ImGui.Text($"Task: {icurrentTask}");
            TimeSpan currentTime = P.stopwatch.Elapsed;
            string currentTimeF = currentTime.ToString(@"mm\:ss\.fff");
            ImGui.Text($"Time Elapsed is: {currentTimeF}");

            if (C.zoneSelected == 0)
                ButtonName = "Pagos";
            else if (C.zoneSelected == 1)
                ButtonName = "Pyros";
            else if (C.zoneSelected == 2)
                ButtonName = "Hydatos";

            DrawDependencyStatus();

            if (!P.pluginDependencies.RequiredDependenciesLoaded)
            {
                ImGui.Text("Load all required plugins to start Bunnies automation.");
            }
            else
            {
                if (ImGui.Button(!IsRunning ? $"Start {ButtonName}" : "Stop"))
                {
                    if (!IsRunning)
                    {
                        ToggleRotationAIOff();
                        SchedulerMain.EnablePlugin();
                    }
                    else
                    {
                        SchedulerMain.DisablePlugin();
                        RunCommand("e [Bunnies] Bunnies Stopped.");
                    }
                }
            }
        }

        private static void DrawDependencyStatus()
        {
            ImGui.Text("Dependencies");
            ImGui.SameLine();
            if (ImGui.Button("Refresh##DependencyStatus"))
            {
                P.pluginDependencies.Refresh(true);
            }

            foreach (var dependency in P.pluginDependencies.RequiredStatuses)
            {
                ImGui.TextColored(GetDependencyColor(dependency.State), $"- {dependency.DisplayName}: {dependency.StateText}");
                ImGui.SameLine();
                if (ImGui.Button($"Get Repo Url ##{dependency.InternalName}"))
                {
                    ImGui.SetClipboardText(dependency.RepoUrl);
                    DuoLog.Information("Repo URL Copied");
                    Notify.Info("Repo URL Copied");
                }
            }
        }

        private static Vector4 GetDependencyColor(PluginDependencyState state)
        {
            return state switch
            {
                PluginDependencyState.Loaded => ImGuiColors.HealerGreen,
                PluginDependencyState.InstalledNotLoaded => ImGuiColors.DalamudYellow,
                _ => ImGuiColors.DalamudRed,
            };
        }
    }
}
