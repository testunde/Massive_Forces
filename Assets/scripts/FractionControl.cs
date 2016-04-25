using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class FractionControl {
		private static FractionControl instance=null;
		public int f=3;	//amount of fractions (incl. player- and neutral fraction)
		private Color[] colors;
		//RSC (resources):
		public int RSC_a=4;	//amount of resource types
		private long[,] RSC_res;
		private long[,] RSC_maxRes;
		private string[] RSC_names;
		//REA (researches):
		public float[] testUnitHP;
		
		private FractionControl(){
			colors=new Color[8+1];	//8 is maximum of possible players
			colors[0]=Color.black;	//neutral fraction has no colors: color.clear for transparent
			colors[1]=Color.green;	//player colors...
			colors[2]=Color.red;
			colors[3]=Color.blue;
			colors[4]=Color.yellow;
			colors[5]=new Color(1f,.416f,0f);	//orange
			colors[6]=Color.magenta;
			colors[7]=Color.cyan;	//bzw. türkis
			colors[8]=new Color(.59f,.32f,0f);	//brown
			
			//RSC
			RSC_res=new long[f,RSC_a];
			RSC_maxRes=new long[f,RSC_a];
			RSC_names=new string[]{"Res0","Res1","Res2","Res3"};
			
			//REA
			testUnitHP=new float[f];
			
			//set start values
			for(int i=0;i<f;i++){
				//RSC
				RSC_res[i,0]=200;
				RSC_maxRes[i,0]=2000;
				
				RSC_res[i,1]=200;
				RSC_maxRes[i,1]=2000;
				
				RSC_res[i,2]=80;
				RSC_maxRes[i,2]=2000;
				
				RSC_res[i,3]=0;
				RSC_maxRes[i,3]=500;
				
				//REA
				testUnitHP[i]=1f;
			}
		}
		
		public static FractionControl getInstance(){
			if(instance==null)
				instance=new FractionControl();
			
			return instance;
		}
		
		public Color getColor(int n){
			return colors[n];
		}
		
		// #########
		// ## RSC ##
		// #########
		
		private void RSC_limit(int fraction){
			for(int i=0;i<RSC_a;i++){
				if(RSC_res[fraction,i]>RSC_maxRes[fraction,i])
					RSC_res[fraction,i]=RSC_maxRes[fraction,i];
				//else if(RSC_res[fraction,i]<0)
				//	RSC_res[fraction,i]=0;
			}
		}
		
		public bool RSC_isAvailable(int fraction,int r,long amount){
			bool result=false;
			if(r<RSC_a){
				result=(RSC_res[fraction,r]>=amount);
			}
			return result;
		}
		
		public bool RSC_costsAvailable(int fraction,long[] costs){
			bool result=true;
			if(costs.Length<=RSC_a){
				for(int i=0;i<RSC_a;i++){
					if(RSC_res[fraction,i]<Mathf.Abs(costs[i])){
						result=false;
						break;
					}
				}
			}else{
				result=false;
			}
			return result;
		}
		
		public string RSC_getName(int n){
			return RSC_names[n];
		}
		
		//CURRENT VALUES
		public void RSC_changeBy(int fraction,long[] amount,bool invert){
			if(amount.Length==RSC_a){
				for(int i=0;i<RSC_a;i++){
					if(invert)
						RSC_res[fraction,i]-=amount[i];
					else
						RSC_res[fraction,i]+=amount[i];
				}
			}
			RSC_limit(fraction);
		}
		public void RSC_changeBy(int fraction,long[] amount){
			RSC_changeBy(fraction,amount,false);
		}
		public void set(int fraction,long[] amount){
			if(amount.Length==RSC_a){
				for(int i=0;i<RSC_a;i++)
					RSC_res[fraction,i]=amount[i];
			}
			RSC_limit(fraction);
		}
		public long[] RSC_get(int fraction){
			long[] result=new long[RSC_a];
			for(int i=0;i<RSC_a;i++)
				result[i]=RSC_res[fraction,i];
			return result;
		}
		
		//MAX VALUES
		public void RSC_changeByMax(int fraction,long[] amount){
			if(amount.Length==RSC_a){
				for(int i=0;i<RSC_a;i++)
					RSC_maxRes[fraction,i]+=amount[i];
			}
		}
		public void RSC_setMax(int fraction,long[] amount){
			if(amount.Length==RSC_a){
				for(int i=0;i<RSC_a;i++)
					RSC_maxRes[fraction,i]=amount[i];
			}
		}
		public long[] RSC_getMax(int fraction){
			long[] result=new long[RSC_a];
			for(int i=0;i<RSC_a;i++)
				result[i]=RSC_maxRes[fraction,i];
			return result;
		}
		
		// #########
		// ## REA ##
		// #########
	}
}
