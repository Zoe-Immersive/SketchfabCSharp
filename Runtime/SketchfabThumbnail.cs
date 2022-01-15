public class SketchfabThumbnail
{
    public string Uid { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Url { get; set; }

    public override string ToString()
    {
        return $"Uid: {Uid}\n" +
            $"Width: {Width}\n" +
            $"Height: {Height}\n" +
            $"Url: {Url}\n";
    }
}
