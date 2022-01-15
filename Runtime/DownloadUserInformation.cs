using UnityEngine.Networking;

public static class DownloadUserInformation
{
    public static SketchfabResponse<SketchfabUserInfo> GetInfo(UnityWebRequest _request)
    {
        return SketchfabResponse<SketchfabUserInfo>.FromUserInformationResponse(_request);
    }
}
