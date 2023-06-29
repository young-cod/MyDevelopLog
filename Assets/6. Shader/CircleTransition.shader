Shader "Custom/CircleTransition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color",Color) = (1,1,1,1)
        _Radius("CircleRadius",Range(0.0,1.0))=0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100

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

            float _Radius;
            fixed4 _Color;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
               o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                return o;
            }

            void DrawCircle(in float2 uv, in float2 center, in float radius, in float smoothValue, out float output)
            {
                float sqrDistance = pow(uv.x - center.x,2) + pow(uv.y - center.y,2);
                float sqrRadius = pow(radius,2);

                if(sqrDistance<radius)output = smoothstep(sqrRadius, sqrRadius-smoothValue,sqrDistance);
                else output = 0;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5,0.5);
                float smoothValue = 0.01;
                float outputAlpha = 0;
                DrawCircle(i.uv,center, _Radius, smoothValue, outputAlpha);

                fixed4 col = tex2D(_MainTex,i.uv);
                return col * fixed4(_Color.rgb,1-outputAlpha);
            }
            ENDCG
        }
    }
}
