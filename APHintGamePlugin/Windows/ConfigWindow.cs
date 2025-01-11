using System;
using System.Numerics;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace APHintGamePlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    private APHintGame apHintGame;

    private string result = String.Empty;

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("APHintGamePlugin Config###APHintGamePlugin config")
    {

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(232, 180),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        Configuration = plugin.Configuration;
        apHintGame = plugin.APHintGame;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var serverUrlValue = Configuration.ServerURL;
        if (ImGui.InputText("Server URL", ref serverUrlValue, 2048))
        {
            Configuration.ServerURL = serverUrlValue;
            Configuration.Save();
        }

        var slotNameValue = Configuration.SlotName;
        if (ImGui.InputText("Slot", ref slotNameValue, 2048))
        {
            Configuration.SlotName = slotNameValue;
            Configuration.Save();
        }

        var passwordValue = Configuration.Password;
        if (ImGui.InputText("Password", ref passwordValue, 2048))
        {
            Configuration.Password = passwordValue;
            Configuration.Save();
        }

        if (ImGui.Button("Connect"))
        {
            Configuration.Save();
            result = apHintGame.Connect(Configuration.ServerURL, Configuration.SlotName, Configuration.Password);
        }
        ImGui.TextUnformatted(result);
    }
}
