using Newtonsoft.Json;

public class SketchfabLicense
{
    public string Uri { get; set; }
    public string Uid { get; set; }
    public string Label { get; set; }
    public string FullName { get; set; }
    public string Requirements { get; set; }
    public string Url { get; set; }
    public string Slug { get; set; }

    public override string ToString()
    {
        string modelString = $"Uri: {Uri}\n" +
            $"Uid: {Uid}\n" +
            $"Label: {Label}\n" +
            $"FullName: {FullName}\n" +
            $"Requirements: {Requirements}\n" +
            $"Url: {Url}\n" +
            $"Slug: {Slug}\n";

        return modelString;
    }
    public string GetJsonString()
    {
        return JsonConvert.SerializeObject(this);
    }
    public static SketchfabLicense FromJson(string _data)
    {
        return JsonConvert.DeserializeObject<SketchfabLicense>(_data);
    }
}
