using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class SelectReact : MonoBehaviour {
		public IngameObject connectedObject=null;
		
		public void select(){
			print(gameObject.name+" selected!");
		}
		
		public void deselect(){
			print(gameObject.name+" deselected!");
		}
	}
}
