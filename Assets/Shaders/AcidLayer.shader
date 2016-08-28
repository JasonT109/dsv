// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:True,qofs:2,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:34684,y:32504,varname:node_4013,prsc:2|diff-696-OUT,spec-755-OUT,gloss-8843-OUT,normal-6986-OUT,transm-9189-OUT,lwrap-9189-OUT,alpha-1118-OUT,refract-1907-OUT,voffset-8490-OUT,tess-116-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:33147,y:32054,ptovrint:False,ptlb:Color1,ptin:_Color1,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.0216263,c2:0.1137096,c3:0.2941176,c4:1;n:type:ShaderForge.SFN_Tex2d,id:6341,x:32818,y:32782,ptovrint:False,ptlb:node_6341,ptin:_node_6341,varname:node_6341,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:2,isnm:False|UVIN-3926-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:8640,x:32798,y:33032,ptovrint:False,ptlb:node_8640,ptin:_node_8640,varname:node_8640,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:2,isnm:False|UVIN-4580-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:5710,x:32254,y:32642,varname:node_5710,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:3926,x:32524,y:32735,varname:node_3926,prsc:2,spu:1,spv:1|UVIN-5710-UVOUT,DIST-931-OUT;n:type:ShaderForge.SFN_Time,id:9500,x:32086,y:32926,varname:node_9500,prsc:2;n:type:ShaderForge.SFN_Panner,id:4580,x:32556,y:32952,varname:node_4580,prsc:2,spu:1,spv:-1|UVIN-5710-UVOUT,DIST-931-OUT;n:type:ShaderForge.SFN_Multiply,id:6986,x:33088,y:32836,varname:node_6986,prsc:2|A-6341-RGB,B-8640-RGB,C-52-RGB;n:type:ShaderForge.SFN_Slider,id:3088,x:33171,y:33158,ptovrint:False,ptlb:VertexOffset,ptin:_VertexOffset,varname:node_3088,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.361024,max:0.5;n:type:ShaderForge.SFN_Multiply,id:8490,x:33818,y:32813,varname:node_8490,prsc:2|A-9275-OUT,B-3088-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1167,x:32034,y:33138,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_1167,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:931,x:32289,y:33002,varname:node_931,prsc:2|A-9500-TSL,B-1167-OUT;n:type:ShaderForge.SFN_Slider,id:755,x:33971,y:32441,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:node_755,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7179487,max:1;n:type:ShaderForge.SFN_Slider,id:8843,x:33971,y:32538,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:node_8843,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7521368,max:1;n:type:ShaderForge.SFN_Multiply,id:5015,x:33503,y:32021,varname:node_5015,prsc:2|A-1304-RGB,B-6986-OUT;n:type:ShaderForge.SFN_Panner,id:5148,x:32545,y:33172,varname:node_5148,prsc:2,spu:-1,spv:1|UVIN-5710-UVOUT,DIST-931-OUT;n:type:ShaderForge.SFN_Tex2d,id:52,x:32798,y:33276,ptovrint:False,ptlb:node_52,ptin:_node_52,varname:node_52,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:2,isnm:False|UVIN-5148-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:116,x:34320,y:32930,ptovrint:False,ptlb:Tesselation,ptin:_Tesselation,varname:node_116,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_OneMinus,id:8088,x:33517,y:32465,varname:node_8088,prsc:2|IN-6986-OUT;n:type:ShaderForge.SFN_Color,id:2788,x:33443,y:32222,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:node_2788,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:9703,x:33785,y:32286,varname:node_9703,prsc:2|A-8088-OUT,B-2788-RGB;n:type:ShaderForge.SFN_Vector1,id:9189,x:34401,y:32645,varname:node_9189,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:7050,x:34315,y:33258,ptovrint:False,ptlb:Refraction Amount,ptin:_RefractionAmount,varname:node_4295,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_DepthBlend,id:6819,x:33831,y:33214,varname:node_6819,prsc:2|DIST-8095-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:8545,x:34104,y:33240,varname:node_8545,prsc:2,min:0,max:1|IN-6819-OUT;n:type:ShaderForge.SFN_Multiply,id:1907,x:34433,y:33042,varname:node_1907,prsc:2|A-3740-OUT,B-7050-OUT;n:type:ShaderForge.SFN_ComponentMask,id:9275,x:33692,y:32687,varname:node_9275,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-5076-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1118,x:34320,y:32764,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_1118,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_ViewPosition,id:3523,x:33353,y:33374,varname:node_3523,prsc:2;n:type:ShaderForge.SFN_Distance,id:8983,x:33636,y:33479,varname:node_8983,prsc:2|A-3523-XYZ,B-5750-XYZ;n:type:ShaderForge.SFN_ObjectPosition,id:5750,x:33369,y:33670,varname:node_5750,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:3740,x:33983,y:33014,varname:node_3740,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-6986-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8095,x:33497,y:33262,ptovrint:False,ptlb:Depth Blend,ptin:_DepthBlend,varname:node_8095,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Add,id:9043,x:33984,y:32024,varname:node_9043,prsc:2|A-5015-OUT,B-9703-OUT;n:type:ShaderForge.SFN_Color,id:6587,x:33290,y:31501,ptovrint:False,ptlb:Fog Color,ptin:_FogColor,varname:node_46,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.282353,c2:0.6196079,c3:0.5764706,c4:1;n:type:ShaderForge.SFN_Multiply,id:6996,x:34153,y:31759,varname:node_6996,prsc:2|A-9346-OUT,B-3799-OUT,C-9043-OUT,D-3042-OUT;n:type:ShaderForge.SFN_DepthBlend,id:3799,x:33622,y:31716,varname:node_3799,prsc:2|DIST-3724-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3724,x:33140,y:31731,ptovrint:False,ptlb:Fog Height,ptin:_FogHeight,varname:node_915,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_LightColor,id:6747,x:33290,y:31351,varname:node_6747,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9346,x:33556,y:31412,varname:node_9346,prsc:2|A-6747-RGB,B-6587-RGB;n:type:ShaderForge.SFN_Fresnel,id:3042,x:34188,y:31282,varname:node_3042,prsc:2|NRM-5166-OUT,EXP-9168-OUT;n:type:ShaderForge.SFN_NormalVector,id:5166,x:33613,y:31190,prsc:2,pt:False;n:type:ShaderForge.SFN_Slider,id:9168,x:33747,y:31351,ptovrint:False,ptlb:Fresnel Exponent,ptin:_FresnelExponent,varname:node_9168,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.955653,max:1.5;n:type:ShaderForge.SFN_Clamp01,id:5076,x:33527,y:32674,varname:node_5076,prsc:2|IN-9043-OUT;n:type:ShaderForge.SFN_Clamp01,id:696,x:34473,y:31920,varname:node_696,prsc:2|IN-6996-OUT;proporder:755-8843-1304-2788-3088-1167-116-6341-8640-52-7050-1118-8095-6587-3724-9168;pass:END;sub:END;*/

