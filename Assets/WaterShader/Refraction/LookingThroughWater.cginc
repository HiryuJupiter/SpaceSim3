#if !defined(LOOKING_THROUGH_WATER)
#define LOOKING_THROUGH_WATER

sampler2D _CameraDepthTexture, _WaterBackground;
float4 _CameraDepthTexture_TexelSize;
float3 _FogColour;
float _FogDepth;


float3 ColourBelowWater(float4 screenPos)
{
    float2 uv = screenPos.xy / screenPos.w;

    #if UNITY_UV_STARTS_AT_TOP
        if(_CameraDepthTexture_TexelSize.y < 0)
        {
            uv.y = 1 - uv.y;
        }
    #endif
    
    float backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
    float surfaceDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screenPos.z);
    float depthDistance = backgroundDepth - surfaceDepth;

    float3 backgroundColour = tex2D(_WaterBackground, uv).rgb;
    float fogFactor = exp2(-_FogDepth * depthDistance);
    return lerp(_FogColour, backgroundColour, fogFactor);
}
#endif