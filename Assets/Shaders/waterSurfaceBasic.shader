// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:0,trmd:1,grmd:1,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33455,y:32210,varname:node_4013,prsc:2|diff-141-OUT,spec-4839-OUT,gloss-1462-OUT,normal-5671-OUT,alpha-6994-A;n:type:ShaderForge.SFN_Time,id:8623,x:31309,y:32395,varname:node_8623,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:8887,x:32545,y:32804,ptovrint:False,ptlb:node_8887,ptin:_node_8887,varname:node_8887,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:90ff426733b42774fa4fb464cc68b2c9,ntxv:3,isnm:True|UVIN-2458-OUT;n:type:ShaderForge.SFN_TexCoord,id:124,x:31727,y:33052,varname:node_124,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:532,x:32052,y:32746,varname:node_532,prsc:2,spu:1,spv:0|UVIN-124-UVOUT,DIST-5551-OUT;n:type:ShaderForge.SFN_Vector1,id:8172,x:32187,y:33018,varname:node_8172,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:6075,x:31610,y:32442,varname:node_6075,prsc:2|A-8623-TSL,B-246-OUT;n:type:ShaderForge.SFN_Color,id:6994,x:32536,y:31691,ptovrint:False,ptlb:MainColor,ptin:_MainColor,varname:node_6994,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5514706,c2:0.5514706,c3:0.5514706,c4:0.497;n:type:ShaderForge.SFN_Panner,id:4396,x:32042,y:32938,varname:node_4396,prsc:2,spu:0,spv:1|UVIN-124-UVOUT,DIST-5551-OUT;n:type:ShaderForge.SFN_Slider,id:246,x:31094,y:32643,ptovrint:False,ptlb:scrollSpeed1,ptin:_scrollSpeed1,varname:node_246,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1230769,max:0.3;n:type:ShaderForge.SFN_Lerp,id:2458,x:32356,y:32804,varname:node_2458,prsc:2|A-532-UVOUT,B-4396-UVOUT,T-8172-OUT;n:type:ShaderForge.SFN_Tex2d,id:362,x:32557,y:32376,ptovrint:False,ptlb:node_8887_copy,ptin:_node_8887_copy,varname:_node_8887_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:90ff426733b42774fa4fb464cc68b2c9,ntxv:3,isnm:True|UVIN-164-OUT;n:type:ShaderForge.SFN_TexCoord,id:3836,x:31703,y:32154,varname:node_3836,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:782,x:31997,y:32339,varname:node_782,prsc:2,spu:0,spv:1|UVIN-3836-UVOUT,DIST-6075-OUT;n:type:ShaderForge.SFN_Vector1,id:3960,x:32142,y:32609,varname:node_3960,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:5551,x:31610,y:32646,varname:node_5551,prsc:2|A-8623-TSL,B-1841-OUT;n:type:ShaderForge.SFN_Panner,id:8580,x:31984,y:32521,varname:node_8580,prsc:2,spu:1,spv:0|UVIN-3836-UVOUT,DIST-6075-OUT;n:type:ShaderForge.SFN_Lerp,id:164,x:32340,y:32417,varname:node_164,prsc:2|A-782-UVOUT,B-8580-UVOUT,T-3960-OUT;n:type:ShaderForge.SFN_Blend,id:161,x:32798,y:32561,varname:node_161,prsc:2,blmd:10,clmp:True|SRC-362-RGB,DST-8887-RGB;n:type:ShaderForge.SFN_Slider,id:1841,x:31074,y:32808,ptovrint:False,ptlb:scrollSpeed2,ptin:_scrollSpeed2,varname:node_1841,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2153846,max:0.3;n:type:ShaderForge.SFN_Tex2d,id:6094,x:32362,y:31914,ptovrint:False,ptlb:GridTexture,ptin:_GridTexture,varname:node_6094,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:fb2d7c9beddd272479706f8ac8a11fdc,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5430,x:32837,y:31961,varname:node_5430,prsc:2|A-6094-RGB,B-6994-RGB,C-8545-OUT;n:type:ShaderForge.SFN_Vector1,id:8545,x:32472,y:32140,varname:node_8545,prsc:2,v1:2;n:type:ShaderForge.SFN_Slider,id:4839,x:32931,y:32190,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:node_4839,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1709402,max:1;n:type:ShaderForge.SFN_Slider,id:1462,x:32872,y:32346,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:node_1462,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8376068,max:1;n:type:ShaderForge.SFN_Subtract,id:5671,x:33029,y:32432,varname:node_5671,prsc:2|A-5430-OUT,B-161-OUT;n:type:ShaderForge.SFN_Blend,id:141,x:33100,y:31842,varname:node_141,prsc:2,blmd:6,clmp:True|SRC-6994-RGB,DST-5430-OUT;proporder:8887-6994-246-362-1841-6094-4839-1462;pass:END;sub:END;*/

