using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public abstract class IngameObject : IngameItem{
		protected static int IDflow=0;	//0=terrain
		public int ID;
		public GameObject model;
		public PlaneFollow plane;
		public int HP,maxHP,damage;
		public int fraction;
		public float markerSize;
		public float energy=0;
		public ActionMatrix actions;
		protected string[,] standardActions=new string[ActionMatrix.width,ActionMatrix.height];
		public int workerUnits=0;
		protected MinimapProjection minimap=new MinimapProjection();
		public SelectReact selectReact;
		public ActionBehaviour actionBeh;
		public Vector3 meetingPoint;
		
		public IngameObject() : base(){
			IngameObject.IDflow++;
			this.ID=IDflow;
			this.type="IngameObject";
		}
		
		public virtual void loadType(Database.IO_Database type,int frac){
			this.fraction=frac;
			this.name=type.name;
			this.type=type.type;
			this.maxHP=type.maxHP;
			this.damage=type.damage;
			this.buildTime=type.buildTime;
			this.markerSize=type.markerSize;
			this.costs=type.costs;
			this.actions=new ActionMatrix(this,applyActions(type.actions));
			this.workerUnits=type.workerUnits;
			applyResearches();
		}
		public virtual void loadType(string type,int frac){
			loadType((Database.IO_Database)System.Activator.CreateInstance(System.Type.GetType("Database."+type)),frac);
		}
		private string[,] applyActions(string[,] actions){
			//so if set standards gets applied
			for(int i=0;i<actions.GetLength(0);i++){
				for(int j=0;j<actions.GetLength(1);j++){
					if(actions[i,j]==null){
						actions[i,j]=standardActions[i,j];
					}
				}
			}
			return actions;
		}
		private void applyResearches(){
			if(this is IO_Building){
				//set values for all buildings
			}
			if(this is IO_Unit){
				//set values for all units
			}
			
			//set induvidual values
			if(type.Equals("IOu_testUnit")){
				maxHP=(int)(maxHP*frCtrl.testUnitHP[fraction]);
			}
		}
		
		public virtual void initModel(){
			model=GameObject.Instantiate((GameObject)Resources.Load("blender/"+type,typeof(GameObject)));
			
			//copy and init MonoBehaviour scripts
			selectReact=model.AddComponent<SelectReact>();
			selectReact.connectedObject=this;
			actionBeh=model.AddComponent<ActionBehaviour>();
			actionBeh.connectedObject=this;
			actionBeh.selectReact=selectReact;
			for(int i=0;i<model.transform.childCount;i++){
				GameObject child=model.transform.GetChild(i).gameObject;
				if(child.name.StartsWith("collider")){
					MonoBehaviour.Destroy(child.GetComponent<MeshRenderer>());
					MeshCollider mc=child.GetComponent<MeshCollider>();
					mc.convex=true;
					mc.isTrigger=true;
					CollideReact collReact=child.AddComponent<CollideReact>();
					collReact.actionBeh=actionBeh;
					collReact.connectedObject=this;
					actionBeh.colliders.Add(collReact);
					
					//generate neutral rigidbody
					Rigidbody rigid=child.AddComponent<Rigidbody>();
					rigid.angularDrag=0f;
					rigid.drag=0f;
					rigid.useGravity=false;
					rigid.isKinematic=false;
					rigid.mass=0;	//TODO: if greater than 0, the player bounce extremly if he touch this cube
					rigid.constraints=RigidbodyConstraints.FreezeAll;
				}
			}
			actionBeh.enabled=false;
			
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
			//only rotate with the y axis
			model.transform.LookAt(targetRot,Vector3.up);
		}
		
		//other tasks for IO_Unit's and IO_Building's
		public virtual void setTargetPos(Vector3 coords){
			this.meetingPoint=coords;
			this.selectReact.moveTargetMarkerModel();
		}
		
		public virtual void deleteModel(){
			MonoBehaviour.Destroy(model);
			MonoBehaviour.Destroy(plane.gameObject);
		}
		
		public virtual void heartbeat(float time){
			//gets called by an FixedUpdate() method of a MonoBehaviour
		}
	}
}
