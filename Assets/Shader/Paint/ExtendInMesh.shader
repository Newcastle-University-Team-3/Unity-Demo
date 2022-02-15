Shader "Unlit/ExtendInMesh"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_UVIslands("Texture UVIsland",2D) = "white"{}
		_OffsetUV("UVOffset",float) = 1
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

			struct MeshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolaters
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;//纹理尺寸
			float _OffsetUV;
			sampler2D _UVIslands;	//展开后贴图的UV值

			Interpolaters vert (MeshData v)
			{
				Interpolaters o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(Interpolaters i) : SV_Target
			{
				//???
				float2 offsets[8] = {float2(-_OffsetUV, 0), float2(_OffsetUV, 0), float2(0, _OffsetUV), float2(0, -_OffsetUV), float2(-_OffsetUV, _OffsetUV), float2(_OffsetUV, _OffsetUV), float2(_OffsetUV, -_OffsetUV), float2(-_OffsetUV, -_OffsetUV)};
				float2 uv = i.uv;	//原始的uv值
				float4 color = tex2D(_MainTex, uv);		//根据原始贴图取得的颜色
				float4 island = tex2D(_UVIslands, uv);	//根据展开后的贴图取得uv值对应的贴图的颜色

				if (island.z <1)		// ??????
				{
					float4 extendColor = color;	//?????初始化？
					for(int i = 0; i<8;i++)
					{
						float2 currentUV = uv + offsets[i] * _MainTex_TexelSize.xy; 	//当前的uv坐标在原有的贴图上等比例的偏移一个范围取值
						float4 offsettedColor = tex2D(_MainTex, currentUV);		//偏移之后坐标在原有贴图上的取得的颜色?
						extendColor = max(offsettedColor, extendColor);
					}
					color = extendColor;		

				}

				return color;	//返回在展开后的贴图上取得的颜色数据
			}
			ENDCG
		}
	}
}
