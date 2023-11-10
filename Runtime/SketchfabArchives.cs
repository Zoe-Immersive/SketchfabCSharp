using Newtonsoft.Json;

public class SketchfabArchives
{
    public Archives Glb { get; set; }
    public Archives Gltf { get; set; }
    public Archives Source { get; set; }
    public Archives Usdz { get; set; }
}

public class Archives
{
    public int TextureCount { get; set; }
    public int Size { get; set; }
    public string Type { get; set; }
    public int TextureMaxResolution { get; set; }
    public int FaceCount { get; set; }
    public int VertexCount { get; set; }
}
