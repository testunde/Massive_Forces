using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	//create the building (TODO: hand over the mesh of the building)
	public class Building : MonoBehaviour {
		public CameraControl camCtrl;
		private GameObject Map,preview,building;
		private int state=0;
		private float down=2f,growSpeed=.03f;
		
		public void Init(CameraControl cC){
			camCtrl=cC;
			gameObject.name="Building";
		}
		void OnDestroy(){
			if(state==0)
				Destroy(preview);
			else
				Destroy(building);
		}
		public GameObject getParent(){
			return gameObject;
		}
		
		public void createPreview(Material previewMat){	//create cube for building preview
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			preview=cube;
			cube.transform.parent=gameObject.transform;
			cube.name="Preview";
			cube.layer=2;
			cube.GetComponent<Renderer>().material=previewMat;
		}
		public void setCoords(Vector3 coords){
			gameObject.transform.position=coords;
		}
		public void rotateTo(Vector3 coords){
			Vector3 targetRot=new Vector3(coords.x,gameObject.transform.position.y,coords.z);
			gameObject.transform.LookAt(targetRot,Vector3.forward);
			//Vector3 rot=gameObject.transform.eulerAngles;
			//rot=new Vector3(0f,rot.y,0f);
		}
		public void createBuilding(){	//create cube
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			building=cube;
			cube.transform.parent=gameObject.transform;
			cube.name="House";
			gameObject.transform.position-=new Vector3(0f,down,0f);
			cube.transform.localPosition=new Vector3(0f,0f,0f);
			cube.transform.localEulerAngles=new Vector3(0f,0f,0f);
			//TODO: include MouseReaction.cs
			//buildMat=cube.GetComponent<Renderer>().material;	//to able to reverse to the old material
			MouseReaction mR=cube.AddComponent<MouseReaction>();
			mR.Init(camCtrl);
			state=1;
			Destroy(preview);
		}
		
		void Start(){
			Map=GameObject.Find("Terrain");
		}
		
		void FixedUpdate(){
			switch(state){
				case 1:{
					gameObject.transform.position+=new Vector3(0f,growSpeed,0f);
					down-=growSpeed;
					if(down<=0)
						state=2;
					break;
				}default:
					break;
			}
		}
	}
}
