using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTextHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Text buttonText; // 버튼의 텍스트 컴포넌트

    private Color normalColor; // 기본 색상
    public Color hoverColor; // 마우스가 올라갔을 때의 색상
    public Color pressColor; // 버튼이 눌렸을 때의 색상

    private void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        normalColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonText.color = pressColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
    }
}
