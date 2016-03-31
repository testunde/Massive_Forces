using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts{
	public abstract class A_Command : Action {
		
		public A_Command() : base(){
			this.name="A_Command";
			//set icon
		}
		
		public abstract override void begin();
		
		public abstract override void abort(IngameItem item);
		
		public abstract override void finish(IngameItem item);
	}
}
