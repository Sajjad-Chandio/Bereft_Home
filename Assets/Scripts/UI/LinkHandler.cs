using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LinkHandler : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI textMeshPro;

    void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }
    }

    // Called when the UI element is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        // Get the index of the clicked link
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, Camera.main);

        if (linkIndex != -1) // If a link was clicked
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string url = linkInfo.GetLinkID(); // Get the link's URL

            Application.OpenURL(url); // Open the link in the default browser
            Debug.Log($"Opening link: {url}");
        }
    }
}
