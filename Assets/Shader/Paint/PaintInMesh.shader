Shader "Unlit/PaintInMesh"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color",color) = (0,0,0,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			 sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _PosW;	//从外面传入的选中的涂色位置

			struct MeshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolaters
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos:TEXCOORD1;
			};

			//Center is the position of mouse pointed
		   float mask(float3 position,float3 center,float radius,float hardness)
		   {
			//各个点到鼠标点击位置的距离
			   float m = distance(center, position);
			//生成涂上去的图案，中间一个图案，外圈都是黑色
		   	   return 1 - smoothstep(radius * hardness, radius, m);
		   }

			Interpolaters vert (MeshData v) 
			{
				Interpolaters o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);//将texture上的坐标转换为世界坐标
				o.uv = v.uv;
				o.vertex = v.vertex;
				return o;
			}

			fixed4 frag (Interpolaters i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float4 f = mask(i.worldPos, _PosW, 0.5f, 0.5f);
				return lerp(col,_Color,f);
			}
			ENDCG
		}
	}
}
