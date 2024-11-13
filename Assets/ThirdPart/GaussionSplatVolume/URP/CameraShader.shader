Shader"GaussianSplatting/URP/CameraShader"
{
    Properties
    {
        _GaussianSplattingTexLeftEye("Left Eye", 2D) = "white" {}
    }
    SubShader
    {
ZTest Always

Blend SrcAlpha OneMinusSrcAlpha

        Cull
Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Pragmas
            #pragma target 2.0
            
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

    struct appdata
    {
        float3 vertex : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        float2 uv : TEXCOORD0;
};

    v2f vert(appdata v)
    {
    v2f o = (v2f) 0;
    float3 positionWS = TransformObjectToWorld(v.vertex);
    o.vertex = TransformWorldToHClip(positionWS);
    o.uv = v.uv;
        return o;
    }
    CBUFFER_START(UnityPerMaterial)
    sampler2D _GaussianSplattingTexLeftEye;
CBUFFER_END
    float4 Gamma2Linear(float4 c)
    {
        return pow(abs(c), 2.2);
    }

    float4 Linear2Gamma(float4 c)
    {
        return pow(abs(c), 1.0 / 2.2);
    }
    float4 frag(v2f i) : SV_Target
    { 
        float4 gcol = tex2D(_GaussianSplattingTexLeftEye, i.uv);
        return Gamma2Linear(gcol);
    }
            ENDHLSL
        }
    }
}
