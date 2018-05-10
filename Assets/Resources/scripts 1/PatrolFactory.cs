using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;

namespace Com.Patrols {
    public class PatrolFactory : System.Object {
        private static PatrolFactory instance;
        private GameObject PatrolItem;
		private GameObject HeroItem;

        private Vector3[] PatrolPosSet = new Vector3[] { new Vector3(-6, 0, 16), new Vector3(-1, 0, 19),
            new Vector3(6, 0, 16), new Vector3(-5, 0, 7), new Vector3(0, 0, 7), new Vector3(6, 0, 7)};

        public static PatrolFactory getInstance() {
            if (instance == null)
                instance = new PatrolFactory();
            return instance;
        }

        public void initPatrol(GameObject _PatrolItem) {
            PatrolItem = _PatrolItem;
        }
		public void initHero(GameObject _Hero){
			HeroItem = _Hero;
		}
        public GameObject getPatrol() {
            GameObject newPatrol = Camera.Instantiate(PatrolItem);
            return newPatrol;
        }
		public GameObject getHero(){
			GameObject newHero = Camera.Instantiate (HeroItem);
			return newHero;
		}
        public Vector3[] getPosSet() {
            return PatrolPosSet;
        }
    }
}

