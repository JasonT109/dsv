Shader "Custom/DepthToColor" 
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_RampNear("Near Ramp", 2D) = "grayscaleRamp" {}
		_Exponent("Exponent", float) = 100
		_Saturation("Saturation", float) = 0.64
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog{ Mode off }

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _RampNear;
			uniform sampler2D _CameraDepthTexture;
			uniform float _Exponent;
			uniform float _Saturation;

			fixed4 frag(v2f_img i) : COLOR
			{
				//sample depth and raise to exponent
				fixed4 depth = pow((UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv))) , _Exponent);
				//return depth ;
				fixed4 orig = tex2D(_MainTex, i.uv);
				//remap depth with gradient
				fixed rr = tex2D(_RampNear, depth.rr).r + 0.00001; // numbers to workaround Cg's bug at D3D code generation :(
				fixed gg = tex2D(_RampNear, depth.gg).g + 0.00002;
				fixed bb = tex2D(_RampNear, depth.bb).b + 0.00003;
				fixed4 depthremap = fixed4(rr, gg, bb, orig.a);
				return depthremap;

				//multiply depth with image
				fixed4 color = (orig * depthremap);

				//saturation
				fixed4 lum = Luminance(color.rgb);
				return lerp(lum, color, _Saturation);
			}
			ENDCG
		}
	}
	Fallback off

}