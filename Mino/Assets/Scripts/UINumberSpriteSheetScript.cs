using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINumberSpriteSheetScript : MonoBehaviour {

    Image thisSprite;
    public Sprite n0, n1, n2, n3;

    private void Awake()
    {
        thisSprite = this.GetComponent<Image>();
    }

    public void ChangeNumberSprite(int number)
    {
        switch (number)
        {
            case 0:
                thisSprite.sprite = n0;
                break;
            case 1:
                thisSprite.sprite = n1;
                break;
            case 2:
                thisSprite.sprite = n2;
                break;
            case 3:
                thisSprite.sprite = n3;
                break;
            default:
                thisSprite.sprite = n0;
                break;
        }       
    }
}
