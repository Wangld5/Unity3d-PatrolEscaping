using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

public class GameModel : SSActionManager, ISSActionCallback {
    public GameObject PatrolItem, HeroItem, sceneModelItem, canvasItem;

    private SceneController scene;
    private GameObject myHero, sceneModel, canvasAndText;
    private List<GameObject> PatrolSet;
    private List<int> PatrolLastDir;
	private CanvasStatus mytext;

    private float PERSON_SPEED_NORMAL = 0.05f;
    private float PERSON_SPEED_CATCHING = 0.06f;
	private float PERSON_SPEED_HERO = 0.1f;
	private int ifStop = 0;

    void Awake() {
		PatrolFactory.getInstance().initPatrol(PatrolItem);
		PatrolFactory.getInstance ().initHero (HeroItem);
    }

    protected new void Start () {
        scene = SceneController.getInstance();
        scene.setGameModel(this);

//        genHero();
        genPatrolsAndHero();
        sceneModel = Instantiate(sceneModelItem);
        canvasAndText = Instantiate(canvasItem);
    }

    protected new void Update() {
        base.Update();
    }

    //生产英雄
//    void genHero() {
//        myHero = Instantiate(HeroItem);
//    }

    //生产巡逻兵
    void genPatrolsAndHero() {
		myHero = PatrolFactory.getInstance ().getHero ();
        PatrolSet = new List<GameObject>(6);
        PatrolLastDir = new List<int>(6);
        Vector3[] posSet = PatrolFactory.getInstance().getPosSet();
        for (int i = 0; i < 6; i++) {
            GameObject newPatrol = PatrolFactory.getInstance().getPatrol();
            newPatrol.transform.position = posSet[i];
            newPatrol.name = "Patrol" + i;
            PatrolLastDir.Add(-2);
            PatrolSet.Add(newPatrol);
            addRandomMovement(newPatrol, true);
        }
    }

    //hero移动
    public void heroMove(int dir) {
		if (ifStop == 1)
			return;
        myHero.transform.rotation = Quaternion.Euler(new Vector3(0, dir * 90, 0));
        switch (dir) {
            case Diretion.UP:
				myHero.transform.position += new Vector3(0, 0, PERSON_SPEED_HERO);
                break;
            case Diretion.DOWN:
				myHero.transform.position += new Vector3(0, 0, -PERSON_SPEED_HERO);
                break;
            case Diretion.LEFT:
				myHero.transform.position += new Vector3(-PERSON_SPEED_HERO, 0, 0);
                break;
            case Diretion.RIGHT:
				myHero.transform.position += new Vector3(PERSON_SPEED_HERO, 0, 0);
                break;
        }
    }

    //动作结束后
    public void SSActionEvent(SSAction source, SSActionEventType eventType = SSActionEventType.Completed, 
        SSActionTargetType intParam = SSActionTargetType.Normal, string strParam = null, object objParam = null) {
        if (intParam == SSActionTargetType.Normal)
            addRandomMovement(source.gameObject, true);
        else
            addDirectMovement(source.gameObject);
    }

