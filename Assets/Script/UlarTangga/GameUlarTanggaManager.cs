using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUlarTanggaManager : MonoBehaviour {

    public static GameUlarTanggaManager MAIN;
    public int playerCount;
    public List<GameObject> players = new List<GameObject>();
    public Transform board;


    private void Awake()
    {
        MAIN = this;
    }

    private void Start()
    {
        players[0].GetComponent<Player>().Move(25,false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
