Shader "Custom/FogOfWarImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogOfWarTex ("Fog of War Texture", 2D) = "white" {}
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 fogOfWarUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _FogOfWarTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = v.vertex;
                o.uv = v.uv;
                o.fogOfWarUV = v.uv * _MainTex_ST.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Get fog value from Fog of War texture
                float fogValue = tex2D(_FogOfWarTex, i.fogOfWarUV).r;

                // Apply fog value to color
                col.rgb *= fogValue;

                return col;
            }
            ENDCG
        }
    }
}
