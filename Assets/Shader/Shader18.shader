Shader "Unlit/Shader18"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} //The "white" is what gets assigned when there is nothing
        _Pattern ("Pattern", 2D) = "white" {} 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define TAU 6.283185307179586

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1; 
            };

            sampler2D _MainTex; 
            sampler2D _Pattern; 

            v2f vert (MeshData v)
            {
                v2f o;
                o.worldPos = mul(UNITY_MATRIX_M, v.vertex); 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float GetWave (float coord)
            {
                float wave = cos((coord - _Time.y * 0.1) * TAU * 5) *.5 - .5;
                wave *= 1 - coord;
                return wave;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 topDownProjection = i.worldPos.xz;
                float4 moss = tex2D(_MainTex, topDownProjection);
                float pattern = tex2D (_Pattern, i.uv);

                float4 finalColor = lerp(float4(1,0,0, 1), moss, pattern);

               //return pattern;
                return finalColor;
            }
            ENDCG
        }
    }
}


/*
Shader "Unlit/Shader18"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} //The "white" is what gets assigned when there is nothing
        _Pattern ("Pattern", 2D) = "white" {} 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define TAU 6.283185307179586

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1; 
            };

            sampler2D _MainTex; 
            sampler2D _Pattern; 

            v2f vert (MeshData v)
            {
                v2f o;
                o.worldPos = mul(UNITY_MATRIX_M, v.vertex); 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float GetWave (float coord)
            {
                float wave = cos((coord - _Time.y * 0.1) * TAU * 5) *.5 - .5;
                wave *= 1 - coord;
                return wave;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 topDownProjection = i.worldPos.xz;
                float4 moss = tex2D(_MainTex, topDownProjection);
                float pattern = tex2D (_MainTex, i.uv);

               //return pattern;
                return GetWave(i.uv);
            }
            ENDCG
        }
    }
}

*/