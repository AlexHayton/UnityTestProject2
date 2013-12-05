using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace TestProject
{
	public class UseHandler : MonoBehaviour 
	{
	
		public UseTarget currentUseTarget = null;
		public float useRange = 2.0f;
		public float checkInterval = 0.2f;
		public float useInterval = 0.5f;
		public float nextUse = 0f;

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
				List<Collider> colliderList = new List<Collider>(hitColliders);
				IEnumerable<Collider> objects = colliderList.OrderBy(c => (c.transform.position - this.transform.position).sqrMagnitude);
				IEnumerable<UseTarget> targets = objects.Select(c => c.GetComponent<UseTarget>());

				currentUseTarget = targets
					.Where(t => t != null)
					.Where(t => t.CanBeUsedBy(this.gameObject))
					.Take(1)
					.FirstOrDefault();
				
				yield return new WaitForSeconds( checkInterval );
			}
		}

		public void Use()
		{
			if (this.currentUseTarget)
			{
				Debug.Log ("Using OK!");
				currentUseTarget.OnUse(this.gameObject);
			}
		}

		void Update()
		{
			if (Input.GetButton("Use"))
			{
				if (Time.time > this.nextUse)
				{
					Use ();
					this.nextUse = Time.time + this.useInterval;
				}
			}
		}
    }
}