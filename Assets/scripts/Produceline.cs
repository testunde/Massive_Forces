using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts;

namespace Scripts {
	public class Produceline {
		private LinkedList<IngameItem> items;
		
		public Produceline(){
			items=new LinkedList<IngameItem>();
		}
		
		public LinkedList<IngameItem> getItems(){
			return new LinkedList<IngameItem>(items);
		}
		
		public void decreaseTime(float by){
			if(items.Count>0){
				IngameItem ii=items.First.Value;
				if(ii.timeRemaining>0){
					ii.timeRemaining-=by;
				}else{
					items.RemoveFirst();
					ii.createdBy.finish(ii);
				}
			}
		}
		
		public void addItem(IngameItem it){
			items.AddLast(it);
		}
		
		public void removeItem(IngameItem it){
			items.Remove(it);
			it.createdBy.abort(it);
		}
	}
}
