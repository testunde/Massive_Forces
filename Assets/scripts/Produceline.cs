using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class Produceline {
		private static Produceline instance=null;
		private Dictionary<IO_Building,List<IngameItem>> items;
		
		private Produceline(){
			items=new Dictionary<IO_Building,List<IngameItem>>();
		}
		
		public static Produceline getInstance(){
			if(instance==null)
				instance=new Produceline();
			
			return instance;
		}
		
		public List<IngameItem> getItems(IO_Building building){
			List<IngameItem> result=null;
			// if(!(items.TryGetValue(building,out result)))
				// result=new List<IngameItem>();
			items.TryGetValue(building,out result);
			return result;
		}
		
		public void decreaseTime(int by){
			foreach(List<IngameItem> list in items.Values){
				foreach(IngameItem ii in list){
					if(ii.timeRemaining>0){
						ii.timeRemaining-=by;
					}else{
						//callback "it's finished"!
					}
				}
			}
		}
		
		public void addBuilding(IO_Building building){
			items.Add(building,new List<IngameItem>());
		}
		
		public void addItem(IO_Building building,IngameItem it){
			List<IngameItem> list=null;
			if(items.TryGetValue(building,out list))
				list.Add(it);
		}
		
		public void removeBuilding(IO_Building building){
			items.Remove(building);
		}
	}
}
