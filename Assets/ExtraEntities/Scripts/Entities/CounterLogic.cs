using UnityEngine;

public class CounterLogic : TriggerBase {	
	
	public int count;
	public int step;
	public int currentCount;	
			
	[Callable]
	public virtual void Add() {	
		currentCount = Mathf.Min (currentCount +step, count);				
		CheckCount();
	}
	
	[Callable]
	public virtual void Sub() {	
		currentCount = Mathf.Max (currentCount -step,0);
		CheckCount();
	}	
	
	[Callable]
	public virtual void Reset() {	
		currentCount = 0;
		CheckCount();
	}	
	
	private void CheckCount() {
		if (currentCount >= count) {
			base.CallMethods();
		}
	}
	
}