Shader "Shader Forge/AcidLayer" {
    Properties {
        _Specular ("Specular", Range(0, 1)) = 0.7179487
        _Roughness ("Roughness", Range(0, 1)) = 0.7521368
        _Color1 ("Color1", Color) = (0.0216263,0.1137096,0.2941176,1)
        _Color2 ("Color2", Color) = (0.5,0.5,0.5,1)
        _VertexOffset ("VertexOffset", Range(0, 0.5)) = 0.361024
        _Speed ("Speed", Float ) = 0.1
        _Tesselation ("Tesselation", Float ) = 1
        _node_6341 ("node_6341", 2D) = "black" {}
        _node_8640 ("node_8640", 2D) = "black" {}
        _node_52 ("node_52", 2D) = "black" {}
        _RefractionAmount ("Refraction Amount", Range(0, 1)) = 0
        _Opacity ("Opacity", Float ) = 0.5
        _DepthBlend ("Depth Blend", Float ) = 0.5
        _FogColor ("Fog Color", Color) = (0.282353,0.6196079,0.5764706,1)
        _FogHeight ("Fog Height", Float ) = 1
        _FresnelExponent ("Fresnel Exponent", Range(0, 1.5)) = 0.955653
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+2"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 5.0
            #pragma glsl
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float4 _Color1;
            uniform sampler2D _node_6341; uniform float4 _node_6341_ST;
            uniform sampler2D _node_8640; uniform float4 _node_8640_ST;
            uniform float _VertexOffset;
            uniform float _Speed;
            uniform float _Specular;
            uniform float _Roughness;
            uniform sampler2D _node_52; uniform float4 _node_52_ST;
            uniform float _Tesselation;
            uniform float4 _Color2;
            uniform float _RefractionAmount;
            uniform float _Opacity;
            uniform float4 _FogColor;
            uniform float _FogHeight;
            uniform float _FresnelExponent;
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
                float4 screenPos : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_9500 = _Time + _TimeEditor;
                float node_931 = (node_9500.r*_Speed);
                float2 node_3926 = (o.uv0+node_931*float2(1,1));
                float4 _node_6341_var = tex2Dlod(_node_6341,float4(TRANSFORM_TEX(node_3926, _node_6341),0.0,0));
                float2 node_4580 = (o.uv0+node_931*float2(1,-1));
                float4 _node_8640_var = tex2Dlod(_node_8640,float4(TRANSFORM_TEX(node_4580, _node_8640),0.0,0));
                float2 node_5148 = (o.uv0+node_931*float2(-1,1));
                float4 _node_52_var = tex2Dlod(_node_52,float4(TRANSFORM_TEX(node_5148, _node_52),0.0,0));
                float3 node_6986 = (_node_6341_var.rgb*_node_8640_var.rgb*_node_52_var.rgb);
                float3 node_9043 = ((_Color1.rgb*node_6986)+((1.0 - node_6986)*_Color2.rgb));
                float node_8490 = (saturate(node_9043).r*_VertexOffset);
                v.vertex.xyz += float3(node_8490,node_8490,node_8490);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return _Tesselation;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_9500 = _Time + _TimeEditor;
                float node_931 = (node_9500.r*_Speed);
                float2 node_3926 = (i.uv0+node_931*float2(1,1));
                float4 _node_6341_var = tex2D(_node_6341,TRANSFORM_TEX(node_3926, _node_6341));
                float2 node_4580 = (i.uv0+node_931*float2(1,-1));
                float4 _node_8640_var = tex2D(_node_8640,TRANSFORM_TEX(node_4580, _node_8640));
                float2 node_5148 = (i.uv0+node_931*float2(-1,1));
                float4 _node_52_var = tex2D(_node_52,TRANSFORM_TEX(node_5148, _node_52));
                float3 node_6986 = (_node_6341_var.rgb*_node_8640_var.rgb*_node_52_var.rgb);
                float3 normalLocal = node_6986;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_6986.rg*_RefractionAmount);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Roughness;
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
                NdotL = dot( normalDirection, lightDirection );
                float node_9189 = 1.0;
                float3 w = float3(node_9189,node_9189,node_9189)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 backLight = max(float3(0.0,0.0,0.0), -NdotLWrap + w ) * float3(node_9189,node_9189,node_9189);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                NdotLWrap = max(float3(0,0,0), NdotLWrap);
                float3 directDiffuse = ((forwardLight+backLight) + ((1 +(fd90 - 1)*pow((1.00001-NdotLWrap), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL))*(0.5-max(w.r,max(w.g,w.b))*0.5) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 node_9043 = ((_Color1.rgb*node_6986)+((1.0 - node_6986)*_Color2.rgb));
                float3 diffuseColor = saturate(((_LightColor0.rgb*_FogColor.rgb)*saturate((sceneZ-partZ)/_FogHeight)*node_9043*pow(1.0-max(0,dot(i.normalDir, viewDirection)),_FresnelExponent)));
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,_Opacity),1);
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
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 5.0
            #pragma glsl
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float4 _Color1;
            uniform sampler2D _node_6341; uniform float4 _node_6341_ST;
            uniform sampler2D _node_8640; uniform float4 _node_8640_ST;
            uniform float _VertexOffset;
            uniform float _Speed;
            uniform float _Specular;
            uniform float _Roughness;
            uniform sampler2D _node_52; uniform float4 _node_52_ST;
            uniform float _Tesselation;
            uniform float4 _Color2;
            uniform float _RefractionAmount;
            uniform float _Opacity;
            uniform float4 _FogColor;
            uniform float _FogHeight;
            uniform float _FresnelExponent;
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
                float4 screenPos : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_9500 = _Time + _TimeEditor;
                float node_931 = (node_9500.r*_Speed);
                float2 node_3926 = (o.uv0+node_931*float2(1,1));
                float4 _node_6341_var = tex2Dlod(_node_6341,float4(TRANSFORM_TEX(node_3926, _node_6341),0.0,0));
                float2 node_4580 = (o.uv0+node_931*float2(1,-1));
                float4 _node_8640_var = tex2Dlod(_node_8640,float4(TRANSFORM_TEX(node_4580, _node_8640),0.0,0));
                float2 node_5148 = (o.uv0+node_931*float2(-1,1));
                float4 _node_52_var = tex2Dlod(_node_52,float4(TRANSFORM_TEX(node_5148, _node_52),0.0,0));
                float3 node_6986 = (_node_6341_var.rgb*_node_8640_var.rgb*_node_52_var.rgb);
                float3 node_9043 = ((_Color1.rgb*node_6986)+((1.0 - node_6986)*_Color2.rgb));
                float node_8490 = (saturate(node_9043).r*_VertexOffset);
                v.vertex.xyz += float3(node_8490,node_8490,node_8490);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return _Tesselation;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_9500 = _Time + _TimeEditor;
                float node_931 = (node_9500.r*_Speed);
                float2 node_3926 = (i.uv0+node_931*float2(1,1));
                float4 _node_6341_var = tex2D(_node_6341,TRANSFORM_TEX(node_3926, _node_6341));
                float2 node_4580 = (i.uv0+node_931*float2(1,-1));
                float4 _node_8640_var = tex2D(_node_8640,TRANSFORM_TEX(node_4580, _node_8640));
                float2 node_5148 = (i.uv0+node_931*float2(-1,1));
                float4 _node_52_var = tex2D(_node_52,TRANSFORM_TEX(node_5148, _node_52));
                float3 node_6986 = (_node_6341_var.rgb*_node_8640_var.rgb*_node_52_var.rgb);
                float3 normalLocal = node_6986;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_6986.rg*_RefractionAmount);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Roughness;
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
                NdotL = dot( normalDirection, lightDirection );
                float node_9189 = 1.0;
                float3 w = float3(node_9189,node_9189,node_9189)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 backLight = max(float3(0.0,0.0,0.0), -NdotLWrap + w ) * float3(node_9189,node_9189,node_9189);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                NdotLWrap = max(float3(0,0,0), NdotLWrap);
                float3 directDiffuse = ((forwardLight+backLight) + ((1 +(fd90 - 1)*pow((1.00001-NdotLWrap), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL))*(0.5-max(w.r,max(w.g,w.b))*0.5) * attenColor;
                float3 node_9043 = ((_Color1.rgb*node_6986)+((1.0 - node_6986)*_Color2.rgb));
                float3 diffuseColor = saturate(((_LightColor0.rgb*_FogColor.rgb)*saturate((sceneZ-partZ)/_FogHeight)*node_9043*pow(1.0-max(0,dot(i.normalDir, viewDirection)),_FresnelExponent)));
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * _Opacity,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 5.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform float4 _Color1;
            uniform sampler2D _node_6341; uniform float4 _node_6341_ST;
            uniform sampler2D _node_8640; uniform float4 _node_8640_ST;
            uniform float _VertexOffset;
            uniform float _Speed;
            uniform sampler2D _node_52; uniform float4 _node_52_ST;
            uniform float _Tesselation;
            uniform float4 _Color2;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_9500 = _Time + _TimeEditor;
                float node_931 = (node_9500.r*_Speed);
                float2 node_3926 = (o.uv0+node_931*float2(1,1));
                float4 _node_6341_var = tex2Dlod(_node_6341,float4(TRANSFORM_TEX(node_3926, _node_6341),0.0,0));
                float2 node_4580 = (o.uv0+node_931*float2(1,-1));
                float4 _node_8640_var = tex2Dlod(_node_8640,float4(TRANSFORM_TEX(node_4580, _node_8640),0.0,0));
                float2 node_5148 = (o.uv0+node_931*float2(-1,1));
                float4 _node_52_var = tex2Dlod(_node_52,float4(TRANSFORM_TEX(node_5148, _node_52),0.0,0));
                float3 node_6986 = (_node_6341_var.rgb*_node_8640_var.rgb*_node_52_var.rgb);
                float3 node_9043 = ((_Color1.rgb*node_6986)+((1.0 - node_6986)*_Color2.rgb));
                float node_8490 = (saturate(node_9043).r*_VertexOffset);
                v.vertex.xyz += float3(node_8490,node_8490,node_8490);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return _Tesselation;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
