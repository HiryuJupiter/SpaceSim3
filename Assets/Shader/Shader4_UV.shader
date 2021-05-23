Shader "Unlit/Shader4_UV"
{
    Properties
    {
        _Color ("Colorr", Color) = (1,1,1,1)
        _Scale ("Scale", Float ) = 1
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

            float4 _Color;
            float _Scale;

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
                o.uv = v.uv0 ;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target 
            {
               // return float4(i.uv0, 0, 1);  
                return float4(i.uv.xxx, 1);  ///Splatter uv.x into RGB
            }
            ENDCG
        }
    }
}
