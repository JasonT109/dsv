// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33099,y:32690,varname:node_3138,prsc:2|emission-2202-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32329,y:32424,ptovrint:False,ptlb:Start Color,ptin:_StartColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_TexCoord,id:2568,x:32003,y:32774,varname:node_2568,prsc:2,uv:0;n:type:ShaderForge.SFN_Color,id:1480,x:32315,y:32648,ptovrint:False,ptlb:End Color,ptin:_EndColor,varname:node_1480,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Lerp,id:2202,x:32663,y:32748,varname:node_2202,prsc:2|A-7241-RGB,B-1480-RGB,T-9355-OUT;n:type:ShaderForge.SFN_Add,id:9355,x:32619,y:33004,varname:node_9355,prsc:2|A-4146-OUT,B-9868-OUT;n:type:ShaderForge.SFN_Slider,id:9868,x:32234,y:33167,ptovrint:False,ptlb:Gradient Bias,ptin:_GradientBias,varname:node_9868,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_SwitchProperty,id:4146,x:32267,y:32916,ptovrint:False,ptlb:vDirection,ptin:_vDirection,varname:node_4146,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-2568-U,B-2568-V;proporder:7241-1480-9868-4146;pass:END;sub:END;*/

Shader "Shader Forge/DCC_button_on" {
    Properties {
        _StartColor ("Start Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _EndColor ("End Color", Color) = (1,0,0,1)
        _GradientBias ("Gradient Bias", Range(-1, 1)) = 0
        [MaterialToggle] _vDirection ("vDirection", Float ) = 0
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _StartColor;
            uniform float4 _EndColor;
            uniform float _GradientBias;
            uniform fixed _vDirection;
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
////// Lighting:
////// Emissive:
                float3 emissive = lerp(_StartColor.rgb,_EndColor.rgb,(lerp( i.uv0.r, i.uv0.g, _vDirection )+_GradientBias));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
