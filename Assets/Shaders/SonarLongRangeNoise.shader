// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:33037,y:32645,varname:node_4795,prsc:2|emission-552-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32283,y:32644,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:797,x:32204,y:33046,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.0008109869,c2:0.07792007,c3:0.1102941,c4:1;n:type:ShaderForge.SFN_Noise,id:9730,x:32214,y:32866,varname:node_9730,prsc:2|XY-6980-OUT;n:type:ShaderForge.SFN_Time,id:9199,x:31828,y:32808,varname:node_9199,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:1994,x:31828,y:32602,varname:node_1994,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:552,x:32488,y:32930,varname:node_552,prsc:2|A-9730-OUT,B-797-RGB;n:type:ShaderForge.SFN_Add,id:6980,x:32031,y:32866,varname:node_6980,prsc:2|A-1994-UVOUT,B-9199-T;proporder:6074-797;pass:END;sub:END;*/

Shader "Shader Forge/SonarLongRangeNoise" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.0008109869,0.07792007,0.1102941,1)
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _TintColor;
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
                float4 node_9199 = _Time + _TimeEditor;
                float2 node_6980 = (i.uv0+node_9199.g);
                float2 node_9730_skew = node_6980 + 0.2127+node_6980.x*0.3713*node_6980.y;
                float2 node_9730_rnd = 4.789*sin(489.123*(node_9730_skew));
                float node_9730 = frac(node_9730_rnd.x*node_9730_rnd.y*(1+node_9730_skew.x));
                float3 emissive = (node_9730*_TintColor.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
