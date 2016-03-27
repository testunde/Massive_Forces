using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts{
	public abstract class A_Update : Action {
		
		public A_Update(IngameObject obj) : base(obj){
			this.name="A_Update";
		}
		
		public abstract override void begin();
		
		public abstract override void abort();
		
		public abstract override void finish();
	}
}
