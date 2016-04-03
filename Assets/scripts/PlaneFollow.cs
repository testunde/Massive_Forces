using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using Scripts;

namespace Scripts {
	public class PlaneFollow : MonoBehaviour {
		private static Res resources;
		private IngameObject connectedObject;
		private bool isBuilding;
		private GameObject cam;	//to set this minimap-"dot" for the cam
		private Material colorMat;
		private Renderer render;
		private float offset=0f;
		
		void Start(){
			resources=Res.getInstance();
			colorMat=new Material(Shader.Find("Transparent/Diffuse"));
			render.material=colorMat;
		}
		
		//for each IngameObject
		public void Init(IngameObject obj){
			cam=null;
			connectedObject=obj;
			if(connectedObject.fraction==1)
				offset=.8f;
			isBuilding=obj is IO_Building;
			if(isBuilding)
				initPlane(6f);
			else
				initPlane(4f);
		}
		//for camera
		public void Init(GameObject obj){
			connectedObject=null;
			cam=obj;
			offset=1f;
			initPlane(10f);
		}
		private void initPlane(float size){
			render=gameObject.GetComponent<Renderer>();
			render.enabled=false;
			render.receiveShadows=false;
			render.shadowCastingMode=ShadowCastingMode.Off;
			render.reflectionProbeUsage=ReflectionProbeUsage.Off;
			render.useLightProbes=false;
			//set size
			transform.localScale=new Vector3(size,size,1f);
			transform.localEulerAngles=new Vector3(-90f,0f,0f);	//correct rotation problem from blender import
			gameObject.layer=10;
			render.enabled=true;
		}
		
		void Update(){
			Vector3 oldPos=Vector3.zero;
			if(connectedObject!=null){	//when IngameObject is set
				//set position and height of plane
				oldPos=connectedObject.model.transform.position;
				
				//set color
				if(connectedObject.selectReact.isSelected())	//check if selected
					colorMat.color=Color.white;
				else
					colorMat.color=resources.getColor(connectedObject.fraction);
				
				if(isBuilding)
					transform.localEulerAngles=new Vector3(-90f,connectedObject.model.transform.localEulerAngles.y+90f,0f);
				
			}else if(cam!=null){	//when camera is set
				oldPos=cam.transform.position;
				colorMat.color=Color.blue;
				transform.localEulerAngles=new Vector3(-90f,cam.transform.localEulerAngles.y+90f,0f);
			}
			transform.position=new Vector3(oldPos.x,100f+offset,oldPos.z);
		}
	}
}
