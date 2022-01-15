using Newtonsoft.Json;
using System.Collections.Generic;

public class SketchfabModelList
{
    [JsonProperty("results")]
    public List<SketchfabModel> Models { get; set; }

    [JsonProperty("next")]
    public string NextPageUrl { get; set; }

    public override string ToString()
    {
        string finalString = string.Empty;
        for (int i = 0; i < Models.Count; i++)
        {
            finalString += $"Model {i+1}: {Models[i].ToString()}\n";
        }

        finalString += $"Next page URL: {NextPageUrl}";

        return finalString;
    }
}
