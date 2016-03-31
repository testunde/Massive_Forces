using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts{
	public abstract class A_Create : Action {
		
		public A_Create() : base(){
			this.name="A_Create";
		}
		
		public abstract override void begin();
		
		public abstract override void abort(IngameItem item);
		
		public abstract override void finish(IngameItem item);
	}
}
