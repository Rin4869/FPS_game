using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonEffect : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public Image buttonBackground;

    public Color normalColor = new Color(1f, 1f, 1f, 0.15f);
    public Color hoverColor = new Color(1f, 1f, 1f, 0.35f);

    public float normalScale = 1f;
    public float hoverScale = 1.05f;

    void Start()
    {
        transform.localScale = Vector3.one * normalScale;

        if (buttonBackground != null)
            buttonBackground.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale =
            Vector3.one * hoverScale;

        if (buttonBackground != null)
            buttonBackground.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale =
            Vector3.one * normalScale;

        if (buttonBackground != null)
            buttonBackground.color = normalColor;
    }
}