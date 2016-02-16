using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class SelectionControl {
		private static SelectionControl instance=null;
		private List<IngameObject> selections=new List<IngameObject>();
		private List<IngameObject> tempMarker=new List<IngameObject>();
		
		private SelectionControl(){
		}
		
		public static SelectionControl getInstance(){
			if(instance==null)
				instance=new SelectionControl();
			
			return instance;
		}
		
		//BASIC SET METHODS
		public void addItem(IngameObject item){
			tempMarker.Add(item);
			processSelect();
		}
		
		public void removeItem(IngameObject item){
			tempMarker.Remove(item);
			item.selectReact.deselect();
			processSelect();
		}
		
		//ADVANCED REMOVE METHODS
		public void removeNonunits(){
			foreach(IngameObject obj in selections){
				if(!(obj is IO_Unit)){
					obj.selectReact.deselect();
					selections.Remove(obj);
				}
			}
		}
		
		public void removeNonbuildings(){
			foreach(IngameObject obj in selections){
				if(!(obj is IO_Building)){
					obj.selectReact.deselect();
					selections.Remove(obj);
				}
			}
		}
		
		//e.g. with double click
		public void leaveSameObjectsSelected(IngameObject Iobj){
			foreach(IngameObject obj in selections){
				//alternative: if(!obj.type==Iobj)
				if(!(Object.ReferenceEquals(obj.GetType(),Iobj.GetType())))
					selections.Remove(obj);
			}
		}
		
		public void clearList(){
			selections=new List<IngameObject>();
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
