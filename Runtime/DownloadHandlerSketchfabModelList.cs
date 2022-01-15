using UnityEngine.Networking;

public static class DownloadHandlerSketchfabModelList
{
    public static SketchfabResponse<SketchfabModelList> GetModelList(UnityWebRequest _unityWebRequest)
    {
        return SketchfabResponse<SketchfabModelList>.FromModelListResponse(_unityWebRequest);
    }
}
