using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image UIImage;
    public Sprite Image;
    public int SlotIndex;
    public void UpdateSlotUI()
    {
        UIImage.sprite = Image;
        UIImage.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        Image = null;
        UIImage.gameObject.SetActive(false);
    }
}