Shader "Shader Forge/waterSurfaceBasic" {
    Properties {
        _node_8887 ("node_8887", 2D) = "bump" {}
        _MainColor ("MainColor", Color) = (0.5514706,0.5514706,0.5514706,0.497)
        _scrollSpeed1 ("scrollSpeed1", Range(0, 0.3)) = 0.1230769
        _node_8887_copy ("node_8887_copy", 2D) = "bump" {}
        _scrollSpeed2 ("scrollSpeed2", Range(0, 0.3)) = 0.2153846
        _GridTexture ("GridTexture", 2D) = "white" {}
        _Specular ("Specular", Range(0, 1)) = 0.1709402
        _Roughness ("Roughness", Range(0, 1)) = 0.8376068
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_8887; uniform float4 _node_8887_ST;
            uniform float4 _MainColor;
            uniform float _scrollSpeed1;
            uniform sampler2D _node_8887_copy; uniform float4 _node_8887_copy_ST;
            uniform float _scrollSpeed2;
            uniform sampler2D _GridTexture; uniform float4 _GridTexture_ST;
            uniform float _Specular;
            uniform float _Roughness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _GridTexture_var = tex2D(_GridTexture,TRANSFORM_TEX(i.uv0, _GridTexture));
                float3 node_5430 = (_GridTexture_var.rgb*_MainColor.rgb*2.0);
                float4 node_8623 = _Time + _TimeEditor;
                float node_6075 = (node_8623.r*_scrollSpeed1);
                float2 node_164 = lerp((i.uv0+node_6075*float2(0,1)),(i.uv0+node_6075*float2(1,0)),0.5);
                float3 _node_8887_copy_var = UnpackNormal(tex2D(_node_8887_copy,TRANSFORM_TEX(node_164, _node_8887_copy)));
                float node_5551 = (node_8623.r*_scrollSpeed2);
                float2 node_2458 = lerp((i.uv0+node_5551*float2(1,0)),(i.uv0+node_5551*float2(0,1)),0.5);
                float3 _node_8887_var = UnpackNormal(tex2D(_node_8887,TRANSFORM_TEX(node_2458, _node_8887)));
                float3 normalLocal = (node_5430-saturate(( _node_8887_var.rgb > 0.5 ? (1.0-(1.0-2.0*(_node_8887_var.rgb-0.5))*(1.0-_node_8887_copy_var.rgb)) : (2.0*_node_8887_var.rgb*_node_8887_copy_var.rgb) )));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 1.0 - _Roughness; // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = float3(_Specular,_Specular,_Specular);
                float specularMonochrome = max( max(specularColor.r, specularColor.g), specularColor.b);
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * (UNITY_PI / 4) );
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = saturate((1.0-(1.0-_MainColor.rgb)*(1.0-node_5430)));
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse * _MainColor.a + specular;
                fixed4 finalRGBA = fixed4(finalColor,_MainColor.a);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_8887; uniform float4 _node_8887_ST;
            uniform float4 _MainColor;
            uniform float _scrollSpeed1;
            uniform sampler2D _node_8887_copy; uniform float4 _node_8887_copy_ST;
            uniform float _scrollSpeed2;
            uniform sampler2D _GridTexture; uniform float4 _GridTexture_ST;
            uniform float _Specular;
            uniform float _Roughness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _GridTexture_var = tex2D(_GridTexture,TRANSFORM_TEX(i.uv0, _GridTexture));
                float3 node_5430 = (_GridTexture_var.rgb*_MainColor.rgb*2.0);
                float4 node_8623 = _Time + _TimeEditor;
                float node_6075 = (node_8623.r*_scrollSpeed1);
                float2 node_164 = lerp((i.uv0+node_6075*float2(0,1)),(i.uv0+node_6075*float2(1,0)),0.5);
                float3 _node_8887_copy_var = UnpackNormal(tex2D(_node_8887_copy,TRANSFORM_TEX(node_164, _node_8887_copy)));
                float node_5551 = (node_8623.r*_scrollSpeed2);
                float2 node_2458 = lerp((i.uv0+node_5551*float2(1,0)),(i.uv0+node_5551*float2(0,1)),0.5);
                float3 _node_8887_var = UnpackNormal(tex2D(_node_8887,TRANSFORM_TEX(node_2458, _node_8887)));
                float3 normalLocal = (node_5430-saturate(( _node_8887_var.rgb > 0.5 ? (1.0-(1.0-2.0*(_node_8887_var.rgb-0.5))*(1.0-_node_8887_copy_var.rgb)) : (2.0*_node_8887_var.rgb*_node_8887_copy_var.rgb) )));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 1.0 - _Roughness; // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = float3(_Specular,_Specular,_Specular);
                float specularMonochrome = max( max(specularColor.r, specularColor.g), specularColor.b);
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * (UNITY_PI / 4) );
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
                float3 diffuseColor = saturate((1.0-(1.0-_MainColor.rgb)*(1.0-node_5430)));
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse * _MainColor.a + specular;
                fixed4 finalRGBA = fixed4(finalColor,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
