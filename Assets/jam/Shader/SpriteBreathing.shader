Shader "Custom/URP/SpriteBreathing"
{
    Properties
    {
        [MainTexture] _BaseMap ("Sprite", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1,1,1,1)

        _AmplitudeY ("Breath Amplitude Y", Range(0,0.1)) = 0.03
        _AmplitudeX ("Breath Amplitude X", Range(0,0.1)) = 0.02
        _Speed      ("Breath Speed", Range(0,5)) = 1.5
        _CenterY    ("Pivot Y (object space)", Float) = 0.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "Forward"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            // URP core
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            float4 _BaseMap_ST;
            float4 _BaseColor;

            float _AmplitudeY;
            float _AmplitudeX;
            float _Speed;
            float _CenterY;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                float t = _Time.y * _Speed;
                float s = sin(t);

                float scaleY = 1.0 + s * _AmplitudeY;
                float scaleX = 1.0 - s * _AmplitudeX;

                float3 pos = IN.positionOS.xyz;

                float pivot = _CenterY;
                float dy = pos.y - pivot;

                pos.y = pivot + dy * scaleY;
                pos.x = pos.x * scaleX;

                float4 posOS = float4(pos, 1.0);
                OUT.positionHCS = TransformObjectToHClip(posOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 c = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                return c;
            }
            ENDHLSL
        }
    }
}
