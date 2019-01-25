using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    int currentPos = 0;
    int nextCurrentPos = 0;
    int targetPos = 0;

    Transform target = null;

    bool isNaikTangga = false;

	public void Move (int much , bool isNaikTangga) {
        targetPos = Mathf.Clamp( currentPos + much , 0 , 100);
        this.isNaikTangga = isNaikTangga;
	}

    void nextPos() {
        nextCurrentPos = currentPos + 1;
        target = GameUlarTanggaManager.MAIN.board.GetChild(nextCurrentPos);
        
    }

    void prevPos() {
        nextCurrentPos = currentPos - 1;
        target = GameUlarTanggaManager.MAIN.board.GetChild(nextCurrentPos);
    }
	
	// Update is called once per frame
	void Update () {
        if (currentPos != targetPos && currentPos == nextCurrentPos && !isNaikTangga)
        {
            if (currentPos < targetPos)
            {
                nextPos();
            }
            else if (currentPos > targetPos)
            {
                prevPos();
            }
        }
        else if(target == null && isNaikTangga){
            target = GameUlarTanggaManager.MAIN.board.GetChild(targetPos);
            currentPos = targetPos;
        }

        if (target != null) {
            if ((target.position - transform.position).magnitude < 0.01f)
            {
                isNaikTangga = false;
                if(currentPos != targetPos) currentPos = nextCurrentPos;
            }
            else if ((target.position - transform.position).magnitude > 0.5f && !isNaikTangga)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x , target.position.y+0.4f) , Time.deltaTime*2.5f);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * 2.5f);
            }
        }
	}
}
