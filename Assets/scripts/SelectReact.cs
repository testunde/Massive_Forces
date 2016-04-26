using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class SelectReact : MonoBehaviour {
		private static SelectionControl selection;
		public IngameObject connectedObject;
		private bool selected=false;
		// private Projector proj;
		private GameObject selIndicator,targetMark;
		private Material markerMat;
		
		void Start(){
			selection=SelectionControl.getInstance();
			// createProjector();
			if(targetMark==null)
				createTargetMarker();
			createSelIndicator();
		}
		
		void OnDisable(){
			if(selected)
				selection.removeItemMarker(connectedObject);
		}
		
		public void select(){
			if(!selected){
				// proj.enabled=true;
				selIndicator.GetComponent<MeshRenderer>().enabled=true;
				if(connectedObject.meetingPoint.Count>0)
					moveTargetMarkerModel(true);
				selected=true;
			}
		}
		
		public void deselect(){
			if(selected){
				// proj.enabled=false;
				selIndicator.GetComponent<MeshRenderer>().enabled=false;
				targetMark.GetComponent<MeshRenderer>().enabled=false;
				selected=false;
			}
		}
		
		public bool isSelected(){
			return selected;
		}
		
		public void moveTargetMarkerModel(bool activate){
			if(targetMark==null)
				createTargetMarker();
			Vector3 lastMP=Vector3.zero;
			foreach(Vector3 mp in connectedObject.meetingPoint)
				lastMP=mp;
			targetMark.transform.position=lastMP;
			targetMark.GetComponent<MeshRenderer>().enabled=activate;
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
		
		private void createSelIndicator(){
			selIndicator=GameObject.CreatePrimitive(PrimitiveType.Sphere);
			selIndicator.GetComponent<MeshRenderer>().enabled=false;
			Destroy(selIndicator.GetComponent<SphereCollider>());
			selIndicator.name="selection indicator";
			selIndicator.transform.parent=gameObject.transform;
			selIndicator.transform.localEulerAngles=new Vector3(0f,0f,0f);
			selIndicator.transform.localPosition=new Vector3(0f,0f,0f);
			selIndicator.GetComponent<Renderer>().material=markerMat;
			float width=connectedObject.markerSize;
			selIndicator.transform.localScale=new Vector3(width,width/2f,width);
		}
		
		private void createTargetMarker(){
			targetMark=GameObject.Instantiate((GameObject)Resources.Load("blender/MeetingPoint",typeof(GameObject)));
			targetMark.GetComponent<MeshRenderer>().enabled=false;
			Destroy(targetMark.GetComponent<Collider>());
			targetMark.name="target marker (meeting point) of ID "+connectedObject.ID;
			markerMat=(Material)Resources.Load("materials/SelectionMarkerObj", typeof(Material));
			targetMark.GetComponent<Renderer>().material=markerMat;
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
