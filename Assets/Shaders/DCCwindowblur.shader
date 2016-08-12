// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:32689,y:32600,varname:node_4013,prsc:2|custl-5228-OUT;n:type:ShaderForge.SFN_Set,id:2702,x:31945,y:32409,varname:__screenUV,prsc:2|IN-3452-UVOUT;n:type:ShaderForge.SFN_Get,id:9562,x:31488,y:32761,varname:node_9562,prsc:2|IN-3213-OUT;n:type:ShaderForge.SFN_ScreenPos,id:3452,x:31742,y:32409,varname:node_3452,prsc:2,sctp:0;n:type:ShaderForge.SFN_Set,id:3213,x:31945,y:32455,varname:__screenU,prsc:2|IN-3452-U;n:type:ShaderForge.SFN_Set,id:4564,x:31945,y:32499,varname:__screenV,prsc:2|IN-3452-V;n:type:ShaderForge.SFN_Slider,id:7596,x:31268,y:32567,ptovrint:False,ptlb:Offset,ptin:_Offset,varname:node_7596,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Set,id:512,x:31702,y:32595,varname:__offset,prsc:2|IN-7596-OUT;n:type:ShaderForge.SFN_Add,id:6974,x:31662,y:32853,varname:node_6974,prsc:2|A-9187-OUT,B-4699-OUT;n:type:ShaderForge.SFN_Get,id:9187,x:31444,y:32853,varname:node_9187,prsc:2|IN-512-OUT;n:type:ShaderForge.SFN_Get,id:4699,x:31434,y:32944,varname:node_4699,prsc:2|IN-4564-OUT;n:type:ShaderForge.SFN_Append,id:286,x:31917,y:32764,varname:node_286,prsc:2|A-9562-OUT,B-6974-OUT;n:type:ShaderForge.SFN_Get,id:320,x:31489,y:33032,varname:node_320,prsc:2|IN-4564-OUT;n:type:ShaderForge.SFN_Add,id:352,x:31663,y:33124,varname:node_352,prsc:2|A-7494-OUT,B-1654-OUT;n:type:ShaderForge.SFN_Get,id:7494,x:31445,y:33124,varname:node_7494,prsc:2|IN-512-OUT;n:type:ShaderForge.SFN_Get,id:1654,x:31435,y:33215,varname:node_1654,prsc:2|IN-3213-OUT;n:type:ShaderForge.SFN_Append,id:9590,x:31918,y:33035,varname:node_9590,prsc:2|A-320-OUT,B-352-OUT;n:type:ShaderForge.SFN_Add,id:6158,x:32371,y:32890,varname:node_6158,prsc:2|A-1769-RGB,B-8689-RGB;n:type:ShaderForge.SFN_SceneColor,id:8689,x:32142,y:33068,varname:node_8689,prsc:2|UVIN-9590-OUT;n:type:ShaderForge.SFN_SceneColor,id:1769,x:32127,y:32716,varname:node_1769,prsc:2|UVIN-286-OUT;n:type:ShaderForge.SFN_Divide,id:5228,x:32445,y:32663,varname:node_5228,prsc:2|A-6158-OUT,B-5964-OUT;n:type:ShaderForge.SFN_Vector1,id:5964,x:32206,y:32570,varname:node_5964,prsc:2,v1:2;proporder:7596;pass:END;sub:END;*/

Shader "Shader Forge/DCCwindowblur" {
    Properties {
        _Offset ("Offset", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
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
            uniform sampler2D _GrabTexture;
            uniform float _Offset;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float __screenU = i.screenPos.r;
                float __offset = _Offset;
                float __screenV = i.screenPos.g;
                float3 finalColor = ((tex2D( _GrabTexture, float2(__screenU,(__offset+__screenV))).rgb+tex2D( _GrabTexture, float2(__screenV,(__offset+__screenU))).rgb)/2.0);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
