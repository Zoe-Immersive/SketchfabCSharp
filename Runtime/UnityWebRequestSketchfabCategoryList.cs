using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public static class UnityWebRequestSketchfabCategoryList
{
    public struct Parameters
    {
        public string sort_by;

        public string UrlEcnode()
        {
            List<string> urlParameters = new List<string>();
            if (!string.IsNullOrWhiteSpace(sort_by))
            {
                urlParameters.Add($"sort_by={sort_by}");
            }

            return string.Join("&", urlParameters);
        }
    }

    //public static UnityWebRequest Search(Parameters _parameters, params string[] _keywords)
    //{
    //    return UnityWebRequest.Get($"{SketchfabEndpoints.ModelSearchEndpoint}&q={string.Join(" ", _keywords)}&{_parameters.UrlEcnode()}");
    //}

    public static UnityWebRequest GetCategoryList(Parameters _parameters)
    {
        return UnityWebRequest.Get($"{SketchfabEndpoints.CategoryEndpoint}?{_parameters.UrlEcnode()}");
    }

    //public static UnityWebRequest GetNextModelListPage(SketchfabModelList _previousModelList)
    //{
    //    return UnityWebRequest.Get(_previousModelList.NextPageUrl);
    //}
}
