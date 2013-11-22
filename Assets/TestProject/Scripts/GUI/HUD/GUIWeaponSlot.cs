using UnityEngine;
using System.Collections;
using System;
using TestProject;
	
[ExecuteInEditMode()]
public class GUIWeaponSlot : GUIContentHolder { 

	public int weaponSlot;
	
	private WeaponBase GetWeapon()
	{
		WeaponBase currentWeapon = null;
		owner = PlayerUtility.GetParentPlayer(this.gameObject);
		WeaponHandler handler = owner.GetComponent<WeaponHandler>();
		if (handler)
		{
			return handler.GetWeaponInSlot(weaponSlot);
		}
		
		return currentWeapon;
	}

	public override string GetText()
	{
		return this.weaponSlot.ToString();
	}
	
	public override Texture2D GetImage()
	{
		WeaponBase weapon = this.GetWeapon();
		if (weapon)
		{
			return weapon.GetIcon();
		}
		else
		{
			return null;
		}
	}
	
	public override void OnGUI()
	{			
		this.OnBaseGUI();
		
		GUIContent imageContent = new GUIContent();
		imageContent.image = this.GetImage();
		
		GUIContent textContent = new GUIContent(); 
		textContent.text = this.GetText();
		
		GUIStyle textStyle = this.GetStyle();
		textStyle.normal.background = null;
		
		this.RenderGUI(delegate() 
		{
			// Render bg + image, then text overlaid
			GUI.Box(new Rect(
				this.GetLeft(),
				this.GetTop(), 
				this.GetPixelWidth(),
				this.GetPixelHeight()), 
				imageContent,
				this.GetStyle());
				
			GUI.Box(new Rect(
				this.GetLeft(),
				this.GetTop()+5, 
				this.GetPixelWidth(),
				this.GetPixelHeight()), 
				textContent);
		});
	}
}

