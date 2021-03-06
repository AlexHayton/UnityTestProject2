using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestProject
{
	public class GUISelectedWeapon : GUIContentHolder
	{
		private GUIWeaponSlot selectedSlotGUI;
		IList<GUIWeaponSlot> _weaponSlots = null;
		private IList<GUIWeaponSlot> WeaponSlots
		{
			get
			{
				if (_weaponSlots == null)
				{
					if (guiHandler)
					{
						// for some reason the slots ain't working!
						IList<GUIWeaponSlot> slots = guiHandler.GetGUIs<GUIWeaponSlot>();
						if (slots.Count > 0)
						{
							_weaponSlots = slots;
						}
					}
				}
				return _weaponSlots;
			}
		}

		public void SetSelectedWeaponIndex(int selectedIndex)
		{
			// Updated the SelectedSlotGUI.
		}

		public override void OnGUI()
		{
			if (Event.current.type == EventType.Repaint)
			{
				//GUIWeaponSlot selectedSlotGUI = WeaponSlots[selectedSlotIndex];

				if (selectedSlotGUI != null)
				{
					Vector3 selectedSlotPosition = selectedSlotGUI.GetScreenPosition();
					Vector3 selectedSlotDimensions = selectedSlotGUI.GetScale();
					Vector3 thisSlotPosition = this.GetScreenPosition();
					Vector3 thisSlotDimensions = this.GetScale();
					Vector3 slerpedPosition = Vector3.Slerp(thisSlotPosition, selectedSlotPosition, Time.deltaTime);
					Vector3 slerpedScale = Vector3.Slerp(thisSlotDimensions, selectedSlotDimensions, Time.deltaTime);

					this.SetLeft(slerpedPosition.x);
					this.SetTop(slerpedPosition.y);
					this.SetScale(slerpedScale);
				}
			}
			
			base.OnGUI();
		}

	}
}
