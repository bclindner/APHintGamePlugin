using System;
using Dalamud.Configuration;

namespace APHintGamePlugin;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public string ServerURL { get; set; } = "localhost:38281";

    public string SlotName { get; set; } = String.Empty;

    public string Password { get; set; } = String.Empty;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }

}
