using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class SketchfabAPI
{
    public static bool Authorized => !string.IsNullOrWhiteSpace(m_Token);

    private static Sketchfab.AuthorizationType m_AuthorizationType;
    private static string m_Token;

    public static void AuthorizeWithAPIToken(string _apiToken)
    {
        m_AuthorizationType = Sketchfab.AuthorizationType.APIToken;
        m_Token = _apiToken;
    }

    public static void AuthorizeWithAccessToken(string _accessToken)
    {
        m_AuthorizationType = Sketchfab.AuthorizationType.AccessToken;
        m_Token = _accessToken;
    }

    public static void Logout()
    {
        m_Token = String.Empty;
    }

    public static void AuthorizeWithAccessToken(SketchfabAccessToken _accessToken)
    {
        AuthorizeWithAccessToken(_accessToken.AccessToken);
    }

    public static void GetAccessToken(string _emailAddress, string _password, Action<SketchfabResponse<SketchfabAccessToken>> _onAccessTokenRetrieved)
    {
        UnityWebRequest uwr = UnityWebRequestSketchfabAccessToken.GetAccessToken(_emailAddress, _password);
        uwr.SetRequestHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{SketchfabSettings.Instance.ClientID}:{SketchfabSettings.Instance.ClientSecret}"))}");

        SketchfabWebRequestManager.Instance.SendRequest(uwr, (UnityWebRequest _request) =>
        {
            _onAccessTokenRetrieved?.Invoke(DownloadHandlerSketchfabAccessToken.GetAccessToken(uwr));
        });
    }

    public static void GetAccessToken(string _refreshToken, Action<SketchfabResponse<SketchfabAccessToken>> _onAccessTokenRetrieved)
    {
        UnityWebRequest uwr = UnityWebRequestSketchfabAccessToken.GetAccessToken(_refreshToken, SketchfabSettings.Instance.ClientID, SketchfabSettings.Instance.ClientSecret);

        SketchfabWebRequestManager.Instance.SendRequest(uwr, (UnityWebRequest _request) =>
        {
            _onAccessTokenRetrieved?.Invoke(DownloadHandlerSketchfabAccessToken.GetAccessToken(uwr));
        });
    }

    public static void GetModel(string _modelUID, Action<SketchfabResponse<SketchfabModel>> _onModelRetrieved, bool _enableCache=false)
    {
        // Make sure that the data is initialized as persistent data path can't be accessed from the main thread
        SketchfabModelImporter.EnsureInitialized();

        bool inCache = false;
        if(_enableCache)
        {
            // TODO: this method needs to run async as now is running from the main thread
            inCache = Task.Run(() => SketchfabModelImporter.IsUidInCache(_modelUID)).Result;
            if (inCache)
            {
                // Try to get the model metadata
                SketchfabModel modelMetadata = Task.Run(() => SketchfabModelImporter.GetCachedModelMetadata(_modelUID)).Result;
                if (modelMetadata != null)
                {
                    SketchfabResponse<SketchfabModel> response = SketchfabResponse<SketchfabModel>.OfflineResponse(modelMetadata);
                    _onModelRetrieved?.Invoke(response);
                    return;
                }
            }

        }

        UnityWebRequest uwr = UnityWebRequestSketchfabModel.GetModel(_modelUID);
        SketchfabWebRequestManager.Instance.SendRequest(uwr, (UnityWebRequest _request) =>
        {
            SketchfabResponse<SketchfabModel> response = DownloadHandlerSketchfabModel.GetModel(uwr);
            // NOTE: In order to have backward compatibility we need to check the case
            // where the metadata is not cached but the model is already cached
            if(_enableCache && inCache)
            {
                if(response.Success && response.Object != null)
                {
                    SketchfabModelImporter.SaveModelMetadataToCache(response.Object);
                }
            }
            _onModelRetrieved?.Invoke(response);
        });
    }

    public static void GetUserInformation(Action<SketchfabResponse<SketchfabUserInfo>> _onInfoReceived)
    {
        UnityWebRequest uwr = UnityWebRequestGetUserInformation.Get();
        AuthenticateRequest(uwr);

        SketchfabWebRequestManager.Instance.SendRequest(uwr, (UnityWebRequest _request) =>
        {
            _onInfoReceived?.Invoke(DownloadUserInformation.GetInfo(_request));
        });
    }


    public static void GetModelList(UnityWebRequestSketchfabModelList.Parameters _requestParameters, Action<SketchfabResponse<SketchfabModelList>> _onModelListRetrieved)
    {
        UnityWebRequest uwr = UnityWebRequestSketchfabModelList.GetModelList(_requestParameters);

        SketchfabWebRequestManager.Instance.SendRequest(uwr, (UnityWebRequest _request) =>
        {
            _onModelListRetrieved?.Invoke(DownloadHandlerSketchfabModelList.GetModelList(uwr));
        });
    }

    public static void ModelSearch(Action<SketchfabResponse<SketchfabModelList>> _onModelListRetrieved, UnityWebRequestSketchfabModelList.Parameters _requestParameters, params string[] _keywords)
    {
        UnityWebRequest uwr = UnityWebRequestSketchfabModelList.Search(_requestParameters, _keywords);

        SketchfabWebRequestManager.Instance.SendRequest(uwr, (UnityWebRequest _request) =>
        {
            _onModelListRetrieved?.Invoke(DownloadHandlerSketchfabModelList.GetModelList(uwr));
        });
    }

    public static void GetNextModelListPage(SketchfabModelList _previousModelList, Action<SketchfabResponse<SketchfabModelList>> _onModelListRetrieved)
    {
        UnityWebRequest uwr = UnityWebRequestSketchfabModelList.GetNextModelListPage(_previousModelList);

        SketchfabWebRequestManager.Instance.SendRequest(uwr, (UnityWebRequest _request) =>
        {
            _onModelListRetrieved?.Invoke(DownloadHandlerSketchfabModelList.GetModelList(uwr));
        });
    }


    public static void GetGLTFModelDownloadUrl(string _modelUID, Action<SketchfabResponse<string>> _onModelDownloadUrlRetrieved)
    {
        UnityWebRequest uwr = UnityWebRequestSketchfabModelDownloadUrl.GetSketchfabModelDownloadUrl(_modelUID);
        AuthenticateRequest(uwr);

        SketchfabWebRequestManager.Instance.SendRequest(uwr, (UnityWebRequest _request) =>
        {
            _onModelDownloadUrlRetrieved?.Invoke(DownloadHandlerSketchfabModelDownloadUrl.GetGLTFModelDownloadUrl(uwr));
        });
    }

    private static void AuthenticateRequest(UnityWebRequest _unityWebRequest)
    {
        switch (m_AuthorizationType)
        {
            case Sketchfab.AuthorizationType.AccessToken:
                _unityWebRequest.SetRequestHeader("Authorization", $"Bearer {m_Token}");
                break;
            case Sketchfab.AuthorizationType.APIToken:
                _unityWebRequest.SetRequestHeader("Authorization", $"Token {m_Token}");
                break;
        }
    }
}
