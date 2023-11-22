using UnityEngine.Networking;

public static class DownloadHandlerSketchfabUser
{
    public static SketchfabResponse<SketchfabUser> GetUser(UnityWebRequest _unityWebRequest)
    {
        return SketchfabResponse<SketchfabUser>.FromUserResponse(_unityWebRequest);
    }
}
