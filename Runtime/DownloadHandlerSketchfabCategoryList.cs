using UnityEngine.Networking;

public static class DownloadHandlerSketchfabCategoryList
{
    public static SketchfabResponse<SketchfabCategoryList> GetCategoryList(UnityWebRequest _unityWebRequest)
    {
        return SketchfabResponse<SketchfabCategoryList>.FromCategoryListResponse(_unityWebRequest);
    }
}
