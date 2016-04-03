using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class InputModul {
		private static InputModul instance=null;
		public bool leftDown,leftUp,leftHold,rightDown,rightHold,rightUp;	//mouse keys
		public bool shiftHold,ctrlHold,cancel;	//control keys
		public bool fDown,rDown,tDown,gDown,hDown;	//alphabetical keys
		public Vector3 pointer;
		public float originalFixedDeltaTime;
		public GameObject terrain;
		
		private InputModul(){
			pointer=new Vector3(0f,0f,0f);
			terrain=GameObject.Find("Terrain");
		}
		
		public static InputModul getInstance(){
			if(instance==null)
				instance=new InputModul();
			
			return instance;
		}
	}
}
