using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class IngameObject : IngameItem{
		protected static int IDflow=1;	//0=terrain
		public int ID;
		public string type;
		public GameObject model;
		public PlaneFollow plane;
		public int HP,maxHP;
		public int fraction;
		public float markerSize;
		public ActionMatrix actions;
		public int workerUnits=0;
		protected MinimapProjection minimap=new MinimapProjection();
		public SelectReact selectReact;
		
		public IngameObject() : base(){
			ID=IDflow;
			IDflow++;
		}
		
		public virtual void loadType(Database.IO_Database type){
			this.name=type.name;
			this.type=type.type;
			this.maxHP=type.maxHP;
			this.buildTime=type.buildTime;
			this.markerSize=type.markerSize;
			this.costs=type.costs;
			this.actions=new ActionMatrix(this,type.actions);
			this.workerUnits=type.workerUnits;
		}
		
		public virtual void loadType(string type){
			loadType((Database.IO_Database)System.Activator.CreateInstance(System.Type.GetType("Database."+type)));
		}
		
		public virtual void initModel(){
			model=GameObject.Instantiate((GameObject)Resources.Load("blender/"+type,typeof(GameObject)));
			selectReact=model.AddComponent<SelectReact>();
			selectReact.connectedObject=this;
			SetLayerRecursively(model,9);
			GameObject planeObj;
			if(this is IO_Building)
				planeObj=GameObject.Instantiate((GameObject)Resources.Load("blender/MinimapSquare",typeof(GameObject)));
			else
				planeObj=GameObject.Instantiate((GameObject)Resources.Load("blender/MinimapCircle",typeof(GameObject)));
			plane=planeObj.AddComponent<PlaneFollow>();
			plane.Init(this);
		}
		
		public virtual void SetLayerRecursively(GameObject obj,int newLayer){
			obj.layer=newLayer;
			foreach(Transform child in obj.transform){
				SetLayerRecursively(child.gameObject,newLayer);
			}
		}
		
		public virtual void setCoords(Vector3 coords){
			model.transform.position=coords;
		}
		
		public virtual void rotateTo(Vector3 coords){
			Vector3 targetRot=new Vector3(coords.x,model.transform.position.y,coords.z);
			model.transform.LookAt(targetRot,Vector3.up);
		}
		
		public virtual void deleteModel(){
			MonoBehaviour.Destroy(model);
			MonoBehaviour.Destroy(plane.gameObject);
		}
		
		public virtual void heartbeat(float time){
			//gets called by an Updates() method of a MonoBehaviour
		}
	}
}
