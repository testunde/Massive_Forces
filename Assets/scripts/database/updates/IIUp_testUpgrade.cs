using UnityEngine;
using System.Collections;
using Scripts;

namespace Database{
	public class IIUp_testUpgrade : II_Upgrade {
		
		public IIUp_testUpgrade() : base(){
			this.name="IIUp_testUpgrade";
			this.buildTime=3f;
			this.costs=new long[]{-5,-5,-3,-0};
		}
		
		public override void perform(){
			obj.workerUnits++;
		}
	}
}
