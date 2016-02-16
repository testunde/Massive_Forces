using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class MarkerControl : MonoBehaviour {
		private static SelectionControl selection;
		private Transform markTr;
		private BoxCollider boxCol;
		private MeshRenderer meshRender;
		private float rot,height=4f;
		private Vector3 startPoint,xPoint,xMid;
		
		public void begin(Vector3 coords){
			activate();
			//set marker cube 2f under terrain
			markTr.position=new Vector3(coords.x,(height/2f)-2f,coords.z);
			markTr.localScale=new Vector3(height,.01f,0.1f);
			startPoint=new Vector3(coords.x,markTr.position.y,coords.z);
		}
		
		public void scaleX(Vector3 coords){
			markTr.position=new Vector3((startPoint.x+coords.x)/2f,markTr.position.y,(startPoint.z+coords.z)/2f);
			float targetScale=Vector3.Distance(startPoint,coords);
			if(Mathf.Abs(targetScale)<.01f)
				targetScale=Mathf.Sign(targetScale)*.01f;
			markTr.localScale=new Vector3(markTr.localScale.x,markTr.localScale.y,Vector3.Distance(startPoint,coords));
			Vector3 targetRot=new Vector3(coords.x,gameObject.transform.position.y,coords.z);
			gameObject.transform.LookAt(targetRot,Vector3.forward);
		}
		
		public void setX(Vector3 coords){
			scaleX(coords);
			xPoint=new Vector3(coords.x,markTr.position.y,coords.z);
			xMid=(startPoint+xPoint)/2f;
			rot=Mathf.Deg2Rad*markTr.localEulerAngles.y;
		}
		
		public void scaleY(Vector3 coords){
			float diff=getPointDiff(startPoint,xPoint,coords);
			if(Mathf.Abs(diff)<.01f)
				diff=Mathf.Sign(diff)*.01f;
			if(float.IsNaN(diff))
				diff=.01f;
			markTr.position=xMid+(new Vector3(diff*Mathf.Cos(rot),0f,-diff*Mathf.Sin(rot)))/2f;
			markTr.localScale=new Vector3(markTr.localScale.x,diff,markTr.localScale.z);
		}
		
		public void finish(Vector3 coords){
			scaleY(coords);
			markTr.localScale=new Vector3(0f,0f,0f);
			deactivate();
		}
		
		public void abort(){
			markTr.localScale=new Vector3(0f,0f,0f);
			deactivate();
		}
		
		private float getPointDiff(Vector3 xO1,Vector3 xO2,Vector3 p){
			Vector3 x1=xO1-p;
			Vector3 x2=xO2-p;
			return (x2.x*x1.z-x1.x*x2.z)/Mathf.Sqrt(Mathf.Pow(x1.z-x2.z,2)+Mathf.Pow(x2.x-x1.x,2));
		}		
		
		private void activate(){
			boxCol.enabled=true;
			meshRender.enabled=true;
		}
		
		private void deactivate(){
			boxCol.enabled=false;
			meshRender.enabled=false;
		}
		
		private IngameObject searchScript(GameObject obj){
			IngameObject result=null;
			SelectReact script=obj.GetComponent<SelectReact>();
			if(script!=null)
				result=script.connectedObject;
			else if(obj.transform.parent!=null)
				result=searchScript(obj.transform.parent.gameObject);
			return result;
		}
		
		void Start(){
			selection=SelectionControl.getInstance();
			markTr=gameObject.transform;
			boxCol=gameObject.GetComponent<BoxCollider>();
			meshRender=gameObject.GetComponent<MeshRenderer>();
		}
		
		void OnCollisionEnter(Collision col){
			IngameObject search=searchScript(col.gameObject);
			if(search!=null)
				selection.addItem(search);
		}
		
		void OnCollisionExit(Collision col){
			IngameObject search=searchScript(col.gameObject);
			if(search!=null)
				selection.removeItem(search);
		}
	}
}
