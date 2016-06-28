// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:34543,y:32023,varname:node_9361,prsc:2|emission-7965-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:1039,x:31897,y:32358,varname:node_1039,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:4849,x:31797,y:32645,ptovrint:False,ptlb:Levels,ptin:_Levels,varname:node_4849,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Vector1,id:9758,x:31797,y:32852,varname:node_9758,prsc:2,v1:-1;n:type:ShaderForge.SFN_DDXY,id:5683,x:32237,y:32610,varname:node_5683,prsc:2|IN-1039-Y;n:type:ShaderForge.SFN_Multiply,id:497,x:32237,y:32435,varname:node_497,prsc:2|A-1039-Y,B-4849-OUT;n:type:ShaderForge.SFN_Frac,id:7371,x:32540,y:32448,varname:node_7371,prsc:2|IN-497-OUT;n:type:ShaderForge.SFN_Multiply,id:5263,x:32122,y:32808,varname:node_5263,prsc:2|A-4849-OUT,B-9758-OUT;n:type:ShaderForge.SFN_Multiply,id:1472,x:32313,y:32791,varname:node_1472,prsc:2|A-1039-Y,B-5263-OUT;n:type:ShaderForge.SFN_Frac,id:5050,x:32539,y:32804,varname:node_5050,prsc:2|IN-1472-OUT;n:type:ShaderForge.SFN_Smoothstep,id:3312,x:32853,y:32539,varname:node_3312,prsc:2|A-5683-OUT,B-746-OUT,V-7371-OUT;n:type:ShaderForge.SFN_Multiply,id:746,x:32607,y:32632,varname:node_746,prsc:2|A-5683-OUT,B-3091-OUT;n:type:ShaderForge.SFN_Smoothstep,id:430,x:32841,y:32762,varname:node_430,prsc:2|A-5683-OUT,B-746-OUT,V-5050-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5527,x:31819,y:33149,ptovrint:False,ptlb:Separation,ptin:_Separation,varname:node_5527,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Fmod,id:8168,x:32090,y:33106,varname:node_8168,prsc:2|A-1039-Y,B-5527-OUT;n:type:ShaderForge.SFN_Vector1,id:1762,x:32122,y:33048,varname:node_1762,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Subtract,id:4065,x:32078,y:33285,varname:node_4065,prsc:2|A-5527-OUT,B-8168-OUT;n:type:ShaderForge.SFN_Vector1,id:7379,x:31871,y:33522,varname:node_7379,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Subtract,id:2159,x:32075,y:33542,varname:node_2159,prsc:2|A-5527-OUT,B-7379-OUT;n:type:ShaderForge.SFN_Step,id:5709,x:32396,y:33166,varname:node_5709,prsc:2|A-4065-OUT,B-1762-OUT;n:type:ShaderForge.SFN_Vector1,id:5180,x:32389,y:33333,varname:node_5180,prsc:2,v1:6;n:type:ShaderForge.SFN_Multiply,id:4226,x:32615,y:33154,varname:node_4226,prsc:2|A-5709-OUT,B-5180-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:3091,x:32770,y:33110,varname:node_3091,prsc:2,min:4,max:8|IN-4226-OUT;n:type:ShaderForge.SFN_Step,id:4872,x:32389,y:33420,varname:node_4872,prsc:2|A-1762-OUT,B-8168-OUT;n:type:ShaderForge.SFN_Add,id:8733,x:32632,y:33448,varname:node_8733,prsc:2|A-4872-OUT,B-936-OUT;n:type:ShaderForge.SFN_Vector1,id:936,x:32326,y:33586,varname:node_936,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Add,id:8509,x:32639,y:33585,varname:node_8509,prsc:2|A-7473-OUT,B-936-OUT;n:type:ShaderForge.SFN_Step,id:7473,x:32374,y:33678,varname:node_7473,prsc:2|A-2159-OUT,B-8168-OUT;n:type:ShaderForge.SFN_Multiply,id:8655,x:32816,y:33390,varname:node_8655,prsc:2|A-8733-OUT,B-8509-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8096,x:32757,y:32130,ptovrint:False,ptlb:Gradient,ptin:_Gradient,varname:node_8096,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Color,id:6728,x:33017,y:32405,ptovrint:False,ptlb:LineColor,ptin:_LineColor,varname:node_6728,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.347,c2:0.5,c3:0.872,c4:1;n:type:ShaderForge.SFN_Multiply,id:5736,x:33051,y:32747,varname:node_5736,prsc:2|A-3312-OUT,B-430-OUT;n:type:ShaderForge.SFN_OneMinus,id:9757,x:33224,y:32747,varname:node_9757,prsc:2|IN-5736-OUT;n:type:ShaderForge.SFN_Multiply,id:9263,x:33308,y:32457,varname:node_9263,prsc:2|A-6728-RGB,B-9757-OUT;n:type:ShaderForge.SFN_Multiply,id:2938,x:33604,y:32529,varname:node_2938,prsc:2|A-9263-OUT,B-8655-OUT;n:type:ShaderForge.SFN_Color,id:1333,x:33547,y:31860,ptovrint:False,ptlb:LoColor,ptin:_LoColor,varname:node_1333,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.1374351,c2:0.4558824,c3:0.1374351,c4:1;n:type:ShaderForge.SFN_Color,id:4702,x:33538,y:32128,ptovrint:False,ptlb:HiColor,ptin:_HiColor,varname:node_4702,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.7843138,c2:1,c3:0.7843138,c4:1;n:type:ShaderForge.SFN_Lerp,id:2774,x:33756,y:32035,varname:node_2774,prsc:2|A-1333-RGB,B-4702-RGB,T-5937-OUT;n:type:ShaderForge.SFN_Subtract,id:5927,x:34175,y:32008,varname:node_5927,prsc:2|A-9498-OUT,B-2938-OUT;n:type:ShaderForge.SFN_Multiply,id:5937,x:32947,y:31811,varname:node_5937,prsc:2|A-1039-Y,B-8096-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:3513,x:33794,y:31675,varname:node_3513,prsc:2;n:type:ShaderForge.SFN_LightColor,id:1205,x:33770,y:31805,varname:node_1205,prsc:2;n:type:ShaderForge.SFN_LightVector,id:8618,x:33561,y:31619,varname:node_8618,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4100,x:33583,y:31409,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:2766,x:33794,y:31498,varname:node_2766,prsc:2,dt:1|A-4100-OUT,B-8618-OUT;n:type:ShaderForge.SFN_Multiply,id:2673,x:34020,y:31614,varname:node_2673,prsc:2|A-2766-OUT,B-3513-OUT,C-1205-RGB;n:type:ShaderForge.SFN_Multiply,id:9498,x:34160,y:31825,varname:node_9498,prsc:2|A-1740-OUT,B-2774-OUT;n:type:ShaderForge.SFN_HalfVector,id:7288,x:33434,y:31348,varname:node_7288,prsc:2;n:type:ShaderForge.SFN_Dot,id:9722,x:33844,y:31323,varname:node_9722,prsc:2,dt:0|A-7288-OUT,B-4100-OUT;n:type:ShaderForge.SFN_Power,id:2210,x:34052,y:31311,varname:node_2210,prsc:2|VAL-9722-OUT,EXP-6310-OUT;n:type:ShaderForge.SFN_Add,id:1740,x:34406,y:31560,varname:node_1740,prsc:2|A-2210-OUT,B-2673-OUT;n:type:ShaderForge.SFN_Slider,id:6310,x:33989,y:31485,ptovrint:False,ptlb:Glossiness,ptin:_Glossiness,varname:node_6310,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:30;n:type:ShaderForge.SFN_Tex2d,id:3284,x:33768,y:32212,ptovrint:False,ptlb:MapEdgeAlpha,ptin:_MapEdgeAlpha,varname:node_3284,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:True,tagnrm:False,tex:6bf88b1e1947c3b428e685b1e43ebc13,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7965,x:34255,y:32189,varname:node_7965,prsc:2|A-5927-OUT,B-3284-RGB;proporder:4849-5527-8096-6728-1333-4702-6310-3284;pass:END;sub:END;*/

