using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[System.Serializable]
public class Soal
{
    public string pertanyaan;
    public string jawaban;
    public bool horizontal ;
    public bool vertikal ;

}




public class BoardGeneratorTTS : MonoBehaviour
{


    [SerializeField] int colom;
    [SerializeField] int row;
    [SerializeField] float widthSpace;
    [SerializeField] float heightSpace;

    [SerializeField] GameObject boxPrefabs;
    [SerializeField] Soal soals;

    private void Start()
    {
        Generate();

    }
    private void Update()
    {
        Generate();
        
    }

    
    void Generate()
    {

            if (soals.horizontal)
            
            {
                for (int i = 0; i < soals.jawaban.Length; i++)
                {

                    Instantiate(boxPrefabs, new Vector2(i * widthSpace, 0), Quaternion.identity);


                }
            }
            else
            {
                for (int i = 0; i < soals.jawaban.Length; i++)
                {

                    Instantiate(boxPrefabs, new Vector2(0 , -i * heightSpace), Quaternion.identity);


                }

            }
       


    }

    void down(Vector2 position, Soal soals) 
    {
        if (soals.horizontal)
        {
            for (int i = 0; i < soals.jawaban.Length; i++)
            {
                
                Instantiate(boxPrefabs, new Vector2(position.x, position.y - 1), Quaternion.identity);
            }
        }
        
        

    }

    void up(Vector2 position, Soal soals)
    {

        if (soals.horizontal)
        {

            for (int i = 0; i < soals.jawaban.Length; i++)
            {
                
                Instantiate(boxPrefabs, new Vector2(position.x, position.y + 1), Quaternion.identity);
            }
        }



    }

    void left(Vector2 position, Soal soals)
    {
        if (soals.horizontal)
        {
            for (int i = 0; i < soals.jawaban.Length; i++)
            {
                Instantiate(boxPrefabs, new Vector2(position.x - 1, position.y ), Quaternion.identity);
            }
        }



    }

    void right(Vector2 position , Soal soals)
    {
        if (soals.horizontal)
        {
            for (int i = 0; i < soals.jawaban.Length; i++)
            {
                Instantiate(boxPrefabs, new Vector2(position.x + 1, position.y - 1), Quaternion.identity);
            }
        }



    }
}