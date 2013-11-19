using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestProject
{
	public class GUISelectedWeapon : GUIContentHolder
	{
		int selectedSlot = 1;
		GUIWeaponSlot selectedSlotGUI = null;

		IList<GUIWeaponSlot> _weaponSlots = null;
		private IList<GUIWeaponSlot> WeaponSlots
		{
			get
			{
				if (_weaponSlots == null)
				{
					_weaponSlots = guiHandler.GetGUIs<GUIWeaponSlot>();
				}
				return _weaponSlots;
			}
		}

		public void SetSelectedWeaponIndex(int index)
		{
			this.selectedSlotGUI = WeaponSlots[index];
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
