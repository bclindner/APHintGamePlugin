using System;
using APHintGamePlugin.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace APHintGamePlugin;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static IDutyState DutyState { get; private set; } = null!;
    [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;

    private const string CommandName = "/aphintgame";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("APHintGamePlugin");
    private ConfigWindow ConfigWindow { get; init; }

    public APHintGame APHintGame { get; set; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        APHintGame = new APHintGame();

        ConfigWindow = new ConfigWindow(this);

        WindowSystem.AddWindow(ConfigWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Launch the APHintGamePlugin config window."
        });

        PluginInterface.UiBuilder.Draw += DrawUI;

        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
        PluginInterface.UiBuilder.OpenMainUi += ToggleConfigUI;

        DutyState.DutyCompleted += OnHint;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);

        DutyState.DutyCompleted -= OnHint;
    }

    private void OnCommand(string command, string args)
    {
        ToggleConfigUI();
    }

    private void OnHint(object? sender, ushort number)
    {
        Log.Information("Sending hint...");
        try
        {
            APHintGame.Hint();
        }
        catch (Exception exc)
        {
            Log.Error($"Error sending hint: {exc}");
        }
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
}
