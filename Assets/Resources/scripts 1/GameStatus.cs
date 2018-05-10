using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

public class GameStatus: MonoBehaviour {
    public delegate void GameScoreAction();
    public static event GameScoreAction myGameScoreAction;

    public delegate void GameOverAction();
    public static event GameOverAction myGameOverAction;

    private SceneController scene;
	private int canMove = 1;

    void Start () {
        scene = SceneController.getInstance();
        scene.setGameStatus(this);
    }
	
	void Update () {
		
	}

    //hero逃离巡逻兵，得分
    public void heroEscapeAndScore() {
        myGameScoreAction();
		canMove = 1;
    }

    //巡逻兵捕获hero，游戏结束
    public void patrolHitHeroAndGameover() {
        myGameOverAction();
		canMove = 0;
    }
}
