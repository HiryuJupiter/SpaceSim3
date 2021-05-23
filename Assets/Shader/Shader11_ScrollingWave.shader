Shader "Unlit/Shader11_ScrollingWave"
{
    Properties
    {
        _Repeats ("Repeats", int) = 1
        _ColorA ("ColorA", Color) = (1,1,1,1)
        _ColorB ("ColorB", Color) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0, 1)) = 0
        _ColorEnd ("Color End", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert 
            #pragma fragment frag

            #define TAU 6.28318530718

            #include "UnityCG.cginc"

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;
            float _Repeats;
            //float _Offset;

            struct MeshData 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv0 : TEXCOORD0; 
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION; 
                float3 normal : TEXCOORD0; 
                //Receives the interpolated UV values
                // from the MeshData to the fragment shader
                float2 uv : TEXCOORD1; 
            };

            v2f vert (MeshData v)
            {
                v2f o; 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv0; //_Offset + v.uv0 * _Scale ;
                return o;
            }

            //Returns t based on a value between a and b;
            float InverseLerp (float a, float b, float v)
            {
                return (v-a) / (b-a);
            }

            fixed4 frag (v2f i) : SV_Target 
            {
               //float t = saturate( InverseLerp(_ColorStart, _ColorEnd, i.uv.x));
               //float t = abs(frac(i.uv.x * _Repeats)* 2 - 1);

                //This is the improved version:
                                //float xOffset = cos(i.uv.x * TAU * _InnerWaveFrequency) * _InnerWaveAmplitude;
                //float t = (cos((i.uv.y * TAU * _OuterWaveFrequency + xOffset - _Time.y)) * .5f + 0.5);

                float xOffset = cos((i.uv.x) * TAU * _Repeats) * .02 ;
                float t = cos((i.uv.y + xOffset - _Time.y) * TAU * _Repeats ) * .5 + 0.5 ;
                return lerp(_ColorA, _ColorB, t);

              //float4 outColor = lerp(_ColorA, _ColorB, t);
                //return outColor;
               //return float4(i.uv, 0, 1);  
               // return float4(i.uv.xxx, 1);  ///Splatter uv.x into RGB
            }
            ENDCG
        }
    }
}
