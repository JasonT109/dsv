// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:35405,y:32120,varname:node_9361,prsc:2|diff-6589-OUT,spec-6310-OUT,amdfl-5202-RGB;n:type:ShaderForge.SFN_FragmentPosition,id:1039,x:31339,y:31944,varname:node_1039,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:4849,x:31756,y:32638,ptovrint:False,ptlb:Levels,ptin:_Levels,varname:node_4849,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Vector1,id:9758,x:31744,y:32833,varname:node_9758,prsc:2,v1:-1;n:type:ShaderForge.SFN_DDXY,id:5683,x:32237,y:32610,varname:node_5683,prsc:2|IN-4040-OUT;n:type:ShaderForge.SFN_Multiply,id:497,x:32237,y:32435,varname:node_497,prsc:2|A-4040-OUT,B-4849-OUT;n:type:ShaderForge.SFN_Frac,id:7371,x:32540,y:32448,varname:node_7371,prsc:2|IN-497-OUT;n:type:ShaderForge.SFN_Multiply,id:5263,x:32081,y:32805,varname:node_5263,prsc:2|A-4849-OUT,B-9758-OUT;n:type:ShaderForge.SFN_Multiply,id:1472,x:32313,y:32805,varname:node_1472,prsc:2|A-4040-OUT,B-5263-OUT;n:type:ShaderForge.SFN_Frac,id:5050,x:32587,y:32805,varname:node_5050,prsc:2|IN-1472-OUT;n:type:ShaderForge.SFN_Smoothstep,id:3312,x:32853,y:32539,varname:node_3312,prsc:2|A-5683-OUT,B-746-OUT,V-7371-OUT;n:type:ShaderForge.SFN_Multiply,id:746,x:32641,y:32640,varname:node_746,prsc:2|A-5683-OUT,B-3091-OUT;n:type:ShaderForge.SFN_Smoothstep,id:430,x:32841,y:32762,varname:node_430,prsc:2|A-5683-OUT,B-746-OUT,V-5050-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5527,x:31819,y:33149,ptovrint:False,ptlb:Separation,ptin:_Separation,varname:node_5527,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Fmod,id:8168,x:32090,y:33106,varname:node_8168,prsc:2|A-4040-OUT,B-5527-OUT;n:type:ShaderForge.SFN_Vector1,id:1762,x:32122,y:33048,varname:node_1762,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Subtract,id:4065,x:32078,y:33285,varname:node_4065,prsc:2|A-5527-OUT,B-8168-OUT;n:type:ShaderForge.SFN_Vector1,id:7379,x:31818,y:33568,varname:node_7379,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Subtract,id:2159,x:32075,y:33542,varname:node_2159,prsc:2|A-5527-OUT,B-7379-OUT;n:type:ShaderForge.SFN_Step,id:5709,x:32401,y:33142,varname:node_5709,prsc:2|A-4065-OUT,B-1762-OUT;n:type:ShaderForge.SFN_Vector1,id:5180,x:32389,y:33341,varname:node_5180,prsc:2,v1:6;n:type:ShaderForge.SFN_Multiply,id:4226,x:32629,y:33142,varname:node_4226,prsc:2|A-5709-OUT,B-5180-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:3091,x:32825,y:33142,varname:node_3091,prsc:2,min:4,max:8|IN-4226-OUT;n:type:ShaderForge.SFN_Step,id:4872,x:32389,y:33420,varname:node_4872,prsc:2|A-1762-OUT,B-8168-OUT;n:type:ShaderForge.SFN_Add,id:8733,x:32632,y:33448,varname:node_8733,prsc:2|A-4872-OUT,B-936-OUT;n:type:ShaderForge.SFN_Vector1,id:936,x:32326,y:33586,varname:node_936,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Add,id:8509,x:32639,y:33585,varname:node_8509,prsc:2|A-7473-OUT,B-936-OUT;n:type:ShaderForge.SFN_Step,id:7473,x:32374,y:33678,varname:node_7473,prsc:2|A-2159-OUT,B-8168-OUT;n:type:ShaderForge.SFN_Multiply,id:8655,x:33001,y:33365,varname:node_8655,prsc:2|A-8733-OUT,B-8509-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8096,x:32671,y:32015,ptovrint:False,ptlb:Gradient,ptin:_Gradient,varname:node_8096,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Color,id:6728,x:33494,y:32087,ptovrint:False,ptlb:LineColor,ptin:_LineColor,varname:node_6728,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:5736,x:33051,y:32747,varname:node_5736,prsc:2|A-3312-OUT,B-430-OUT;n:type:ShaderForge.SFN_OneMinus,id:9757,x:33224,y:32747,varname:node_9757,prsc:2|IN-5736-OUT;n:type:ShaderForge.SFN_Multiply,id:9263,x:33577,y:32744,varname:node_9263,prsc:2|A-9757-OUT,B-8655-OUT;n:type:ShaderForge.SFN_Multiply,id:2938,x:33985,y:32721,varname:node_2938,prsc:2|A-2345-OUT,B-9263-OUT;n:type:ShaderForge.SFN_Color,id:1333,x:33773,y:31431,ptovrint:False,ptlb:LoColor,ptin:_LoColor,varname:node_1333,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2941176,c2:0.2941176,c3:0.2941176,c4:1;n:type:ShaderForge.SFN_Color,id:4702,x:33773,y:31609,ptovrint:False,ptlb:HiColor,ptin:_HiColor,varname:node_4702,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.28125,c2:0.362069,c3:0.75,c4:1;n:type:ShaderForge.SFN_Lerp,id:2774,x:34063,y:31823,varname:node_2774,prsc:2|A-1333-RGB,B-4702-RGB,T-5353-OUT;n:type:ShaderForge.SFN_Subtract,id:5927,x:34281,y:32084,varname:node_5927,prsc:2|A-2774-OUT,B-2938-OUT;n:type:ShaderForge.SFN_Multiply,id:5937,x:32947,y:31811,varname:node_5937,prsc:2|A-4040-OUT,B-8096-OUT;n:type:ShaderForge.SFN_Slider,id:6310,x:34837,y:32106,ptovrint:False,ptlb:Glossiness,ptin:_Glossiness,varname:node_6310,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2d,id:3284,x:33906,y:32264,ptovrint:False,ptlb:MapEdgeAlpha,ptin:_MapEdgeAlpha,varname:node_3284,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:True,tagnrm:False,tex:6bf88b1e1947c3b428e685b1e43ebc13,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7965,x:34645,y:32335,varname:node_7965,prsc:2|A-9328-OUT,B-3284-RGB;n:type:ShaderForge.SFN_Multiply,id:4040,x:31588,y:32141,varname:node_4040,prsc:2|A-1039-Y,B-1285-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1285,x:31301,y:32345,ptovrint:False,ptlb:HeightMul,ptin:_HeightMul,varname:node_1285,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Add,id:3166,x:34645,y:31890,varname:node_3166,prsc:2|A-2774-OUT,B-2938-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:6722,x:34260,y:32361,ptovrint:False,ptlb:Additive,ptin:_Additive,varname:node_6722,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_If,id:9328,x:34606,y:32173,varname:node_9328,prsc:2|A-6722-OUT,B-4453-OUT,GT-3166-OUT,EQ-5927-OUT,LT-5927-OUT;n:type:ShaderForge.SFN_Vector1,id:4453,x:34260,y:32455,varname:node_4453,prsc:2,v1:0;n:type:ShaderForge.SFN_Lerp,id:2345,x:33837,y:32468,varname:node_2345,prsc:2|A-6728-RGB,B-8714-RGB,T-427-OUT;n:type:ShaderForge.SFN_Color,id:8714,x:33494,y:32277,ptovrint:False,ptlb:LineColorHi,ptin:_LineColorHi,varname:node_8714,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:5202,x:35194,y:32593,ptovrint:False,ptlb:Ambient,ptin:_Ambient,varname:node_5202,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3235294,c2:0.3235294,c3:0.3235294,c4:1;n:type:ShaderForge.SFN_OneMinus,id:427,x:33170,y:32327,varname:node_427,prsc:2|IN-5353-OUT;n:type:ShaderForge.SFN_Fresnel,id:6167,x:35022,y:31641,varname:node_6167,prsc:2|EXP-8773-OUT;n:type:ShaderForge.SFN_Multiply,id:200,x:35181,y:31804,varname:node_200,prsc:2|A-6167-OUT,B-4099-RGB,C-7977-OUT;n:type:ShaderForge.SFN_Vector1,id:8773,x:34780,y:31664,varname:node_8773,prsc:2,v1:2;n:type:ShaderForge.SFN_Vector1,id:7977,x:34948,y:31858,varname:node_7977,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Add,id:6589,x:35194,y:31990,varname:node_6589,prsc:2|A-200-OUT,B-7965-OUT;n:type:ShaderForge.SFN_Clamp01,id:5353,x:33182,y:31811,varname:node_5353,prsc:2|IN-5937-OUT;n:type:ShaderForge.SFN_Color,id:4099,x:34486,y:31556,ptovrint:False,ptlb:FresnelColor,ptin:_FresnelColor,varname:node_4099,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:4849-5527-8096-1333-4702-6310-3284-1285-6722-8714-5202-6728-4099;pass:END;sub:END;*/

