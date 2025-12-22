Shader "Hidden/Texture Mixer Image Effect Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    CGINCLUDE
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
        };

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }

        // The texture that will be mixed in.
        sampler2D _MixTexture;

        // The t-value used for mixing.
        fixed _MixT;

    ENDCG

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                // Base color.
                fixed4 col = tex2D(_MainTex, i.uv);
               
                // Mix texture olor.
                fixed4 mixTexCol = tex2D(_MixTexture, i.uv);

                // The new color.
                fixed4 newCol = lerp(col, mixTexCol, _MixT);

                // Lerp works with individual components and entire colors.
                // newCol.r = lerp(col.r, mixTexCol.r, _MixT);
                // newCol.g = lerp(col.g, mixTexCol.g, _MixT);
                // newCol.b = lerp(col.b, mixTexCol.b, _MixT);
                // newCol.a = lerp(col.a, mixTexCol.a, _MixT);

                // Returns the new color.
                return newCol;
            }
            ENDCG
        }
    }
}