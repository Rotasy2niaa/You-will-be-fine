Shader "Hidden/SimpleNormalOutline"
{
    Properties
    {
        _Color("Main Color",  Color) = (1,1,1,1)
        _OutlineWidth("Outline Width", Float) = 0.05
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        // ---------------- Pass 0: outline (extruded backfaces) ----------------
        Pass
        {
            Name "OUTLINE"
            Cull Front
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline
            #include "UnityCG.cginc"

            float4    _Color;
            float     _OutlineWidth;

            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vertOutline(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex + float4(normalize(v.normal) * _OutlineWidth, 0));
                return o;
            }

            fixed4 fragOutline(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}