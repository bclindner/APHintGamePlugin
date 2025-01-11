using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;

namespace APHintGamePlugin;

public class APHintGame
{
    public ArchipelagoSession? APSession { get; set; } = null;
    private string[] Locations { get; set; } = [];

    private Random random = new Random();

    public string Connect(string url, string slot, string password) // TODO handle connection fail/success more elegantly...
    {
        APSession = ArchipelagoSessionFactory.CreateSession(url);
        var loginResult = APSession.TryConnectAndLogin("", slot, ItemsHandlingFlags.NoItems, null, ["HintGame"]);
        if (!loginResult.Successful)
        {
            var failure = (LoginFailure)loginResult;
            var errorMsg = "Failed to connect to Archipelago:";
            foreach (string failmsg in failure.Errors)
            {
                errorMsg += " " + failmsg;
            }
            return errorMsg;
        } else
        {
            return "Connection successful.";
        }
    }

    public void Hint()
    {
        if (APSession == null) {
            throw new Exception("APSession not connected");
        }
        var locations = APSession.Locations.AllMissingLocations;
        var locationCount = locations.Count;
        var locationId = locations[random.Next() % locationCount];
        APSession.Locations.ScoutLocationsAsync(HintCreationPolicy.CreateAndAnnounce, locationId);
    }
}
