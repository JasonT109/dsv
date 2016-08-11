// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1,x:34612,y:31542,varname:node_1,prsc:2|emission-391-OUT;n:type:ShaderForge.SFN_Floor,id:2,x:31966,y:31701,cmnt:p,varname:node_2,prsc:2|IN-151-OUT;n:type:ShaderForge.SFN_Frac,id:3,x:31829,y:31960,varname:node_3,prsc:2|IN-151-OUT;n:type:ShaderForge.SFN_Multiply,id:5,x:32032,y:32087,varname:node_5,prsc:2|A-3-OUT,B-8-OUT;n:type:ShaderForge.SFN_Vector1,id:6,x:32019,y:31994,varname:node_6,prsc:2,v1:3;n:type:ShaderForge.SFN_Vector1,id:8,x:31829,y:32105,varname:node_8,prsc:2,v1:2;n:type:ShaderForge.SFN_Subtract,id:9,x:32198,y:32004,varname:node_9,prsc:2|A-6-OUT,B-5-OUT;n:type:ShaderForge.SFN_Multiply,id:10,x:33090,y:31870,cmnt:f,varname:node_10,prsc:2|A-3-OUT,B-3-OUT,C-9-OUT;n:type:ShaderForge.SFN_ComponentMask,id:11,x:32126,y:31648,varname:node_11,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-2-OUT;n:type:ShaderForge.SFN_Vector1,id:12,x:32137,y:31828,varname:node_12,prsc:2,v1:57;n:type:ShaderForge.SFN_Multiply,id:13,x:32336,y:31709,varname:node_13,prsc:2|A-11-G,B-12-OUT;n:type:ShaderForge.SFN_Add,id:14,x:32486,y:31641,cmnt:n,varname:node_14,prsc:2|A-11-R,B-13-OUT;n:type:ShaderForge.SFN_Sin,id:16,x:33059,y:31205,varname:node_16,prsc:2|IN-17-OUT;n:type:ShaderForge.SFN_Add,id:17,x:32690,y:31210,varname:node_17,prsc:2|A-14-OUT,B-21-OUT;n:type:ShaderForge.SFN_Add,id:18,x:32708,y:31345,varname:node_18,prsc:2|A-14-OUT,B-22-OUT;n:type:ShaderForge.SFN_Add,id:19,x:32683,y:31489,varname:node_19,prsc:2|A-14-OUT,B-23-OUT;n:type:ShaderForge.SFN_Vector1,id:21,x:32442,y:31266,varname:node_21,prsc:2,v1:58;n:type:ShaderForge.SFN_Vector1,id:22,x:32460,y:31405,varname:node_22,prsc:2,v1:57;n:type:ShaderForge.SFN_Vector1,id:23,x:32501,y:31515,varname:node_23,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:24,x:33269,y:31193,varname:node_24,prsc:2|A-25-OUT,B-16-OUT;n:type:ShaderForge.SFN_Vector1,id:25,x:33059,y:31078,varname:node_25,prsc:2,v1:473.5854;n:type:ShaderForge.SFN_Multiply,id:27,x:33269,y:31349,varname:node_27,prsc:2|A-25-OUT,B-29-OUT;n:type:ShaderForge.SFN_Sin,id:29,x:33059,y:31360,varname:node_29,prsc:2|IN-18-OUT;n:type:ShaderForge.SFN_Multiply,id:31,x:33269,y:31510,varname:node_31,prsc:2|A-25-OUT,B-33-OUT;n:type:ShaderForge.SFN_Sin,id:33,x:33059,y:31521,varname:node_33,prsc:2|IN-19-OUT;n:type:ShaderForge.SFN_Multiply,id:35,x:33269,y:31657,varname:node_35,prsc:2|A-25-OUT,B-37-OUT;n:type:ShaderForge.SFN_Sin,id:37,x:33059,y:31668,varname:node_37,prsc:2|IN-14-OUT;n:type:ShaderForge.SFN_Frac,id:38,x:33478,y:31193,varname:node_38,prsc:2|IN-24-OUT;n:type:ShaderForge.SFN_Frac,id:40,x:33478,y:31336,varname:node_40,prsc:2|IN-27-OUT;n:type:ShaderForge.SFN_Frac,id:42,x:33478,y:31510,varname:node_42,prsc:2|IN-31-OUT;n:type:ShaderForge.SFN_Frac,id:44,x:33478,y:31657,varname:node_44,prsc:2|IN-35-OUT;n:type:ShaderForge.SFN_Lerp,id:45,x:33811,y:31300,varname:node_45,prsc:2|A-40-OUT,B-38-OUT,T-48-R;n:type:ShaderForge.SFN_Lerp,id:46,x:33779,y:31488,varname:node_46,prsc:2|A-44-OUT,B-42-OUT,T-48-R;n:type:ShaderForge.SFN_Lerp,id:47,x:33964,y:31386,varname:node_47,prsc:2|A-46-OUT,B-45-OUT,T-48-G;n:type:ShaderForge.SFN_ComponentMask,id:48,x:33406,y:31857,varname:node_48,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-10-OUT;n:type:ShaderForge.SFN_Multiply,id:151,x:31673,y:31874,varname:node_151,prsc:2|A-378-OUT,B-152-OUT;n:type:ShaderForge.SFN_Vector1,id:152,x:31220,y:32023,cmnt:frequency,varname:node_152,prsc:2,v1:30;n:type:ShaderForge.SFN_Vector2,id:197,x:31252,y:31553,cmnt:move,varname:node_197,prsc:2,v1:0.1,v2:0.1;n:type:ShaderForge.SFN_TexCoord,id:198,x:31353,y:31814,varname:node_198,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:200,x:31252,y:31673,varname:node_200,prsc:2;n:type:ShaderForge.SFN_Multiply,id:201,x:31451,y:31623,varname:node_201,prsc:2|A-197-OUT,B-200-T;n:type:ShaderForge.SFN_Vector1,id:203,x:33964,y:31608,cmnt:amplitude,varname:node_203,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:291,x:34141,y:32400,cmnt:a single octave of noise,varname:node_291,prsc:2|A-295-OUT,B-293-OUT;n:type:ShaderForge.SFN_Vector1,id:293,x:33892,y:32609,cmnt:amplitude,varname:node_293,prsc:2,v1:1;n:type:ShaderForge.SFN_Lerp,id:295,x:33892,y:32387,varname:node_295,prsc:2|A-299-OUT,B-297-OUT,T-301-G;n:type:ShaderForge.SFN_Lerp,id:297,x:33739,y:32301,varname:node_297,prsc:2|A-307-OUT,B-309-OUT,T-301-R;n:type:ShaderForge.SFN_Lerp,id:299,x:33707,y:32489,varname:node_299,prsc:2|A-303-OUT,B-305-OUT,T-301-R;n:type:ShaderForge.SFN_ComponentMask,id:301,x:33334,y:32858,varname:node_301,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-319-OUT;n:type:ShaderForge.SFN_Frac,id:303,x:33406,y:32658,varname:node_303,prsc:2|IN-317-OUT;n:type:ShaderForge.SFN_Frac,id:305,x:33406,y:32511,varname:node_305,prsc:2|IN-315-OUT;n:type:ShaderForge.SFN_Frac,id:307,x:33406,y:32337,varname:node_307,prsc:2|IN-313-OUT;n:type:ShaderForge.SFN_Frac,id:309,x:33406,y:32194,varname:node_309,prsc:2|IN-311-OUT;n:type:ShaderForge.SFN_Multiply,id:311,x:33197,y:32194,varname:node_311,prsc:2|A-329-OUT,B-327-OUT;n:type:ShaderForge.SFN_Multiply,id:313,x:33197,y:32350,varname:node_313,prsc:2|A-329-OUT,B-325-OUT;n:type:ShaderForge.SFN_Multiply,id:315,x:33197,y:32511,varname:node_315,prsc:2|A-329-OUT,B-323-OUT;n:type:ShaderForge.SFN_Multiply,id:317,x:33197,y:32658,varname:node_317,prsc:2|A-329-OUT,B-321-OUT;n:type:ShaderForge.SFN_Multiply,id:319,x:33018,y:32871,cmnt:f,varname:node_319,prsc:2|A-355-OUT,B-355-OUT,C-347-OUT;n:type:ShaderForge.SFN_Sin,id:321,x:32987,y:32669,varname:node_321,prsc:2|IN-343-OUT;n:type:ShaderForge.SFN_Sin,id:323,x:32987,y:32522,varname:node_323,prsc:2|IN-331-OUT;n:type:ShaderForge.SFN_Sin,id:325,x:32987,y:32361,varname:node_325,prsc:2|IN-333-OUT;n:type:ShaderForge.SFN_Sin,id:327,x:32987,y:32206,varname:node_327,prsc:2|IN-335-OUT;n:type:ShaderForge.SFN_Vector1,id:329,x:32987,y:32079,varname:node_329,prsc:2,v1:473.5854;n:type:ShaderForge.SFN_Add,id:331,x:32611,y:32490,varname:node_331,prsc:2|A-343-OUT,B-341-OUT;n:type:ShaderForge.SFN_Add,id:333,x:32636,y:32346,varname:node_333,prsc:2|A-343-OUT,B-339-OUT;n:type:ShaderForge.SFN_Add,id:335,x:32618,y:32211,varname:node_335,prsc:2|A-343-OUT,B-337-OUT;n:type:ShaderForge.SFN_Vector1,id:337,x:32370,y:32267,varname:node_337,prsc:2,v1:58;n:type:ShaderForge.SFN_Vector1,id:339,x:32388,y:32406,varname:node_339,prsc:2,v1:57;n:type:ShaderForge.SFN_Vector1,id:341,x:32429,y:32516,varname:node_341,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:343,x:32414,y:32642,cmnt:n,varname:node_343,prsc:2|A-349-R,B-345-OUT;n:type:ShaderForge.SFN_Multiply,id:345,x:32264,y:32710,varname:node_345,prsc:2|A-349-G,B-351-OUT;n:type:ShaderForge.SFN_Subtract,id:347,x:32126,y:33005,varname:node_347,prsc:2|A-359-OUT,B-357-OUT;n:type:ShaderForge.SFN_ComponentMask,id:349,x:32054,y:32649,varname:node_349,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-353-OUT;n:type:ShaderForge.SFN_Vector1,id:351,x:32065,y:32829,varname:node_351,prsc:2,v1:57;n:type:ShaderForge.SFN_Floor,id:353,x:31894,y:32702,cmnt:p,varname:node_353,prsc:2|IN-363-OUT;n:type:ShaderForge.SFN_Frac,id:355,x:31757,y:32961,varname:node_355,prsc:2|IN-363-OUT;n:type:ShaderForge.SFN_Multiply,id:357,x:31960,y:33088,varname:node_357,prsc:2|A-355-OUT,B-361-OUT;n:type:ShaderForge.SFN_Vector1,id:359,x:31947,y:32995,varname:node_359,prsc:2,v1:3;n:type:ShaderForge.SFN_Vector1,id:361,x:31757,y:33106,varname:node_361,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:363,x:31601,y:32875,varname:node_363,prsc:2|A-365-OUT,B-375-OUT;n:type:ShaderForge.SFN_Add,id:365,x:31565,y:32697,varname:node_365,prsc:2|A-369-OUT,B-367-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:367,x:31327,y:32823,varname:node_367,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:369,x:31379,y:32624,varname:node_369,prsc:2|A-371-OUT,B-373-T;n:type:ShaderForge.SFN_Vector2,id:371,x:31180,y:32554,cmnt:move,varname:node_371,prsc:2,v1:0.1,v2:0;n:type:ShaderForge.SFN_Time,id:373,x:31180,y:32674,varname:node_373,prsc:2;n:type:ShaderForge.SFN_Vector1,id:375,x:31148,y:33024,cmnt:frequency,varname:node_375,prsc:2,v1:16;n:type:ShaderForge.SFN_Add,id:378,x:31722,y:31605,varname:node_378,prsc:2|A-201-OUT,B-380-OUT;n:type:ShaderForge.SFN_Multiply,id:380,x:31529,y:32019,varname:node_380,prsc:2|A-198-UVOUT,B-291-OUT;n:type:ShaderForge.SFN_Multiply,id:391,x:34244,y:31572,varname:node_391,prsc:2|A-47-OUT,B-203-OUT;pass:END;sub:END;*/

