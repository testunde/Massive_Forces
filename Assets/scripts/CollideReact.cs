using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class CollideReact : MonoBehaviour {
		public IngameObject connectedObject;
		public ActionBehaviour actionBeh;
		
		//currently only for ability to enable/disable this script
		void OnDisable(){
		}
		void OnEnable(){
		}
		
		void OnTriggerEnter(Collider col){
			IngameObject search=null;
			if(col.gameObject.name=="united")
				search=col.gameObject.transform.parent.gameObject.GetComponent<SelectReact>().connectedObject;
			
			if(search!=null){
				actionBeh.addCollision(search,this);
			}
		}
		
		void OnTriggerExit(Collider col){
			IngameObject search=null;
			if(col.gameObject.name=="united")
				search=col.gameObject.transform.parent.gameObject.GetComponent<SelectReact>().connectedObject;
			
			if(search!=null){
				actionBeh.removeCollision(search,this);
			}
		}
	}
}
