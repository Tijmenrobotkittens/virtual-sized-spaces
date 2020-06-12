using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshProLinks : MonoBehaviour, IPointerClickHandler
{
    public class TextMeshProLinksEvent : UnityEvent<string> { };
    public TextMeshProLinksEvent Clicked = new TextMeshProLinksEvent();
    public void OnPointerClick(PointerEventData eventData)
    {

        TextMeshProUGUI pTextMeshPro = gameObject.GetComponent<TextMeshProUGUI>();

       // Debug.LogError("LINK CLICKED!");
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, Input.mousePosition, null);
        if (linkIndex != -1)
        {
           // Debug.LogError("LINK != -1");
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];

            if (linkInfo.GetLinkID().Contains("http") == true || linkInfo.GetLinkID().Contains("mailto") == true)
            {
                Application.OpenURL(linkInfo.GetLinkID());
            }
            else {
               // Debug.Log("LINK clicked on " + linkInfo.GetLinkID());
                Clicked.Invoke(linkInfo.GetLinkID());
            }
        }
        else
        {
            
           // Debug.LogError("LINK nuppes, geen klik index "+ linkIndex);
            Clicked.Invoke("");
        }
    }
}