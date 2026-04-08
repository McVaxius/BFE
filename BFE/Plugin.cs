using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ECommons;
using ECommons.Automation.NeoTaskManager;
using ECommons.Configuration;
using ECommons.DalamudServices;
using BFE.IPC;
using BFE.Ui.MainWindow;
using BFE.Ui.DebugWindow;
using BFE.Ui.SettingWindow;
using AutoRetainerAPI;
using System.Diagnostics;
using BFE.IPC.Lifestream;
using BFE.Scheduler;

namespace BFE;

public sealed class Plugin : IDalamudPlugin
{
    public string Name => PluginInfo.DisplayName;
    internal static Plugin P = null!;
    private Config config;

    internal IChatGui ChatGui { get; private init; } = null!;

    internal IToastGui ToastGui { get; private init; } = null!;

    public static Config C => P.config;

    // internal window names
    internal WindowSystem windowSystem;
    internal MainWindow mainWindow;
    internal SettingsWindow settingsWindow;
    internal DebugWindow debugWindow;
    internal string clipboardPath = string.Empty;

    public Filter filter { get; }
    // IPC's/Internals
    internal AutoRetainerApi autoRetainerApi;
    internal LifestreamIPC lifestream;
    internal TaskManager taskManager;
    internal AutoRetainerIPC autoRetainer;
    internal PandoraIPC pandora;
    internal NavmeshIPC navmesh;
    internal BossModIPC bossmod;
    internal WrathIPC wrath;
    internal BunniesIPC bunniesIPC;

    // Timers
    internal Stopwatch stopwatch;
    internal TimeSpan totalRunTime;

    #pragma warning disable CS8618
    public Plugin(IDalamudPluginInterface pluginInterface, IChatGui chatGui, IToastGui toastGui)
    {
        P = this;
        ChatGui = chatGui;
        ToastGui = toastGui;
        filter = new Filter();
        ECommonsMain.Init(pluginInterface, this, ECommons.Module.DalamudReflector, ECommons.Module.ObjectFunctions);
        new ECommons.Schedulers.TickScheduler(Load);
    }

    public void Load()
    {
        EzConfig.Migrate<Config>();
        config = EzConfig.Init<Config>();

        // IPC's
        taskManager = new();
        autoRetainer = new();
        autoRetainerApi = new();
        lifestream = new();
        navmesh = new();
        pandora = new();
        bossmod = new();
        wrath = new();
        bunniesIPC = new();

        // Windows
        windowSystem = new(PluginInfo.DisplayName);
        mainWindow = new();
        debugWindow = new();
        settingsWindow = new();

        // Timers
        stopwatch = new();

        Svc.PluginInterface.UiBuilder.Draw += windowSystem.Draw;

        Svc.PluginInterface.UiBuilder.OpenMainUi += OpenMainUi;
        Svc.PluginInterface.UiBuilder.OpenConfigUi += OpenConfigUi;

        EzCmd.Add(PluginInfo.Command, OnCommand,
            """
            - Opens the BFE main window
            /bfe settings - Opens settings
            /bfe pyros - Starts Pyros bunnies
            /bfe stop - Stops BFE
            /bfe ws - Resets windows to 1,1
            /bfe j - Jumps windows to visible random positions
            """);
        EzCmd.Add(PluginInfo.LegacyCommand, OnCommand,
            """
            Legacy Bunnies alias for BFE.
            """);

        Svc.Framework.Update += Tick;
        ResetSessionStats();
    }

    private void OpenMainUi()
        => mainWindow.IsOpen = true;

    private void OpenConfigUi()
        => settingsWindow.IsOpen = !settingsWindow.IsOpen;

    private void ResetSessionStats()
    {
        C.sessionStats.Reset();
        C.pagosSessionStats.Reset();
        C.pyrosSessionStats.Reset();
        C.hydatosSessionStats.Reset();
    }

    private void Tick(IFramework _)
    {
        if (SchedulerMain.DoWeTick && Svc.ClientState.LocalPlayer != null)
        {
            SchedulerMain.Tick();
        }
    }

    public void Dispose()
    {
        Svc.Framework.Update -= Tick;
        Svc.PluginInterface.UiBuilder.Draw -= windowSystem.Draw;
        Svc.PluginInterface.UiBuilder.OpenMainUi -= OpenMainUi;
        Svc.PluginInterface.UiBuilder.OpenConfigUi -= OpenConfigUi;
        windowSystem.RemoveAllWindows();
        filter.Dispose();
        autoRetainerApi?.Dispose();
        ECommonsMain.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        if (args.EqualsIgnoreCaseAny("ws"))
        {
            ResetWindowPositions();
        }
        else if (args.EqualsIgnoreCaseAny("j"))
        {
            JumpWindowsToRandomVisibleLocations();
        }
        else if (args.EqualsIgnoreCaseAny("d", "debug"))
        {
            debugWindow.IsOpen = !debugWindow.IsOpen;
        }

        else if (args.EqualsIgnoreCaseAny("config", "s", "settings", "setting"))
        {
            settingsWindow.IsOpen = !settingsWindow.IsOpen;
        }

        else if (args.EqualsIgnoreCaseAny("stop"))
        {
            SchedulerMain.DisablePlugin();
            RunCommand("e [Bunnies] Bunnies Stopped.");
        }

        else if (args.EqualsIgnoreCaseAny("pagos"))
        {
            //C.zoneSelected = 0;
            //SchedulerMain.EnablePlugin();
        }

        else if (args.EqualsIgnoreCaseAny("pyros"))
        {
            if (PluginInstalled("vnavmesh") && PluginInstalled("RotationSolver") && PluginInstalled("BossModReborn"))
            {
                C.zoneSelected = 1;
                SchedulerMain.EnablePlugin();
            }
            else
            {
                NotifyPlugins();
                SchedulerMain.DisablePlugin();
            }
        }

        else if (args.EqualsIgnoreCaseAny("hydatps"))
        {
            //C.zoneSelected = 2;
            //SchedulerMain.EnablePlugin();
        }

        else
        {
            mainWindow.IsOpen = !mainWindow.IsOpen;
        }
    }

    internal void ResetWindowPositions()
    {
        mainWindow.QueueResetToOrigin();
        settingsWindow.QueueResetToOrigin();
        debugWindow.QueueResetToOrigin();
        mainWindow.IsOpen = true;
        settingsWindow.IsOpen = true;
    }

    internal void JumpWindowsToRandomVisibleLocations()
    {
        mainWindow.QueueRandomVisibleJump();
        settingsWindow.QueueRandomVisibleJump();
        debugWindow.QueueRandomVisibleJump();
        mainWindow.IsOpen = true;
        settingsWindow.IsOpen = true;
    }
}
