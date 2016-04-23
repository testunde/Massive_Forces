using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class IO_Building : IngameObject {
		public Produceline production=new Produceline();
		private Material previewMat,previewNoResMat;
		private GameObject preview;
		
		public IO_Building() : base(){
			previewMat=(Material)Resources.Load("materials/buildingPreview", typeof(Material));
			previewNoResMat=(Material)Resources.Load("materials/buildingPreviewNoRes", typeof(Material));
			standardActions[3,2]="Acom_SetMeetPoint";
		}
		
		public virtual void setPreview(int frac,bool available){
			//disable all non-preview objects
			fraction=frac;
			Transform modelTr=model.transform;
			for(int i=0;i<modelTr.childCount;i++){
				GameObject child=modelTr.GetChild(i).gameObject;
				if(child.name=="preview"){
					child.SetActive(true);
					preview=child;	//set preview model reference
				}else if(child.name.StartsWith("collider")){
					child.SetActive(true);
					CollideReact cr=child.GetComponent<CollideReact>();
					cr.enabled=true;
				}else{
					child.SetActive(false);
				}
			}
			
			//set preview material, depends on available resources
			changePreview(available);
		}
		
		public virtual void changePreview(bool available){
			if(available)
				preview.GetComponent<Renderer>().material=previewMat;
			else
				preview.GetComponent<Renderer>().material=previewNoResMat;
		}
		
		public virtual void abortBuild(){
			deleteModel();
		}
		
		public virtual void build(){
			//set default meetingPoint
			Vector3 offset=new Vector3(Mathf.Sin(Mathf.Deg2Rad*model.transform.localEulerAngles.y)*markerSize*.64f,0f,
										Mathf.Cos(Mathf.Deg2Rad*model.transform.localEulerAngles.y)*markerSize*.64f);
			meetingPoint=model.transform.position+offset;
			//disable preview; enable all building objects
			Transform modelTr=model.transform;
			for(int i=0;i<modelTr.childCount;i++){
				GameObject child=modelTr.GetChild(i).gameObject;
				if(child.name=="united" || child.name=="Marker" || child.name.StartsWith("collider")){
					child.SetActive(true);
				}else{
					child.SetActive(false);
				}
			}
			actionBeh.enabled=true;
			//PLAY BUILD ANIMATION
			//add HP
		}
		
		public virtual void finishedBuild(){
			//MAKE FUNCTIONS OF THE BUILDING NOW AVAILABLE
			Debug.Log(type+" with ID "+ID+" has builded!");
		}
		
		public override void heartbeat(float time){
			production.decreaseTime(time);
		}
	}
}
