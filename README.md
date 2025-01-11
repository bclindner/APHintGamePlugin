# APHintGamePlugin

Plugin allowing you to use XIV duty completions for Archipelago multiworld hints.

## Installation

Download the latest [release](https://github.com/bclindner/APHintGamePlugin/releases) and extract it.

Open Dalamud via `/xlplugins` and add the extracted folder as a plugin in your developer options.

## Usage

Open the options menu with `/aphintgame` and set your server URL (including port!), slot name, and password (if applicable). When you press "Connect", you should see "Connected!", or an error that should tell you something went wrong.

Close the window and launch into any duty, and when the duty completes, your Archipelago world should receive a random hint from your missing locations.

## Known Issues

* Connecting to Archipelago briefly freezes XIV.
* Hints may be sent for already hinted locations.

## Developing

See the [Dalamud docs](https://dalamud.dev/plugin-development/getting-started) - this plugin is very simple and should be pretty easy to navigate.

## Ideas/Issues?

Please use the GitHub issues tab to file a request or bug.
