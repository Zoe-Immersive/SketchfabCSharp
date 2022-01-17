using UnityEngine;

[CreateAssetMenu(fileName = "SketchfabSettings", menuName="SketchfabSettings")]
public class SketchfabSettings : ScriptableObject
{
    private static SketchfabSettings m_Instance;
    public static SketchfabSettings Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Resources.Load<SketchfabSettings>("SketchfabSettings");
            }

            if (m_Instance == null)
            {
                Debug.LogError("SketchfabSettings couldn't be found, make sure it was properly installed to the Resources folder by re-importing SketchfabSettings.asset" +
                    " in the package");
            }

            return m_Instance;
        }
    }
#pragma warning disable 0649
    [SerializeField]
    private string m_ClientID;
    public string ClientID => m_ClientID;

    [SerializeField]
    private string m_ClientSecret;
    public string ClientSecret => m_ClientSecret;
#pragma warning restore 0649
}
