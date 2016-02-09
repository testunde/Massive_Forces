using UnityEngine;
using System.Collections;

namespace Scripts {
	//create the building (later hand over the mesh of the building)
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
			if(state==0){
				Destroy(preview);
			}else{
				Destroy(building);
			}
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
		public void setPreview(Vector3 coords){
			preview.transform.position=coords;
		}
		public void createBuilding(Vector3 coords){	//create cube
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			building=cube;
			cube.transform.parent=gameObject.transform;
			cube.name="House";
			Destroy(preview);
			state=1;
			cube.transform.position=coords-(new Vector3(0f,down,0f));
			//insert MouseReaction.cs ...
			//buildMat=cube.GetComponent<Renderer>().material;	//to able to reverse to the old material
			MouseReaction mR=cube.AddComponent<MouseReaction>();
			mR.Init(camCtrl);
		}
		
		void Start(){
			Map=GameObject.Find("Terrain");
		}
		
		void FixedUpdate(){
			switch(state){
				case 1:{
					building.transform.position+=new Vector3(0f,growSpeed,0f);
					down-=growSpeed;
					if(down<=0){
						state=2;
					}
					break;
				}default:{
					break;
				}
			}
		}
	}
}