Shader "Shader Forge/mapTerrain" {
    Properties {
        _Levels ("Levels", Float ) = 2
        _Separation ("Separation", Float ) = 2
        _Gradient ("Gradient", Float ) = 2
        _LineColor ("LineColor", Color) = (0.347,0.5,0.872,1)
        _LoColor ("LoColor", Color) = (0.1374351,0.4558824,0.1374351,1)
        _HiColor ("HiColor", Color) = (0.7843138,1,0.7843138,1)
        _Glossiness ("Glossiness", Range(1, 30)) = 1
        [NoScaleOffset]_MapEdgeAlpha ("MapEdgeAlpha", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _Levels;
            uniform float _Separation;
            uniform float _Gradient;
            uniform float4 _LineColor;
            uniform float4 _LoColor;
            uniform float4 _HiColor;
            uniform float _Glossiness;
            uniform sampler2D _MapEdgeAlpha;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float node_5683 = fwidth(i.posWorld.g);
                float node_8168 = fmod(i.posWorld.g,_Separation);
                float node_1762 = 0.1;
                float node_746 = (node_5683*clamp((step((_Separation-node_8168),node_1762)*6.0),4,8));
                float node_936 = 0.5;
                float3 node_5927 = (((pow(dot(halfDirection,i.normalDir),_Glossiness)+(max(0,dot(i.normalDir,lightDirection))*attenuation*_LightColor0.rgb))*lerp(_LoColor.rgb,_HiColor.rgb,(i.posWorld.g*_Gradient)))-((_LineColor.rgb*(1.0 - (smoothstep( node_5683, node_746, frac((i.posWorld.g*_Levels)) )*smoothstep( node_5683, node_746, frac((i.posWorld.g*(_Levels*(-1.0)))) ))))*((step(node_1762,node_8168)+node_936)*(step((_Separation-0.1),node_8168)+node_936))));
                float4 _MapEdgeAlpha_var = tex2D(_MapEdgeAlpha,i.uv0);
                float3 emissive = (node_5927*_MapEdgeAlpha_var.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
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
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _Levels;
            uniform float _Separation;
            uniform float _Gradient;
            uniform float4 _LineColor;
            uniform float4 _LoColor;
            uniform float4 _HiColor;
            uniform float _Glossiness;
            uniform sampler2D _MapEdgeAlpha;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 finalColor = 0;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
