// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "Custom/CellNoise" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SecondaryColor("Secondary Color", Color) = (1,1,1,1)
		_Random("Random Noise", 2D) = "white" {}
		_Intensity("Intensity", Range(0.001, 10.0)) = 1.0
		_Size("Size", Range(0.000, 10.0)) = 1.0
		_Offset("Time", float) = 0.0
	}

		SubShader{
			
		pass {
			Tags{ "RenderType" = "Opaque" }
		Name "Noise"

			/* // Enable this if you want addative blending
			ZWrite Off
			Tags { "Queue" = "Transparent" }
			Blend One One
			*/

			CGPROGRAM
		// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members noisepos)
		//#pragma exclude_renderers d3d11 xbox360
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_fog_exp2
		#include "UnityCG.cginc"

			uniform float4 _Color;
		uniform float4 _SecondaryColor;
		uniform float _Intensity;
		uniform sampler2D _Random;
		uniform float _Size;
		uniform float _Offset;

		struct v2f {
			float4 pos : SV_POSITION;
			float3 noisepos : NORMAL;
		};

		v2f vert(appdata_base v) {
			v2f o;

			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.noisepos = v.vertex;

			return o;
		}

		float4 getCell3D(const in int x, const in int y, const in int z) {
			float u = (x + y * 31) / 20.0;
			float v = (z - x * 3) / 30.0;
			return tex2D(_Random, float2(u, v));
		}

		float2 cellNoise3D(float3 xyz) {
			int xi = int(floor(xyz.x));
			int yi = int(floor(xyz.y));
			int zi = int(floor(xyz.z));

			float xf = xyz.x - float(xi);
			float yf = xyz.y - float(yi);
			float zf = xyz.z - float(zi);

			float dist1 = 9999999.0;
			float dist2 = 9999999.0;
			float3 cell;

			for (int z = -1; z <= 1; z++) {
				for (int y = -1; y <= 1; y++) {
					for (int x = -1; x <= 1; x++) {
						cell = getCell3D(xi + x, yi + y, zi + z).xyz;
						cell.x += (float(x) - xf);
						cell.y += (float(y) - yf);
						cell.z += (float(z) - zf);
						float dist = dot(cell, cell);
						if (dist < dist1) {
							dist2 = dist1;
							dist1 = dist;
						}
						else if (dist < dist2) {
							dist2 = dist;
						}
					}
				}
			}

			return float2(sqrt(dist1), sqrt(dist2));
		}

		float4 frag(v2f IN) : COLOR{
			float2 dists = cellNoise3D((IN.noisepos + _Offset) * _Size);
			// float4 c = ((_Color * dists.x) + (_SecondaryColor * dists.y)) * _Intensity; // Add the terms for a different look
			float4 c = ((_Color * dists.x) * (_SecondaryColor * dists.y)) * _Intensity;
			return c;
		}

			ENDCG
	}
	}

		FallBack "Diffuse"
}