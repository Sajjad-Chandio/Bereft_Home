using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LinkHandler : MonoBehaviour
{
    public string url;
    public void OpenURL()
    {
        Application.OpenURL(url);
        Debug.Log($"Opening URL: {url}"); 
    }
}
