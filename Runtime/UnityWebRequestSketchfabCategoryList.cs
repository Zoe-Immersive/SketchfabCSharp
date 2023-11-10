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

    public static UnityWebRequest GetCategoryList(Parameters _parameters)
    {
        return UnityWebRequest.Get($"{SketchfabEndpoints.CategoryEndpoint}?{_parameters.UrlEcnode()}");
    }
}
