using UnityEngine.Networking;

public static class UnityWebRequestGetUserByUid
{
    public static UnityWebRequest Get(string _uid)
    {
        return UnityWebRequest.Get(string.Join("/", SketchfabEndpoints.UserByUidEndpoint, _uid));
    }
}
