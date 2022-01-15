using Newtonsoft.Json;
using System.Collections.Generic;


public class SketchfabThumbnailList
{
    [JsonProperty("images")]
    public List<SketchfabThumbnail> Thumbnails { get; set; }

    public override string ToString()
    {
        string thumbnailsString = string.Empty;
        foreach (SketchfabThumbnail thumbnail in Thumbnails)
        {
            thumbnailsString += thumbnail.ToString();
        }

        return thumbnailsString;
    }

    public SketchfabThumbnail ClosestThumbnailToSizeWithoutGoingBelow(int _witdth, int _height)
    {
        int diff = int.MaxValue;
        SketchfabThumbnail resultThumbnail = new SketchfabThumbnail();
        foreach (SketchfabThumbnail thumbnail in Thumbnails)
        {
            if (thumbnail.Width < _witdth ||
                thumbnail.Height < _height)
            {
                continue;
            }

            if (thumbnail.Width - _witdth < diff)
            {
                diff = thumbnail.Width - _witdth;
                resultThumbnail = thumbnail;
            }

            if (thumbnail.Height - _height < diff)
            {
                diff = thumbnail.Height - _height;
                resultThumbnail = thumbnail;
            }
        }

        return resultThumbnail;
    }
}
