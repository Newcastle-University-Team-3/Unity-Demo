Shader "Custom/LightShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Gloss ("Gloss",Range(0,1))= 1
		_Color("Color",color) = (0,0,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct MeshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal:NORMAL;
			};

			struct Interpolaters
			{
				float2 uv : TEXCOORD0;
				float3 normal:TEXCOORD1;
				float4 vertex : SV_POSITION;
				float3 wPos:TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Gloss;
			float4 _Color;

			Interpolaters vert (MeshData v)
			{
				Interpolaters o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal =UnityObjectToWorldNormal(v.normal) ;
				o.wPos = mul(unity_ObjectToWorld, v.vertex);	//World Position
				return o;
			}

			float4 frag(Interpolaters i) : SV_Target
			{
				//diffuse light
				float3 N = normalize(i.normal);
				float3 L = _WorldSpaceLightPos0.xyz;	//Direction
				float3 lambert = saturate(dot(N, L));
				float3 diffuseLight = lambert * _LightColor0.xyz ;

				//specular light
				float3 V = normalize(_WorldSpaceCameraPos - i.wPos);	//View Vector -normalized as direction
				//float3 R = reflect(-L, N);		//Reflect light direction around normal //Phong
				float3 H =normalize(L + V);
				float3 specularLight = saturate(dot(H, N)) * (lambert > 0);//Blinn-phong
				float specularExponent = exp2(_Gloss * 6)+ 2;
				specularLight = pow(specularLight, specularExponent);//specular exponent
				specularLight *= _LightColor0.xyz;

				float fresnel = dot(V, N);
				 

				return float4(diffuseLight * _Color + specularLight ,1);
			}
			ENDCG
		}
	}
}
