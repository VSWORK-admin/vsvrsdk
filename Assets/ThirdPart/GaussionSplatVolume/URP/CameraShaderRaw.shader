Shader"GaussianSplatting/URP/CameraShaderRaw"
{
    Properties
    {
        //_MainTex ("Main Texture", 2D) = "white" {}
        _GaussianSplattingTexLeftEye("Left Eye", 2D) = "white" {}
        _GaussianSplattingTexRightEye("Right Eye", 2D) = "white" {}
        _GaussianSplattingDepthTexLeftEye("Depth Left Eye", 2D) = "white" {}
        _GaussianSplattingDepthTexRightEye("Depth Right Eye", 2D) = "white" {}
        _Scale("Scale", float) = 1
    }
    SubShader
    {
ZTest Always

Blend OneMinusDstAlpha One

//Blend SrcAlpha DstAlpha

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
    //float4 screenPos : TEXCOORD1;
};

v2f vert(appdata v)
{
    v2f o = (v2f) 0;
    float3 positionWS = TransformObjectToWorld(v.vertex);
    o.vertex = TransformWorldToHClip(positionWS);
    o.uv = v.uv;
    //o.screenPos = ComputeScreenPos(o.vertex);
    return o;
}
    CBUFFER_START(UnityPerMaterial)
sampler2D _GaussianSplattingTexLeftEye;
sampler2D _GaussianSplattingTexRightEye;
sampler2D _GaussianSplattingDepthTexLeftEye;
sampler2D _GaussianSplattingDepthTexRightEye;
float _Scale;
CBUFFER_END
                TEXTURE2D(_CameraDepthTexture);SAMPLER(sampler_CameraDepthTexture);
                TEXTURE2D(_CameraColorTexture);SAMPLER(sampler_CameraColorTexture);
//TEXTURE2D_X(_CameraOpaqueTexture);SAMPLER(sampler_CameraOpaqueTexture);
                //TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
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
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
            
             //unity_StereoEyeIndex left = 0 right = 1
    float4 gcol;
    float4 gcol2;
    float gdepth;
    if (unity_StereoEyeIndex == 0)
    {
        gcol = tex2D(_GaussianSplattingTexLeftEye, i.uv);
        gcol2 = tex2D(_GaussianSplattingTexRightEye, i.uv);
        gdepth = tex2D(_GaussianSplattingDepthTexLeftEye, i.uv).r;
    }
    else
    {
        gcol = tex2D(_GaussianSplattingTexRightEye, i.uv);
        gdepth = tex2D(_GaussianSplattingDepthTexRightEye, i.uv).r;
    }

    gdepth *= _Scale;

    float4 cameraDepthTex = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv);
    float cam_depth = LinearEyeDepth(cameraDepthTex.x, _ZBufferParams);

    //float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
    float4 col = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, i.uv);
    //float4 col = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, i.uv);
    //return col;
    
    //return Gamma2Linear(float4(gcol2.rgb * (1 - gcol.a) + gcol.rgb * gcol.a, 1));
    
    if (gdepth < cam_depth)
    {
                        //Mix background color with gaussian splatting
        return Gamma2Linear(gcol);
          //return Gamma2Linear(float4(col.rgb * (1 - gcol.a) + gcol.rgb * gcol.a, 1));
    }
    else
    {
        return Gamma2Linear(col);
    }
}
            ENDHLSL
        }
    }
}
