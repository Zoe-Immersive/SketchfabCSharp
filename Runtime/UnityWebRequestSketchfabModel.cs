using UnityEngine.Networking;

public static class UnityWebRequestSketchfabModel
{
    public static UnityWebRequest GetModel(string _modelUID)
    {
        return UnityWebRequest.Get(string.Join("/", SketchfabEndpoints.ModelEndpoint, _modelUID));
    }
}
