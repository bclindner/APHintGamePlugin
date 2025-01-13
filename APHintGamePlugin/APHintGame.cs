using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Lumina.Excel.Sheets;
using LogMessage = Archipelago.MultiClient.Net.MessageLog.Messages.LogMessage;

namespace APHintGamePlugin;

public class APHintGame
{
    public ArchipelagoSession? APSession { get; set; } = null;

    private readonly Random random = new();

    public void Connect(string url, string slot, string password)
    {
        APSession = ArchipelagoSessionFactory.CreateSession(url);
        var loginResult = APSession.TryConnectAndLogin("", slot, ItemsHandlingFlags.NoItems, null, ["HintGame"]);
        if (!loginResult.Successful)
        {
            var failure = (LoginFailure)loginResult;
            var errorMsg = "";
            foreach (var failmsg in failure.Errors)
            {
                errorMsg += failmsg + " ";
            }
            APSession = null;
            throw new Exception(errorMsg);
        } else
        {
            APSession.MessageLog.OnMessageReceived += HandleAPMessage;
        }
    }

    public void Disconnect()
    {
        APSession?.Socket.DisconnectAsync().Wait();
    }

    public void Hint()
    {
        if (APSession == null)
        {
            throw new Exception("APSession not connected");
        }
        var locations = APSession.Locations.AllMissingLocations;
        var locationCount = locations.Count;
        var locationId = locations[random.Next() % locationCount];
        APSession.Locations.ScoutLocationsAsync(HintCreationPolicy.CreateAndAnnounce, locationId);
    }

    private void HandleAPMessage(LogMessage message)
    {
        OnMessageReceived?.Invoke(this, message);
    }

    public void SendAPMessage(string message)
    {
        if (APSession == null)
        {
            throw new Exception("APSession not connected");
        }
        APSession.Say(message);
    }

    public event EventHandler<LogMessage>? OnMessageReceived;
}
