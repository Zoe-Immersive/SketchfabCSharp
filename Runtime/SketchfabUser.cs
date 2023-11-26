using System;
using Newtonsoft.Json; 

public class SketchfabUser
{
    public string Uid { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Account { get; set; }
    public string ProfileUrl { get; set; }
    public string Uri { get; set; }
    
    
    public override string ToString()
    {
        string modelString = $"Uid: {Uid}\n" +
                             $"Username: {Username}\n" +
                             $"DisplayName: {DisplayName}\n" +
                             $"Account: {Account}\n" +
                             $"ProfileUrl: {ProfileUrl}\n" +
                             $"Uri: {Uri}\n";

        return modelString;
    }

    public string GetJsonString ()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static SketchfabUser FromJson(string _data)
    {
        return JsonConvert.DeserializeObject<SketchfabUser>(_data);
    }
}
