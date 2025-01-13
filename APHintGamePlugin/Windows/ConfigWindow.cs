using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace APHintGamePlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    private readonly APHintGame apHintGame;

    private string status = string.Empty;

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

    public void Dispose() {
        GC.SuppressFinalize(this);
    }

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
                status = "Connected!";
            }
            catch (Exception exc)
            {
                status = "Failed to connect to Archipelago: " + exc.Message;
            }
        }
        if (ImGui.Button("Disconnect"))
        {
            try
            {
                apHintGame.Disconnect();
            }
            catch (Exception exc)
            {
                status = "Disconnect failed: " + exc.Message;
            }
        }
        ImGui.TextUnformatted(status);
    }
}