    //isActive说明是否主动变向（动作结束）
    public void addRandomMovement(GameObject sourceObj, bool isActive) {
		if (ifStop == 1)
			return;
        int index = getIndexOfObj(sourceObj);
        int randomDir = getRandomDirection(index, isActive);
        PatrolLastDir[index] = randomDir;

        sourceObj.transform.rotation = Quaternion.Euler(new Vector3(0, randomDir * 90, 0));
        Vector3 target = sourceObj.transform.position;
        switch (randomDir) {
            case Diretion.UP:
                target += new Vector3(0, 0, 1);
                break;
            case Diretion.DOWN:
                target += new Vector3(0, 0, -1);
                break;
            case Diretion.LEFT:
                target += new Vector3(-1, 0, 0);
                break;
            case Diretion.RIGHT:
                target += new Vector3(1, 0, 0);
                break;
        }
        addSingleMoving(sourceObj, target, PERSON_SPEED_NORMAL, false);
    }
	public void addStop(){
		//1->stop 0->move
		ifStop = 1;
	}
    int getIndexOfObj(GameObject sourceObj) {
        string name = sourceObj.name;
        char cindex = name[name.Length - 1];
        int result = cindex - '0';
        return result;
    }
    int getRandomDirection(int index, bool isActive) {
        int randomDir = Random.Range(-1, 3);
        if (!isActive) {    //当碰撞时，不走同方向
            while (PatrolLastDir[index] == randomDir || PatrolOutOfArea(index, randomDir)) {
                randomDir = Random.Range(-1, 3);
            }
        }
        else {              //当非碰撞时，不走反方向
            while (PatrolLastDir[index] == 0 && randomDir == 2 
                || PatrolLastDir[index] == 2 && randomDir == 0
                || PatrolLastDir[index] == 1 && randomDir == -1
                || PatrolLastDir[index] == -1 && randomDir == 1
                || PatrolOutOfArea(index, randomDir)) {
                randomDir = Random.Range(-1, 3);
            }
        }
        //Debug.Log(isActive + " isActive " + "PatrolLastDir " + PatrolLastDir[index] + " -- randomDir " + randomDir);
        return randomDir;
    }
    //判定巡逻兵走出了自己的区域
    bool PatrolOutOfArea(int index, int randomDir) {
        Vector3 patrolPos = PatrolSet[index].transform.position;
        float posX = patrolPos.x;
        float posZ = patrolPos.z;
        switch (index) {
            case 0:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertLeft
                    || randomDir == 2 && posZ - 1 < FenchLocation.FenchHori)
                    return true;
                break;
            case 1:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertRight
                    || randomDir == -1 && posX - 1 < FenchLocation.FenchVertLeft
                    || randomDir == 2 && posZ - 1 < FenchLocation.FenchHori)
                    return true;
                break;
            case 2:
                if (randomDir == -1 && posX - 1 < FenchLocation.FenchVertRight
                    || randomDir == 2 && posZ - 1 < FenchLocation.FenchHori)
                    return true;
                break;
            case 3:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertLeft
                    || randomDir == 0 && posZ + 1 > FenchLocation.FenchHori)
                    return true;
                break;
            case 4:
                if (randomDir == 1 && posX + 1 > FenchLocation.FenchVertRight
                    || randomDir == -1 && posX - 1 < FenchLocation.FenchVertLeft
                    || randomDir == 0 && posZ + 1 > FenchLocation.FenchHori)
                    return true;
                break;
            case 5:
                if (randomDir == -1 && posX - 1 < FenchLocation.FenchVertRight
                    || randomDir == 0 && posZ + 1 > FenchLocation.FenchHori)
                    return true;
                break;
        }
        return false;
    }

    //追捕hero
    public void addDirectMovement(GameObject sourceObj) {
		if (ifStop == 1)
			return;
        int index = getIndexOfObj(sourceObj);
        PatrolLastDir[index] = -2;

        sourceObj.transform.LookAt(sourceObj.transform);
        Vector3 oriTarget = myHero.transform.position - sourceObj.transform.position;
        Vector3 target = new Vector3(oriTarget.x / 4.0f, 0, oriTarget.z / 4.0f);
        target += sourceObj.transform.position;
        //Debug.Log("addDirectMovement: " + target);
		addSingleMoving(sourceObj, target, PERSON_SPEED_CATCHING, true);
    }

    void addSingleMoving(GameObject sourceObj, Vector3 target, float speed, bool isCatching) {
        this.runAction(sourceObj, CCMoveToAction.CreateSSAction(target, speed, isCatching), this);
    }
		

    //获取hero所在区域
    public int getHeroStandOnArea() {
        return myHero.GetComponent<HeroStatus>().standOnArea;
    }
}
