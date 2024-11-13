Shader "GaussianSplatting/CameraShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _GaussianSplattingTexLeftEye("Left Eye", 2D) = "white" {}
        _GaussianSplattingTexRightEye("Right Eye", 2D) = "white" {}
        _GaussianSplattingDepthTexLeftEye("Depth Left Eye", 2D) = "white" {}
        _GaussianSplattingDepthTexRightEye("Depth Right Eye", 2D) = "white" {}
        _Scale("Scale", float) = 1
    }
    SubShader
    {
        ZTest Always

Blend SrcAlpha OneMinusSrcAlpha

        Cull Off
        ZWrite Off

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

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _GaussianSplattingTexLeftEye;
            sampler2D _GaussianSplattingTexRightEye;
            sampler2D _GaussianSplattingDepthTexLeftEye;
            sampler2D _GaussianSplattingDepthTexRightEye;
            uniform float _Scale;
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
            half4 _MainTex_ST;
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
                float gdepth;
                if (unity_StereoEyeIndex == 0) {
                    gcol = tex2D(_GaussianSplattingTexLeftEye, i.uv);
        
                    gdepth = tex2D(_GaussianSplattingDepthTexLeftEye, i.uv).r;
                } else {
                    gcol = tex2D(_GaussianSplattingTexRightEye, i.uv);
                    gdepth = tex2D(_GaussianSplattingDepthTexRightEye, i.uv).r;
                }

                gdepth *= _Scale;

                float cam_depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                cam_depth = LinearEyeDepth(cam_depth);

                float4 col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));

    ///return gcol;
    //return float4(col.rgb * (1 - gcol.a) + gcol.rgb * gcol.a, 1);
                if (gdepth < cam_depth) {
                    //Mix background color with gaussian splatting
                    //return Gamma2Linear(float4(col.rgb * (1 - gcol.a) + gcol.rgb * gcol.a, 1));
        return Gamma2Linear(gcol);
        //return Gamma2Linear(float4(res.rgb * (1 - gcol.a) + gcol.rgb * gcol.a, gcol2.a));
    } else {
                    return Gamma2Linear(col);
                }
}
            ENDCG
        }
    }
}
