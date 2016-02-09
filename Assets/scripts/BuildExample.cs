using UnityEngine;
using System.Collections;

public class BuildExample : MonoBehaviour {
	private Vector3 coords;
	public CameraControl camCtrl;
	public Material previewMat,buildMat,hoverMat,selectMat;
	public GameObject Map;
	private GameObject preview,building;
	private int state=0;
	private bool selected=false;
	private Vector3 down=new Vector3(0f,2f,0f);

	GameObject createPreview(){	//create cube for building preview
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.layer=2;
		cube.GetComponent<Renderer>().material=previewMat;
		return cube;
	}
	GameObject createBuilding(Vector3 pos){	//create cube
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.name="BuildingExample";
		cube.transform.position=pos;
		buildMat=cube.GetComponent<Renderer>().material;	//to able to reverse to the old material
		return cube;
	}
	
	void Start(){
		camCtrl=gameObject.GetComponent<CameraControl>();
		Map=GameObject.Find("Terrain");
	}
	
	void FixedUpdate(){
		switch(state){
			case 0:{
				if(Input.GetKeyDown("f")){
					preview=createPreview();
					state=1;
				}
				break;
			}case 1:{
				preview.transform.position=coords;
				if(Input.GetButtonDown("Fire1")){
					building=createBuilding(coords-down);
					Destroy(preview);
					state=2;
				}
				break;
			}case 2:{
				float step=.02f;
				building.transform.position+=new Vector3(0f,step,0f);
				down-=new Vector3(0f,step,0f);
				if(down.y<=0){
					state=3;
				}
				break;
			}default:{
				break;
			}
		}
	}
	
	void Update(){
		coords=camCtrl.Pointer;
	}
}
