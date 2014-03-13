using UnityEngine;
using System.Collections.Generic;

public partial class SGT_Starfield
{
	/*[SerializeField]*/
	private bool modified = true;
	
	[SerializeField]
	private GameObject starfieldGameObject;
	
	[SerializeField]
	private SGT_MultiMesh starfieldMultiMesh;
	
	[SerializeField]
	private Texture2D starfieldTexture;
	
	[SerializeField]
	private int starfieldTextureTilesX = 1;
	
	[SerializeField]
	private int starfieldTextureTilesY = 1;
	
	/*[SerializeField]*/
	private Mesh[] meshes;
	
	[SerializeField]
	private Material starfieldMaterial;
	
	[SerializeField]
	private string starfieldTechnique;
	
	[SerializeField]
	private bool starfieldInBackground = true;
	
	[SerializeField]
	private int starfieldStarCount;
	
	[SerializeField]
	private int starfieldSeed;
	
	[SerializeField]
	private int starfieldRenderQueue = 1001;
	
	[SerializeField]
	private Camera starfieldCamera;
	
	[SerializeField]
	private SGT_StarfieldDistribution distribution = SGT_StarfieldDistribution.OnSphere;
	
	[SerializeField]
	private float distributionRadius = 100.0f;
	
	[SerializeField]
	private float distributionConstantA = 1.0f;
	
	[SerializeField]
	private float distributionConstantB = 0.0f;
	
	[SerializeField]
	private float starRadiusMin = 1.0f;
	
	[SerializeField]
	private float starRadiusMax = 3.0f;
	
	[SerializeField]
	private float starPulseRadiusMax = 1.0f;
	
	[SerializeField]
	private float starPulseRateMax = 1.0f;
	
	[SerializeField]
	private List<SGT_StarfieldStarVariant> starVariants = new List<SGT_StarfieldStarVariant>();
	
	[SerializeField]
	private bool starfieldAutoRegen = true;
	
	[SerializeField]
	private Quaternion starfieldCameraRotation;
	
	[SerializeField]
	private float starfieldCameraRoll;
	
	public bool Modified
	{
		set
		{
			modified = value;
		}
		
		get
		{
			return modified;
		}
	}
	
	public bool StarfieldAutoRegen
	{
		set
		{
			starfieldAutoRegen = value;
		}
		
		get
		{
			return starfieldAutoRegen;
		}
	}
	
	public bool StarfieldInBackground
	{
		set
		{
			if (value != starfieldInBackground)
			{
				starfieldInBackground = value;
				starfieldRenderQueue  = starfieldInBackground == true ? 1001 : 3000;
			}
		}
		
		get
		{
			return starfieldInBackground;
		}
	}
	
	public int StarfieldSeed
	{
		set
		{
			if (value != starfieldSeed)
			{
				starfieldSeed = value;
				modified      = true;
			}
		}
		
		get
		{
			return starfieldSeed;
		}
	}
	
	public int StarfieldStarCount
	{
		set
		{
			value = Mathf.Max(1, value);
			
			if (value != starfieldStarCount)
			{
				starfieldStarCount = value;
				modified           = true;
			}
		}
		
		get
		{
			return starfieldStarCount;
		}
	}
	
	public int StarfieldRenderQueue
	{
		set
		{
			if (value != starfieldRenderQueue)
			{
				starfieldRenderQueue = value;
			}
		}
		
		get
		{
			return starfieldRenderQueue;
		}
	}
	
	public Camera StarfieldCamera
	{
		set
		{
			if (value != starfieldCamera)
			{
				starfieldCamera = value;
				
				if (starfieldCamera != null)
				{
					starfieldCameraRotation = starfieldCamera.transform.rotation;
				}
			}
		}
		
		get
		{
			return starfieldCamera;
		}
	}
	
	public SGT_StarfieldDistribution Distribution
	{
		set
		{
			if (value != distribution)
			{
				distribution = value;
				modified     = true;
				
				// Reset constants
				switch (distribution)
				{
					case SGT_StarfieldDistribution.OnSphere:         distributionConstantA = 1.0f; break;
					case SGT_StarfieldDistribution.InSphere:         distributionConstantA = 1.0f; distributionConstantB = 0.0f; break;
					case SGT_StarfieldDistribution.EllipticalGalaxy: distributionConstantA = 1.0f; break;
				}
			}
		}
		
		get
		{
			return distribution;
		}
	}
	
	public float DistributionRadius
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != distributionRadius)
			{
				distributionRadius = value;
				modified           = true;
			}
		}
		
		get
		{
			return distributionRadius;
		}
	}
	
	public float DistributionSymmetry
	{
		set
		{
			value = Mathf.Clamp01(value);
			
			if (value != distributionConstantA)
			{
				distributionConstantA = value;
				modified              = true;
			}
		}
		
		get
		{
			return distributionConstantA;
		}
	}
	
	public float DistributionOuter
	{
		set
		{
			value = Mathf.Clamp01(value);
			
			if (value != distributionConstantB)
			{
				distributionConstantB = value;
				modified              = true;
			}
		}
		
		get
		{
			return distributionConstantB;
		}
	}
	
	public Texture StarfieldTexture
	{
		set
		{
			starfieldTexture = value as Texture2D;
		}
		
		get
		{
			return starfieldTexture;
		}
	}
	
	public int StarfieldTextureTilesX
	{
		set
		{
			value = Mathf.Max(value, 1);
			
			if (value != starfieldTextureTilesX)
			{
				starfieldTextureTilesX = value;
				modified               = true;
			}
		}
		
		get
		{
			return starfieldTextureTilesX;
		}
	}
	
	public int StarfieldTextureTilesY
	{
		set
		{
			value = Mathf.Max(value, 1);
			
			if (value != starfieldTextureTilesY)
			{
				starfieldTextureTilesY = value;
				modified               = true;
			}
		}
		
		get
		{
			return starfieldTextureTilesY;
		}
	}
	
	public float StarRadiusMin
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != starRadiusMin)
			{
				starRadiusMin = value;
				modified      = true;
			}
		}
		
		get
		{
			return starRadiusMin;
		}
	}
	
	public float StarRadiusMax
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != starRadiusMax)
			{
				starRadiusMax = value;
				modified      = true;
			}
		}
		
		get
		{
			return starRadiusMax;
		}
	}
	
	public float StarPulseRadiusMax
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != starPulseRadiusMax)
			{
				starPulseRadiusMax = value;
				modified           = true;
			}
		}
		
		get
		{
			return starPulseRadiusMax;
		}
	}
	
	public float StarPulseRateMax
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != starPulseRateMax)
			{
				starPulseRateMax = value;
				modified         = true;
			}
		}
		
		get
		{
			return starPulseRateMax;
		}
	}
	
	public int StarVariantCount
	{
		get
		{
			return starfieldTextureTilesX * starfieldTextureTilesY;
		}
	}
	
	public override void BuildUndoTargets(List<Object> list)
	{
		base.BuildUndoTargets(list);
		
		starfieldMultiMesh.BuildUndoTargets(list);
		
		list.Add(starfieldGameObject);
	}
}