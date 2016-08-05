// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:33033,y:32623,varname:node_4795,prsc:2|emission-7695-OUT,custl-3596-OUT,alpha-6074-A;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32095,y:32527,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:84bec49e1c4f6fd4183ba646ecb3cbb5,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:797,x:31759,y:32858,ptovrint:True,ptlb:GradientColor1,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_TexCoord,id:7240,x:31639,y:33253,varname:node_7240,prsc:2,uv:0;n:type:ShaderForge.SFN_Lerp,id:5260,x:32171,y:33053,varname:node_5260,prsc:2|A-797-RGB,B-4755-RGB,T-7240-U;n:type:ShaderForge.SFN_Color,id:4755,x:31745,y:33041,ptovrint:False,ptlb:GradientColor2,ptin:_GradientColor2,varname:node_4755,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9705882,c2:0.546,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:7695,x:32570,y:32661,varname:node_7695,prsc:2|A-6074-RGB,B-5260-OUT,C-3474-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3474,x:31964,y:32795,ptovrint:False,ptlb:Brightness,ptin:_Brightness,varname:node_3474,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3596,x:32647,y:32890,varname:node_3596,prsc:2|A-5260-OUT,B-3474-OUT,C-4893-OUT;n:type:ShaderForge.SFN_RemapRange,id:4568,x:32067,y:33331,varname:node_4568,prsc:2,frmn:-100,frmx:100,tomn:0,tomx:1|IN-4497-X;n:type:ShaderForge.SFN_ScreenPos,id:4075,x:32113,y:33167,varname:node_4075,prsc:2,sctp:0;n:type:ShaderForge.SFN_ObjectPosition,id:4497,x:31852,y:33303,varname:node_4497,prsc:2;n:type:ShaderForge.SFN_Subtract,id:999,x:32353,y:33283,varname:node_999,prsc:2|A-4075-U,B-4568-OUT;n:type:ShaderForge.SFN_RemapRange,id:4590,x:32587,y:33239,varname:node_4590,prsc:2,frmn:0,frmx:2,tomn:0,tomx:1|IN-999-OUT;n:type:ShaderForge.SFN_Sin,id:8699,x:32420,y:33613,varname:node_8699,prsc:2|IN-7821-OUT;n:type:ShaderForge.SFN_Multiply,id:7821,x:32242,y:33613,varname:node_7821,prsc:2|A-8618-OUT,B-5861-OUT,C-2428-OUT;n:type:ShaderForge.SFN_Tau,id:2428,x:31985,y:33897,varname:node_2428,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:3116,x:32627,y:33511,varname:node_3116,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-8699-OUT;n:type:ShaderForge.SFN_Add,id:4893,x:32944,y:33282,varname:node_4893,prsc:2|A-4590-OUT,B-9223-OUT;n:type:ShaderForge.SFN_ScreenPos,id:111,x:31721,y:33596,varname:node_111,prsc:2,sctp:0;n:type:ShaderForge.SFN_Rotator,id:8817,x:31919,y:33596,varname:node_8817,prsc:2|UVIN-111-UVOUT,ANG-3325-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8618,x:32090,y:33542,varname:node_8618,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-8817-UVOUT;n:type:ShaderForge.SFN_Slider,id:3325,x:31599,y:33898,ptovrint:False,ptlb:ChevronRotation,ptin:_ChevronRotation,varname:node_3325,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:-0.4544125,max:1;n:type:ShaderForge.SFN_Step,id:7769,x:32897,y:33575,varname:node_7769,prsc:2|A-3116-OUT,B-1291-OUT;n:type:ShaderForge.SFN_Vector1,id:1291,x:32721,y:33773,varname:node_1291,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:9223,x:33122,y:33413,varname:node_9223,prsc:2|A-7769-OUT,B-8442-OUT;n:type:ShaderForge.SFN_Slider,id:8442,x:32700,y:33481,ptovrint:False,ptlb:ChevronStrength,ptin:_ChevronStrength,varname:node_8442,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1445855,max:1;n:type:ShaderForge.SFN_Slider,id:5861,x:31754,y:33761,ptovrint:False,ptlb:ChevronAmount,ptin:_ChevronAmount,varname:node_5861,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:60,max:100;proporder:6074-797-4755-3474-3325-8442-5861;pass:END;sub:END;*/

Shader "Shader Forge/DCC_window_frame" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("GradientColor1", Color) = (0,0,0,1)
        _GradientColor2 ("GradientColor2", Color) = (0.9705882,0.546,0,1)
        _Brightness ("Brightness", Float ) = 1
        _ChevronRotation ("ChevronRotation", Range(-1, 1)) = -0.4544125
        _ChevronStrength ("ChevronStrength", Range(0, 1)) = 0.1445855
        _ChevronAmount ("ChevronAmount", Range(1, 100)) = 60
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
            Blend SrcAlpha OneMinusSrcAlpha
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float4 _GradientColor2;
            uniform float _Brightness;
            uniform float _ChevronRotation;
            uniform float _ChevronStrength;
            uniform float _ChevronAmount;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( _Object2World, float4(0,0,0,1) );
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_5260 = lerp(_TintColor.rgb,_GradientColor2.rgb,i.uv0.r);
                float3 emissive = (_MainTex_var.rgb*node_5260*_Brightness);
                float node_8817_ang = _ChevronRotation;
                float node_8817_spd = 1.0;
                float node_8817_cos = cos(node_8817_spd*node_8817_ang);
                float node_8817_sin = sin(node_8817_spd*node_8817_ang);
                float2 node_8817_piv = float2(0.5,0.5);
                float2 node_8817 = (mul(i.screenPos.rg-node_8817_piv,float2x2( node_8817_cos, -node_8817_sin, node_8817_sin, node_8817_cos))+node_8817_piv);
                float3 finalColor = emissive + (node_5260*_Brightness*(((i.screenPos.r-(objPos.r*0.005+0.5))*0.5+0.0)+(step((sin((node_8817.r*_ChevronAmount*6.28318530718))*0.5+0.5),0.5)*_ChevronStrength)));
                fixed4 finalRGBA = fixed4(finalColor,_MainTex_var.a);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
