Shader "Custom/Brot" {
	Properties {
		_MyFloat ("My float", Float) = 0.33
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_builtin_noshadows
			#pragma fragmentoption ARB_fog_exp2
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			float _MyFloat;

			struct v2f {
				half4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			half4 frag (v2f IN) : COLOR
			{
				half co = cos(_MyFloat * 0.25f);
				half si = sin(_MyFloat * 0.25f);
				half2 c = half2( IN.uv.x*co-IN.uv.y*si, IN.uv.x*si+IN.uv.y*co );
				c *= sin(_MyFloat * 0.5f);
				c += half2( co*0.25f, si*0.5f );
				half2 z = c;
				half d;
				half col;
				half4 res;
				half i = 0.0f;

				for (; i < 35.0f; i += 1.0f)
				{
					z = half2( z.x*z.x - z.y*z.y, 2.0f*z.x*z.y ) + c;
					d = dot( z, z );
					if (d > 4.0f)
					{
						break;
					}
				}

				if (d > 4.0f)
					col = 1.0f - (cos(sqrt((i - log2(log2(d) + 0.5f))/4.0f)) * 0.5f + 0.5f);
				else
					col = 0.0f;

				return half4(col*0.33f, col*0.33f, col, 1.0f);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
