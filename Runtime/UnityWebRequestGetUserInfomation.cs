using UnityEngine.Networking;

public static class UnityWebRequestGetUserInformation
{
    public static UnityWebRequest Get()
    {
        return UnityWebRequest.Get(SketchfabEndpoints.UserInfomationEndpoint);
    }
}
