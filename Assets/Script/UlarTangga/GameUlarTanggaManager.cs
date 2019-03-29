using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;


[System.Serializable]
public class Question
{
    public string question { get; set; }
    public List<string> answer { get; set; }
    public int trueAnswer { get; set; }
}



public class GameUlarTanggaManager : MonoBehaviour {

    public static GameUlarTanggaManager MAIN;
    [SerializeField] SoundBase sound;

    [Header("GameScreen")]
    [SerializeField] GameObject optionGame;

    [Header("Scene Data")]
    [SerializeField] GameObject MainMenuScreen;
    [SerializeField] GameObject GameScreen;
    [SerializeField] GameObject ResultScreen;

    [Header("MainMenuScreen")]
    [SerializeField] UnityEngine.UI.Text TextPlayerCount;
    [SerializeField] GameObject optionMenu;

    [Header("ResultScreen")]
    [SerializeField] Transform podium;



    [Header("PlayerData")]
    [SerializeField] GameObject playerPrefabs;
    [SerializeField] Transform playerTransform;
    public int playerCount;
    public List<GameObject> players = new List<GameObject>();
    public UnityEngine.UI.Text currentPlayerText;

    [Header("BoardData")]
    public Transform board;

    [Header("DiceData")]
    public UnityEngine.UI.Image dice;
    public Sprite[] diceSide;

    [Header("Question Data")]
    [SerializeField]List<Question> questions;
    [SerializeField] QuestionCard card;


    int currentPlayer = 0;
    bool isDiceRolled = false;
    int tempMove = 0;
    [SerializeField]Vector2[] posSnakeAndLadder;


    public void addPlayerCount() {
        playerCount = Mathf.Clamp(playerCount + 1, 1, 6);
        TextPlayerCount.text = playerCount.ToString();
    }
    public void minusPlayerCount()
    {
        playerCount = Mathf.Clamp(playerCount-1 , 1 , playerCount);
        TextPlayerCount.text = playerCount.ToString();
    }

    public bool isSnakeOrLadder(int pos , ref int step) {
        step = -1;
        foreach (Vector2 v in posSnakeAndLadder)
        {
            if (v.x == pos)
            {

                step = (int)(v.y - v.x);

                break;
            }

        }
        return step != -1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (MainMenuScreen.activeInHierarchy)
            {
                optionMenu.SetActive(true);
            }
            else if (GameScreen.activeInHierarchy)
            {
                optionGame.SetActive(true);
            }
            else if (ResultScreen.activeInHierarchy) {
                SetActiveMainMenu();
            }
        }
    }

    public void SetActiveMainMenu() {

        foreach (GameObject p in players)
        {
            Destroy(p);
        }
        players.Clear();

        card.Hide();
        isDiceRolled = false;
        MainMenuScreen.SetActive(true);
        GameScreen.SetActive(false);
        ResultScreen.SetActive(false);

        
    }
    public void SetActiveGame() {
        MainMenuScreen.SetActive(false);
        GameScreen.SetActive(true);
        ResultScreen.SetActive(false);
    }
    public void SetActiveResult() {
        MainMenuScreen.SetActive(false);
        GameScreen.SetActive(false);
        ResultScreen.SetActive(true);
    }

    private void Awake()
    {
        MAIN = this;
    }

    void Start()
    {
        TextPlayerCount.text = playerCount.ToString();

        //Load Soal
        string a = Resources.Load<TextAsset>("Data/UlarTangga/Data").text;
        Debug.Log(a);
       questions = JsonConvert.DeserializeObject<List<Question>>(a);
        for (int i = 0; i < questions.Count; i++)
        {
            Question temp = questions[i];
            int randomIndex = Random.Range(i, questions.Count);
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }

    }

    public void StartGame() {
        currentPlayer = 0;
        SetActiveGame();

        for (int i = 0; i < GameUlarTanggaManager.MAIN.playerCount; i++)
        {
            Player p = Instantiate(playerPrefabs, board.GetChild(0).position, Quaternion.identity, playerTransform).GetComponent<Player>();
            p.id = i + 1;
            p.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(getBiner(i, 0) * 255, getBiner(i, 1) * 255, getBiner(i, 2) * 255);
            
            GameUlarTanggaManager.MAIN.players.Add(p.gameObject);
        }
    }

    int getBiner(int ke, int urutan) {
        int temp = 0;
       
        for (int i = 0; i <= urutan; i++) {
            temp = ke % 2;
            ke = Mathf.FloorToInt( ke / 2 );
            
        }

        return temp;
    }

    public void NextPlayer() {
        currentPlayer++;
        if (currentPlayer >= players.Count) {
            currentPlayer = 0;
        }
        Debug.Log("Player berganti");
        currentPlayerText.text = "Player " + (currentPlayer + 1);
        isDiceRolled = false;
    }

    public void MovePlayer(int moveCount , bool naikTangga = false) {

        Debug.Log("Player move " + moveCount + "step");
        players[currentPlayer].GetComponent<Player>().Move(moveCount, naikTangga);

        
        
    }

    public void RollDice() {
        if (isDiceRolled) { Debug.Log("Dice still Rolled Now"); return; }

        Debug.Log("Dice Rolled");


        StartCoroutine(RollDiceAnim(2));
    }

    IEnumerator RollDiceAnim(float RollTime)
    {
        isDiceRolled = true;
        float time = 0;
        int i = 0;
        while (time < RollTime)
        {
            i = Random.RandomRange(0, diceSide.Length);
            dice.sprite = diceSide[i];
            

            yield return new WaitForSeconds(0.1f);
            time += 0.1f;
        }
        
        tempMove = i + 1;
        
        card.question = (players[currentPlayer].GetComponent<Player>().GetCurrentPos()<questions.Count) ? questions[players[currentPlayer].GetComponent<Player>().GetCurrentPos()] :  questions[Random.Range(0,questions.Count)];
        card.Show(result);
        //MovePlayer(i+1);

    }

    public void result(bool res) {
        if (res)
        {
            MovePlayer(tempMove);
        }
        else {
            MovePlayer(-tempMove);
        }
        Debug.Log("Terjawab");
        
        //tempMove = 0;
    }

    public void setWinner() {

        Debug.Log("Player ke " + currentPlayer+"Winning The Game");
        sound.PlaySound("TepukTangan");
        players.Sort((b,a)=>  a.GetComponent<Player>().GetCurrentPos().CompareTo(b.GetComponent<Player>().GetCurrentPos()));
        int loop = players.Count >= 3 ? 3 : players.Count;
        for (int i = 0; i < loop; i++) {
            podium.GetChild(i).GetComponent<UnityEngine.UI.Text>().text = "Player - "+players[i].GetComponent<Player>().id.ToString();
        }

        SetActiveResult();
    }


    public void QuitGame()
    {
        //Kembali ke Home
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
        LoadingScreen.MAIN.ChangeScene("Home");
    }
}
