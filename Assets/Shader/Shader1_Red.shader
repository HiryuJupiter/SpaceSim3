Shader "Unlit/Shader1_Red"
{
    Properties
    {
        //Created a float of name "Value" at 1.0 value.
        _Value ("Value", Float) = 1.0 

    }
    SubShader
    {
        //Tags can help to specify how something is rendered
        //Usually more render related
        Tags { "RenderType"="Opaque" } 

        //Pass is more graphic related for this specific pass
        Pass
        {
            CGPROGRAM
            #pragma vertex vert //We want the vertex shader to be the function called vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //This is automatically sent to mesh by unity
            struct MeshData //Always per vertex mesh data
            {
                float4 vertex : POSITION; //Vertex position
                float2 uv : TEXCOORD0; //You can use UV for anything, not just texture mappping.
            };
             
            //Interpolators. The data passed from vertex shader to
            //the fragment shader. It defines what data do you want to 
            //interpolate across the whole surface.
            struct v2f
            {
                float4 vertex : SV_POSITION; //Clip space pos of each vertex
                //float2 uv : TEXCOORD0;
            };

            v2f vert (MeshData v)
            {
                v2f o;
                //ObjectToClip - converts local space to clip space.
                //It is multiplying the vertex by the 
                //MVP matrix, the model, view, projection matrix.
                o.vertex = UnityObjectToClipPos(v.vertex);
                //Without it, the rendering doesn't move: o.vertex = v.vertex;
                //you can do that for post-processing shaders that don't need
                //any world positional matrix and just want to apply to the 
                //entire screen.
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return float4 (1, 0, 0, 1);
            }
            ENDCG
        }
    }
}
