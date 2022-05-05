using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private GameManager gameManager; //debug
    public float gap = 1.3f;
    public float imagesize = 2.2f;


    public int Width;
    public int Height;


    public Block cardBlock, truckBlock;

    public GameObject cardFind, truckFind, clockFind;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.boardSetup();
        cardFind = GameObject.Find("cardFind");
        cardFind.SetActive(false);
        truckFind = GameObject.Find("truckFind");
        truckFind.SetActive(false);
        clockFind = GameObject.Find("clockFind");
        clockFind.SetActive(false);

        GenerateStage();
        cardBlock.OFF();

        StartCoroutine(CardFind());
    }

    public void GenerateStage()
    {
        float startX = -(imagesize * (Width / 2) + gap * (Width / 2)) + 5 * gap;
        float startY = -(imagesize * (Height / 2) + gap * (Height / 2)) + 1.2f * gap;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 position = new Vector3(startX + (imagesize * x) + (gap * x), startY + (imagesize * y) + (gap * y), 0);

                SpawnCard(gameManager.board[x, y], x, y, position);
            }
        }
    }

    //카드에 대해 카드 이미지 번호, 카드 width, 카드 height, 카드 위치 정보 전달.
    private void SpawnCard(int cardType, int positionWidth, int positionHeight, Vector3 position)
    {
        GameObject clone = Instantiate(cardPrefab, position, Quaternion.identity);

        clone.name = "Card" + positionWidth + positionHeight;
        clone.transform.SetParent(transform);
        Card card = clone.GetComponent<Card>();
        card.setCard(cardType, positionWidth, positionHeight);


        //Debug.Log("Spawned " + clone.name);
    }

    public IEnumerator CardFind()
    {
        yield return new WaitForSeconds(0.5f);
        cardFind.gameObject.SetActive(true);
    }

    public void TruckFind()
    {
        cardFind.gameObject.SetActive(false);
        truckFind.gameObject.SetActive(true);
    }

    public void ClockFind()
    {
        truckFind.gameObject.SetActive(false);
        clockFind.gameObject.SetActive(true);
        StartCoroutine(ToNext());
    }

    IEnumerator ToNext()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
