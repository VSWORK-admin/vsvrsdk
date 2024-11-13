Shader"GaussianSplatting/URP/GaussionSplatCameraDepth"
{
    Properties 
    {        
        //_MainTex("MainTex",2D) = "white"{}
        _Scale("Scale", float) = 1
    }
    SubShader
    {
        //Blend One One
ZWrite Off
        Pass
        {
Name"Unlit"
          
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Pragmas
            #pragma target 2.0
            
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

            CBUFFER_START(UnityPerMaterial)
float _Scale;
CBUFFER_END

            
            //纹理的定义，如果是编译到GLES2.0平台，则相当于sample2D _MainTex;否则相当于 Texture2D _MainTex;
            //TEXTURE2D(_MainTex);SAMPLER(SamplerState_linear_mirrorU_ClampV);
//float4 _MainTex_ST;
            TEXTURE2D(_CameraDepthTexture);SAMPLER(sampler_CameraDepthTexture);
            
            //struct appdata
            //顶点着色器的输入
struct Attributes
{
    float3 positionOS : POSITION;
    float2 uv : TEXCOORD0;
};
            //struct v2f
            //片元着色器的输入
struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 screenPos : TEXCOORD1;
};
            //v2f vert(Attributes v)
            //顶点着色器
Varyings vert(Attributes v)
{
    Varyings o = (Varyings) 0;
    float3 positionWS = TransformObjectToWorld(v.positionOS);
    o.positionCS = TransformWorldToHClip(positionWS);
    o.uv = v.uv;
    o.screenPos = ComputeScreenPos(o.positionCS);
    return o;
}
            //fixed4 frag(v2f i) : SV_TARGET
            //片元着色器
float4 frag(Varyings i) : SV_TARGET
{
    //half4 c;
                
    //float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, SamplerState_linear_mirrorU_ClampV, i.uv);
                //c = _Color *  mainTex;
                //深度图
                //float2 uv = i.screenPos.xy / i.screenPos.w;
    //float2 uv = i.positionCS / _ScreenParams.xy;
    float4 cameraDepthTex = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv);
   // float depthTex = Linear01Depth(cameraDepthTex, _ZBufferParams);
    float depth = LinearEyeDepth(cameraDepthTex.x, _ZBufferParams);
    depth /= _Scale;
    return depth;
}
            ENDHLSL
        }
    }

FallBack"Hidden/Shader Graph/FallbackError"
}
