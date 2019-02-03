using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    int currentPos = 0;
    int nextCurrentPos = 0;
    int targetPos = 0;

    public int id = 0;

    Transform target = null;

    bool isNaikTangga = false;
    bool isMove = false;

	public void Move (int much , bool isNaikTangga) {
        isMove = true;
        targetPos = Mathf.Clamp( currentPos + much , 0 , 100);
        this.isNaikTangga = isNaikTangga;

        Debug.Log(targetPos);

        if (isNaikTangga)
        {
            target = GameUlarTanggaManager.MAIN.board.GetChild(targetPos);
        }
        else {
            target = GameUlarTanggaManager.MAIN.board.GetChild(nextCurrentPos);
        }
	}

    public int GetCurrentPos() {
        return currentPos;
    }

    void nextPos() {
        nextCurrentPos = currentPos + 1;
        target = GameUlarTanggaManager.MAIN.board.GetChild(nextCurrentPos);
        Debug.Log(currentPos);
    }

    void prevPos() {
        nextCurrentPos = currentPos - 1;
        target = GameUlarTanggaManager.MAIN.board.GetChild(nextCurrentPos);
    }
	
	// Update is called once per frame
	void Update () {

        if (!isMove) return;


        if (target != null)
        {
            if (isNaikTangga)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * 3f);
                if ((target.position - transform.position).magnitude < 0.01f)
                {
                    target = null;
                    isMove = false;
                    isNaikTangga = false;
                    currentPos = targetPos;
                    nextCurrentPos = currentPos;
                }
            }
            else
            {

                if ((target.position - transform.position).magnitude > 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, target.position.y + 0.4f), Time.deltaTime * 2.5f);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * 3f);
                }

                if ((target.position - transform.position).magnitude < 0.01f)
                {
                    currentPos = nextCurrentPos;
                    if (currentPos < targetPos && currentPos < 31)
                    {
                        
                        nextPos();
                    }
                    else if (currentPos > targetPos && currentPos < 31)
                    {

                        prevPos();
                    }
                    else if (currentPos >= 31 && targetPos > 31)
                    {
                        targetPos = 31 - (targetPos - 31);
                        prevPos();
                        Debug.Log("Masuk Sini");
                    }
                    else
                    {
                        target = null;
                        isMove = false;
                        int step = -1;
                        
                        if (GameUlarTanggaManager.MAIN.isSnakeOrLadder(currentPos, ref step))
                        {
                            Move(step, true);

                        }
                        if(currentPos == 31){
                            GameUlarTanggaManager.MAIN.setWinner();
                        }
                        else { GameUlarTanggaManager.MAIN.NextPlayer(); }
                        
                        

                    }
                }

            }
        }



        /*
        //Debug.Log("move");
        if (currentPos != targetPos && currentPos == nextCurrentPos && !isNaikTangga)
        {
            if (currentPos < targetPos && currentPos < 49)
            {
                nextPos();
            }
            else if (currentPos > targetPos && currentPos < 49)
            {
                prevPos();
            }
            else if (currentPos >= 49)
            {
                targetPos = targetPos - (targetPos - 49);
            }
        }
        else if (currentPos != targetPos && currentPos == nextCurrentPos && isNaikTangga)
        {
            target = GameUlarTanggaManager.MAIN.board.GetChild(targetPos);
            currentPos = targetPos;
            nextCurrentPos = currentPos;
        }
        else if (currentPos == targetPos && currentPos == nextCurrentPos && !isNaikTangga)
        {
            isMove = false;
            GameUlarTanggaManager.MAIN.isSnakeOrLadder(currentPos);
        }*/


        /*
        if (target != null) {
            if ((target.position - transform.position).magnitude < 0.01f)
            {

                isNaikTangga = false;
                Debug.Log("masuk sini");

                if (currentPos != targetPos) currentPos = nextCurrentPos;
                else {
                    target = null;
                    isMove = false;

                    GameUlarTanggaManager.MAIN.isSnakeOrLadder(currentPos);
                }
            }
            else if ((target.position - transform.position).magnitude > 0.5f && !isNaikTangga)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x , target.position.y+0.4f) , Time.deltaTime*1.5f);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * 3f);
            }
        }*/

       
	}
}
