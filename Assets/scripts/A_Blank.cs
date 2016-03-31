using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts{
	public abstract class A_Blank : Action {
		
		public A_Blank() : base(){
			this.name="A_Blank";
		}
		
		public override void begin(){
			
		}
		
		public override void abort(IngameItem item){
			
		}
		
		public override void finish(IngameItem item){
			
		}
	}
}
