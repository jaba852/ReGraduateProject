using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI ItemValueText;
    public GameObject Panel;
    // Start is called before the first frame update
    public void SetupTooltip(string name, string value,Vector2 initialPosition)
    {
        Panel.SetActive(true);
        ItemNameText.text = name;
        ItemValueText.text = value;
        Panel.gameObject.transform.position = initialPosition + new Vector2(0f, 0f);

    }
    public void RemoveTooltip()
    {
        Panel.SetActive(false);
    }

}
