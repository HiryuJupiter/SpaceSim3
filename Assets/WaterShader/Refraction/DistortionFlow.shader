Shader "AndrewStuff/DistortionFlow"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

        [NoScaleOffset] _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [NoScaleOffset] _FlowMap ("Flow (RG, A noise)", 2D) = "black" {}
        [NoScaleOffset] _DerivHeightMap ("Deriv (AG) Height (B)", 2D) = "black" {}
        
        _UJump ("U jump per phase", Range(-0.25, 0.25)) = 0.25
        _VJump ("V jump per phase", Range(-0.25, 0.25)) = 0.25

        _Tiling ("Tiling", Float) = 1
        _Speed ("Speed", Float) = 1
        _Strength ("Distortion Strength", Range(0, 0.25)) = 0
        _FlowOffset ("Offset", Float) = 0
        _HeightScale ("Height Scale (constant)", Float) = 0.5
        _HeightModulated ("Height Scale (modular)", Float) = 0.25
        
        _FogColour ("Fog Colour", Color) = (0,0,0,0)
        _FogDepth ("Fog Depth", Float) = 1
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 200
        
        GrabPass {"_WaterBackground"}

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha finalcolor:ResetAlpha
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "Flow.cginc"
        #include "LookingThroughWater.cginc"

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        half _Glossiness, _Metallic;
        fixed4 _Color;
        sampler2D _MainTex, _FlowMap, _DerivHeightMap; //, _NormalMap
        float _UJump, _VJump, _Tiling, _FlowOffset, _Strength, _Speed, _HeightScale, _HeightModulated;

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void ResetAlpha(Input IN,SurfaceOutputStandard o, inout fixed4 colour)
        {
            colour.a = 1;
        }

        float3 UnpackDerivativeHeight(float4 textureData)
        {
            float3 dh = textureData.agb;
            dh.xy = dh.xy * 2 - 1;
            
            return dh;
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 flowVector = tex2D(_FlowMap, IN.uv_MainTex).rgb;
            flowVector.xy = flowVector.xy * 2 - 1;
            flowVector *= _Strength;
            float noise = tex2D(_FlowMap, IN.uv_MainTex).a;
            float time = _Time.y * _Speed + noise;

            float2 jump = float2(_UJump , _VJump);
            float3 uvwA = FlowUVW(IN.uv_MainTex, flowVector.xy, jump, _FlowOffset, _Tiling, time, false);
            float3 uvwB = FlowUVW(IN.uv_MainTex, flowVector.xy, jump, _FlowOffset, _Tiling, time, true);
            
            float localHeightScale = flowVector.z * _HeightModulated + _HeightScale;
            
            float3 dhA = UnpackDerivativeHeight(tex2D(_DerivHeightMap, uvwA.xy)) * uvwA.z * localHeightScale;
            float3 dhB = UnpackDerivativeHeight(tex2D(_DerivHeightMap, uvwB.xy)) * uvwB.z * localHeightScale;
            float3 heightMap = float3(-(dhA.xy + dhB.xy), 1);
            o.Normal = normalize(heightMap);
            
            fixed4 texA = tex2D (_MainTex, uvwA.xy) * uvwA.z;
            fixed4 texB = tex2D (_MainTex, uvwB.xy) * uvwB.z;
            fixed4 c = (texA + texB) * _Color;
            
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            
            o.Emission *= ColourBelowWater(IN.screenPos) * (1 - c.a);
            
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

        }
        ENDCG
    }
}