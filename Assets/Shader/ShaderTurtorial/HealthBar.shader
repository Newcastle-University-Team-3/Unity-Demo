Shader "Custom/HealthBar"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Texture", 2D) = "black" {}
        _Health("Health",Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Trasparent" "Queue" = "Transparent"}

        Pass
        {
        	
        	ZWrite off
        	//src * X + dst * Y
        	Blend SrcAlpha OneMinusSrcAlpha
        	
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolater
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Health;

            Interpolater vert (MeshData v)
            {
                Interpolater o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv, _MainTex;
                return o;
            }

            float InversLerp(float a,float b,float v)
            {
                return (v - a) / (b - a);
            }

            float4 frag(Interpolater i) : SV_Target
            {
                float3 healthbarColor = tex2D( _MainTex, float2(_Health,i.uv.y));

                float flash = cos(_Time.y * 4) * 0.1 +1;
                float healthbarMask = _Health > i.uv.x;
                healthbarMask *= flash;


            	return float4(healthbarColor * healthbarMask,1);

            }
            ENDCG
        }
    }
}
