using UnityEngine;
using System.Collections;
using Scripts_o;

namespace Scripts_o {
	public class CameraControl : MonoBehaviour {
		
		private GameObject mainCamera;
		private Transform camTr;
		private float oldMapHigh,currentHigh;
		private float minHigh=5f,maxHigh=15f;
		private GameObject ground,cube;
		//private float x=0f,y=0f,z=0f;
		public static MouseLook mouseLook = new MouseLook();
		public Vector3 Pointer;
		public bool leftClick=false,shiftPress=false,ctrlPress=false;
		
		//set y-coord bolow zero! method decrement y by 1 when terrain wasn't hittet
		public Vector3 getCoordsAtXZ(Vector3 where){
			RaycastHit height;
			Vector3 result=where-(new Vector3(0f,1f,0f));
			if(Physics.Raycast(where,Vector3.up,out height)&&height.transform.Equals(ground.transform))
				result=height.point;
			return result;
		}
		
		void Start () {
			mainCamera=gameObject;
			camTr=mainCamera.transform;
			ground=GameObject.Find("Terrain");
			cube=GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.layer=2;	//to the cube ignores the raycast
			cube.transform.localScale=new Vector3(.3f,.3f,.3f);
		}
		
		void Update () {
			leftClick=Input.GetButtonDown("Fire1");
			shiftPress=Input.GetKey(KeyCode.LeftShift);
			ctrlPress=Input.GetKey(KeyCode.LeftControl);
			
			//#>height identifier
			Vector3 rayPos=new Vector3(camTr.position.x,-15f,camTr.position.z);	//raycast looks up form -15 if a collider is in the x/z coord
			Vector3 rayPosNew=getCoordsAtXZ(rayPos);
			//Physics.Raycast NEEDED! to perform the raycast calculations; enters 'if' only if the hitted object was the ground
			if(rayPosNew.y>rayPos.y){
				//#>height control and limit
				float high=currentHigh;
				float inputValue=(Input.GetAxis("Mouse ScrollWheel")*Vector3.down).y;
				if(high>minHigh&&inputValue<0||high<maxHigh&&inputValue>0){	//min/max limit of height
					high+=inputValue;
				}
				//cut overlaps over min/max
				if(high<minHigh)
					high=minHigh;
				else if(high>maxHigh)
					high=maxHigh;
				float targetHigh=camTr.position.y+(rayPosNew.y-oldMapHigh)+(high-currentHigh);
				camTr.position=new Vector3(camTr.position.x,targetHigh,camTr.position.z);
				oldMapHigh=rayPosNew.y;
				currentHigh=camTr.position.y-oldMapHigh;
			}
			
			//#>CAMERA MOVEMENT
			float sinX=Mathf.Pow(Mathf.Sin(Mathf.Deg2Rad*camTr.eulerAngles.x),2f);	//already multiplied with it self
			float sinY=Mathf.Sin(Mathf.Deg2Rad*camTr.eulerAngles.y);
			float cosY=Mathf.Cos(Mathf.Deg2Rad*camTr.eulerAngles.y);
			Vector3 forw=new Vector3(	//Pythagoras (squareroot of x²+x²) and multiplied with sin/cos of Y-axis
				sinY*(Mathf.Sqrt(Mathf.Pow(transform.forward.x,2f)+sinX)),	//sidewards
				0f,	//eliminate the up/down movement of the transform.forward Verctor3
				cosY*(Mathf.Sqrt(Mathf.Pow(transform.forward.z,2f)+sinX)));	//for-/backwards
			Vector3 input=Input.GetAxis("Horizontal")*transform.right+	//sidewards
				Input.GetAxis("Vertical")*forw;	//for-/backwards
				//+Input.GetAxis("Mouse ScrollWheel")*Vector3.down;	//zoom in worlds y axis
				//+Input.GetAxis("Mouse ScrollWheel")*transform.forward;	//zoom in view direction
			camTr.position+=input*.5f*(currentHigh/maxHigh);
			//#>rotate view & auto adjust when scroll back
			if(Input.GetMouseButton(2)||(Input.GetAxis("Mouse ScrollWheel"))!=0f){
				float factor=.3f;
				mouseLook.LookRotation(camTr,80f,35f*(currentHigh*factor/maxHigh+(1f-factor)));
			}
			
			//#>CLICK POSITION
			//Input.GetButtonDown("Fire1")once true if hitted
			//Input.GetMouseButton(0) true of hold down
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit)&&hit.transform.Equals(ground.transform)){
				Pointer=hit.point;
				if(Input.GetMouseButton(0)){
					cube.transform.position=hit.point;
					Debug.Log("last click: "+hit.point);
				}
				// transform the positions to material coordinates
				/*Renderer renderer = hit.transform.GetComponent<Renderer>();
				Collider collider = hit.collider as Collider;
				if(!(renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || collider == null)){
					//Texture2D tex = (Texture2D)renderer.material.mainTexture;
					Vector2 pixelUV = hit.textureCoord;
					x=(pixelUV.x * renderer.material.mainTexture.width-2048f)/8.192f+250f+hit.transform.position.x;
					y=hit.transform.position.y;
					z=(pixelUV.y * renderer.material.mainTexture.height-2048f)/8.192f+250f+hit.transform.position.z;
					cube.transform.position=new Vector3(x,y,z);
					//Debug.Log(x + " / " + y + " / " + z);
				}*/
			}
		}
	}
}
