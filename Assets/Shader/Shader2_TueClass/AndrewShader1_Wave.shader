Shader "Unlit/AndrewShader1"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ColorA("ColorA", Color) = (1, 1, 1, 1)
		_ColorB("ColorB", Color) = (1, 1, 1, 1)
		_ColorStart("ColorStart", Range(0, 1)) = 0
		_ColorEnd("ColorEnd", Range(0, 1)) = 1
		_MoveSpeed("MoveSpeed", Range(0, 1)) = 1
		_OuterWaveFrequency("OuterWaveFrequency", Range(0, 100)) = 10
		_InnerWaveAmplitude("InnerWaveAmplitude", Range(0, 100)) = 1
		_InnerWaveFrequency("InnerWaveFrequency", Range(0, 100)) = 1
	//_RefrColor("Refraction color", Color) = (.34, .85, .92, 1)
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		//Tags { "RenderType" = "Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			#define TAU 6.283185307179586

			float4 _ColorA;
			float4 _ColorB;
			float _ColorStart;
			float _ColorEnd;
			float _MoveSpeed;
			float _OuterWaveFrequency;
			float _InnerWaveAmplitude;
			float _InnerWaveFrequency;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float InverseLerp(float a, float b, float v)
			{
				return (v - a) / (b - a);
			}

			float4 frag(v2f i) : SV_Target
			{
				//float xOffset = i.uv.y;
				//Original problematic problem
				/*float xOffset = cos(i.uv.x * TAU * 8) * 0.01;
				float t = cos((i.uv.y * xOffset * 0.1) * TAU * 5) * 0.5f + 0.5;*/

				float xOffset = cos(i.uv.x * TAU * _InnerWaveFrequency) * _InnerWaveAmplitude;
				float t = (cos((i.uv.y * TAU * _OuterWaveFrequency + xOffset - _Time.y))* .5f + 0.5) ;
				//float xOffset = cos(i.uv.x * TAU) * .02;
				//float t = cos((i.uv.y + xOffset - _Time.y) * TAU) * .5 + 0.5;

				//return t;

				//T *= 1 - I.UV.Y;

				//float t = cos(i.uv.x * TAU * _Time.y) * 8.5f + 8.5f;
				//float t = abs(frac(i.uv.x * 5) * 2 - 1);

				//float4 col - float4(i.uv.xxx, i);
				float4 outColor = lerp(_ColorA, _ColorB, t);
				return outColor;			
			}
			ENDCG
		}
	}
}