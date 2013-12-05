using UnityEngine;
using System.Collections;
using System.Linq;

namespace TestProject
{
	public class UseHandler : MonoBehaviour 
	{
	
		private UseTarget currentUseTarget = null;
		public float useRange = 5.0f;
		public float checkInterval = 0.3f;

		void Start()
		{
			StartCoroutine( PickNewUseTarget() );
		}

		public bool CanUse()
    	{
			return currentUseTarget != null && currentUseTarget.CanBeUsedBy(gameObject);
    	}

		public UseTarget GetUseTarget()
		{
			return currentUseTarget;
		}

		public IEnumerator PickNewUseTarget()
		{
			// Infinite loop executed every "frenquency" secondes.
			while( true )
			{
				Collider[] hitColliders = Physics.OverlapSphere(transform.position, useRange);
				UseTarget target = hitColliders.OrderBy(c => (c.transform.position - this.transform.position).sqrMagnitude)
											   .Select(c => c.GetComponent<UseTarget>())
						    				   .Where(c => c != null)
											   .Take(1)
											   .FirstOrDefault();

				currentUseTarget = target;
				
				yield return new WaitForSeconds( checkInterval );
			}
		}
    }
}