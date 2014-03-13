using MeshList = System.Collections.Generic.List<UnityEngine.Mesh>;

using UnityEngine;
using SGT_Internal;

public partial class SGT_Starfield
{
	private static SGT_WeightedRandom weights;
	
	public void Regenerate()
	{
		if (modified == true)
		{
			DestroyGeneratedMeshes();
			
			if (starVariants.Count > 0)
			{
				SGT_Helper.BeginRandomSeed(starfieldSeed);
				{
					RecalculateCoords();
					RecalculateWeights();
					
					var remainingStars = starfieldStarCount;
					var starsPerMesh   = SGT_Helper.MeshVertexLimit / 4;
					var newMeshes      = new MeshList();
					
					while (remainingStars > 0)
					{
						var starsInMesh = Mathf.Min(remainingStars, starsPerMesh);
						
						newMeshes.Add(GenerateStarMesh(starfieldStarCount - remainingStars, starsInMesh));
						
						remainingStars -= starsInMesh;
					}
					
					meshes = newMeshes.ToArray();
					
					if (starfieldMultiMesh != null)
					{
						starfieldMultiMesh.ReplaceAll(meshes);
						starfieldMultiMesh.Update();
					}
				}
				SGT_Helper.EndRandomSeed();
			}
			
			modified = false;
		}
	}
	
	private void DestroyGeneratedMeshes()
	{
		meshes = SGT_Helper.DestroyObjects(meshes);
	}
	
	private void RecalculateCoords()
	{
		SGT_ArrayHelper.Resize(ref starVariants, starfieldTextureTilesX * starfieldTextureTilesY, false);
		
		for (var i = 0; i < starVariants.Count; i++)
		{
			GetStarVariant(i).RecalculateCoords(starfieldTextureTilesX, starfieldTextureTilesY, i);
		}
	}
	
	private void RecalculateWeights()
	{
		weights = new SGT_WeightedRandom(100);
		
		for (var i = 0; i < StarVariantCount; i++)
		{
			var ssv = GetStarVariant(i);
			
			weights.Add(i, ssv != null ? ssv.SpawnProbability : 1.0f);
		}
	}
	
	private Mesh GenerateStarMesh(int starOffset, int starCount)
	{
		var positions = new Vector3[starCount * 4];
		var indices   = new int[starCount * 6];
		var uv0s      = new Vector2[starCount * 4];
		var uv1s      = new Vector2[starCount * 4];
		var normals   = new Vector3[starCount * 4];
		var colours   = new Color[starCount * 4];
		var bounds    = new Bounds();
		
		for (var i = 0; i < starCount; i++)
		{
			var i0 =  i * 6;
			var i1 = i0 + 1;
			var i2 = i1 + 1;
			var i3 = i2 + 1;
			var i4 = i3 + 1;
			var i5 = i4 + 1;
			
			var v0 =  i * 4;
			var v1 = v0 + 1;
			var v2 = v1 + 1;
			var v3 = v2 + 1;
			
			// Index data
			indices[i0] = v0;
			indices[i1] = v1;
			indices[i2] = v2;
			indices[i3] = v3;
			indices[i4] = v2;
			indices[i5] = v1;
			
			// Calculate star values
			var starData    = GenerateStar();
			var midRadius   = (starData.RadiusMin + starData.RadiusMax) * 0.5f;
			var pulseRadius = (starData.RadiusMax - starData.RadiusMin) * 0.5f;
			
			var position = starData.Position;
			var colour   = new Color(starData.RadiusPulseRate, starData.RadiusPulseOffset, 0.0f);
			var uv0      = starData.Variant.Coords;
			var uv1      = new Vector2(midRadius, pulseRadius);
			var right    = SGT_Helper.Rotate(Vector2.right * SGT_Helper.InscribedBox, starData.Angle);
			var up       = SGT_Helper.Rotate(Vector2.up    * SGT_Helper.InscribedBox, starData.Angle);
			
			bounds.Encapsulate(position);
			
			// Write star values into vertex data
			positions[v0] = position;
			positions[v1] = position;
			positions[v2] = position;
			positions[v3] = position;
			
			normals[v0] = SGT_Helper.NewVector3(-right + up, 0.0f);
			normals[v1] = SGT_Helper.NewVector3( right + up, 0.0f);
			normals[v2] = SGT_Helper.NewVector3(-right - up, 0.0f);
			normals[v3] = SGT_Helper.NewVector3( right - up, 0.0f);
			
			colours[v0] = colour;
			colours[v1] = colour;
			colours[v2] = colour;
			colours[v3] = colour;
			
			uv0s[v0] = uv0[0];
			uv0s[v1] = uv0[1];
			uv0s[v2] = uv0[2];
			uv0s[v3] = uv0[3];
			
			uv1s[v0] = uv1;
			uv1s[v1] = uv1;
			uv1s[v2] = uv1;
			uv1s[v3] = uv1;
		}
		
		bounds.Expand(starRadiusMax);
		
		var starMesh = new Mesh();
		
		starMesh.hideFlags = HideFlags.DontSave;
		starMesh.name      = "Starfield";
		starMesh.bounds    = bounds;
		starMesh.vertices  = positions;
		starMesh.normals   = normals;
		starMesh.colors    = colours;
		starMesh.uv        = uv0s;
		starMesh.uv1       = uv1s;
		starMesh.triangles = indices;
		
		return starMesh;
	}
	
