Shader "Unlit/Shader15_VertexFlagWave"
{
    Properties
    {
        _Repeats ("Repeats", int) = 1
        _ColorA ("ColorA", Color) = (1,1,1,1)
        _ColorB ("ColorB", Color) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0, 1)) = 0
        _ColorEnd ("Color End", Range(0, 1)) = 1
        _WaveAmp ("Wave Amplitude", Range(0, 0.2)) = .1
        _WaveSpeed ("Wave Speed", Range(0, 1)) = .1
    }
    SubShader
    {
        Tags { 
            //"RenderType"="Opaque" 
            "RenderType"="Transparent" 
            "Queue" = "Transparent"
        } 

        Pass
        {
            //ZWrite On
            Blend One One  //We want additive blend mode: (src * 1 + dst * 1)

            CGPROGRAM
            #pragma vertex vert 
            #pragma fragment frag

            #define TAU 6.28318530718
            #define PI 3.1415926535

            #include "UnityCG.cginc"
              
            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;
            float _Repeats;
            float _WaveAmp;
            float _WaveSpeed;
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

                float movingWave = sin(((v.uv0.y + v.uv0.x) - _Time.y * _WaveSpeed) * PI * _Repeats);
                //float movingWave = sin(((v.uv0.y * v.uv0.x) - _Time.y * _WaveSpeed) * PI * _Repeats);
                //float movingWave = cos((v.uv0.y - _Time.y * .1) * TAU * _Repeats);
                v.vertex.y = movingWave * _WaveAmp;

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
                //float offset = cos((i.uv.x) * TAU * _Repeats) * 0.02;
                //float movingWave = cos((i.uv.y + offset - _Time.y) * TAU * _Repeats) * 0.4 + 0.6;
                //float brightGradientWave = movingWave * (1 - i.uv.y);
                //float noTopAndBottom = brightGradientWave * (abs(i.normal.y) < 0.999);
               return 0.5;
                
                //return sin(i.uv.x * PI) * sin(i.uv.y * PI);  
            }
            ENDCG
        }
    }
}
