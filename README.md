# SketchfabCSharp
C# wrapper for the Sketchfab API (Unity Plugin)

This is a homemade plugin that encapsulates some of the REST API endpoints from Sketchfab. Most of them are detailed here: https://docs.sketchfab.com/data-api/v3/index.html#/ while the endpoints for authentication are detailed here: https://sketchfab.com/developers/oauth.

## Dependencies
- glTFast v4.0.0 (https://openupm.com/packages/com.atteneder.gltfast/)
- Newtonsoft.Json for Unity v 12.0.201 (https://openupm.com/packages/jillejr.newtonsoft.json-for-unity/)

## How to make modifications

The project does not contain anything from a Unity project apart from the meta files and it is best it remains that way.

In order to make modifications and test the package in Unity, the best way is to create a Unity project and clone this repository somewhere in the Asset folder of said Unity project.

## How to use the plugin


### Importing the code
There are two options to use this project

1) You can clone the whole plugin as a submodule or just download the code.
2) Import it as a plugin in your unity project.

### Solve the dependencies
1) Install the glTFast and Newtonsoft.Json packages into the project

### Sketchfab Settings
1) Create an Sketchfab folder inside the Assets folder in your unity project
2) Create a Resources folder inside your new Sketchfab Folder (Sketchfab/Resources)
3) Navigate to Sketchfab/Resources (inside Unity)
4) Right Click > Create > SketchfabSettings
5) Add your Client ID and Client Secret into the SketchfabSettings.

# Functions Included

1) Authentication.
2) Get Model Information.
3) Download and Instantiate Models.
4) Get User Information (Own Account).
5) Get Model Lists (With thumbnails).
6) Search Models.
7) Model and Info cache in order to avoid queries for models or model information that you already downloaded.


# Examples

## Authentication

Once this code is executed all the following requests will be authenticated automatically

```
    SketchfabAPI.GetAccessToken(Email, Password, (SketchfabResponse<SketchfabAccessToken> answer) =>
    {
        if(answer.Success)
        {
            AccessToken = answer.Object.AccessToken;
            SketchfabAPI.AuthorizeWithAccessToken(answer.Object);
        }
        else
        {
            Debug.LogError(answer.ErrorMessage);
        }

    });
```

To Logout just call:

```
    SketchfabAPI.Logout(); that is equivalent to setting the access token to String.Empty
```


## Download Model

Downloads a model using its uid (if you want to use the cache system to save API calls see next example)

```
  // This first call will get the model information
  SketchfabAPI.GetModel(_uid, (resp) =>
  {
      // This second call will get the model information, download it and instantiate it
      SketchfabModelImporter.Import(resp.Object, (obj) =>
      {
          if(obj != null)
          {
              // Here you can do anything you like to obj (A unity game object containing the sketchfab model)
          }
      });
  });
```

Download model enabling the cache (Usefull if you want to avoid making API calls for models that you already downloaded)

```
  // This first call will get the model information
  bool enableCache = true;
  SketchfabAPI.GetModel(_uid, (resp) =>
  {
      // This second call will get the model information, download it and instantiate it
      SketchfabModelImporter.Import(resp.Object, (obj) =>
      {
          if(obj != null)
          {
              // Here you can do anything you like to obj (A unity game object containing the sketchfab model)
          }
      }, enableCache);
  }, enableCache);
```


## Get List of Models

In this snipped we get all the models that are downloadable, you will get SketchfabModel objects that can be used to download the models directly by calling SketchfabModelImporter.Import or you can use them to list them with their uid, name, if can be downloaded, a description, a face count, a vertex count and a list of thumbnails of the model that can be rendered as a preview.

```
    UnityWebRequestSketchfabModelList.Parameters p = new UnityWebRequestSketchfabModelList.Parameters();
    p.downloadable = true;
    SketchfabAPI.GetModelList(p,((SketchfabResponse<SketchfabModelList> _answer) =>
    {
        SketchfabResponse<SketchfabModelList> ans = _answer;
        m_ModelList = ans.Object.Models; 
    }));
```

## Model Searching by keywords

It follows the same logic than GetModelList but with the option of setting some keyworkds in that case we are looking for the keyword "Cat" but you can add many keywords as you want as the signature of the method is accepting `params string[] _keywords` as you can see on the method signature:
```
 public static void ModelSearch(Action<SketchfabResponse<SketchfabModelList>> _onModelListRetrieved, UnityWebRequestSketchfabModelList.Parameters _requestParameters, params string[] _keywords) 
```

```
    UnityWebRequestSketchfabModelList.Parameters p = new UnityWebRequestSketchfabModelList.Parameters();
    p.downloadable = true;
    string searchKeyword = "Cat";
    SketchfabAPI.ModelSearch(((SketchfabResponse<SketchfabModelList> _answer) =>
    {
        SketchfabResponse<SketchfabModelList> ans = _answer;
        m_ModelList = ans.Object.Models; 
    }), p, searchKeyword);
```



