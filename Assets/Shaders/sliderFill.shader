// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:1,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33218,y:32640,varname:node_3138,prsc:2|emission-5194-RGB,clip-4883-OUT;n:type:ShaderForge.SFN_Tex2d,id:5194,x:32435,y:32621,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_5194,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:7e9683c869919f04da901fa469fd1868,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:6883,x:32160,y:32875,varname:node_6883,prsc:2,uv:0;n:type:ShaderForge.SFN_Lerp,id:4489,x:32659,y:32954,varname:node_4489,prsc:2|A-2782-R,B-1015-R,T-5700-UVOUT;n:type:ShaderForge.SFN_Panner,id:5700,x:32403,y:32991,varname:node_5700,prsc:2,spu:0,spv:1|UVIN-6883-UVOUT,DIST-6712-OUT;n:type:ShaderForge.SFN_Color,id:1015,x:32470,y:33168,ptovrint:False,ptlb:color2,ptin:_color2,varname:node_1015,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5019608,c2:0.5019608,c3:0.5019608,c4:1;n:type:ShaderForge.SFN_Color,id:2782,x:32435,y:32826,ptovrint:False,ptlb:color1,ptin:_color1,varname:node_2782,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ComponentMask,id:4883,x:32949,y:33079,varname:node_4883,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-9247-OUT;n:type:ShaderForge.SFN_Round,id:9247,x:32794,y:33101,varname:node_9247,prsc:2|IN-4489-OUT;n:type:ShaderForge.SFN_ValueProperty,id:98,x:32019,y:33101,ptovrint:False,ptlb:sliderValue,ptin:_sliderValue,varname:node_98,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_OneMinus,id:6712,x:32210,y:33082,varname:node_6712,prsc:2|IN-98-OUT;proporder:5194-1015-2782-98;pass:END;sub:END;*/

Shader "Shader Forge/sliderFill" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        [HideInInspector]_color2 ("color2", Color) = (0.5019608,0.5019608,0.5019608,1)
        [HideInInspector]_color1 ("color1", Color) = (1,1,1,1)
        _sliderValue ("sliderValue", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="Geometry+1"
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _color2;
            uniform float4 _color1;
            uniform float _sliderValue;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                clip(round(lerp(float2(_color1.r,_color1.r),float2(_color2.r,_color2.r),(i.uv0+(1.0 - _sliderValue)*float2(0,1)))).g - 0.5);
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 emissive = _MainTex_var.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _color2;
            uniform float4 _color1;
            uniform float _sliderValue;
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
                clip(round(lerp(float2(_color1.r,_color1.r),float2(_color2.r,_color2.r),(i.uv0+(1.0 - _sliderValue)*float2(0,1)))).g - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
