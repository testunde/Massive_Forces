using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class SelectionControl {
		private static SelectionControl instance=null;
		private List<IngameObject> selections=new List<IngameObject>();
		public List<IngameObject> tempMarker=new List<IngameObject>();
		public List<IngameObject> inView=new List<IngameObject>();
		
		private SelectionControl(){
		}
		
		public static SelectionControl getInstance(){
			if(instance==null)
				instance=new SelectionControl();
			
			return instance;
		}
		
		//BASIC SET METHODS
		public void addItemMarker(IngameObject obj){
			if(!tempMarker.Contains(obj)){
				tempMarker.Add(obj);
				processSelect();
			}
		}
		
		public void removeItemMarker(IngameObject obj){
			tempMarker.Remove(obj);
			obj.selectReact.deselect();
			processSelect();
		}
		
		public void invertItemMarker(IngameObject obj){
			if(tempMarker.Contains(obj))
				removeItemMarker(obj);
			else
				addItemMarker(obj);
		}
		
		//ADVANCED REMOVE METHODS
		private void processSelect(){
			selections=new List<IngameObject>(tempMarker);
			if(areSomeUnits())
				removeNonunits();
			else if(areSomeBuildings())
				removeNonbuildings();
			
			foreach(IngameObject obj in selections)
				obj.selectReact.select();
		}
		
		public void removeNonunits(){
			//iterate backwards: the index of each element changes when an element before it gets removed or added!
			for(int i=selections.Count-1;i>=0;i--){
				IngameObject obj=selections[i];
				if(!(obj is IO_Unit)){
					obj.selectReact.deselect();
					selections.Remove(obj);
				}
			}
		}
		
		public void removeNonbuildings(){
			//iterate backwards: the index of each element changes when an element before it gets removed or added!
			for(int i=selections.Count-1;i>=0;i--){
				IngameObject obj=selections[i];
				if(!(obj is IO_Building)){
					obj.selectReact.deselect();
					selections.Remove(obj);
				}
			}
		}
		
		//e.g. with double click
		public void selectSameObjectsInView(){
			if(selections.Count>0){
				IngameObject Sobj=selections[0];
				tempMarker=new List<IngameObject>(inView);
				Debug.Log(tempMarker.Count);
				for(int i=tempMarker.Count-1;i>=0;i--){	//#######DON'T REMOVE WHEN ITERATE!!! (see above)
					IngameObject obj=tempMarker[i];
					if(obj.type.Equals(Sobj.type)){
					//if((Object.ReferenceEquals(obj.GetType(),Sobj.GetType()))){
						obj.selectReact.select();
						selections.Add(obj);
					}
				}
			}
		}
		
		public void copyToTemp(){
			tempMarker=new List<IngameObject>(selections);
		}
		
		public void clearList(IngameObject Iobj){
			foreach(IngameObject obj in selections){
				if(obj!=Iobj)
					obj.selectReact.deselect();
			}
			selections.Clear();
			if(Iobj!=null)
				selections.Add(Iobj);
		}
		
		public void clearTempList(){
			tempMarker.Clear();
		}
		
		//GET METHODS
		public bool areOnlyUnits(){
			bool result=true;
			foreach(IngameObject obj in selections){
				if(!(obj is IO_Unit)){
					result=false;
					break;
				}
			}
			return result;
		}
		
		public bool areSomeUnits(){
			bool result=false;
			foreach(IngameObject obj in selections){
				if(obj is IO_Unit){
					result=true;
					break;
				}
			}
			return result;
		}
		
		public bool areOnlyBuildings(){
			bool result=true;
			foreach(IngameObject obj in selections){
				if(!(obj is IO_Building)){
					result=false;
					break;
				}
			}
			return result;
		}
		
		public bool areSomeBuildings(){
			bool result=false;
			foreach(IngameObject obj in selections){
				if(obj is IO_Building){
					result=true;
					break;
				}
			}
			return result;
		}
		
		public int isOnlyOneFraction(){	//TO USE!
			int result=selections[0].fraction;
			foreach(IngameObject obj in selections){
				if(obj.fraction!=result){
					result=-1;
					break;
				}
			}
			return result;
		}
		
		public List<IngameObject> getSelectedObjects(){
			return new List<IngameObject>(selections);
		}
		
		public int getSelectedCount(){
			return selections.Count;
		}
		
		public IngameObject getIfOnlyOne(){
			IngameObject result=null;
			if(selections.Count==1)
				result=selections[0];
			return result;
		}
	}
}
