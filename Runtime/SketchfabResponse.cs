using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SketchfabResponse<T>
{
    private SketchfabResponse() {}
    public SketchfabErrorResponseType ErrorType { get; private set; } = SketchfabErrorResponseType.Unknown;
    public T Object { get; private set; } = default;
    public bool Success { get; private set; } = false;
    public string ErrorMessage { get; private set; } = string.Empty;

    internal static SketchfabResponse<SketchfabModelList> FromModelListResponse(UnityWebRequest _modelListRequest)
    {
        SketchfabResponse<SketchfabModelList> response = new SketchfabResponse<SketchfabModelList>();
        if (_modelListRequest.responseCode != 200)
        {
            ParseModelErrorResponse(response, _modelListRequest);

            return response;
        }

        SketchfabModelList modelList = JsonConvert.DeserializeObject<SketchfabModelList>(_modelListRequest.downloadHandler.text);

        if (modelList == null)
        {
            return response;
        }

        response.Object = modelList;
        response.Success = true;

        return response;
    }

    internal static SketchfabResponse<SketchfabCategoryList> FromCategoryListResponse(UnityWebRequest _categoryListRequest)
    {
        SketchfabResponse<SketchfabCategoryList> response = new SketchfabResponse<SketchfabCategoryList>();
        if (_categoryListRequest.responseCode != 200)
        {
            ParseModelErrorResponse(response, _categoryListRequest);

            return response;
        }

        SketchfabCategoryList categoryList = JsonConvert.DeserializeObject<SketchfabCategoryList>(_categoryListRequest.downloadHandler.text);

        if (categoryList == null)
        {
            return response;
        }

        response.Object = categoryList;
        response.Success = true;

        return response;
    }

    internal static SketchfabResponse<SketchfabAccessToken> FromAccessTokenResponse(UnityWebRequest _accessTokenRequest)
    {
        SketchfabResponse<SketchfabAccessToken> response = new SketchfabResponse<SketchfabAccessToken>();
        if (_accessTokenRequest.responseCode != 200)
        {
            ParseAccessTokenErrorResponse(response, _accessTokenRequest);

            return response;
        }

        SketchfabAccessToken accessToken = JsonConvert.DeserializeObject<SketchfabAccessToken>(_accessTokenRequest.downloadHandler.text);

        if (accessToken == null)
        {
            return response;
        }

        response.Object = accessToken;
        response.Success = true;

        return response;
    }

    internal static SketchfabResponse<SketchfabUserInfo> FromUserInformationResponse(UnityWebRequest _userInfoRequest)
    {
        SketchfabResponse<SketchfabUserInfo> response = new SketchfabResponse<SketchfabUserInfo>();
        if (_userInfoRequest.responseCode != 200)
        {
            ParseUserInfoErrorResponse(response, _userInfoRequest);
            return response;
        }

        JObject data = JObject.Parse(_userInfoRequest.downloadHandler.text);
        response.Object = new SketchfabUserInfo(data["uid"].ToString(), data["email"].ToString(), data["account"].ToString(), data["username"].ToString());
        response.Success = true;
        return response;
    }


    internal static SketchfabResponse<string> FromDownloadUrlResponse(UnityWebRequest _downloadUrlRequest)
    {
        SketchfabResponse<string> response = new SketchfabResponse<string>();
        if (_downloadUrlRequest.responseCode != 200)
        {
            ParseModelErrorResponse(response, _downloadUrlRequest);

            return response;
        }

        JObject jsonRootObject = JObject.Parse(_downloadUrlRequest.downloadHandler.text);

        if (jsonRootObject == null ||
            jsonRootObject["gltf"] == null)
        {
            Debug.LogError("Unexpected Error: Model archive is not available");

            response.Object = string.Empty;

            return response;
        }

        response.Object = jsonRootObject["gltf"].Value<string>("url");
        response.Success = true;

        return response;
    }

    internal static SketchfabResponse<SketchfabModel> FromModelResponse(UnityWebRequest _modelRequest)
    {
        SketchfabResponse<SketchfabModel> response = new SketchfabResponse<SketchfabModel>();
        if (_modelRequest.responseCode != 200)
        {
            ParseModelErrorResponse(response, _modelRequest);

            return response;
        }

        response.Object = JsonConvert.DeserializeObject<SketchfabModel>(_modelRequest.downloadHandler.text);
        response.Success = true;

        return response;
    }
    internal static SketchfabResponse<SketchfabUser> FromUserResponse(UnityWebRequest _userRequest)
    {
        SketchfabResponse<SketchfabUser> response = new SketchfabResponse<SketchfabUser>();
        if (_userRequest.responseCode != 200)
        {
            ParseModelErrorResponse(response, _userRequest);

            return response;
        }

        response.Object = JsonConvert.DeserializeObject<SketchfabUser>(_userRequest.downloadHandler.text);
        response.Success = true;

        return response;
    }

    private static void ParseModelErrorResponse<T>(SketchfabResponse<T> response, UnityWebRequest _unityWebRequest)
    {
        if (ParseCommonErrors(response, _unityWebRequest))
        {
            return;
        }

        if (_unityWebRequest.responseCode == 400)
        {
            response.ErrorType = SketchfabErrorResponseType.ModelNotDownloadable;
        }
        else if (_unityWebRequest.responseCode == 401 || _unityWebRequest.responseCode == 403)
        {
            response.ErrorType = SketchfabErrorResponseType.Unauthorized;
        }
        else if (_unityWebRequest.responseCode == 404)
        {
            response.ErrorType = SketchfabErrorResponseType.ModelNotFound;
        }
        else if(_unityWebRequest.responseCode == 429)
        {
            response.ErrorType = SketchfabErrorResponseType.DalilyLimitReached;
        }

        JObject jsonRootObject = JObject.Parse(_unityWebRequest.downloadHandler.text);

        string errorDetails = jsonRootObject.Value<string>("detail");

        if (!string.IsNullOrWhiteSpace(errorDetails))
        {
            response.ErrorMessage = errorDetails;
        }
    }

    private static void ParseAccessTokenErrorResponse<T>(SketchfabResponse<T> response, UnityWebRequest _unityWebRequest)
    {
        if (ParseCommonErrors(response, _unityWebRequest))
        {
            return;
        }

        JObject jsonRootObject = JObject.Parse(_unityWebRequest.downloadHandler.text);

        string errorType = jsonRootObject.Value<string>("error");
        string errorDescription = jsonRootObject.Value<string>("error_description");

        if (errorType == "invalid_grant")
        {
            response.ErrorType = SketchfabErrorResponseType.InvalidCredentials;
            response.ErrorMessage = errorDescription;
        }
        else if (errorType == "invalid_client")
        {
            response.ErrorType = SketchfabErrorResponseType.InvalidClient;
            response.ErrorMessage = "ClientID and/or Client secret is invalid.";
        }
    }

    private static void ParseUserInfoErrorResponse<T>(SketchfabResponse<T> _response, UnityWebRequest _unityWebRequest)
    {
        if (ParseCommonErrors(_response, _unityWebRequest))
        {
            return;
        }
        JObject jsonRootObject = JObject.Parse(_unityWebRequest.downloadHandler.text);

        string errorType = jsonRootObject.Value<string>("error");
        string errorDescription = jsonRootObject.Value<string>("error_description");
        Debug.Log($"Sketchfab API Error: type={errorType}, description={errorDescription}");
    }

    // Common logic to parse the errors common to all requests, if true is returned
    // then a common error was parsed, if not, returns false
    private static bool ParseCommonErrors<T>(SketchfabResponse<T> _response, UnityWebRequest _unityWebRequest)
    {
        if (_unityWebRequest.responseCode != 0 &&
            _unityWebRequest.responseCode != 401)
        {
            return false;
        }

        if (_unityWebRequest.responseCode == 0)
        {
            _response.ErrorType = SketchfabErrorResponseType.Unknown;
            _response.ErrorMessage = "An unknown error has occurred. Check your internet connection.";
        }
        else if (_unityWebRequest.responseCode == 401)
        {
            _response.ErrorType = SketchfabErrorResponseType.Unauthorized;
            _response.ErrorMessage = "Unauthorized request";
        }

        return true;
    }

    public static SketchfabResponse<T> OfflineResponse(T _obj)
    {
        SketchfabResponse<T> resp = new SketchfabResponse<T>();
        resp.Success = true;
        resp.Object = _obj;

        return resp;
    }
}
