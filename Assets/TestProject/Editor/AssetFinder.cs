using System;

namespace AssemblyCSharpEditor
{
	public class AssetFinder
	{
		public AssetFinder ()
		{
		}
		
		/*protected static gameObject[] getAllEditorAssets()
	    {
		    DirectoryInfo = new DirectoryInfo(Application.dataPath);
		    FileInfo[] = directory.GetFiles("*.prefab", SearchOption.AllDirectories);
		    uint i = 0; 
			uint goFileInfoLength = goFileInfo.length;
		    var tempGoFileInfo:FileInfo; var tempFilePath:String; var assetIndex:int;
		    var tempGO:GameObject;
		    for(i = 0; i < goFileInfoLength; i++)
		    {
		    tempGoFileInfo = goFileInfo[i] as FileInfo;
		    if(tempGoFileInfo == null) continue;
		    tempFilePath = tempGoFileInfo.FullName;
		     
		    assetIndex = tempFilePath.IndexOf("Assets/");
		    //assetIndex = tempFilePath.IndexOf("Assets\\");
		    if (assetIndex < 0) assetIndex = 0;
		    tempFilePath = tempFilePath.Substring(assetIndex, tempFilePath.length - assetIndex);
		    //tempFilePath = tempFilePath.Replace('\\', '/');
		    tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, GameObject) as GameObject;
		    if(tempGO == null) continue;
		    tempObjects.push(tempGO);
		    }
		     
		    return tempObjects.ToBuiltin(GameObject) as GameObject[];
	    }*/
	}
}

