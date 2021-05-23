Shader "Unlit/Shader3_MagmaFixed"
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
                //Here the TextCord is referring to UV data, but after passing into
                //interpolator, it is just a data structure that can be used for 
                //anything. It's just a channel.
                float3 normal : TEXCOORD0; 
            };

            v2f vert (MeshData v)
            {
                v2f o; 
                o.vertex = UnityObjectToClipPos(v.vertex);
                //Just pass data from mesh, through the interpolator, to the frament shader.
                o.normal = v.normal; 
                //Also the normals are in local space, not world space
                //To convert it, you can do it both in the vertex shader or the fragment shader.
                o.normal = UnityObjectToWorldNormal(v.normal);

                //Since usually you have more pixels than vertex, you wnt to do as much calculations
                //in the vertex shader and as little as possible in the fragment shader.

                return o;
            }

            fixed4 frag (v2f i) : SV_Target //i for input
            {
                //Showing the direction of the normal for every pixel
                return float4(i.normal, 1);  
            }
            ENDCG
        }
    }
}
