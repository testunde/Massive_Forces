using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	//create the building (TODO: hand over the mesh of the building / building ID)
	//it's an example script for the interfaces
	public class Building : MonoBehaviour {
		private CameraControl camCtrl;
		private GameObject Map,preview,building;
		private int state=0;
		private State buildControllerState;
		private float down=2f,growSpeed=.03f;
		private int HP=0,fraction=0;
		private const int baseHP=1000;
		private int maxHP=baseHP;	//later here it's possible to set factors
		
		public void Init(CameraControl cC,State s,int frac){
			camCtrl=cC;
			buildControllerState=s;
			gameObject.name="Building";
			fraction=frac;
		}
		void OnDestroy(){	//destroy all GameObjects when the Building gets destroyed
			if(state==0)
				Destroy(preview);
			else
				Destroy(building);
		}
		public GameObject getParent(){
			return gameObject;
		}
		public string getName(){
			return gameObject.name;
		}
		public int getHP(){
			return HP;
		}
		public int getMaxHP(){
			return maxHP;
		}
		public void changeHPBy(int chHP){
			chHP+=HP;
			if(chHP>maxHP)
				chHP=maxHP;
			HP=chHP;
		}
		public int getFraction(){
			return fraction;
		}
		public void setFraction(int frac){
			fraction=frac;
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
			
			MouseReaction mR=cube.AddComponent<MouseReaction>();
			mR.Init(camCtrl,buildControllerState);
			
			state=1;
			Destroy(preview);
		}
		
		void Start(){
			Map=GameObject.Find("Terrain");
		}
		
		void FixedUpdate(){
			switch(state){
				case 1:{	//build process
					gameObject.transform.position+=new Vector3(0f,growSpeed,0f);
					down-=growSpeed;
					if(down<=0){
						HP=maxHP;
						state=2;
					}
					break;
				}default:
					break;
			}
		}
	}
}
