//
//	Code repository for GPU noise development blog
//	http://briansharpe.wordpress.com
//	https://github.com/BrianSharpe
//
//	I'm not one for copyrights.  Use the code however you wish.
//	All I ask is that credit be given back to the blog or myself when appropriate.
//	And also to let me know if you come up with any changes, improvements, thoughts or interesting uses for this stuff. :)
//	Thanks!
//
//	Brian Sharpe
//	brisharpe CIRCLE_A yahoo DOT com
//	http://briansharpe.wordpress.com
//	https://github.com/BrianSharpe
//
//===============================================================================
//  Scape Software License
//===============================================================================
//
//Copyright (c) 2007-2012, Giliam de Carpentier
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without
//modification, are permitted provided that the following conditions are met: 
//
//1. Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer. 
//2. Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNERS OR CONTRIBUTORS BE LIABLE 
//FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
//DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
//CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
//OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
//OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.;

Shader "Noise/SimplexNoise1" 
{
	Properties 
	{
		_Octaves ("Octaves", Float) = 8.0
		_Frequency ("Frequency", Float) = 1.0
		_Amplitude ("Amplitude", Float) = 1.0
		_Lacunarity ("Lacunarity", Float) = 1.92
		_Persistence ("Persistence", Float) = 0.8
		_Offset ("Offset", Vector) = (0.0, 0.0, 0.0, 0.0)
		_Transparency ("Transparency", Range(0.0, 1.0)) = 1.0
		_LowColor("Low Color", Vector) = (0.0, 0.0, 0.0, 1.0)
		_HighColor("High Color", Vector) = (1.0, 1.0, 1.0, 1.0)
		_LowTexture("Low Texture", 2D) = "" {}
		_HighTexture("High Texture", 2D) = "" {}

	}

	CGINCLUDE
		//
		//	FAST32_hash
		//	A very fast hashing function.  Requires 32bit support.
		//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/
		//
		//	The hash formula takes the form....
		//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT
		//	We truncate and offset the domain to the most interesting part of the noise.
		//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.
		//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate
		//
		void FAST32_hash_2D( float2 gridcell, out float4 hash_0, out float4 hash_1 )	//	generates 2 random numbers for each of the 4 cell corners
		{
			//    gridcell is assumed to be an integer coordinate
			const float2 OFFSET = float2( 26.0, 161.0 );
			const float DOMAIN = 71.0;
			const float2 SOMELARGEFLOATS = float2( 951.135664, 642.949883 );
			float4 P = float4( gridcell.xy, gridcell.xy + 1.0 );
			P = P - floor(P * ( 1.0 / DOMAIN )) * DOMAIN;
			P += OFFSET.xyxy;
			P *= P;
			P = P.xzxz * P.yyww;
			hash_0 = frac( P * ( 1.0 / SOMELARGEFLOATS.x ) );
			hash_1 = frac( P * ( 1.0 / SOMELARGEFLOATS.y ) );
		}
		//
		//	SimplexPerlin2D  ( simplex gradient noise )
		//	Perlin noise over a simplex (triangular) grid
		//	Return value range of -1.0->1.0
		//	http://briansharpe.files.wordpress.com/2012/01/simplexperlinsample.jpg
		//
		//	Implementation originally based off Stefan Gustavson's and Ian McEwan's work at...
		//	http://github.com/ashima/webgl-noise
		//
		float SimplexPerlin2D( float2 P )
		{
			//	simplex math constants
			const float SKEWFACTOR = 0.36602540378443864676372317075294;			// 0.5*(sqrt(3.0)-1.0)
			const float UNSKEWFACTOR = 0.21132486540518711774542560974902;			// (3.0-sqrt(3.0))/6.0
			const float SIMPLEX_TRI_HEIGHT = 0.70710678118654752440084436210485;	// sqrt( 0.5 )	height of simplex triangle
			const float3 SIMPLEX_POINTS = float3( 1.0-UNSKEWFACTOR, -UNSKEWFACTOR, 1.0-2.0*UNSKEWFACTOR );		//	vertex info for simplex triangle
		
			//	establish our grid cell.
			P *= SIMPLEX_TRI_HEIGHT;		// scale space so we can have an approx feature size of 1.0  ( optional )
			float2 Pi = floor( P + dot( P, float2( SKEWFACTOR, SKEWFACTOR ) ) );
		
			//	calculate the hash.
			float4 hash_x, hash_y;
			FAST32_hash_2D( Pi, hash_x, hash_y );
		
			//	establish vectors to the 3 corners of our simplex triangle
			float2 v0 = Pi - dot( Pi, float2( UNSKEWFACTOR, UNSKEWFACTOR ) ) - P;
			float4 v1pos_v1hash = (v0.x < v0.y) ? float4(SIMPLEX_POINTS.xy, hash_x.y, hash_y.y) : float4(SIMPLEX_POINTS.yx, hash_x.z, hash_y.z);
			float4 v12 = float4( v1pos_v1hash.xy, SIMPLEX_POINTS.zz ) + v0.xyxy;
		
			//	calculate the dotproduct of our 3 corner vectors with 3 random normalized vectors
			float3 grad_x = float3( hash_x.x, v1pos_v1hash.z, hash_x.w ) - 0.49999;
			float3 grad_y = float3( hash_y.x, v1pos_v1hash.w, hash_y.w ) - 0.49999;
			float3 grad_results = rsqrt( grad_x * grad_x + grad_y * grad_y ) * ( grad_x * float3( v0.x, v12.xz ) + grad_y * float3( v0.y, v12.yw ) );
		
			//	Normalization factor to scale the final result to a strict 1.0->-1.0 range
			//	x = ( sqrt( 0.5 )/sqrt( 0.75 ) ) * 0.5
			//	NF = 1.0 / ( x * ( ( 0.5 ? x*x ) ^ 4 ) * 2.0 )
			//	http://briansharpe.wordpress.com/2012/01/13/simplex-noise/#comment-36
			const float FINAL_NORMALIZATION = 99.204334582718712976990005025589;
		
			//	evaluate the surflet, sum and return
			float3 m = float3( v0.x, v12.xz ) * float3( v0.x, v12.xz ) + float3( v0.y, v12.yw ) * float3( v0.y, v12.yw );
			m = max(0.5 - m, 0.0);		//	The 0.5 here is SIMPLEX_TRI_HEIGHT^2
			m = m*m;
			m = m*m;
			return dot(m, grad_results) * FINAL_NORMALIZATION;
		}
		float SimplexNormal(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)
		{
			float sum = 0;
			for (int i = 0; i < octaves; i++)
			{
				float h = 0;
				h = SimplexPerlin2D((p + offset) * frequency);
				sum += h*amplitude;
				frequency *= lacunarity;
				amplitude *= persistence;
			}
			return sum;
		}

	ENDCG

	SubShader 
	{
		Tags {"Queue"="Transparent"}
		ZWrite Off
		Blend One One
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma glsl
		#pragma target 3.0
		
		fixed _Octaves;
		float _Frequency;
		float _Amplitude;
		float2 _Offset;
		float _Lacunarity;
		float _Persistence;
		fixed _Transparency;
		fixed4 _LowColor;
		fixed4 _HighColor;
		sampler2D _LowTexture;
		sampler2D _HighTexture;


		struct Input 
		{
			float2 pos;
			float2 uv_LowTexture;
			float2 uv_HighTexture;
		};

		void vert (inout appdata_full v, out Input OUT)
		{
			UNITY_INITIALIZE_OUTPUT(Input, OUT);
			OUT.pos = v.texcoord;
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float h = SimplexNormal(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);
			

			
			float4 color = float4(0.0, 0.0, 0.0, 0.0);
			float4 lowTexColor = tex2D(_LowTexture, IN.uv_LowTexture);
			float4 highTexColor = tex2D(_HighTexture, IN.uv_HighTexture);
			color = lerp(_LowColor * lowTexColor, _HighColor * highTexColor, h);

			o.Albedo = color.rgb;
			o.Alpha = h * _Transparency;
		}
		ENDCG
	}
	
	FallBack "Diffuse"
}