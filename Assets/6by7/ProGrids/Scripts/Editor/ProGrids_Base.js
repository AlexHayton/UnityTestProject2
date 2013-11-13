class ProGrids_Base extends EditorWindow 
{
	
	//external variables
	var snapSizeGraphic = (Resources.LoadAssetAtPath("Assets/6by7/Shared/GUI/spcl_GridSize.tga", typeof(Texture2D)));
	var snapOnGraphic = (Resources.LoadAssetAtPath("Assets/6by7/Shared/GUI/ind_OnLight.tga", typeof(Texture2D)));
	var snapOffGraphic = (Resources.LoadAssetAtPath("Assets/6by7/Shared/GUI/ind_OffLight.tga", typeof(Texture2D)));
	var snapSelectedGraphic = (Resources.LoadAssetAtPath("Assets/6by7/Shared/GUI/btn_snapToGrid.tga", typeof(Texture2D)));
	var visOnGraphic = (Resources.LoadAssetAtPath("Assets/6by7/Shared/GUI/ind_VisOn.tga", typeof(Texture2D)));
	var visOffGraphic = (Resources.LoadAssetAtPath("Assets/6by7/Shared/GUI/ind_VisOff.tga", typeof(Texture2D)));
	var anglesOnGraphic = (Resources.LoadAssetAtPath("Assets/6by7/Shared/GUI/ind_AnglesOn.tga", typeof(Texture2D)));
	var anglesOffGraphic = (Resources.LoadAssetAtPath("Assets/6by7/Shared/GUI/ind_AnglesOff.tga", typeof(Texture2D)));
	var mySkin = (Resources.LoadAssetAtPath("Assets/6by7/Shared/GUI/CustomSkin_Unity4.guiskin", typeof(GUISkin)));
	//
	
	var proGrids : ProGrids;
	
	var gridUnitsOptions : String[] = ["m", "m"];
	var gridUnitsFactors : float[] = [1.0,  1.0];
	
	var toggleSnapGraphic : Texture2D;
	var toggleVisGraphic : Texture2D;
	var toggleAnglesGraphic : Texture2D;
	var sideOffset : int = 3;
	var ngp : int;
	var itemWidth : int;
	var itemHeight : int = 18;
	var toggleOffset2 : int = 18;
	var showOptions : boolean = false;

	var nearestSnapPos : Vector3;
	var activeTransform : Transform;
	var activeTransformPos : Vector3;
	var storedLength : float;
	var savedSelectionIDs = new Array();
	var offsetArray : Vector3[];
	
	var rightRotation = Vector3(-1,0,0);
	var leftRotation = Vector3(1,0,0);
	var topRotation = Vector3(0,-1,0);
	var bottomRotation = Vector3(0,1,0);
	var frontRotation = Vector3(0,0,-1);
	var backRotation = Vector3(0,0,1);
		
	function OnEnable()
	{
		InitProGrids();
	}

	function InitProGrids()
	{		
		var go : GameObject = GameObject.Find("_grid");
		
		if(go == null) {
			go = Instantiate((Resources.LoadAssetAtPath("Assets/6by7/ProGrids/_grid.prefab", typeof(GameObject))), Vector3(-1,0,0), Quaternion.identity);
			go.name = "_grid";
		}
		
		proGrids = go.GetComponent.<ProGrids>();
	}

	//100 times/second
	function Update()
	{
		if(!proGrids.snapToGrid || Application.isPlaying)
			return;			

		if(proGrids) 
		{	
			if(Selection.transforms.Length > 0) //if grid is found
			{
				if(Selection.transforms[0].position != activeTransformPos)
					DoSnap();
			
				proGrids.activePoint = activeTransformPos;
			}
			//else
				//proGrids.activePoint = null;
		}
	}
	
		//snap object(s)
	function DoSnap()
	{               
		for(var i=0;i<Selection.transforms.Length;i++)
		{
			if(Selection.gameObjects[i].GetComponent(MeshFilter))
			{
				if(Selection.gameObjects[i].GetComponent(MeshFilter).sharedMesh && Selection.gameObjects[i].GetComponent(MeshFilter).sharedMesh.name.Equals("DecalMeshObject"))
				{
						return;
				}
			}
			Selection.transforms[i].position = FindNearestSnapPos(Selection.transforms[i].position);
		}

		activeTransformPos = Selection.activeTransform.position; //position of the main transform center
	}
	
	//finds the nearest grid point for the grid to center on
    function FindNearestSnapPos(v3 : Vector3) : Vector3
    {
		return Vector3(
			.25 * Mathf.Round(v3.x / .25),
			.25 * Mathf.Round(v3.y / .25),
			.25 * Mathf.Round(v3.z / .25));
	}
	
	function BuildOffsetArray()
	{	
		if(storedLength > 1)
		{
			offsetArray = new Vector3[storedLength];
			for(var i=0;i<storedLength;i++)
			{
				offsetArray[i] = Selection.objects[i].transform.position-activeTransformPos;
			}
		}
	}
	
	function SetupSelectionForSnap()
	{
		activeTransform = Selection.activeTransform; //set the main transform- all other objects will follow this one
		// storedLength = Selection.objects.length; //store the selection length
		// storedLength = Selection.objects.length; //set the selection length- helps determine if selection has changed
		// if(activeTransform)
		// {
		if(activeTransform)
			gridReferencePos = activeTransform.position; //position of the main transform center
		// 	storedPos = gridReferencePos; //position of the main transform center
		// 	FindNearestSnapPos(); //find the closest snap position
		// 	BuildOffsetArray(); //create an array to hold all the object's offsets
		// }
	}
	
	function SnapSelectedObjects()
	{
		for(obj in Selection.transforms)
			obj.position = FindNearestSnapPos(obj.position);
		// if(Selection.activeTransform)
		// {
		// 	for(theObj in Selection.transforms)
		// 	{
		// 		var snapPos = theObj.position;
		// 		snapPos.x = .25 * Mathf.Round(snapPos.x / .25); //find it's x position, rounded to the snap amount 
		// 		snapPos.y = .25 * Mathf.Round(snapPos.y / .25); //find it's y position, rounded to the snap amount 
		// 		snapPos.z = .25 * Mathf.Round(snapPos.z / .25); //find it's z position, rounded to the snap amount
		// 		theObj.position = snapPos;
		// 	}
		// 	BuildOffsetArray(); //create an array to hold all the object's offsets
		// }
	}
	
	function UndoSpecial()
	{
		var gridOn : boolean = false;
		if(proGrids.snapToGrid)
		{
			proGrids.snapToGrid = false;
			gridOn = true;
		}
		Undo.PerformUndo();
		if(gridOn)
		{
			proGrids.snapToGrid = true;
		}
	}
	
	function RedoSpecial()
	{
		var gridOn : boolean = false;
		if(proGrids.snapToGrid)
		{
			proGrids.snapToGrid = false;
			gridOn = true;
		}
		Undo.PerformRedo();
		if(gridOn)
		{
			proGrids.snapToGrid = true;
		}
	}
	
	// this is unnecessary... also Unity has a built in OnSelectionChange() method
	function CheckSelectionForChange()
	{
		//first, the quick length/active item check
		if(Selection.activeTransform != activeTransform || Selection.objects.length != storedLength)
		{
			//Debug.Log("Selection changed: Length/Active Object mismatch!");
			return true;
		}
		//second, the full one-by-one test
		var i : int = 0;
		
		// if(Selection.instanceIDs.Length != savedSelectionIDs.Length)	
		// 	return true;

		for(var theID : int in Selection.instanceIDs)
		{
			if(savedSelectionIDs[i] != theID)
			{
				//Debug.Log("Selection changed: ID mismatch!");
				return true;
			}
			i++;
		}
		//otherwise, the selection hasn't changed!
		return false;
	}
	
	/**
	 *	PROBUILDER BRIDGE FUNCTIONS (it is not guaranteed that ProBuilder & ProGrids
	 *	will always exist in parallel).
	 */
	function pb_ToggleSnapToGrid(snap : boolean)
	{
		for(var t : Transform in Selection.transforms)
		{
			if(t.GetComponent("pb_Object")) // Magic strings are sometimes okay!
			{
				var tp : System.Type = t.GetComponent("pb_Object").GetType();
				var obj : System.Object = t.GetComponent("pb_Object");
				// -- for whatever reason this doesn't work... [snap, proGrids.gridSnapSize_Factored];
				var param : System.Object[] = new System.Object[2];
				param[0] = snap;
				param[1] = proGrids.gridSnapSize_Factored;
				var method : MethodInfo = tp.GetMethod("OnProGridsChange");
				method.Invoke( obj, param );
			}
		}
	}
}