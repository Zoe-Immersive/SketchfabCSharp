using System.Collections.Generic;
using UnityEngine.Networking;

public static class UnityWebRequestSketchfabAccessToken
{
    public static UnityWebRequest GetAccessToken(string _username, string _password)
    {
        return UnityWebRequest.Post(SketchfabEndpoints.AccessTokenEndpoint,
            new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", _username },
                { "password", _password }
            });
    }

    public static UnityWebRequest GetAccessToken(string _refreshToken, string _clientID, string _clientSecret)
    {
        return UnityWebRequest.Post(SketchfabEndpoints.AccessTokenEndpoint,
            new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "client_id", _clientID },
                { "client_secret", _clientSecret },
                { "refresh_token", _refreshToken }
            });
    }
}
