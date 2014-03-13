using UnityEngine;
using VariantList = System.Collections.Generic.List<SGT_StarfieldStarVariant>;

public partial class SGT_Starfield
{
	public void Awake()
	{
		if (ThisHasBeenDuplicated() == true)
		{
			starfieldMaterial = SGT_Helper.CloneObject(starfieldMaterial);
			meshes            = SGT_Helper.CloneObjects(meshes);
		}
	}
	
	public void LateUpdate()
	{
		if (starfieldGameObject == null) starfieldGameObject = SGT_Helper.CreateGameObject("Starfield", gameObject);
		if (starfieldMultiMesh  == null) starfieldMultiMesh  = new SGT_MultiMesh();
		if (starfieldCamera     == null) StarfieldCamera     = SGT_Helper.FindCamera(); // NOTE: Assigning property
		
		SGT_Helper.SetParent(starfieldGameObject, gameObject);
		SGT_Helper.SetLayer(starfieldGameObject, gameObject.layer);
		SGT_Helper.SetTag(starfieldGameObject, gameObject.tag);
		
		if (starfieldAutoRegen == true)
		{
			Regenerate();
		}
		
		UpdateMaterial();
		UpdateShader();
		UpdateBackground();
		
		starfieldMultiMesh.GameObject           = starfieldGameObject;
		starfieldMultiMesh.HasMeshRenderers     = true;
		starfieldMultiMesh.MeshRenderersEnabled = true;
		starfieldMultiMesh.SharedMaterial       = starfieldMaterial;
		starfieldMultiMesh.ReplaceAll(meshes);
		starfieldMultiMesh.Update();
		
#if UNITY_EDITOR == true
		starfieldMultiMesh.HideInEditor();
#endif
	}
	
	public void OnEnable()
	{
		if (starfieldMultiMesh != null) starfieldMultiMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (starfieldMultiMesh != null) starfieldMultiMesh.OnDisable();
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(starfieldGameObject);
		SGT_Helper.DestroyObject(starfieldMaterial);
		SGT_Helper.DestroyObjects(meshes);
	}
}