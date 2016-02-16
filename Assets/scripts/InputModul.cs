using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class InputModul {
		private static InputModul instance=null;
		public bool leftDown,leftUp,leftHold,rightDown,rightHold,shiftHold,ctrlHold;
		public bool fDown,cancel;
		public Vector3 pointer;
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
