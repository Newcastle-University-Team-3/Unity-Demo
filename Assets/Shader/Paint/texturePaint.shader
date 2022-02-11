Shader "Custom/TexturePaint"
{
	Properties
	{
		_PaintColor("Paint Color",color) = (0,0,0,0)
	}
	SubShader
	{
		Cull off 
		ZWrite off
		ZTest off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _PaintColor;
			float3 _PaintPosition;
			float _Radius;
			float _Hardness;
			float _Strength;
			float _PrepareUV;


			struct MeshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolaters
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldPos:TEXCOORD1;  //Vertex Position in world space
			};

			float mask(float position,float3 center,float3 hardness,float3 radius)
			{
				//Current distance between DrawPosition and fragment position
				float m = distance(center, position);
				return 1 - smoothstep(radius * hardness, radius, m);

			}

			Interpolaters vert (MeshData v)
			{
				Interpolaters o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);    //Vertex Position in world space
				o.uv = v.uv;
				float4 uv = float4(0, 0, 0, 1);
				uv.xy = (v.uv.xy * float2(2,2) - float2(1,1) )*(1,_ProjectionParams.x);
				o.vertex = uv;
				return o;
			}
			

			float4 frag(Interpolaters i) : SV_Target
			{
				//Color of texture
				float4 col = tex2D(_MainTex,i.uv);

				//Mask
				float f = mask(i.worldPos, _PaintPosition, _Hardness, _Radius);
				float edge = f * _Strength;

				return lerp(col, _PaintColor, edge);
			}
			ENDCG
		}
	}
}
