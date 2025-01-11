using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace APHintGamePlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;

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

        configuration = plugin.Configuration;
        apHintGame = plugin.APHintGame;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var serverUrlValue = configuration.ServerURL;
        if (ImGui.InputText("Server URL", ref serverUrlValue, 2048))
        {
            configuration.ServerURL = serverUrlValue;
            configuration.Save();
        }

        var slotNameValue = configuration.SlotName;
        if (ImGui.InputText("Slot", ref slotNameValue, 2048))
        {
            configuration.SlotName = slotNameValue;
            configuration.Save();
        }

        var passwordValue = configuration.Password;
        if (ImGui.InputText("Password", ref passwordValue, 2048))
        {
            configuration.Password = passwordValue;
            configuration.Save();
        }

        if (ImGui.Button("Connect"))
        {
            configuration.Save();
            try
            {
                apHintGame.Connect(configuration.ServerURL, configuration.SlotName, configuration.Password);
                result = "Connected!";
            }
            catch (Exception exc)
            {
                result = "Failed to connect to Archipelago: " + exc.Message;
            }
        }
        ImGui.TextUnformatted(result);
    }
}
