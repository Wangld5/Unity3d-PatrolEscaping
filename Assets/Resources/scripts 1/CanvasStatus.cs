using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------
// 此脚本加在text上
//----------------------------------

public class CanvasStatus : MonoBehaviour {
    private int score = 0;
	private int textType;  

	void Start () {
		distinguishText();
	}
	
	void Update () {
	}
    void distinguishText() {
		if (gameObject.name.Contains ("Score"))
			textType = 0;
		else {
			Debug.Log (gameObject.name);
			textType = 1;
		}
    }
    void OnEnable() {
       	GameStatus.myGameScoreAction += gameScore;
		GameStatus.myGameOverAction += gameOver;
    }

    void OnDisable() {
        GameStatus.myGameScoreAction -= gameScore;
        GameStatus.myGameOverAction -= gameOver;
    }

    void gameScore() {
		if (textType == 0 && this.gameObject.name.Contains("Score")) {
            score++;
            this.gameObject.GetComponent<Text>().text = "Score: " + score;
        }
    } 

    void gameOver() {
		if (textType == 1) {
			this.gameObject.GetComponent<Text> ().text = "Game Over!";

		}
	}
}
