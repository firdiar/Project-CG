using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Question {
    public string question;
    public string[] answer;
    public int TrueAnswer;
}

public class GameUlarTanggaManager : MonoBehaviour {

    public static GameUlarTanggaManager MAIN;
    [Header("PlayerData")]
    public int playerCount;
    public List<GameObject> players = new List<GameObject>();
    public UnityEngine.UI.Text currentPlayerText;

    [Header("BoardData")]
    public Transform board;

    [Header("DiceData")]
    public UnityEngine.UI.Image dice;
    public Sprite[] diceSide;

    [Header("Question Data")]
    [SerializeField] Question[] questions;
    [SerializeField] QuestionCard card;


    int currentPlayer = 0;
    bool isDiceRolled = false;
    int tempMove = 0;




    private void Awake()
    {
        MAIN = this;
    }

    void NextPlayer() {
        currentPlayer++;
        if (currentPlayer >= players.Count) {
            currentPlayer = 0;
        }
        currentPlayerText.text = "Player " + (currentPlayer + 1);
    }

    public void MovePlayer(int moveCount) {

        Debug.Log("Player move " + moveCount + "step");
        players[currentPlayer].GetComponent<Player>().Move(moveCount, false);

        NextPlayer();
        
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
        
        card.question = questions[players[currentPlayer].GetComponent<Player>().GetCurrentPos()];
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
        isDiceRolled = false;
        //tempMove = 0;
    }

    

    // Update is called once per frame
    void Update () {
		
	}
}
