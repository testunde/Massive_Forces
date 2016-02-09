using UnityEngine;
using System.Collections;

namespace Scripts {
	//sets material if mousehover or selected
	//must be imported as component in the to its refered object!
	public class MouseReaction : MonoBehaviour {
		public CameraControl camCtrl;
		private GameObject Map;
		private int state=0;	//0-nothing; 1-hover; 2-selected
		public Material buildMat,hoverMat,selectMat;
		
		public void Init(CameraControl cC){
			camCtrl=cC;
		}
		
		void Start(){
			Map=GameObject.Find("Terrain");
		}
		
		void Update(){
		}
	}
}
