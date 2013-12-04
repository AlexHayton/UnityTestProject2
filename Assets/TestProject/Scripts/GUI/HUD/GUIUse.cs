using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestProject
{
	public class GUIUseIcon : GUIContentHolder
	{
		private UseHandler useHandler;
	
		public override void Start()
		{
			base.Start();
			
			useHandler = owner.GetComponent<UseHandler>();
			if (useHandler == null)
			{
				Debug.Error("Error: Can't use GUIUse if player doesn't have UseHandler");
			}
		}
		
		public override Texture2D GetImage()
		{
			Texture2D foundImage = null;
			if (useHandler != null)
			{
				UseTarget target = useHandler.GetUseTarget();
				if (target != null)
				{
					foundImage = target.GetUseIcon();
				}
			}
			
			return foundImage;
		}
		
	}
}
