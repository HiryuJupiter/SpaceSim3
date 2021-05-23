Shader "Unlit/Shader17_WorldSpaceProjection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} //There are 2D, 3D, cubemaps types of textures
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1; //For mapping texture to worldspace.
            };

            sampler2D _MainTex; 
            float4 _MainTex_ST; 

            v2f vert (appdata v)
            {
                v2f o;
                //We are transforming v.vertex from local space to world space
                //...by multiplying it by the model matrix.
                //UNITY_MATRIX_M and unity_ObjectToWorld are the same thing
                //The 4th value has an effect on the matrix multiplication. If
                //.. it is a 1, it will transpose it into a position, taking into account
                //.. offsets. If it's a 0, it will transpose it as a direction, not taking 
                //.. into account of offset.
                o.worldPos = mul(UNITY_MATRIX_M, float4(v.vertex.xyz, 1));  //Obj to world
                //o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
                o.uv.x += _Time.y * 0.1;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //We want to map the texture using world space coordinates instead of uv space coordinates
                float2 topDownProjection = i.worldPos.xz;

                //return float4(topDownProjection, 0, 1) ;

                float4 col = tex2D(_MainTex,float4(topDownProjection, 0, 1));
                return col;
            }
            ENDCG
        }
    }
}