Shader "Shader Forge/ValueNoise" {
    Properties {
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
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _TimeEditor;
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
                float node_25 = 473.5854;
                float4 node_200 = _Time + _TimeEditor;
                float node_329 = 473.5854;
                float4 node_373 = _Time + _TimeEditor;
                float2 node_363 = (((float2(0.1,0)*node_373.g)+i.uv0)*16.0);
                float2 node_349 = floor(node_363).rg;
                float node_343 = (node_349.r+(node_349.g*57.0)); // n
                float2 node_355 = frac(node_363);
                float2 node_301 = (node_355*node_355*(3.0-(node_355*2.0))).rg;
                float2 node_151 = (((float2(0.1,0.1)*node_200.g)+(i.uv0*(lerp(lerp(frac((node_329*sin(node_343))),frac((node_329*sin((node_343+1.0)))),node_301.r),lerp(frac((node_329*sin((node_343+57.0)))),frac((node_329*sin((node_343+58.0)))),node_301.r),node_301.g)*1.0)))*30.0);
                float2 node_11 = floor(node_151).rg;
                float node_14 = (node_11.r+(node_11.g*57.0)); // n
                float2 node_3 = frac(node_151);
                float2 node_48 = (node_3*node_3*(3.0-(node_3*2.0))).rg;
                float node_391 = (lerp(lerp(frac((node_25*sin(node_14))),frac((node_25*sin((node_14+1.0)))),node_48.r),lerp(frac((node_25*sin((node_14+57.0)))),frac((node_25*sin((node_14+58.0)))),node_48.r),node_48.g)*1.0);
                float3 emissive = float3(node_391,node_391,node_391);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
