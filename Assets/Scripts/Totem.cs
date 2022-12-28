using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    //public string activeElement;                           


    private SpriteRenderer spriteRenderer;        //Store a component reference to the attached SpriteRenderer.

    private RoomGameManager roomGameManager;

    void Awake()
    {
        //Get a component reference to the SpriteRenderer.
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ChoseElement()
    {
        //Set spriteRenderer to the damaged wall sprite.
        //spriteRenderer.sprite = dmgSprite;

        roomGameManager.elementsAvailable.Add("Fire");

        //Disable the food object the player collided with.
        gameObject.SetActive(false);
    }
    
}
