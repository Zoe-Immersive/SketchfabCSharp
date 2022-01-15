using Newtonsoft.Json;
using System;

public class SketchfabAccessToken
{
    public string AccessToken { get; private set; }
    public DateTime AccessTokenExpiryDate { get; private set; }
    public string RefreshToken { get; private set; }

    private int m_ResponseTimeToExpiryDateSeconds;

    private SketchfabAccessToken() { }

    [JsonConstructor]
    private SketchfabAccessToken(string access_token, int expires_in, string refresh_token)
    {
        AccessToken = access_token;
        m_ResponseTimeToExpiryDateSeconds = expires_in;
        AccessTokenExpiryDate = DateTime.Now.AddSeconds(m_ResponseTimeToExpiryDateSeconds);
        RefreshToken = refresh_token;
    }

    public SketchfabAccessToken(string _accessToken, DateTime _tokenExpiryDate, string _refreshToken)
    {
        AccessToken = _accessToken;
        AccessTokenExpiryDate = _tokenExpiryDate;
        RefreshToken = _refreshToken;
    }

    public override string ToString()
    {
        return $"Access token: {AccessToken}\n" +
            $"Expiry date: {AccessTokenExpiryDate}\n" +
            $"Refresh token: {RefreshToken}";
    }
}
