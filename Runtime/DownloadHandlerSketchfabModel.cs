using UnityEngine.Networking;

public static class DownloadHandlerSketchfabModel
{
    public static SketchfabResponse<SketchfabModel> GetModel(UnityWebRequest _unityWebRequest)
    {
        return SketchfabResponse<SketchfabModel>.FromModelResponse(_unityWebRequest);
    }
}
