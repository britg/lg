using MeshList    = System.Collections.Generic.List<UnityEngine.Mesh>;

using UnityEngine;
using SGT_Internal;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Starfield")]
public partial class SGT_Starfield : SGT_MonoBehaviourUnique<SGT_Starfield>
{
	public Mesh GetMesh(int index)
	{
		return SGT_ArrayHelper.Index(meshes, index);
	}
	
	public SGT_StarfieldStarVariant GetStarVariant(int index)
	{
		SGT_ArrayHelper.Resize(ref starVariants, starfieldTextureTilesX * starfieldTextureTilesY, false);
		
		var starVariant = starVariants[index];
		
		if (starVariant == null)
		{
			starVariant = starVariants[index] = new SGT_StarfieldStarVariant();
		}
		
		starVariant.Parent = this;
		
		return starVariant;
	}
	
	private void UpdateMaterial()
	{
		var targetStarfieldTechnique = "Variant";
		
		if (starfieldMaterial != null && starfieldTechnique != targetStarfieldTechnique) starfieldMaterial = SGT_Helper.DestroyObject(starfieldMaterial);
		
		// New material?
		if (starfieldMaterial == null)
		{
			starfieldTechnique = targetStarfieldTechnique;
			starfieldMaterial  = SGT_Helper.CreateMaterial("Hidden/SGT/Starfield/" + starfieldTechnique, starfieldRenderQueue);
		}
		else
		{
			SGT_Helper.SetRenderQueue(starfieldMaterial, starfieldRenderQueue);
		}
	}
	
	private void UpdateShader()
	{
		var oldCameraRotation   = starfieldCameraRotation; if (starfieldCamera != null) starfieldCameraRotation = starfieldCamera.transform.rotation;
		var cameraRotationDelta = Quaternion.Inverse(oldCameraRotation) * starfieldCameraRotation;
		
		starfieldCameraRoll = SGT_Helper.Wrap(starfieldCameraRoll - cameraRotationDelta.eulerAngles.z, 0.0f, 360.0f);
		
		var roll = Quaternion.Euler(new Vector3(0.0f, 0.0f, starfieldCameraRoll));
		
		starfieldMaterial.SetTexture("starTexture", starfieldTexture);
		starfieldMaterial.SetFloat("starPulseRateMax", starPulseRateMax);
		starfieldMaterial.SetMatrix("cameraRoll", SGT_MatrixHelper.Rotation(roll));
	}
	
	private void UpdateBackground()
	{
		if (starfieldGameObject != null)
		{
			if (starfieldInBackground == true)
			{
				if (starfieldCamera != null)
				{
					// Stretch to camera's far view frustum
					if (distributionRadius != 0.0f)
					{
						var scale = SGT_Helper.NewVector3(starfieldCamera.farClipPlane / distributionRadius) * 0.9f;
						
						SGT_Helper.SetLocalScale(starfieldGameObject.transform, scale);
					}
					
					// Centre to main camera
					SGT_Helper.SetPosition(starfieldGameObject.transform, starfieldCamera.transform.position);
				}
			}
			else
			{
				SGT_Helper.SetLocalPosition(starfieldGameObject.transform, Vector3.zero);
				SGT_Helper.SetLocalScale(starfieldGameObject.transform, Vector3.one);
			}
		}
	}
}