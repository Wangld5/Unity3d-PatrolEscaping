using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

//----------------------------------
// 此脚本加在巡逻兵上
//----------------------------------

public class PatrolStatus : MonoBehaviour {
    private IAddAction addAction;
    private IGameStatusOp gameStatusOp;

    public int ownIndex;
    public bool isCatching;    //是否感知到hero

    private float CATCH_RADIUS = 3.0f;

    void Start () {
        addAction = SceneController.getInstance() as IAddAction;
        gameStatusOp = SceneController.getInstance() as IGameStatusOp;

        ownIndex = getOwnIndex();
        isCatching = false;
    }
	
	void Update () {
        checkNearByHero();
	}

    int getOwnIndex() {
        string name = this.gameObject.name;
        char cindex = name[name.Length - 1];
        int result = cindex - '0';
        return result;
    }

    //检测进入自己区域的hero
    void checkNearByHero () {
        if (gameStatusOp.getHeroStandOnArea() == ownIndex) {    //只有当走进自己的区域
            if (!isCatching) {
                isCatching = true;
                addAction.addDirectMovement(this.gameObject);
            }
        }
        else {
            if (isCatching) {    //刚才为捕捉状态，但此时hero已经走出所属区域
                gameStatusOp.heroEscapeAndScore();
                isCatching = false;
                addAction.addRandomMovement(this.gameObject, false);
            }
        }
    }

    void OnCollisionStay(Collision e) {
//		Debug.Log ("Pump somthing");
        //撞击围栏，选择下一个点移动
        if (e.gameObject.name.Contains("Patrol") || e.gameObject.name.Contains("fence")
            || e.gameObject.tag.Contains("FenceAround")) {
			Debug.Log ("Pump wall");
            isCatching = false;
            addAction.addRandomMovement(this.gameObject, false);
        }

        //撞击hero，游戏结束
        if (e.gameObject.name.Contains("hero")) {
            gameStatusOp.patrolHitHeroAndGameover();
			isCatching = false;
			addAction.addStop ();
        }
    }
}
