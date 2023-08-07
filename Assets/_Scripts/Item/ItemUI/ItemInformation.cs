using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemInformation : MonoBehaviour
{
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI ItemValueText;
    // Start is called before the first frame update
    public void SetupTooltip(string name, string value,Vector2 initialPosition)
    {
        ItemNameText.text = name;
        ItemValueText.text = value;
        transform.position = initialPosition + new Vector2(0f, 3f);
    }


}
