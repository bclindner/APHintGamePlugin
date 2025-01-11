using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;

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
            throw new Exception(errorMsg);
        }
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
}
