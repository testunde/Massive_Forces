using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class SelectReact : MonoBehaviour {
		private static SelectionControl selection;
		public IngameObject connectedObject;
		private bool selected=false;
		private Projector proj;
		
		void Start(){
			selection=SelectionControl.getInstance();
			createProjector();
		}
		
		public void select(){
			if(!selected){
				proj.enabled=true;
				selected=true;
			}
		}
		
		public void deselect(){
			if(selected){
				proj.enabled=false;
				selected=false;
			}
		}
		
		private void createProjector(){
			GameObject projObj=new GameObject();
			projObj.name="Marker Projector";
			projObj.transform.parent=gameObject.transform;
			projObj.transform.localEulerAngles=new Vector3(90f,0f,0f);
			projObj.transform.localPosition=new Vector3(0f,0f,0f);
			proj=projObj.AddComponent<Projector>();
			proj.enabled=false;
			proj.orthographic=true;
			proj.orthographicSize=connectedObject.markerSize;
			proj.ignoreLayers=1<<9;	//ignore the IngameObject's layer
			proj.nearClipPlane=-16f;
			proj.farClipPlane=24f;
			proj.material=(Material)Resources.Load("materials/SelectionMarker");
		}
		
		void OnBecameVisible(){
			Debug.Log("in");
			selection.inView.Add(connectedObject);
		}
		
		void OnBecameInvisible(){
			Debug.Log("out");
			selection.inView.Remove(connectedObject);
		}
	}
}
