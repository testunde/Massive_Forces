using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class MarkerControl : MonoBehaviour {
		private static InputModul inputMod;
		private static SelectionControl selection;
		private Transform markTr;
		private BoxCollider boxCol;
		private MeshRenderer meshRender;
		private float rot,height=32f;
		public Vector3 startPoint;
		private Vector3 xPoint,xMid;
		//needed, so that the selection() and deselection() methods don't get called more then one time
		// private Dictionary<IngameObject,GameObject> selObj=new Dictionary<IngameObject,GameObject>();
		private List<IngameObject> selObj=new List<IngameObject>();
		public bool add,remove;
		
		public void begin(Vector3 coords){
			selObj.Clear();
			if(!(add||remove)){
				selection.clearList(null);
				selection.clearTempList();
			}else{
				selection.copyToTemp();
			}
			activate();
			//set marker cube 2f under terrain
			markTr.position=new Vector3(coords.x,(height/2f)-2f,coords.z);
			markTr.localScale=new Vector3(.01f,height,0.1f);
			startPoint=new Vector3(coords.x,markTr.position.y,coords.z);
		}
		
		public void scaleX(Vector3 coords){
			markTr.position=new Vector3((startPoint.x+coords.x)/2f,markTr.position.y,(startPoint.z+coords.z)/2f);
			float targetScale=Vector3.Distance(startPoint,coords);
			if(Mathf.Abs(targetScale)<.01f)
				targetScale=Mathf.Sign(targetScale)*.01f;
			markTr.localScale=new Vector3(markTr.localScale.x,markTr.localScale.y,Vector3.Distance(startPoint,
						new Vector3(coords.x,markTr.position.y,coords.z)));
			Vector3 targetRot=new Vector3(coords.x,gameObject.transform.position.y,coords.z);
			gameObject.transform.LookAt(targetRot,Vector3.up);
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
			markTr.localScale=new Vector3(diff,markTr.localScale.y,markTr.localScale.z);
		}
		
		public void finish(Vector3 coords){
			scaleY(coords);
			deactivate();
		}
		
		public void abort(){
			foreach(IngameObject obj in selObj){
				selection.removeItemMarker(obj);
			}
			deactivate();
		}
		
		private float getPointDiff(Vector3 aO,Vector3 bO,Vector3 p){
			Vector3 a=aO-p;
			Vector3 b=bO-p;
			return (b.x*a.z-a.x*b.z)/Mathf.Sqrt(Mathf.Pow(a.z-b.z,2)+Mathf.Pow(b.x-a.x,2));
		}		
		
		private void activate(){
			boxCol.enabled=true;
			meshRender.enabled=true;
		}
		
		private void deactivate(){
			boxCol.enabled=false;
			meshRender.enabled=false;
			markTr.localScale=new Vector3(0f,0f,0f);
			selection.clearTempList();
		}
		
		// private IngameObject searchScript(GameObject obj){
			// IngameObject result=null;
			// SelectReact script=obj.GetComponent<SelectReact>();
			// if(script!=null)
				// result=script.connectedObject;
			// else if(obj.transform.parent!=null)
				// result=searchScript(obj.transform.parent.gameObject);
			// return result;
		// }
		
		void Start(){
			inputMod=InputModul.getInstance();
			height+=inputMod.terrain.transform.position.y;
			selection=SelectionControl.getInstance();
			markTr=gameObject.transform;
			boxCol=gameObject.GetComponent<BoxCollider>();
			meshRender=gameObject.GetComponent<MeshRenderer>();
			add=false;
			remove=false;
		}
		
		void OnTriggerEnter(Collider col){
			//IngameObject search=searchScript(col.gameObject);
			IngameObject search=null;
			if(col.gameObject.name=="united")
				search=col.gameObject.transform.parent.gameObject.GetComponent<SelectReact>().connectedObject;
			
			if(search!=null && search.selectReact.enabled && !selObj.Contains(search)){
				if(remove){
					if(selection.tempMarker.Contains(search)){
						selection.removeItemMarker(search);
						selObj.Add(search);
					}else if(add){
						selection.invertItemMarker(search);
						selObj.Add(search);
					}
				}else if(add && !remove){
					if(!selection.tempMarker.Contains(search)){
							selObj.Add(search);
							selection.addItemMarker(search);
					}
				}else{
					selection.addItemMarker(search);
					selObj.Add(search);
				}
			}
		}
		
		void OnTriggerExit(Collider col){
			// IngameObject search=searchScript(col.gameObject);
			IngameObject search=null;
			if(col.gameObject.name=="united")
				search=col.gameObject.transform.parent.gameObject.GetComponent<SelectReact>().connectedObject;
			
			if(search!=null && selObj.Contains(search)){
				if(remove){
					if(add){
						selection.invertItemMarker(search);
					}else{
						selection.addItemMarker(search);
					}
				}else if(add && !remove){
					if(selObj.Contains(search)){
						selection.removeItemMarker(search);
					}
				}else{
					selection.removeItemMarker(search);
				}
				selObj.Remove(search);
			}
		}
	}
}
