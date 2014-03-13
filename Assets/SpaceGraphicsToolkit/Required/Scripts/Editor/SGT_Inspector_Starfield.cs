using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Starfield))]
public class SGT_Inspector_Starfield : SGT_Inspector<SGT_Starfield>
{
	private bool editStar;
	private bool editVariant;
	private int  editStarIndex;
	private int  editVariantIndex;
	
	private SGT_StarfieldStarData ssd;
	
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Starfield");
		{
			Target.StarfieldStarCount     = SGT_EditorGUI.IntField("Star Count", "Amount of stars to generate in the starfield.", Target.StarfieldStarCount);
			Target.StarfieldSeed          = SGT_EditorGUI.SeedField("Seed", "The random seed to use when generating the starfield.", Target.StarfieldSeed);
			Target.StarfieldTexture       = SGT_EditorGUI.ObjectField("Texture", "The tilesheet used by the starfield.", Target.StarfieldTexture, true);
			SGT_EditorGUI.BeginIndent();
			{
				Target.StarfieldTextureTilesX = SGT_EditorGUI.IntField("Tiles X", "The amount of horizontal tiles in the tilesheet.", Target.StarfieldTextureTilesX);
				Target.StarfieldTextureTilesY = SGT_EditorGUI.IntField("Tiles Y", "The amount of vertical tiles in the tilesheet.", Target.StarfieldTextureTilesY);
			}
			SGT_EditorGUI.EndIndent();
			Target.StarfieldRenderQueue   = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the starfield mesh.", Target.StarfieldRenderQueue);
			Target.StarfieldCamera        = SGT_EditorGUI.ObjectField("Camera", "The camera rendering this.", Target.StarfieldCamera, true);
			Target.StarfieldInBackground  = SGT_EditorGUI.BoolField("In Background", "Push stars into background?", Target.StarfieldInBackground);
			Target.StarfieldAutoRegen     = SGT_EditorGUI.BoolField("Auto Regen", null, Target.StarfieldAutoRegen);
			
			if (Target.StarfieldAutoRegen == false)
			{
				SGT_EditorGUI.BeginFrozen(Target.Modified == true);
				{
					if (SGT_EditorGUI.Button("Regenerate") == true)
					{
						Target.Regenerate();
					}
				}
				SGT_EditorGUI.EndFrozen();
			}
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.MarkNextFieldAsBold();
		Target.Distribution = (SGT_StarfieldDistribution)SGT_EditorGUI.EnumField("Distribution", "The star placement distribution.", Target.Distribution);
		SGT_EditorGUI.BeginIndent();
		{
			Target.DistributionRadius = SGT_EditorGUI.FloatField("Radius", "The size of the starfield across each axis.", Target.DistributionRadius);
			
			switch (Target.Distribution)
			{
				case SGT_StarfieldDistribution.InSphere:
				{
					Target.DistributionOuter = SGT_EditorGUI.FloatField("Outer", "The minimum radius for stars in the starfield.", Target.DistributionOuter, 0.0f, 1.0f);
				}
				goto case SGT_StarfieldDistribution.OnSphere;
				case SGT_StarfieldDistribution.EllipticalGalaxy:
				{
				}
				goto case SGT_StarfieldDistribution.OnSphere;
				case SGT_StarfieldDistribution.OnSphere:
				{
					Target.DistributionSymmetry = SGT_EditorGUI.FloatField("Symmetry", "A lower symmetry value will place more stars near the horizon/equator of the skysphere than at the poles.", Target.DistributionSymmetry, 0.0f, 1.0f);
				}
				break;
			}
		}
		SGT_EditorGUI.EndIndent();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Star");
		{
			Target.StarRadiusMin      = SGT_EditorGUI.FloatField("Radius Min", "The minimum radius of a star.", Target.StarRadiusMin);
			Target.StarRadiusMax      = SGT_EditorGUI.FloatField("Radius Max", "The maximum radius of a star.", Target.StarRadiusMax);
			Target.StarPulseRadiusMax = SGT_EditorGUI.FloatField("Pulse Radius Max", "The maximum amount a star's radius can change while pulsing (note: the final radius will always fall between the Star Radius Min/Max).", Target.StarPulseRadiusMax);
			Target.StarPulseRateMax   = SGT_EditorGUI.FloatField("Pulse Rate Max", "The maximum rate (speed) at which the stars can pulse.", Target.StarPulseRateMax);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		editVariant = SGT_EditorGUI.BeginToggleGroup("Edit Star Variant", null, editVariant);
		{
			SGT_EditorGUI.MarkNextFieldAsBold();
			editVariantIndex = SGT_EditorGUI.IntField("Index", "The star variant currently being edited.", editVariantIndex, 0, Target.StarVariantCount - 1);
			
			var ssv = Target.GetStarVariant(editVariantIndex);
			
			SGT_EditorGUI.BeginIndent();
			{
				SGT_EditorGUI.DrawFieldTexture("Preview", null, Target.StarfieldTexture, ssv.Uv);
				
				ssv.SpawnProbability = SGT_EditorGUI.FloatField("Spawn Probability", null, ssv.SpawnProbability, 0.0f, 1.0f);
				
				if (ssv.Custom == true)
				{
					SGT_EditorGUI.MarkNextFieldAsBold();
				}
				
				var oldCustom = ssv.Custom;
				
				ssv.Custom = SGT_EditorGUI.BoolField("Custom", null, ssv.Custom);
				
				if (ssv.Custom == true)
				{
					if (oldCustom == false)
					{
						ssv.CustomRadiusMin      = Target.StarRadiusMin;
						ssv.CustomRadiusMax      = Target.StarRadiusMax;
						ssv.CustomPulseRadiusMax = Target.StarPulseRadiusMax;
						ssv.CustomPulseRateMax   = Target.StarPulseRateMax;
					}
					
					SGT_EditorGUI.BeginIndent();
					{
						ssv.CustomRadiusMin      = SGT_EditorGUI.FloatField("Radius Min", null, ssv.CustomRadiusMin);
						ssv.CustomRadiusMax      = SGT_EditorGUI.FloatField("Radius Max", null, ssv.CustomRadiusMax);
						ssv.CustomPulseRadiusMax = SGT_EditorGUI.FloatField("Pulse Radius Max", null, ssv.CustomPulseRadiusMax);
						ssv.CustomPulseRateMax   = SGT_EditorGUI.FloatField("Pulse Rate Max", null, ssv.CustomPulseRateMax);
					}
					SGT_EditorGUI.EndIndent();
				}
			}
			SGT_EditorGUI.EndIndent();
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
}