using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameManager gm;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    private void OnMouseDown()
    {
        if (this.gameObject.name == "GiftTruck") gm.TruckSelected(0);
        else gm.TruckSelected(1);
    }


    private void OnMouseEnter()
    {
        transform.localScale = new Vector2(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f);
    }

    private void OnMouseExit()
    {
        transform.localScale = new Vector2(transform.localScale.x - 0.2f, transform.localScale.y - 0.2f);
    }
}
