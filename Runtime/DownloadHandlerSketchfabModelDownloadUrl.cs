using UnityEngine.Networking;

public static class DownloadHandlerSketchfabModelDownloadUrl
{
    public static SketchfabResponse<string> GetGLTFModelDownloadUrl(UnityWebRequest _request)
    {
        return SketchfabResponse<string>.FromDownloadUrlResponse(_request);
    }
}
