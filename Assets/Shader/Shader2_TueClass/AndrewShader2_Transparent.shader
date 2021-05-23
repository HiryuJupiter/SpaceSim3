Shader "Unlit/AndrewShader2_Transparent"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ColorA("ColorA", Color) = (1, 0, 0, 1)
		_ColorB("ColorB", Color) = (0, 0, 0.5, 1)
		_ColorStart("ColorStart", Range(0, 1)) = 0
		_ColorEnd("ColorEnd", Range(0, 1)) = 1
		_MoveSpeed("MoveSpeed", Range(0, 1)) = 1
		_OuterWaveFrequency("OuterWaveFrequency", Range(0, 100)) = 4.2
		_InnerWaveAmplitude("InnerWaveAmplitude", Range(0, 100)) = 0.6
		_InnerWaveFrequency("InnerWaveFrequency", Range(0, 100)) = 8
		//_RefrColor("Refraction color", Color) = (.34, .85, .92, 1)
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent"
				"Queue" = "Transparent"
		}
		//Tags { "RenderType" = "Opaque" }

		Pass
		{
			Cull Off
			ZWrite On
			Blend One One

			//https://docs.unity3d.com/Manual/SL-Blend.html
			//ZWrite Off
			//Blend One One
			//BlendOp Sub
			//Blend DstColor Zero
			//Blend SrcAlpha OneMinusSrcAlpha //Traditional transparency
			//Blend One OneMinusSrcAlpha //Premultiplied transp
			//Blend One One //Additive
			//Blend OneMinusDstColor One //Soft Additive
			//Blend DstColor Zero // Multiplicative
			//BlendOp RawSub
			//Blend DstColor SrcColor // 2x Multiplication
			//ZTest LEqual //some default
				
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normals : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f 
			{
				//These TEXCOORD do no have the same meaning as in meshdata. They are just slots here.
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0; 
				float3 normal : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float2 uv : TEXCOORD0;
			float3 normal : TEXCOORD1;

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
				o.normal = UnityObjectToWorldNormal(v.normals);
				o.uv = v.uv;
				return o;
			}

			float InverseLerp(float a, float b, float v)
			{
				return (v - a) / (b - a);
			}

			float4 frag(v2f i) : SV_Target
			{
				float xOffset = cos(i.uv.x * TAU * _InnerWaveFrequency) * _InnerWaveAmplitude;
				float t = (cos((i.uv.y * TAU * _OuterWaveFrequency + xOffset - _Time.y))* .5f + 0.5) ;

				t *= 1 - i.uv.y;
				float topBottomRemover = t * (abs(i.normal.y) < 0.999);

				float waves = t * topBottomRemover;
				float4 gradient = lerp(_ColorA, _ColorB, i.uv.y);

				return gradient * waves; 
			}
			ENDCG
		}
	}
}