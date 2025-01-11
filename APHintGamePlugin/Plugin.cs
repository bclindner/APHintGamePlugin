using System;
using APHintGamePlugin.Windows;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Dalamud.Game.Command;
using Dalamud.Game.Text;
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

    private const string CommandName = "/aphg";
    private const string ChatCommandName = "/aphgchat";

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

        CommandManager.AddHandler(ChatCommandName, new CommandInfo(OnSendAPMessage)
        {
            HelpMessage = "Send a message to the Archipelago chat."
        });

        PluginInterface.UiBuilder.Draw += DrawUI;

        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
        PluginInterface.UiBuilder.OpenMainUi += ToggleConfigUI;

        DutyState.DutyCompleted += OnHint;

        APHintGame.OnMessageReceived += OnReceiveAPMessage;

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

    private void OnReceiveAPMessage(object? sender, LogMessage message)
    {
        var entry = new XivChatEntry();
        entry.Message = message.ToString();
        ChatGui.Print(entry);
    }

    private void OnSendAPMessage(object? sender, string args)
    {
        APHintGame.SendAPMessage(args);
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
}
