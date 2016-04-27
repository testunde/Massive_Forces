using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class GraphCheckCube : MonoBehaviour {
		public bool hitted=false;
		
		void OnTriggerStay(Collider col){
			IngameObject search=null;
			if(col.gameObject.name=="collider")
				search=col.gameObject.transform.parent.gameObject.GetComponent<SelectReact>().connectedObject;
			
			if(search is IO_Building || search is IO_Neutral){
				hitted=true;
			}
		}
	}
}
