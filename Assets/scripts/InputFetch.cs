using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class InputFetch : MonoBehaviour {
		private static InputModul inputMod;
		
		void Start(){
			inputMod=InputModul.getInstance();
			inputMod.originalFixedDeltaTime=Time.fixedDeltaTime;
		}
		
		void Update(){
			inputMod.leftDown=Input.GetButtonDown("Fire1");
			inputMod.leftUp=Input.GetButtonUp("Fire1");
			inputMod.leftHold=Input.GetButton("Fire1");
			inputMod.rightDown=Input.GetButtonDown("Fire2");
			inputMod.rightHold=Input.GetButton("Fire2");
			inputMod.rightUp=Input.GetButtonUp("Fire2");
			inputMod.shiftHold=Input.GetKey(KeyCode.LeftShift);
			inputMod.ctrlHold=Input.GetKey(KeyCode.LeftControl);
			inputMod.cDown=Input.GetKeyDown(KeyCode.C);
			inputMod.fDown=Input.GetKeyDown(KeyCode.F);
			inputMod.gDown=Input.GetKeyDown(KeyCode.G);
			inputMod.hDown=Input.GetKeyDown(KeyCode.H);
			inputMod.mDown=Input.GetKeyDown(KeyCode.M);
			inputMod.rDown=Input.GetKeyDown(KeyCode.R);
			inputMod.tDown=Input.GetKeyDown(KeyCode.T);
			inputMod.vDown=Input.GetKeyDown(KeyCode.V);
			inputMod.cancel=Input.GetButtonDown("Cancel");
			
			//numeral keys (not numpad!)
			inputMod.numDown[0]=Input.GetKeyDown(KeyCode.Alpha0);
			inputMod.numDown[1]=Input.GetKeyDown(KeyCode.Alpha1);
			inputMod.numDown[2]=Input.GetKeyDown(KeyCode.Alpha2);
			inputMod.numDown[3]=Input.GetKeyDown(KeyCode.Alpha3);
			inputMod.numDown[4]=Input.GetKeyDown(KeyCode.Alpha4);
			inputMod.numDown[5]=Input.GetKeyDown(KeyCode.Alpha5);
			inputMod.numDown[6]=Input.GetKeyDown(KeyCode.Alpha6);
			inputMod.numDown[7]=Input.GetKeyDown(KeyCode.Alpha7);
			inputMod.numDown[8]=Input.GetKeyDown(KeyCode.Alpha8);
			inputMod.numDown[9]=Input.GetKeyDown(KeyCode.Alpha9);
		}
	}
}
