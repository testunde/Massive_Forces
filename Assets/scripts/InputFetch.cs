using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class InputFetch : MonoBehaviour {
		private static InputModul inputMod;
		
		void Start(){
			inputMod=InputModul.getInstance();
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
			inputMod.fDown=Input.GetKeyDown(KeyCode.F);
			inputMod.rDown=Input.GetKeyDown(KeyCode.R);
			inputMod.cancel=Input.GetButtonDown("Cancel");
		}
	}
}
