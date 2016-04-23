using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts{
	public abstract class A_Upgrade : Action {
		protected new IO_Building obj=null;	//only IO_Building's can do upgrades
		
		public A_Upgrade() : base(){
			this.name="A_Upgrade";
		}
		
		//override because obj is now an IO_Building type
		public override void setObject(IngameObject obj){
			if(obj is IO_Building){
				this.obj=(IO_Building)obj;
				this.production=((IO_Building)obj).production;
			}
		}
		
		public abstract override void begin();
		
		public abstract override void abort(IngameItem item);
		
		public abstract override void finish(IngameItem item);
	}
}
