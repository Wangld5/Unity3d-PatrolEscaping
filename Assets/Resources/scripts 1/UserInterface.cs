using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
    private IUserAction action;
	private GameStatus type;
    void Start () {
        action = SceneController.getInstance() as IUserAction;
    }
	
	void Update () {
        detectKeyInput();
    }

    void detectKeyInput() {
		if (Input.GetKey(KeyCode.DownArrow)) {
			action.heroMove (Diretion.DOWN);
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			action.heroMove (Diretion.UP);
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			action.heroMove (Diretion.LEFT);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			action.heroMove (Diretion.RIGHT);
		}
    }
}
