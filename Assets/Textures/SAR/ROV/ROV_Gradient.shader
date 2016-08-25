// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33256,y:32729,varname:node_4013,prsc:2|diff-968-OUT,emission-3234-OUT,alpha-7664-OUT,clip-6664-A;n:type:ShaderForge.SFN_Slider,id:8159,x:32089,y:32464,ptovrint:False,ptlb:Bias,ptin:_Bias,varname:node_4692,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.1794872,max:1;n:type:ShaderForge.SFN_TexCoord,id:3495,x:31936,y:32667,varname:node_3495,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:6459,x:31888,y:32940,ptovrint:False,ptlb:Step,ptin:_Step,varname:node_786,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1025641,max:1;n:type:ShaderForge.SFN_SwitchProperty,id:1826,x:32148,y:32687,ptovrint:False,ptlb:Vertical,ptin:_Vertical,varname:node_5494,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-3495-U,B-3495-V;n:type:ShaderForge.SFN_Add,id:289,x:32409,y:32627,varname:node_289,prsc:2|A-1826-OUT,B-8159-OUT;n:type:ShaderForge.SFN_Color,id:1747,x:32461,y:32434,ptovrint:False,ptlb:Color1,ptin:_Color1,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4485294,c2:0.6936274,c3:1,c4:1;n:type:ShaderForge.SFN_Slider,id:3845,x:32359,y:32796,ptovrint:False,ptlb:Blend,ptin:_Blend,varname:node_6526,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.931624,max:2;n:type:ShaderForge.SFN_Step,id:2267,x:32329,y:32973,varname:node_2267,prsc:2|A-1826-OUT,B-6459-OUT;n:type:ShaderForge.SFN_Vector1,id:3833,x:32485,y:32911,varname:node_3833,prsc:2,v1:3;n:type:ShaderForge.SFN_Color,id:8729,x:32512,y:33092,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:node_5775,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.04411763,c2:0.04411763,c3:0.04411763,c4:0;n:type:ShaderForge.SFN_Divide,id:4822,x:32685,y:32921,varname:node_4822,prsc:2|A-2267-OUT,B-3833-OUT;n:type:ShaderForge.SFN_Add,id:7664,x:32914,y:32793,varname:node_7664,prsc:2|A-9253-OUT,B-4822-OUT;n:type:ShaderForge.SFN_Multiply,id:9253,x:32757,y:32621,varname:node_9253,prsc:2|A-3001-OUT,B-3845-OUT;n:type:ShaderForge.SFN_OneMinus,id:3001,x:32569,y:32621,varname:node_3001,prsc:2|IN-289-OUT;n:type:ShaderForge.SFN_Lerp,id:3234,x:32960,y:32638,varname:node_3234,prsc:2|A-1747-RGB,B-8729-RGB,T-7664-OUT;n:type:ShaderForge.SFN_Multiply,id:968,x:33174,y:32425,varname:node_968,prsc:2|A-3234-OUT,B-6664-RGB;n:type:ShaderForge.SFN_Tex2d,id:6664,x:32896,y:32465,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_6664,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;proporder:6664-8159-6459-1826-1747-3845-8729;pass:END;sub:END;*/

Shader "Shader Forge/ROV_Gradient" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Bias ("Bias", Range(-1, 1)) = 0.1794872
        _Step ("Step", Range(0, 1)) = 0.1025641
        [MaterialToggle] _Vertical ("Vertical", Float ) = 0
        _Color1 ("Color1", Color) = (0.4485294,0.6936274,1,1)
        _Blend ("Blend", Range(0, 2)) = 1.931624
        _Color2 ("Color2", Color) = (0.04411763,0.04411763,0.04411763,0)
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
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _Bias;
            uniform float _Step;
            uniform fixed _Vertical;
            uniform float4 _Color1;
            uniform float _Blend;
            uniform float4 _Color2;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
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
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(_Diffuse_var.a - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float _Vertical_var = lerp( i.uv0.r, i.uv0.g, _Vertical );
                float node_7664 = (((1.0 - (_Vertical_var+_Bias))*_Blend)+(step(_Vertical_var,_Step)/3.0));
                float3 node_3234 = lerp(_Color1.rgb,_Color2.rgb,node_7664);
                float3 diffuseColor = (node_3234*_Diffuse_var.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = node_3234;
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,node_7664);
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
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _Bias;
            uniform float _Step;
            uniform fixed _Vertical;
            uniform float4 _Color1;
            uniform float _Blend;
            uniform float4 _Color2;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
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
                float3 normalDirection = i.normalDir;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(_Diffuse_var.a - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float _Vertical_var = lerp( i.uv0.r, i.uv0.g, _Vertical );
                float node_7664 = (((1.0 - (_Vertical_var+_Bias))*_Blend)+(step(_Vertical_var,_Step)/3.0));
                float3 node_3234 = lerp(_Color1.rgb,_Color2.rgb,node_7664);
                float3 diffuseColor = (node_3234*_Diffuse_var.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * node_7664,0);
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
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                clip(_Diffuse_var.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
