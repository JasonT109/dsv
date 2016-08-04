// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:33033,y:32623,varname:node_4795,prsc:2|emission-7695-OUT,custl-3596-OUT,alpha-6074-A;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32095,y:32527,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:84bec49e1c4f6fd4183ba646ecb3cbb5,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:797,x:31759,y:32858,ptovrint:True,ptlb:GradientColor1,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_TexCoord,id:7240,x:31647,y:33251,varname:node_7240,prsc:2,uv:0;n:type:ShaderForge.SFN_Lerp,id:5260,x:32171,y:33053,varname:node_5260,prsc:2|A-797-RGB,B-4755-RGB,T-7240-U;n:type:ShaderForge.SFN_Color,id:4755,x:31745,y:33041,ptovrint:False,ptlb:GradientColor2,ptin:_GradientColor2,varname:node_4755,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9705882,c2:0.546,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:7695,x:32570,y:32661,varname:node_7695,prsc:2|A-6074-RGB,B-5260-OUT,C-3474-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3474,x:31964,y:32795,ptovrint:False,ptlb:Brightness,ptin:_Brightness,varname:node_3474,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3596,x:32636,y:32862,varname:node_3596,prsc:2|A-5260-OUT,B-3474-OUT,C-4590-OUT;n:type:ShaderForge.SFN_RemapRange,id:4568,x:32103,y:33257,varname:node_4568,prsc:2,frmn:-100,frmx:100,tomn:0,tomx:1|IN-4497-X;n:type:ShaderForge.SFN_ScreenPos,id:4075,x:31959,y:33433,varname:node_4075,prsc:2,sctp:0;n:type:ShaderForge.SFN_ObjectPosition,id:4497,x:31910,y:33243,varname:node_4497,prsc:2;n:type:ShaderForge.SFN_Subtract,id:999,x:32406,y:33239,varname:node_999,prsc:2|A-4075-U,B-4568-OUT;n:type:ShaderForge.SFN_RemapRange,id:4590,x:32800,y:33207,varname:node_4590,prsc:2,frmn:0,frmx:2,tomn:0,tomx:1|IN-999-OUT;n:type:ShaderForge.SFN_ProjectionParameters,id:8194,x:31740,y:33480,varname:node_8194,prsc:2;proporder:6074-797-4755-3474;pass:END;sub:END;*/

Shader "Shader Forge/DCC_window_frame" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("GradientColor1", Color) = (0,0,0,1)
        _GradientColor2 ("GradientColor2", Color) = (0.9705882,0.546,0,1)
        _Brightness ("Brightness", Float ) = 1
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
                float3 finalColor = emissive + (node_5260*_Brightness*((i.screenPos.r-(objPos.r*0.005+0.5))*0.5+0.0));
                fixed4 finalRGBA = fixed4(finalColor,_MainTex_var.a);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
