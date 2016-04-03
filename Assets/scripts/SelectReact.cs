using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class SelectReact : MonoBehaviour {
		private static SelectionControl selection;
		public IngameObject connectedObject;
		private bool selected=false;
		// private Projector proj;
		private GameObject mark;
		private Material markerMat;
		
		void Start(){
			selection=SelectionControl.getInstance();
			// createProjector();
			markerMat=(Material)Resources.Load("materials/SelectionMarkerObj", typeof(Material));
			createMarker();
		}
		
		void OnDisable(){
			if(selected)
				selection.removeItemMarker(connectedObject);
		}
		
		public void select(){
			if(!selected){
				// proj.enabled=true;
				mark.GetComponent<MeshRenderer>().enabled=true;
				selected=true;
			}
		}
		
		public void deselect(){
			if(selected){
				// proj.enabled=false;
				mark.GetComponent<MeshRenderer>().enabled=false;
				selected=false;
			}
		}
		
		public bool isSelected(){
			return selected;
		}
		
		// private void createProjector(){
			// GameObject projObj=new GameObject();
			// projObj.name="Marker";
			// projObj.transform.parent=gameObject.transform;
			// projObj.transform.localEulerAngles=new Vector3(90f,0f,0f);
			// projObj.transform.localPosition=new Vector3(0f,0f,0f);
			// proj=projObj.AddComponent<Projector>();
			// proj.enabled=false;
			// proj.orthographic=true;
			// proj.orthographicSize=connectedObject.markerSize;
			// proj.ignoreLayers=1<<9;	//ignore the IngameObject's layer
			// proj.nearClipPlane=-16f;
			// proj.farClipPlane=24f;
			// proj.material=(Material)Resources.Load("materials/SelectionMarker");
		// }
		
		private void createMarker(){
			mark=GameObject.CreatePrimitive(PrimitiveType.Sphere);
			mark.GetComponent<MeshRenderer>().enabled=false;
			Destroy(mark.GetComponent<SphereCollider>());
			mark.name="Marker";
			mark.transform.parent=gameObject.transform;
			mark.transform.localEulerAngles=new Vector3(0f,0f,0f);
			mark.transform.localPosition=new Vector3(0f,0f,0f);
			mark.GetComponent<Renderer>().material=markerMat;
			float width=connectedObject.markerSize;
			mark.transform.localScale=new Vector3(width,width/2f,width);
		}
		
		void OnBecameVisible(){
			Debug.Log("in ID "+connectedObject.ID);
			selection.inView.Add(connectedObject);
		}
		
		void OnBecameInvisible(){
			Debug.Log("out ID "+connectedObject.ID);
			selection.inView.Remove(connectedObject);
		}
	}
}
