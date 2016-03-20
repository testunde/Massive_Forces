using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class IO_Building : IngameObject {
		private Material previewMat,previewNoResMat;
		private GameObject preview;
		
		public IO_Building() : base(){
			previewMat=(Material)Resources.Load("materials/buildingPreview", typeof(Material));
			previewNoResMat=(Material)Resources.Load("materials/buildingPreviewNoRes", typeof(Material));
		}
		
		public virtual void setPreview(int frac){
			//disable all non-preview objects
			fraction=frac;
			Transform modelTr=model.transform;
			for(int i=0;i<modelTr.childCount;i++){
				GameObject child=modelTr.GetChild(i).gameObject;
				if(child.name!="preview"){
					child.SetActive(false);
				}else{
					child.SetActive(true);
					preview=child;	//get preview model
				}
			}
			//set preview material, depends on available resources
			if(resources.costsAvailable(fraction,costs))
				preview.GetComponent<Renderer>().material=previewMat;
			else
				preview.GetComponent<Renderer>().material=previewNoResMat;
		}
		
		public virtual void abortBuild(){
			deleteModel();
		}
		
		public virtual void build(){
			//disable preview; enable all building objects
			timeRemaining=buildTime;
			Transform modelTr=model.transform;
			for(int i=0;i<modelTr.childCount;i++){
				GameObject child=modelTr.GetChild(i).gameObject;
				if(child.name=="united" || child.name=="Marker"){
					child.SetActive(true);
				}else{
					child.SetActive(false);
				}
			}
			//PLAY BUILD ANIMATION
		}
		
		public virtual void finishedBuild(){
			//MAKE FUNCTIONS OF THE BUILDING NOW AVAILABLE
			Debug.Log(name+" with ID "+ID+" has builded!");
		}
	}
}
