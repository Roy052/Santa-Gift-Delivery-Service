using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
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

    public GameObject timeUp;

    public AudioClip alarm;

    public Button toMain;

    public Text Gift, Trash, Missed, Score;
    public GameObject Recipt;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.boardSetup();
        GenerateStage();
        cardBlock.OFF();
        timeUp.SetActive(false);
        Recipt.SetActive(false);
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

    public IEnumerator GameEnd()
    {
        cardBlock.ON();
        truckBlock.ON();
        timeUp.SetActive(true);
        gameManager.GetComponent<AudioSource>().Stop();
        this.GetComponent<AudioSource>().clip = alarm;
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3f);
        this.GetComponent<AudioSource>().Stop();
        timeUp.SetActive(false);
        Recipt.gameObject.SetActive(true);
        Gift.text = gameManager.giftCount.ToString();
        yield return new WaitForSeconds(1f);
        Trash.text = gameManager.trashCount.ToString();
        yield return new WaitForSeconds(1f);
        Missed.text = gameManager.missedCount.ToString();
        yield return new WaitForSeconds(1f);
        Score.text = gameManager.score.ToString();

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
