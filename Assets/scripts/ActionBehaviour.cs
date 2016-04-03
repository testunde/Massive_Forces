using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class ActionBehaviour : MonoBehaviour {
		public SelectReact selectReact;
		private static InputModul inputMod;
		public IngameObject connectedObject;
		
		void OnDisable(){
			if(selectReact!=null)
				selectReact.enabled=false;
		}
		void OnEnable(){
			if(selectReact!=null)
				selectReact.enabled=true;
		}
		
		void Start(){
			selectReact=gameObject.GetComponent<SelectReact>();
			inputMod=InputModul.getInstance();
			this.enabled=false;
		}
		
		void FixedUpdate(){
			connectedObject.heartbeat(inputMod.originalFixedDeltaTime);
		}
	}
}
