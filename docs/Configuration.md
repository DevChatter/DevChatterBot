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

## TwitchUsername
This is the username of the Twitch account you want to use to connect to the Twich API and the Twitch chat. This could be your own user account or an account you created as a "bot" account. This is mostly used by the v3 API of Twitch, so we avoid it where possible.

**Example: DevChatterBot**

## TwitchUserID
The v5 Twitch API primarily uses this userId instead of using the Username. You can use a tool like Postman to call the Twitch API to find your userid on their [Users Reference](https://dev.twitch.tv/docs/v5/reference/users)

**Example: 44322889**

## TwitchOAuth
You set this up on the Twitch developer site. **Be very careful with this, since it's essentially a password for the account.**


## TwitchChannel
This is the channel you want your bot to be watching and whose chat channel you want it to connect to.

**Example: DevChatter**

## TwitchClientId
Your Twitch Client ID. You set this up on the Twitch Developer site.

**Example: 0123456789abcdefghijABCDEFGHIJ**

