using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SketchfabWebRequestManager : MonoBehaviour
{
    private static SketchfabWebRequestManager m_Instance;
    public static SketchfabWebRequestManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<SketchfabWebRequestManager>();

                if (m_Instance == null)
                {
                    GameObject sketchfabWebRequestManagerGameObject = new GameObject("SketchfabWebRequestManager");
                    m_Instance = sketchfabWebRequestManagerGameObject.AddComponent<SketchfabWebRequestManager>();
                }
            }

            return m_Instance;
        }
    }

    public void SendRequest(UnityWebRequest _request, Action<UnityWebRequest> _onRequestDone = null)
    {
        StartCoroutine(SendRequestAndWaitForDone(_request, _onRequestDone));
    }

    private IEnumerator SendRequestAndWaitForDone(UnityWebRequest _request, Action<UnityWebRequest> _onRequestDone = null)
    {
        yield return _request.SendWebRequest();
        _onRequestDone?.Invoke(_request);
    }

}
