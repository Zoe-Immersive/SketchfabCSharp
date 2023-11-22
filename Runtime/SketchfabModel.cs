using Newtonsoft.Json; 

public class SketchfabModel
{
    public string Uid { get; set; }
    public string Name { get; set; }
    public bool IsDownloadable { get; set; }
    public string Description { get; set; }
    public int FaceCount { get; set; }
    public int VertexCount { get; set; }
    public SketchfabArchives Archives { get; set; } 
    public SketchfabThumbnailList Thumbnails { get; set; }
    public SketchfabLicense License { get; set; }
    public SketchfabUser User { get; set; }
    public override string ToString()
    {
        string modelString = $"Uid: {Uid}\n" +
            $"Name: {Name}\n" +
            $"IsDownloadable: {IsDownloadable}\n" +
            $"Description: {Description}\n" +
            $"Face count: {FaceCount}\n" +
            $"Vertex count: {VertexCount}\n" +
            $"License: {License}\n" +
            $"Thumbnails: {Thumbnails}\n" +
            $"User: {User}";

        return modelString;
    }

    public string GetJsonString ()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static SketchfabModel FromJson(string _data)
    {
        return JsonConvert.DeserializeObject<SketchfabModel>(_data);
    }
}
