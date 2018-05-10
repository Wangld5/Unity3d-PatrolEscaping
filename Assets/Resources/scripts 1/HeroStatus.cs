using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

//----------------------------------
// 此脚本加在主人公hero上
//----------------------------------

public class HeroStatus : MonoBehaviour {
    public int standOnArea = -1;

	void Start () {
		
	}
	
	void Update () {
        modifyStandOnArea();
	}

    //检测所在区域
    void modifyStandOnArea() {
        float posX = this.gameObject.transform.position.x;
        float posZ = this.gameObject.transform.position.z;
        if (posZ >= FenchLocation.FenchHori) {
            if (posX < FenchLocation.FenchVertLeft)
                standOnArea = 0;
            else if (posX > FenchLocation.FenchVertRight)
                standOnArea = 2;
            else
                standOnArea = 1;
        }
        else {
            if (posX < FenchLocation.FenchVertLeft)
                standOnArea = 3;
            else if (posX > FenchLocation.FenchVertRight)
                standOnArea = 5;
            else
                standOnArea = 4;
        }
    }
}
