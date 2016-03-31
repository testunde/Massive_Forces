using UnityEngine;
using System.Collections;
using Scripts;

namespace Scripts {
	public class Res {
		private static Res instance=null;
		public int f=3;	//amount of fractions with player- and neutral fraction
		public int a=4;	//amount of resource types
		private long[,] res;
		private long[,] maxRes;
		private string[] names;
		private Color[] colors;
		
		private Res(){
			res=new long[f,a];
			maxRes=new long[f,a];
			names=new string[]{"Res0","Res1","Res2","Res3"};
			colors=new Color[f];
			colors[0]=Color.black;	//neutral fraction has no colors: color.clear for transparent
			colors[1]=Color.red;	//player colors
			//colors[...]=...
			
			//set start values
			for(int i=0;i<f;i++){
				res[i,0]=200;
				maxRes[i,0]=2000;
				
				res[i,1]=200;
				maxRes[i,1]=2000;
				
				res[i,2]=80;
				maxRes[i,2]=2000;
				
				res[i,3]=0;
				maxRes[i,3]=500;
			}
		}
		
		public static Res getInstance(){
			if(instance==null)
				instance=new Res();
			
			return instance;
		}
		
		private void limit(int fraction){
			for(int i=0;i<a;i++){
				if(res[fraction,i]>maxRes[fraction,i])
					res[fraction,i]=maxRes[fraction,i];
				//else if(res[fraction,i]<0)
				//	res[fraction,i]=0;
			}
		}
		
		public bool isAvailable(int fraction,int r,long amount){
			bool result=false;
			if(r<a){
				result=(res[fraction,r]>=amount);
			}
			return result;
		}
		
		public bool costsAvailable(int fraction,long[] costs){
			bool result=true;
			if(costs.Length<=a){
				for(int i=0;i<a;i++){
					if(res[fraction,i]<Mathf.Abs(costs[i])){
						result=false;
						break;
					}
				}
			}else{
				result=false;
			}
			return result;
		}
		
		public string getName(int n){
			return names[n];
		}
		
		public Color getColor(int n){
			return colors[n];
		}
		
		//CURRENT VALUES
		public void changeBy(int fraction,long[] amount,bool invert){
			if(amount.Length==a){
				for(int i=0;i<a;i++){
					if(invert)
						res[fraction,i]-=amount[i];
					else
						res[fraction,i]+=amount[i];
				}
			}
			limit(fraction);
		}
		public void changeBy(int fraction,long[] amount){
			changeBy(fraction,amount,false);
		}
		public void set(int fraction,long[] amount){
			if(amount.Length==a){
				for(int i=0;i<a;i++)
					res[fraction,i]=amount[i];
			}
			limit(fraction);
		}
		public long[] get(int fraction){
			long[] result=new long[a];
			for(int i=0;i<a;i++)
				result[i]=res[fraction,i];
			return result;
		}
		
		//MAX VALUES
		public void changeByMax(int fraction,long[] amount){
			if(amount.Length==a){
				for(int i=0;i<a;i++)
					maxRes[fraction,i]+=amount[i];
			}
		}
		public void setMax(int fraction,long[] amount){
			if(amount.Length==a){
				for(int i=0;i<a;i++)
					maxRes[fraction,i]=amount[i];
			}
		}
		public long[] getMax(int fraction){
			long[] result=new long[a];
			for(int i=0;i<a;i++)
				result[i]=maxRes[fraction,i];
			return result;
		}
	}
}
