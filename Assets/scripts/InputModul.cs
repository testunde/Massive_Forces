using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class InputModul {
		private static InputModul instance=null;
		public bool leftDown,leftUp,leftHold,rightDown,rightHold,rightUp;	//mouse keys
		public bool shiftHold,ctrlHold,cancel;	//control keys
		public bool cDown,fDown,gDown,hDown,mDown,rDown,tDown,vDown;	//alphabetical keys
		public bool[] numDown=new bool[10];	//number keys (not numpad!)
		public Vector3 pointer;
		public float originalFixedDeltaTime;
		public GameObject terrain;
		public GraphXYfast graph=new GraphXYfast();
		
		private InputModul(){
			pointer=new Vector3(0f,0f,0f);
			terrain=GameObject.Find("Terrain");
		}
		
		public static InputModul getInstance(){
			if(instance==null)
				instance=new InputModul();
			
			return instance;
		}
		
		//sets result's y to -16 when terrain wasn't hittet
		public Vector3 getCoordsAtXZ(Vector3 where){
			RaycastHit height;
			Vector3 result=new Vector3(where.x,-16f,where.z);
			if(Physics.Raycast(result,Vector3.up,out height,Mathf.Infinity,1<<8) &&
								height.transform.Equals(terrain.transform))
				result=height.point;
			return result;
		}
		public Vector3 getCoordsAtXZ(Vector2 where){	//overload
			return getCoordsAtXZ(new Vector3(where.x,0f,where.y));
		}
		public Vector3 getCoordsAtXZ(float x,float y){	//overload
			return getCoordsAtXZ(new Vector3(x,0f,y));
		}
	}
}
