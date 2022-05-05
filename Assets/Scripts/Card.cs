using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private Sprite[] cardImages;
    [SerializeField]
    private Sprite backImage;

    private int cardType;

    private SpriteRenderer spriteRenderer;
    int positionWidth, positionHeight;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void setCard(int cardType, int positionWidth, int positionHeight)
    {
        this.cardType = cardType;
        this.positionWidth = positionWidth;
        this.positionHeight = positionHeight;
    }

    private void OnMouseDown()
    {
        //카드 이미지 추가.
        spriteRenderer.sprite = cardImages[cardType];

        StartCoroutine(gameManager.CardMatch(positionWidth, positionHeight));
    }

    public void Flip()
    {
        spriteRenderer.sprite = backImage;
    }

    private void OnMouseEnter()
    {
        //만약 매칭 되지 않았다면 선택되는 느낌이 없도록
        if (gameManager.check[positionWidth, positionHeight] == false)
            transform.localScale = new Vector2(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f);
    }

    private void OnMouseExit()
    {
        //만약 매칭 되지 않았다면 선택되는 느낌이 없도록
        if (gameManager.check[positionWidth, positionHeight] == false)
            transform.localScale = new Vector2(transform.localScale.x - 0.2f, transform.localScale.y - 0.2f);
    }

    public void beforeMatched()
    {
        transform.localScale = new Vector2(transform.localScale.x - 0.2f, transform.localScale.y - 0.2f);
    }

    public void Matched()
    {
        this.gameObject.SetActive(false);
    }
}
