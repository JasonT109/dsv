// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:7,dpts:2,wrdp:True,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:1,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33443,y:32530,varname:node_3138,prsc:2|diff-7770-OUT,emission-7770-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32397,y:32370,ptovrint:False,ptlb:Color1,ptin:_Color1,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4485294,c2:0.6936274,c3:1,c4:1;n:type:ShaderForge.SFN_TexCoord,id:4326,x:31872,y:32603,varname:node_4326,prsc:2,uv:0;n:type:ShaderForge.SFN_Color,id:5775,x:32448,y:33028,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:node_5775,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.04411763,c2:0.04411763,c3:0.04411763,c4:0;n:type:ShaderForge.SFN_Lerp,id:7770,x:32990,y:32665,varname:node_7770,prsc:2|A-7241-RGB,B-5775-RGB,T-3985-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:5494,x:32084,y:32623,ptovrint:False,ptlb:Vertical,ptin:_Vertical,varname:node_5494,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-4326-U,B-4326-V;n:type:ShaderForge.SFN_Slider,id:4692,x:32025,y:32400,ptovrint:False,ptlb:Bias,ptin:_Bias,varname:node_4692,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.1794872,max:1;n:type:ShaderForge.SFN_Add,id:9454,x:32345,y:32563,varname:node_9454,prsc:2|A-5494-OUT,B-4692-OUT;n:type:ShaderForge.SFN_Step,id:7776,x:32265,y:32909,varname:node_7776,prsc:2|A-5494-OUT,B-786-OUT;n:type:ShaderForge.SFN_Slider,id:786,x:31824,y:32876,ptovrint:False,ptlb:Step,ptin:_Step,varname:node_786,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1025641,max:1;n:type:ShaderForge.SFN_Add,id:3985,x:32805,y:32683,varname:node_3985,prsc:2|A-1929-OUT,B-4858-OUT;n:type:ShaderForge.SFN_OneMinus,id:324,x:32505,y:32557,varname:node_324,prsc:2|IN-9454-OUT;n:type:ShaderForge.SFN_Divide,id:4858,x:32621,y:32857,varname:node_4858,prsc:2|A-7776-OUT,B-2167-OUT;n:type:ShaderForge.SFN_Vector1,id:2167,x:32421,y:32847,varname:node_2167,prsc:2,v1:3;n:type:ShaderForge.SFN_Multiply,id:1929,x:32693,y:32557,varname:node_1929,prsc:2|A-324-OUT,B-6526-OUT;n:type:ShaderForge.SFN_Slider,id:6526,x:32295,y:32732,ptovrint:False,ptlb:Blend,ptin:_Blend,varname:node_6526,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.931624,max:2;proporder:7241-5775-5494-4692-786-6526;pass:END;sub:END;*/

Shader "Shader Forge/UnlitGradient" {
    Properties {
        _Color1 ("Color1", Color) = (0.4485294,0.6936274,1,1)
        _Color2 ("Color2", Color) = (0.04411763,0.04411763,0.04411763,0)
        [MaterialToggle] _Vertical ("Vertical", Float ) = 0
        _Bias ("Bias", Range(-1, 1)) = 0.1794872
        _Step ("Step", Range(0, 1)) = 0.1025641
        _Blend ("Blend", Range(0, 2)) = 1.931624
    }
    SubShader {
        Tags {
            "Queue"="Transparent+1"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _Color1;
            uniform float4 _Color2;
            uniform fixed _Vertical;
            uniform float _Bias;
            uniform float _Step;
            uniform float _Blend;
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
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float _Vertical_var = lerp( i.uv0.r, i.uv0.g, _Vertical );
                float3 node_7770 = lerp(_Color1.rgb,_Color2.rgb,(((1.0 - (_Vertical_var+_Bias))*_Blend)+(step(_Vertical_var,_Step)/3.0)));
                float3 diffuseColor = node_7770;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = node_7770;
/// Final Color:
                float3 finalColor = diffuse + emissive;
                return fixed4(finalColor,1);
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
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _Color1;
            uniform float4 _Color2;
            uniform fixed _Vertical;
            uniform float _Bias;
            uniform float _Step;
            uniform float _Blend;
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
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float _Vertical_var = lerp( i.uv0.r, i.uv0.g, _Vertical );
                float3 node_7770 = lerp(_Color1.rgb,_Color2.rgb,(((1.0 - (_Vertical_var+_Bias))*_Blend)+(step(_Vertical_var,_Step)/3.0)));
                float3 diffuseColor = node_7770;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
