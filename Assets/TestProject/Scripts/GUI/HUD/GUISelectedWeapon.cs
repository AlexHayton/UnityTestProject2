using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestProject
{
	public class GUISelectedWeapon : GUIContentHolder
	{
		GUIHandler guiHandler = null;
		WeaponHandler weaponHandler = null;
		IList<GUIWeaponSlot> weaponSlots = null;
		int selectedSlot = 1;
		GUIWeaponSlot selectedSlotGUI = null;

		public override void Start()
		{
			base.Start();

			this.guiHandler = GUIUtility.GetLocalGUIHandler();
			this.weaponSlots = this.guiHandler.GetGUIs<GUIWeaponSlot>();
			GameObject playerObject = PlayerUtility.GetLocalPlayer();
			this.weaponHandler = playerObject.GetComponent<WeaponHandler>();
		}

		public void SetSelectedWeaponIndex(int index)
		{
			this.selectedSlotGUI = weaponSlots[index];
		}

		public override void OnGUI()
		{
			if (this.selectedSlotGUI != null)
			{
				Vector3 selectedSlotPosition = this.selectedSlotGUI.GetScreenPosition();
				Vector3 selectedSlotDimensions = this.selectedSlotGUI.GetScale();
				Vector3 thisSlotPosition = this.GetScreenPosition();
				Vector3 thisSlotDimensions = this.GetScale();
				Vector3 slerpedPosition = Vector3.Slerp(thisSlotPosition, selectedSlotPosition, Time.deltaTime);
				Vector3 slerpedScale = Vector3.Slerp(thisSlotDimensions, selectedSlotDimensions, Time.deltaTime);

				this.SetLeft(slerpedPosition.x);
				this.SetTop(slerpedPosition.y);
				this.SetScale(slerpedScale);
			}
		}

	}
}
