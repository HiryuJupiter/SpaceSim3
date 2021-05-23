Shader "Unlit/Shader2_Magma"
{
    Properties
    {
        _Color ("Colorr", Color) = (1,1,1,1)

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

            struct MeshData 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0; 
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION; 
                float3 normal : TEXCOORD0; 
            };

            v2f vert (MeshData v)
            {
                v2f o; 
                o.vertex = UnityObjectToClipPos(v.vertex);
                //Just pass data from mesh, through the interpolator, to the frament shader.
                o.normal = v.normal; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target //i for input
            {
                return float4(i.normal, 1); 
            }
            ENDCG
        }
    }
}
