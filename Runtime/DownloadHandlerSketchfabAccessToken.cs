using UnityEngine.Networking;

public static class DownloadHandlerSketchfabAccessToken
{
    public static SketchfabResponse<SketchfabAccessToken> GetAccessToken(UnityWebRequest _request)
    {
        return SketchfabResponse<SketchfabAccessToken>.FromAccessTokenResponse(_request);
    }
}
