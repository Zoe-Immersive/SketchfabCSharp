using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class SketchfabThumbnailImporter
{
    public static async Task<Sprite> ImportAsync(SketchfabThumbnail _thumbnail)
    {
        return await ImportAsync(_thumbnail, new Rect(0.0f, 0.0f, _thumbnail.Width, _thumbnail.Height));
    }

    public static async Task<Sprite> ImportAsync(SketchfabThumbnail _thumbnail, Rect _spriteRect)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(_thumbnail.Url);

        unityWebRequest.SendWebRequest();

        while (!unityWebRequest.isDone) { }

        Texture2D imgTexture = new Texture2D(_thumbnail.Width, _thumbnail.Height);
        imgTexture.LoadImage(unityWebRequest.downloadHandler.data);
        Sprite sprite = Sprite.Create(imgTexture, _spriteRect, new Vector2(0.5f, 0.5f));
        sprite.name = "Ma bite";

        return sprite;
    }
}
