using Newtonsoft.Json;
using System.Collections.Generic;

public class SketchfabCategoryList
{
    [JsonProperty("results")]
    public List<SketchfabCategory> Categorys { get; set; }

    [JsonProperty("next")]
    public string NextPageUrl { get; set; }

    public override string ToString()
    {
        string finalString = string.Empty;
        for (int i = 0; i < Categorys.Count; i++)
        {
            finalString += $"Category {i+1}: {Categorys[i].ToString()}\n";
        }

        finalString += $"Next page URL: {NextPageUrl}";

        return finalString;
    }
}
