using Newtonsoft.Json; 

public class SketchfabCategory
{
    public string Uid { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Uri { get; set; }
    public string Icon { get; set; }
    public SketchfabThumbnail[] Thumbnails { get; set; }

    public override string ToString()
    {
        string categoryString = $"Uid: {Uid}\n" +
            $"Name: {Name}\n" +
            $"Slug: {Slug}\n" +
            $"Uri: {Uri}\n" +
            $"Icon: {Icon}\n" +
            $"Thumbnails: {Thumbnails}";

        return categoryString;
    }

    public string GetJsonString ()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static SketchfabCategory FromJson(string _data)
    {
        return JsonConvert.DeserializeObject<SketchfabCategory>(_data);
    }
}
