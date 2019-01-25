using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour{

    
    [SerializeField] GameObject _boardTile;
    [SerializeField] GameObject player;
    [SerializeField] Transform board;
    [SerializeField] Transform players;

    [Header("BoardGenerator")]
    [SerializeField] int rowCount = 5;
    [SerializeField] int colomCount = 10;
    [SerializeField] float scaleWidth = 0.6f;
    [SerializeField] float scaleHeaight = 0.6f;
    [SerializeField] float offsetWidth = 0;
    [SerializeField] float offsetHeight = 0;

    float halfHeight = 0;
    float halfWidth = 0;

    private void Start()
    {

        halfHeight = ((rowCount - 1) * scaleHeaight) / 2;
        halfWidth = ((colomCount - 1) * scaleWidth) / 2;

        Generate();
        Vector2 startPos = new Vector2((0 * scaleWidth + offsetWidth) - halfWidth, (0 * scaleHeaight + offsetHeight) - halfHeight);
        for (int i = 0; i < GameUlarTanggaManager.MAIN.playerCount; i++) {
            GameUlarTanggaManager.MAIN.players.Add( Instantiate(player, startPos, Quaternion.identity , players));
        }
        Destroy(gameObject);
    }

    // Use this for initialization
    void Generate()
    {

        
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colomCount; j++)
            {
                
                Instantiate(_boardTile, new Vector2( ( Mathf.Abs(( (i%2 == 0?  0 : 1 * (colomCount-1)) - j)) * scaleWidth + offsetWidth) - halfWidth, (i * scaleHeaight + offsetHeight) - halfHeight), Quaternion.identity, board);
                
                //sr.color = (((i+j)%2 == 0)? new Color(231, 210, 167) : new Color(104, 79, 27));
            }
        }

    }


}
