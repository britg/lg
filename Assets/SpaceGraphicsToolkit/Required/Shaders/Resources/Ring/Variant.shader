Shader "Hidden/SGT/Ring/Variant"
{
	Properties
	{
		ringTexture("ringTexture",  2D) = "white" {}
		position("position", Vector) = (0.0, 0.0, 0.0, 0.0)
		ringRadius("ringRadius", Float) = 0.0
		ringThickness("ringThickness", Float) = 0.0
	}
	
	Category
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		ZTest LEqual
		
		SubShader
		{
			Pass
			{
				CGPROGRAM
				
				#pragma vertex   Vert
				#pragma fragment Frag
				
				
				#include "../../CGInclude/Ring.cginc"
				
				ENDCG
			}
		}
	}
}