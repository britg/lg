using UnityEngine;

public partial class SGT_SurfaceTessellator
{
	public void Awake()
	{
		RebuildLUT();
		RebuildPatches();
		
		sides              = null;
		sideCombinedMeshes = null;
	}
	
	public void LateUpdate()
	{
		if (displacementTexture.Modified == true)
		{
			displacementTexture.Modified = false;
			
			RebuildPatches();
		}
		
		if (running == false)
		{
			running = true;
			
			if (Application.isPlaying == true)
			{
				StartCoroutine(Update_Coroutine());
			}
		}
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		if (sideCombinedMeshes != null)
		{
			for (var i = 0; i < 6; i++)
			{
				SGT_Helper.DestroyObjects(sideCombinedMeshes[i]);
			}
		}
		
		DestroyPatches();
	}
	
	public void OnEnable()
	{
		if (patchIndices == null)
		{
			RebuildPatchIndices();
		}
		
		RebuildPatches();
	}
	
	public void OnDisable()
	{
		StopAllCoroutines();
	}
	
	public void SetSurfaceConfiguration(SGT_SurfaceConfiguration newConfiguration)
	{
		if (newConfiguration != surfaceConfiguration)
		{
			surfaceConfiguration = newConfiguration;
			
			RebuildPatches();
		}
	}
}