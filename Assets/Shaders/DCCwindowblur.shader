// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33368,y:32628,varname:node_4013,prsc:2|custl-5228-OUT;n:type:ShaderForge.SFN_Set,id:2702,x:31945,y:32409,varname:__screenUV,prsc:2|IN-3452-UVOUT;n:type:ShaderForge.SFN_Get,id:9562,x:31459,y:32748,varname:node_9562,prsc:2|IN-3213-OUT;n:type:ShaderForge.SFN_ScreenPos,id:3452,x:31742,y:32409,varname:node_3452,prsc:2,sctp:2;n:type:ShaderForge.SFN_Set,id:3213,x:31945,y:32455,varname:__screenU,prsc:2|IN-3452-U;n:type:ShaderForge.SFN_Set,id:4564,x:31945,y:32499,varname:__screenV,prsc:2|IN-3452-V;n:type:ShaderForge.SFN_Slider,id:7596,x:31108,y:32185,ptovrint:False,ptlb:Offset,ptin:_Offset,varname:node_7596,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.2;n:type:ShaderForge.SFN_Set,id:512,x:31698,y:32266,varname:__offset,prsc:2|IN-1317-OUT;n:type:ShaderForge.SFN_Add,id:6974,x:31662,y:32853,varname:node_6974,prsc:2|A-9187-OUT,B-4699-OUT;n:type:ShaderForge.SFN_Get,id:9187,x:31444,y:32853,varname:node_9187,prsc:2|IN-512-OUT;n:type:ShaderForge.SFN_Get,id:4699,x:31434,y:32944,varname:node_4699,prsc:2|IN-4564-OUT;n:type:ShaderForge.SFN_Append,id:286,x:31951,y:32766,varname:node_286,prsc:2|A-9562-OUT,B-6974-OUT;n:type:ShaderForge.SFN_Get,id:320,x:31489,y:33032,varname:node_320,prsc:2|IN-4564-OUT;n:type:ShaderForge.SFN_Add,id:352,x:31663,y:33124,varname:node_352,prsc:2|A-1654-OUT,B-7494-OUT;n:type:ShaderForge.SFN_Get,id:7494,x:31445,y:33124,varname:node_7494,prsc:2|IN-512-OUT;n:type:ShaderForge.SFN_Get,id:1654,x:31435,y:33215,varname:node_1654,prsc:2|IN-3213-OUT;n:type:ShaderForge.SFN_Append,id:9590,x:31918,y:33035,varname:node_9590,prsc:2|A-352-OUT,B-320-OUT;n:type:ShaderForge.SFN_Add,id:6158,x:32555,y:32919,varname:node_6158,prsc:2|A-1769-RGB,B-8689-RGB,C-1858-RGB,D-1606-RGB;n:type:ShaderForge.SFN_SceneColor,id:8689,x:32127,y:33057,varname:node_8689,prsc:2|UVIN-9590-OUT;n:type:ShaderForge.SFN_SceneColor,id:1769,x:32151,y:32802,varname:node_1769,prsc:2|UVIN-286-OUT;n:type:ShaderForge.SFN_Divide,id:5228,x:33081,y:32881,varname:node_5228,prsc:2|A-3911-OUT,B-5964-OUT;n:type:ShaderForge.SFN_Vector1,id:5964,x:32614,y:32530,varname:node_5964,prsc:2,v1:8;n:type:ShaderForge.SFN_Get,id:3578,x:31471,y:33360,varname:node_3578,prsc:2|IN-3213-OUT;n:type:ShaderForge.SFN_Get,id:8009,x:31427,y:33452,varname:node_8009,prsc:2|IN-512-OUT;n:type:ShaderForge.SFN_Get,id:7696,x:31417,y:33543,varname:node_7696,prsc:2|IN-4564-OUT;n:type:ShaderForge.SFN_Append,id:5111,x:31900,y:33363,varname:node_5111,prsc:2|A-3578-OUT,B-8935-OUT;n:type:ShaderForge.SFN_SceneColor,id:1858,x:32124,y:33396,varname:node_1858,prsc:2|UVIN-5111-OUT;n:type:ShaderForge.SFN_Subtract,id:8935,x:31661,y:33416,varname:node_8935,prsc:2|A-8009-OUT,B-7696-OUT;n:type:ShaderForge.SFN_Get,id:7103,x:31363,y:33647,varname:node_7103,prsc:2|IN-4564-OUT;n:type:ShaderForge.SFN_Get,id:40,x:31363,y:33753,varname:node_40,prsc:2|IN-512-OUT;n:type:ShaderForge.SFN_Get,id:1802,x:31363,y:33826,varname:node_1802,prsc:2|IN-3213-OUT;n:type:ShaderForge.SFN_Append,id:3827,x:31903,y:33666,varname:node_3827,prsc:2|A-5243-OUT,B-7103-OUT;n:type:ShaderForge.SFN_SceneColor,id:1606,x:32146,y:33705,varname:node_1606,prsc:2|UVIN-3827-OUT;n:type:ShaderForge.SFN_Subtract,id:5243,x:31678,y:33704,varname:node_5243,prsc:2|A-1802-OUT,B-40-OUT;n:type:ShaderForge.SFN_SceneColor,id:6658,x:32354,y:32603,varname:node_6658,prsc:2|UVIN-1569-OUT;n:type:ShaderForge.SFN_Get,id:1569,x:32091,y:32603,varname:node_1569,prsc:2|IN-2702-OUT;n:type:ShaderForge.SFN_Add,id:3484,x:31660,y:33921,varname:node_3484,prsc:2|A-7944-OUT,B-634-OUT;n:type:ShaderForge.SFN_Get,id:7944,x:31262,y:33982,varname:node_7944,prsc:2|IN-2702-OUT;n:type:ShaderForge.SFN_Get,id:634,x:31250,y:34136,varname:node_634,prsc:2|IN-512-OUT;n:type:ShaderForge.SFN_Subtract,id:1947,x:31660,y:34121,varname:node_1947,prsc:2|A-7944-OUT,B-634-OUT;n:type:ShaderForge.SFN_Add,id:8051,x:32465,y:33948,varname:node_8051,prsc:2|A-1591-RGB,B-1557-RGB,C-725-OUT,D-3943-OUT;n:type:ShaderForge.SFN_SceneColor,id:1591,x:31903,y:33929,varname:node_1591,prsc:2|UVIN-3484-OUT;n:type:ShaderForge.SFN_SceneColor,id:1557,x:31903,y:34121,varname:node_1557,prsc:2|UVIN-1947-OUT;n:type:ShaderForge.SFN_Add,id:8404,x:31660,y:34361,varname:node_8404,prsc:2|A-6062-OUT,B-7194-OUT;n:type:ShaderForge.SFN_Subtract,id:7326,x:31660,y:34540,varname:node_7326,prsc:2|A-7194-OUT,B-6337-OUT;n:type:ShaderForge.SFN_Add,id:6256,x:31660,y:34736,varname:node_6256,prsc:2|A-1694-OUT,B-5928-OUT;n:type:ShaderForge.SFN_Subtract,id:9553,x:31660,y:34929,varname:node_9553,prsc:2|A-1694-OUT,B-3336-OUT;n:type:ShaderForge.SFN_Get,id:6062,x:31230,y:34369,varname:node_6062,prsc:2|IN-3213-OUT;n:type:ShaderForge.SFN_Get,id:7194,x:31230,y:34471,varname:node_7194,prsc:2|IN-512-OUT;n:type:ShaderForge.SFN_Get,id:6337,x:31245,y:34557,varname:node_6337,prsc:2|IN-4564-OUT;n:type:ShaderForge.SFN_Get,id:5928,x:31245,y:34694,varname:node_5928,prsc:2|IN-4564-OUT;n:type:ShaderForge.SFN_Get,id:1694,x:31235,y:34816,varname:node_1694,prsc:2|IN-512-OUT;n:type:ShaderForge.SFN_Get,id:3336,x:31235,y:34932,varname:node_3336,prsc:2|IN-3213-OUT;n:type:ShaderForge.SFN_Append,id:725,x:31944,y:34446,varname:node_725,prsc:2|A-8404-OUT,B-7326-OUT;n:type:ShaderForge.SFN_Append,id:3943,x:31954,y:34822,varname:node_3943,prsc:2|A-9553-OUT,B-6256-OUT;n:type:ShaderForge.SFN_Add,id:3911,x:32756,y:33053,varname:node_3911,prsc:2|A-6158-OUT,B-8051-OUT;n:type:ShaderForge.SFN_ObjectPosition,id:1096,x:31053,y:32295,varname:node_1096,prsc:2;n:type:ShaderForge.SFN_ViewPosition,id:4945,x:31053,y:32465,varname:node_4945,prsc:2;n:type:ShaderForge.SFN_Distance,id:7311,x:31265,y:32295,varname:node_7311,prsc:2|A-1096-XYZ,B-4945-XYZ;n:type:ShaderForge.SFN_Divide,id:1317,x:31481,y:32266,varname:node_1317,prsc:2|A-7596-OUT,B-7311-OUT;proporder:7596;pass:END;sub:END;*/

Shader "Shader Forge/DCCwindowblur" {
    Properties {
        _Offset ("Offset", Range(0, 0.2)) = 0
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
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
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
                float __screenU = sceneUVs.r;
                float __offset = (_Offset/distance(objPos.rgb,_WorldSpaceCameraPos));
                float __screenV = sceneUVs.g;
                float2 __screenUV = sceneUVs.rg;
                float2 node_7944 = __screenUV;
                float node_634 = __offset;
                float node_7194 = __offset;
                float node_1694 = __offset;
                float3 finalColor = (((tex2D( _GrabTexture, float2(__screenU,(__offset+__screenV))).rgb+tex2D( _GrabTexture, float2((__screenU+__offset),__screenV)).rgb+tex2D( _GrabTexture, float2(__screenU,(__offset-__screenV))).rgb+tex2D( _GrabTexture, float2((__screenU-__offset),__screenV)).rgb)+(tex2D( _GrabTexture, (node_7944+node_634)).rgb+tex2D( _GrabTexture, (node_7944-node_634)).rgb+float3(float2((__screenU+node_7194),(node_7194-__screenV)),0.0)+float3(float2((node_1694-__screenU),(node_1694+__screenV)),0.0)))/8.0);
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
