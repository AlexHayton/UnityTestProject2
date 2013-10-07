using UnityEngine;
using System.Collections;
	
[ExecuteInEditMode()]
public class GUIStylePrefab : MonoBehaviour, ISelfTest {
	public GUIStyle style;
	public Vector2 scale;
	
	public void Start()
	{
	}
	
	public bool SelfTest()
	{
		bool fail = false;
		
		SelfTestUtility.GreaterThanZero(ref fail, "scale.x", this.scale.x);
		SelfTestUtility.GreaterThanZero(ref fail, "scale.y", this.scale.y);
		
		return fail;
	}
}
