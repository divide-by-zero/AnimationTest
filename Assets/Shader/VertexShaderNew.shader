// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VertexColor2" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
	}
	SubShader{
		Tags{ "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off 
		Cull Off

		Pass{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Color;

				struct data {
					float4 vertex : POSITION;
					fixed4 color: COLOR;
					float2 tex_uv:TEXCOORD0;
				};
 
				data vert (appdata_full v) {
					data o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.tex_uv = TRANSFORM_TEX(v.texcoord,_MainTex);
					o.color = v.color;
					return o;
				}

				fixed4 frag(data f) : COLOR {
//					return tex2D(_MainTex,f.tex_uv) * (f.color+ float4(0.5,0.5,0.5,0));
//					return tex2D(_MainTex,f.tex_uv) * float4(f.color.rgb*3,f.color.a);
					return tex2D(_MainTex,f.tex_uv) * f.color * _Color;
				}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
