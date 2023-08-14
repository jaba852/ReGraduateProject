using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingimage : MonoBehaviour
{   public Sprite change_img1;
    public Sprite change_img2;
    public Sprite change_img3;
    public Sprite change_img4;
    public Sprite change_img5;
    public int RandomInt;
    Image load_img;
    public Text load_txt;
    void Awake()
    {  
        RandomInt = Random.Range(1,5);
        load_img = GetComponent<Image>();
        if(RandomInt  == 1)
        {
        load_img.sprite = change_img1;
        load_txt.text = "Portal,어디론가 이어져 있는 생물";
        }
        else if(RandomInt  == 2)
        {
        load_img.sprite = change_img2;
        load_txt.text = "MetorShower, 적들에게 강한 충격을 준다";
        }
        else if(RandomInt  == 3)
        {
        load_img.sprite = change_img3;
        load_txt.text ="Slash,거대한 괴물도 간단히 베어버린다";
        }
        else if(RandomInt  == 4)
        {
        load_img.sprite = change_img4;
        load_txt.text = "Recon,약점을 분석하여 더 강한 피해를 입힐 수 있다";
        }
        else if(RandomInt  == 5)
        {
        load_img.sprite = change_img5;
        load_txt.text = "Buff,자신의 육체를 강화한다";
        
        }


        //loadingimg.Image.SourceImage =loadimg1;
    }

   
    
}
