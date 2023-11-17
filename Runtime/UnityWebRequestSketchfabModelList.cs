using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public static class UnityWebRequestSketchfabModelList
{
    public struct Parameters
    {
        public string user;
        public List<string> tags;
        public List<string> categories;
        public List<string> licenses;
        // only used for search
        public int? minFaceCount;
        public int? maxFaceCount;
        // only used for model list
        public int? maxVertexCount;
        public DateTime? publishedSince;
        public bool? staffpicked;
        public bool? downloadable;
        public bool? animated;
        public bool? hasSound;
        public bool? restricted;

        public bool? archives_flavours;
        public int? available_archive_type;
        public int? archives_max_size;
        public int? archives_max_face_count;
        public int? archives_max_vertex_count;
        public int? archives_max_texture_count;
        public int? archives_texture_max_resolution;

        public int? count;
        public int? offset;

        public string UrlEcnode()
        {
            List<string> urlParameters = new List<string>();
            if (!string.IsNullOrWhiteSpace(user))
            {
                urlParameters.Add($"user={user}");
            }

            if (tags != null &&
                tags.Count != 0)
            {
                foreach (string tag in tags)
                {
                    urlParameters.Add($"tags={tag}");
                }
            }

            if (categories != null &&
                categories.Count != 0)
            {
                foreach (string category in categories)
                {
                    urlParameters.Add($"categories={category}");
                }
            }

            if (licenses != null &&
                licenses.Count != 0)
            {
                foreach (string license in licenses)
                {
                    urlParameters.Add($"licenses={license}");
                }
            }

            if (minFaceCount != null &&
                minFaceCount > 0)
            {
                urlParameters.Add($"min_face_count={minFaceCount.Value}");
            }

            if (maxFaceCount != null &&
                maxFaceCount > 0)
            {
                urlParameters.Add($"max_face_count={maxFaceCount.Value}");
            }

            if (maxVertexCount != null &&
                maxVertexCount > 0)
            {
                urlParameters.Add($"max_vertex_count={maxVertexCount.Value}");
            }

            if (publishedSince != null)
            {
                urlParameters.Add($"published_since={publishedSince.Value.ToString("yyyyMMddTHH:mm:ssZ")}");
            }

            if (staffpicked != null)
            {
                urlParameters.Add($"staffpicked={staffpicked.Value}");
            }

            if (downloadable != null)
            {
                urlParameters.Add($"downloadable={downloadable.Value}");
            }

            if (animated != null)
            {
                urlParameters.Add($"animated={animated.Value}");
            }

            if (hasSound != null)
            {
                urlParameters.Add($"has_sound={hasSound.Value}");
            }

            if (restricted != null)
            {
                urlParameters.Add($"restricted={restricted.Value}");
            }

            if(archives_flavours != null)
            {
                urlParameters.Add($"archives_flavours={archives_flavours.Value}");
            }

            if (available_archive_type != null)
            {
                urlParameters.Add($"available_archive_type={available_archive_type.Value}");
            }

            if (archives_max_size != null)
            {
                urlParameters.Add($"archives_max_size={archives_max_size.Value}");
            }

            if (archives_max_face_count != null)
            {
                urlParameters.Add($"archives_max_face_count={archives_max_face_count.Value}");
            }

            if (archives_max_vertex_count != null)
            {
                urlParameters.Add($"archives_max_vertex_count={archives_max_vertex_count.Value}");
            }

            if (archives_max_texture_count != null)
            {
                urlParameters.Add($"archives_max_texture_count={archives_max_texture_count.Value}");
            }

            if (archives_texture_max_resolution != null)
            {
                urlParameters.Add($"archives_texture_max_resolution={archives_texture_max_resolution.Value}");
            }

            if (count != null && count > 0)
            {
                urlParameters.Add($"count={count.Value}");
            }

            if (offset != null && offset > 0)
            {
                urlParameters.Add($"offset={offset.Value}");
            }

            return string.Join("&", urlParameters);
        }
    }

    public static UnityWebRequest Search(Parameters _parameters, params string[] _keywords)
    {
        return UnityWebRequest.Get($"{SketchfabEndpoints.ModelSearchEndpoint}&q={string.Join(" ", _keywords)}&{_parameters.UrlEcnode()}");
    }

    public static UnityWebRequest GetModelList(Parameters _parameters)
    {
        return UnityWebRequest.Get($"{SketchfabEndpoints.ModelEndpoint}?{_parameters.UrlEcnode()}");
    }

    public static UnityWebRequest GetNextModelListPage(SketchfabModelList _previousModelList)
    {
        return UnityWebRequest.Get(_previousModelList.NextPageUrl);
    }
}
