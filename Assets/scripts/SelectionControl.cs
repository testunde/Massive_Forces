using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class SelectionControl {
		private static SelectionControl instance=null;
		private List<IngameObject> selections=new List<IngameObject>();
		public List<IngameObject> tempMarker=new List<IngameObject>();
		
		private SelectionControl(){
		}
		
		public static SelectionControl getInstance(){
			if(instance==null)
				instance=new SelectionControl();
			
			return instance;
		}
		
		//BASIC SET METHODS
		public void addItemMarker(IngameObject item){
			if(!tempMarker.Contains(item)){
				tempMarker.Add(item);
				processSelect();
			}
		}
		
		public void removeItemMarker(IngameObject item){
			item.selectReact.deselect();
			tempMarker.Remove(item);
			processSelect();
		}
		
		public void invertItemMarker(IngameObject item){
			if(tempMarker.Contains(item))
				removeItemMarker(item);
			else
				addItemMarker(item);
		}
		
		//ADVANCED REMOVE METHODS
		public void removeNonunits(){
			for(int i=0;i<selections.Count;i++){
				IngameObject obj=selections[i];
				if(!(obj is IO_Unit)){
					obj.selectReact.deselect();
					selections.Remove(obj);
				}
			}
		}
		
		public void removeNonbuildings(){
			for(int i=0;i<selections.Count;i++){
				IngameObject obj=selections[i];
				if(!(obj is IO_Building)){
					obj.selectReact.deselect();
					selections.Remove(obj);
				}
			}
		}
		
		private void processSelect(){
			selections=new List<IngameObject>(tempMarker);
			if(areSomeUnits())
				removeNonunits();
			else if(areSomeBuildings())
				removeNonbuildings();
			
			foreach(IngameObject obj in selections)
				obj.selectReact.select();
		}
		
		//e.g. with double click
		public void leaveSameObjectsSelected(IngameObject Iobj){
			for(int i=0;i<selections.Count;i++){
				IngameObject obj=selections[i];
				//alternative: if(!obj.type==Iobj.type)
				if(!(Object.ReferenceEquals(obj.GetType(),Iobj.GetType()))){
					obj.selectReact.deselect();
					selections.Remove(obj);
				}
			}
		}
		
		public void copyToTemp(){
			tempMarker=new List<IngameObject>(selections);
		}
		
		public void clearList(){
			foreach(IngameObject obj in selections){
				obj.selectReact.deselect();
			}
			selections.Clear();
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
		
		public int isOnlyOneFraction(){
			int result=selections[0].fraction;
			foreach(IngameObject obj in selections){
				if(!(obj is IO_Unit)){
					result=0;
					break;
				}
			}
			return result;
		}
	}
}
