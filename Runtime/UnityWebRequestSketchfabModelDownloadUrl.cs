using UnityEngine.Networking;

public class UnityWebRequestSketchfabModelDownloadUrl
{
    public static UnityWebRequest GetSketchfabModelDownloadUrl(string _modelUID)
    {
        return new UnityWebRequest(string.Format(SketchfabEndpoints.DownloadUrlRequestEndpoint, _modelUID), UnityWebRequest.kHttpVerbGET, new DownloadHandlerBuffer(), null);
    }
}