	private SGT_StarfieldStarData GenerateStar()
	{
		var starData    = new SGT_StarfieldStarData();
		var starVariant = GetStarVariant(weights.RandomIndex);
		
		if (starVariant.Custom == true)
		{
			var baseRadius = Random.Range(starVariant.CustomRadiusMin, starVariant.CustomRadiusMax);
			
			starData.RadiusMin = Mathf.Max(baseRadius - starVariant.CustomPulseRadiusMax, starVariant.CustomRadiusMin);
			starData.RadiusMax = Mathf.Min(baseRadius + starVariant.CustomPulseRadiusMax, starVariant.CustomRadiusMax);
		}
		else
		{
			var baseRadius = Random.Range(starRadiusMin, starRadiusMax);
			
			starData.RadiusMin = Mathf.Max(baseRadius - starPulseRadiusMax, starRadiusMin);
			starData.RadiusMax = Mathf.Min(baseRadius + starPulseRadiusMax, starRadiusMax);
		}
		
		starData.Variant           = starVariant;
		starData.Position          = GenerateStarPosition();
		starData.RadiusPulseRate   = Random.Range(0.0f, 1.0f);
		starData.RadiusPulseOffset = Random.Range(0.0f, 1.0f);
		starData.Angle             = Random.Range(-Mathf.PI, Mathf.PI);
		
		return starData;
	}
	
	private Vector3 GenerateStarPosition()
	{
		var position = default(Vector3);
		
		switch (distribution)
		{
			case SGT_StarfieldDistribution.OnSphere:
			{
				position = Random.onUnitSphere;
				position.y *= distributionConstantA;
				position.Normalize();
			}
			break;
			
			case SGT_StarfieldDistribution.OnDome:
			{
				position = Random.onUnitSphere;
				position.y = Mathf.Abs(position.y);
			}
			break;
			
			case SGT_StarfieldDistribution.OnCircle:
			{
				var ang = Random.Range(-180.0f, 180.0f);
				
				position.x = Mathf.Sin(ang);
				position.z = Mathf.Cos(ang);
			}
			break;
			
			case SGT_StarfieldDistribution.OnCube:
			{
				var x = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
				var y = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
				var z = Random.value >= 0.5f ? -SGT_Helper.InscribedCube : SGT_Helper.InscribedCube;
				
				switch (Random.Range(0, 3))
				{
					case 0: position = new Vector3(z, x, y); break;
					case 1: position = new Vector3(x, z, y); break;
					case 2: position = new Vector3(x, y, z); break;
				}
			}
			break;
			
			case SGT_StarfieldDistribution.InSphere:
			{
				position = Random.insideUnitSphere;
				var magnitude = distributionConstantB + position.magnitude * (1.0f - distributionConstantB);
				position.y *= distributionConstantA;
				position = position.normalized * magnitude;
			}
			break;
			
			case SGT_StarfieldDistribution.EllipticalGalaxy:
			{
				position = Random.insideUnitSphere;
				var magnitude = position.magnitude;
				position.y *= distributionConstantA;
				position = position.normalized * (1.0f - magnitude);
			}
			break;
			
			case SGT_StarfieldDistribution.InDome:
			{
				position = Random.insideUnitSphere;
				position.y = Mathf.Abs(position.y);
			}
			break;
			
			case SGT_StarfieldDistribution.InCircle:
			{
				var o = Random.insideUnitCircle;
				
				position.x = o.x;
				position.z = o.y;
			}
			break;
			
			case SGT_StarfieldDistribution.InCube:
			{
				position.x = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
				position.y = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
				position.z = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
			}
			break;
		}
		
		return position * distributionRadius;
	}
}