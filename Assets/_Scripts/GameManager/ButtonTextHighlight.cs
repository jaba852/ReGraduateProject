using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTextHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Text buttonText; // ��ư�� �ؽ�Ʈ ������Ʈ

    private Color normalColor; // �⺻ ����
    public Color hoverColor; // ���콺�� �ö��� ���� ����
    public Color pressColor; // ��ư�� ������ ���� ����

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
