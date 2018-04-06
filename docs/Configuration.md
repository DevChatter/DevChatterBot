# Configuration
The configuration of DevChatterBot is in the appsettings.json file, however, if you want to develop without checking in your details, the project is configured to use the [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?tabs=visual-studio).

This is the current contents of the appsettings.json file.

```
{
  "TwitchUsername": "secret",
  "TwitchUserID": "secret",
  "TwitchOAuth": "secret",
  "TwitchChannel": "secret",
  "TwitchClientId": "secret"
}
```
If you're not worried about your twitch credentials, skip using the secret manager and just replace the word "secret" with each credential that is required. Keep the quotation marks, of course. 

You don't need to be an active twitch streamer to run the bot: 

First, add your credentials in the appsettings.json file. Then, simply start the bot in Visual Studio. Now switch to your twitch desktop client and use the drop down menu on the upper right corner to select "channel": the bot will be running in your own chat channel and you can test it to your heart's desire.

## TwitchUsername
This is the username of the Twitch account you want to use to connect to the Twich API and the Twitch chat, simply speaking: It's your twitch name. This could be your own user account or an account you created as a "bot" account. This is mostly used by the v3 API of Twitch, so we avoid it where possible.

**TwitchUsername example: DevChatterBot**

## TwitchClientId
Your Twitch Client ID. This is the first thing you need to get. If you've never registered for twitch developer/API access you won't have this yet. Don't waste your time looking for it in the Twitch client. Instead, head over to the Twitch Developer site, log in and go to "dashboard" and [register your app](https://dev.twitch.tv/dashboard/apps/create) . Give your app a name, e.g. DevchatterTwitchBotClone and for "redirect URI" just put http://localhost since the bot doesn't use a callback yet. Click register and then copy/save your client ID somewhere safe. You will need this for the next step!

**TwitchClientId example: 0123456789abcdefghijABCDEFGHIJ**

## TwitchUserID
The v5 Twitch API primarily uses this userId instead of using the Username. You can use a tool like Postman to call the Twitch API to find your userid on their [Users Reference](https://dev.twitch.tv/docs/v5/reference/users). 

If you're completely new to all of this, just do this: Open a new tab in your browser. Paste this in https://api.twitch.tv/kraken/users/YOURTWITCHUSERNAME?client_id=YOURTWITCHCLIENTID . Replace YOURTWITCHUSERNAME with, you guessed it, your TwitchUsername (see above) and replace YOURTWITCHCLIENTID with your TwitchClientID that you just got on the twitch dev website. Now hit enter. You should get a response back and the second thing in it will be your TwitchUserId. You're almost done!

**TwitchUserID example: 44322889**

## TwitchOAuth
You set this up on the Twitch developer site: https://twitchapps.com/tmi/
**Be very careful with this, since it's essentially a password for the account.**

**TwitchOAuth example: oauth:hi3curghksvcjefirskm1b81jbf452**

## TwitchChannel
This is the channel you want your bot to be watching and whose chat channel you want it to connect to. Again, if you only have one twitch account it's simply your twitch name and the same as your TwitchUsername.

**TwitchChannel example: DevChatter**