Shader "Shader Forge/mapTerrain" {
    Properties {
        _Levels ("Levels", Float ) = 2
        _Separation ("Separation", Float ) = 2
        _Gradient ("Gradient", Float ) = 2
        _LoColor ("LoColor", Color) = (0.2941176,0.2941176,0.2941176,1)
        _HiColor ("HiColor", Color) = (0.28125,0.362069,0.75,1)
        _Glossiness ("Glossiness", Range(0, 1)) = 1
        [NoScaleOffset]_MapEdgeAlpha ("MapEdgeAlpha", 2D) = "white" {}
        _HeightMul ("HeightMul", Float ) = 2
        [MaterialToggle] _Additive ("Additive", Float ) = 0
        _LineColorHi ("LineColorHi", Color) = (0,0,0,1)
        _Ambient ("Ambient", Color) = (0.3235294,0.3235294,0.3235294,1)
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _FresnelColor ("FresnelColor", Color) = (0.5,0.5,0.5,1)
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _Levels;
            uniform float _Separation;
            uniform float _Gradient;
            uniform float4 _LineColor;
            uniform float4 _LoColor;
            uniform float4 _HiColor;
            uniform float _Glossiness;
            uniform sampler2D _MapEdgeAlpha;
            uniform float _HeightMul;
            uniform fixed _Additive;
            uniform float4 _LineColorHi;
            uniform float4 _Ambient;
            uniform float4 _FresnelColor;
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
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Glossiness,_Glossiness,_Glossiness);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                indirectDiffuse += _Ambient.rgb; // Diffuse Ambient Light
                float node_9328_if_leA = step(_Additive,0.0);
                float node_9328_if_leB = step(0.0,_Additive);
                float node_4040 = (i.posWorld.g*_HeightMul);
                float node_5353 = saturate((node_4040*_Gradient));
                float3 node_2774 = lerp(_LoColor.rgb,_HiColor.rgb,node_5353);
                float node_5683 = fwidth(node_4040);
                float node_8168 = fmod(node_4040,_Separation);
                float node_1762 = 0.1;
                float node_746 = (node_5683*clamp((step((_Separation-node_8168),node_1762)*6.0),4,8));
                float node_936 = 0.5;
                float3 node_2938 = (lerp(_LineColor.rgb,_LineColorHi.rgb,(1.0 - node_5353))*((1.0 - (smoothstep( node_5683, node_746, frac((node_4040*_Levels)) )*smoothstep( node_5683, node_746, frac((node_4040*(_Levels*(-1.0)))) )))*((step(node_1762,node_8168)+node_936)*(step((_Separation-0.1),node_8168)+node_936))));
                float3 node_5927 = (node_2774-node_2938);
                float4 _MapEdgeAlpha_var = tex2D(_MapEdgeAlpha,i.uv0);
                float3 diffuseColor = ((pow(1.0-max(0,dot(normalDirection, viewDirection)),2.0)*_FresnelColor.rgb*0.5)+(lerp((node_9328_if_leA*node_5927)+(node_9328_if_leB*(node_2774+node_2938)),node_5927,node_9328_if_leA*node_9328_if_leB)*_MapEdgeAlpha_var.rgb));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
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
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _Levels;
            uniform float _Separation;
            uniform float _Gradient;
            uniform float4 _LineColor;
            uniform float4 _LoColor;
            uniform float4 _HiColor;
            uniform float _Glossiness;
            uniform sampler2D _MapEdgeAlpha;
            uniform float _HeightMul;
            uniform fixed _Additive;
            uniform float4 _LineColorHi;
            uniform float4 _FresnelColor;
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
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Glossiness,_Glossiness,_Glossiness);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float node_9328_if_leA = step(_Additive,0.0);
                float node_9328_if_leB = step(0.0,_Additive);
                float node_4040 = (i.posWorld.g*_HeightMul);
                float node_5353 = saturate((node_4040*_Gradient));
                float3 node_2774 = lerp(_LoColor.rgb,_HiColor.rgb,node_5353);
                float node_5683 = fwidth(node_4040);
                float node_8168 = fmod(node_4040,_Separation);
                float node_1762 = 0.1;
                float node_746 = (node_5683*clamp((step((_Separation-node_8168),node_1762)*6.0),4,8));
                float node_936 = 0.5;
                float3 node_2938 = (lerp(_LineColor.rgb,_LineColorHi.rgb,(1.0 - node_5353))*((1.0 - (smoothstep( node_5683, node_746, frac((node_4040*_Levels)) )*smoothstep( node_5683, node_746, frac((node_4040*(_Levels*(-1.0)))) )))*((step(node_1762,node_8168)+node_936)*(step((_Separation-0.1),node_8168)+node_936))));
                float3 node_5927 = (node_2774-node_2938);
                float4 _MapEdgeAlpha_var = tex2D(_MapEdgeAlpha,i.uv0);
                float3 diffuseColor = ((pow(1.0-max(0,dot(normalDirection, viewDirection)),2.0)*_FresnelColor.rgb*0.5)+(lerp((node_9328_if_leA*node_5927)+(node_9328_if_leB*(node_2774+node_2938)),node_5927,node_9328_if_leA*node_9328_if_leB)*_MapEdgeAlpha_var.rgb));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
