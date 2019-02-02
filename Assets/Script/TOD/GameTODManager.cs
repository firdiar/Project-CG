using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class GameTODManager : MonoBehaviour
{
    [Header("GameScreen")]
    [SerializeField] GameObject gameScreen;
    [SerializeField]Transform baseCircle;
    [SerializeField] GameObject circle;
    [SerializeField] RectTransform bottle;
    [SerializeField] Text currentPLayerText;
    [SerializeField] Text targetPLayerText;
    [SerializeField] Text cardPlayerText;
    [SerializeField] RectTransform card;
    [SerializeField] Text soalText;
    [SerializeField] GameObject optionGame;
    Vector2 targetCard;
    List<string> soal;
    bool isMovingCard;
    int currentPlayer = 0;
    bool isRolling = false;
    public bool isCanRollAgain = true;
    float rollingPower = 0;
    float rotationPerPlayer = 0;
    int playerCount = 2;

    [Header("GameScreen")]
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject optionMenu;
    [SerializeField] Text pcount;


    // Start is called before the first frame update
    void Start()
    {
        //GenerateBoard(2);
        menuScreen.SetActive(true);
        gameScreen.SetActive(false);
        playerCount = 2;
    }

    public void Back() {
        targetCard = new Vector2(Screen.width * 2, card.localPosition.y);
        isMovingCard = true;
        isCanRollAgain = true;

        currentPLayerText.text = "Player " + (currentPlayer);
        targetPLayerText.text = "Player -";

    }

    public void addPlayer() {
        playerCount = Mathf.Clamp(playerCount + 1, 2, 8);
        updatePlayerCountText();
    }
    public void removePlayer()
    {
        playerCount = Mathf.Clamp(playerCount - 1, 2, 8);
        updatePlayerCountText();
    }
    public void updatePlayerCountText() {
        pcount.text = playerCount.ToString();
    }

    public void GenerateBoard() {

        menuScreen.SetActive(false);
        gameScreen.SetActive(true);
        currentPlayer = 1;
        //this.playerCount = int.Parse(pcount.text);
        string a = Resources.Load<TextAsset>("Data/TOD/Data").text;
        Debug.Log(a);
        soal = JsonConvert.DeserializeObject<List<string>>(a);

        bottle.eulerAngles = Vector3.zero;
        isCanRollAgain = true;

        currentPLayerText.text = "Player "+(currentPlayer);
        targetPLayerText.text = "Player -";
        
        float amount = Mathf.InverseLerp(0, 360, 360 / (playerCount*1f));
        rotationPerPlayer = 360 / (playerCount * 1f);
        for (int i = 0; i < playerCount; i++)
        {
            Image image = Instantiate(circle, baseCircle.position, Quaternion.Euler(0,0,i* rotationPerPlayer), baseCircle).GetComponent<Image>();
            image.fillAmount = amount;
            image.color = getColor(i);
        }

        Debug.Log("Generated");

    }

    public Color getColor(int i) {
        Debug.Log("Re Colored");
        return new Color(getBiner(i, 0) * 255, getBiner(i, 1) * 255, getBiner(i, 2) * 255);
    }

    int getBiner(int ke, int urutan)
    {
        int temp = 0;

        for (int i = 0; i <= urutan; i++)
        {
            temp = ke % 2;
            ke = Mathf.FloorToInt(ke / 2);

        }

        return temp;
    }

    public void Home() {
        // back to home
    }

    private void Update()
    {
        if (isRolling) {
            float nextRot = Mathf.Lerp(bottle.eulerAngles.z, bottle.eulerAngles.z+rollingPower , Time.deltaTime * 1.5f);

            rollingPower = Mathf.Clamp( rollingPower - (nextRot - bottle.eulerAngles.z ) , 0 ,  Mathf.Infinity);

            bottle.eulerAngles = new Vector3(0,0, nextRot );

            //Debug.Log(rollingPower);

            if (rollingPower < 3) {
                isRolling = false;
                Debug.Log(bottle.eulerAngles.z);
                int player = Mathf.FloorToInt(bottle.eulerAngles.z / rotationPerPlayer);
                Debug.Log("Kena PLayer " +( playerCount-player));
               
                currentPLayerText.text = "Player " + (currentPlayer );
                targetPLayerText.text = "Player "+ (playerCount - player);
                cardPlayerText.text = "Player " + (playerCount - player);

                currentPlayer = (currentPlayer + 1 <= playerCount ? currentPlayer + 1 : 1);

                soalText.text = soal[Random.RandomRange( 0 , soal.Count)];
                targetCard = new Vector2(0, card.localPosition.y); ;
                isMovingCard = true;
            }
            

        }

        if (isMovingCard) {
            card.localPosition = Vector2.Lerp(card.localPosition, targetCard, Time.deltaTime*5 );
            if (Vector2.Distance(card.localPosition, targetCard) < 1) {
                isMovingCard = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameScreen.activeInHierarchy)
            {
                optionGame.SetActive(true);
            }
            else {
                optionMenu.SetActive(true);
            }
        }
    }

    public void RollBottle(float power) {
        isCanRollAgain = false;

        isRolling = true;
        rollingPower = power * 5;
        Debug.Log(rollingPower);
    }
}
