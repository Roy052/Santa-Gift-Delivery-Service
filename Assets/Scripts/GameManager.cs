using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int[,] board;  //������
    public bool[,] check; //1. setup�� ī�� �ֱ�, 2. �� ī�尡 �̹� �����ִ°�?
    public int[] list;  //ī�� ���� 
    private int cardCount = 0; //���� Card�� �� �� �����ߴ°�?
    private int[,] tempCard;
    public int width, height;

    MainSceneManager msm;
    TutorialSceneManager tsm;

    int cardAmount = 20; //���� ī�� ����
    int trashAmount = 2; //���� ������ ����

    public AudioClip good, bad;

    public int score = 0, totalCardMatched = 0, cardMatched = 0, giftCount = 0, trashCount = 0, missedCount = 0; 
    //����, ��Ī�� ī�� ��

    int currentStage = 0; //0. Start 1. Tutorial 2. Main 3. Result


    void Start()
    {
        tempCard = new int[2,2];
        Screen.SetResolution(1920, 1080, true);
    }

    private void Update()
    {
        if(cardMatched == 20)
        {
            boardSetup();
            msm.GenerateStage();
            cardMatched = 0;
        }
        if (currentStage !=1 && SceneManager.GetActiveScene().name == "TutorialScene")
        {
            currentStage = 1;
            tsm = GameObject.Find("TutorialSceneManager").GetComponent<TutorialSceneManager>();
        }
        else if (currentStage != 2 && SceneManager.GetActiveScene().name == "MainScene")
        {
            currentStage = 2;
            msm = GameObject.Find("MainSceneManager").GetComponent<MainSceneManager>();
        }
        else if (currentStage != 3 && SceneManager.GetActiveScene().name == "ResultScene")
        {
            currentStage = 3;
            msm = GameObject.Find("MainSceneManager").GetComponent<MainSceneManager>();
        }
    }

    public void boardSetup()
    {
        int i, j, k, temp;

        board = new int[width, height];
        check = new bool[width, height];
        list = new int[width * height / 2];

        bool[] randomCheck = new bool[cardAmount];
        for (i = 0; i < cardAmount; i++) randomCheck[i] = false;
        for (i = 0; i < width * height / 2; i++)
        {
            while (true)
            {
                temp = Random.Range(0, cardAmount);
                if (randomCheck[temp] == false)
                {
                    list[i] = temp;
                    randomCheck[temp] = true;
                    break;

                }
            }
        }
        //for (i = 0; i < width * height / 2; i++) Debug.Log(i + "th is" + list[i]);

        //������ ����. check�� 1�� �뵵�� ���.
        for (i = 0; i < width * height / 2; i++)
        {
            while (true)
            {
                j = Random.Range(0, width);
                k = Random.Range(0, height);
                if (check[j, k] == false)
                {
                    board[j, k] = list[i];
                    check[j, k] = true;
                    break;
                }
            }

            while (true)
            {
                j = Random.Range(0, width);
                k = Random.Range(0, height);
                if (check[j, k] == false)
                {
                    board[j, k] = list[i];
                    check[j, k] = true;
                    break;
                }
            }
        }

        //check�� 2�� �뵵�� ���� ���� �ʱ�ȭ
        for (i = 0; i < width; i++)
            for (j = 0; j < height; j++)
                check[i, j] = false;
    }

    //ī�� ��Ī
    public IEnumerator CardMatch(int positionWidth, int positionHeight)
    {
        if (check[positionWidth, positionHeight] == true) yield return null; //�̹� ����� ������ skip
        //ó�� ���� ī���� ���,
        else if (cardCount == 0)
        {
            tempCard[0,0] = positionWidth;
            tempCard[0,1] = positionHeight;
            cardCount++;
        }
        //�ι�° ���� ī���� ���,
        else if (positionWidth == tempCard[0,0] && positionHeight == tempCard[0,1])
        {
            yield return null;
        }
        else
        {
            //�� ī�尡 ��ġ�� ���,
            if (board[positionWidth, positionHeight] == board[tempCard[0,0], tempCard[0,1]])
            {
                check[positionWidth, positionHeight] = true;
                check[tempCard[0,0], tempCard[0,1]] = true;
                tempCard[1, 0] = positionWidth;
                tempCard[1, 1] = positionHeight;
                GameObject.Find("Card" + positionWidth + positionHeight).GetComponent<Card>().beforeMatched();
                GameObject.Find("Card" + tempCard[0, 0] + tempCard[0, 1]).GetComponent<Card>().Matched();
                GameObject.Find("Card" + tempCard[1, 0] + tempCard[1, 1]).GetComponent<Card>().Matched();

                totalCardMatched += 2;
                cardMatched += 2;
                //Ʈ���� ������
                Trucktime();
            }
            //�� ī�尡 ��ġ���� ���� ���,
            else
            {
                yield return new WaitForSeconds(0.5f);
                GameObject.Find("Card" + positionWidth + positionHeight).GetComponent<Card>().Flip();
                GameObject.Find("Card" + tempCard[0,0] + tempCard[0,1]).GetComponent<Card>().Flip();
            }
            cardCount = 0;
        }
    }

    public void Trucktime()
    {
        if(currentStage == 1)
        {
            tsm.truckBlock.OFF();
            tsm.cardBlock.ON();
            tsm.TruckFind();
        }
        else if(currentStage == 2)
        {
            msm.truckBlock.OFF();
            msm.cardBlock.ON();
        }
    }

    public void TruckSelected(int truckNum)
    {
        Debug.Log(board[tempCard[0, 0], tempCard[0, 1]] + ", " + cardAmount + ", " + trashAmount);
        if (truckNum == 0 && board[tempCard[0,0],tempCard[0,1]] >= cardAmount - trashAmount)
        {
            missedCount += 2;
            if(currentStage == 1)
            {
                tsm.GetComponent<AudioSource>().clip = bad;
                tsm.GetComponent<AudioSource>().Play();
            }
            if(currentStage == 2)
            {
                msm.GetComponent<AudioSource>().clip = bad;
                msm.GetComponent<AudioSource>().Play();
            }
            
        }
        else if(truckNum == 1 && board[tempCard[0, 0], tempCard[0, 1]] < cardAmount - trashAmount)
        {
            missedCount += 2;
            score -= 10;
            if (currentStage == 1)
            {
                tsm.GetComponent<AudioSource>().clip = bad;
                tsm.GetComponent<AudioSource>().Play();
            }
            if (currentStage == 2)
            {
                msm.GetComponent<AudioSource>().clip = bad;
                msm.GetComponent<AudioSource>().Play();
            }
        }
        else if (truckNum == 0 && board[tempCard[0, 0], tempCard[0, 1]] < cardAmount - trashAmount)
        {
            giftCount += 2;
            score += 10;
            if (currentStage == 1)
            {
                tsm.GetComponent<AudioSource>().clip = good;
                tsm.GetComponent<AudioSource>().Play();
            }
            if (currentStage == 2)
            {
                msm.GetComponent<AudioSource>().clip = good;
                msm.GetComponent<AudioSource>().Play();
            }
        }
        else if (truckNum == 1 && board[tempCard[0, 0], tempCard[0, 1]] >= cardAmount - trashAmount)
        {
            trashCount += 2;
            score += 10;
            if (currentStage == 1)
            {
                tsm.GetComponent<AudioSource>().clip = good;
                tsm.GetComponent<AudioSource>().Play();
            }
            if (currentStage == 2)
            {
                msm.GetComponent<AudioSource>().clip = good;
                msm.GetComponent<AudioSource>().Play();
            }
        }

        if (currentStage == 1)
        {
            tsm.truckBlock.ON();
            tsm.cardBlock.ON();
            tsm.ClockFind();
        }
        else if (currentStage == 2)
        {
            msm.truckBlock.ON();
            msm.cardBlock.OFF();
        }
    }
}
