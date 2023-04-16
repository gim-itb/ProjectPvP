// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32929,y:32687,varname:node_4795,prsc:2|emission-2053-RGB,clip-8463-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32657,y:32578,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:8831,x:31006,y:32594,varname:node_8831,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_RemapRange,id:1806,x:31829,y:32732,varname:node_1806,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-2258-OUT;n:type:ShaderForge.SFN_Length,id:2816,x:32012,y:32732,varname:node_2816,prsc:2|IN-1806-OUT;n:type:ShaderForge.SFN_Step,id:5664,x:32408,y:32892,varname:node_5664,prsc:2|A-3141-OUT,B-8831-Z;n:type:ShaderForge.SFN_OneMinus,id:3141,x:32182,y:32708,varname:node_3141,prsc:2|IN-2816-OUT;n:type:ShaderForge.SFN_Multiply,id:8463,x:32657,y:32819,varname:node_8463,prsc:2|A-9739-OUT,B-5664-OUT;n:type:ShaderForge.SFN_Ceil,id:9739,x:32398,y:32708,varname:node_9739,prsc:2|IN-3141-OUT;n:type:ShaderForge.SFN_Posterize,id:8393,x:31399,y:32717,varname:node_8393,prsc:2|IN-8831-UVOUT,STPS-7757-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7407,x:30952,y:32917,ptovrint:False,ptlb:Pixelation,ptin:_Pixelation,varname:node_7407,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:25;n:type:ShaderForge.SFN_Round,id:7757,x:31163,y:32873,varname:node_7757,prsc:2|IN-7407-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:2258,x:31574,y:32655,ptovrint:False,ptlb:IsPixelated,ptin:_IsPixelated,varname:node_2258,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-8831-UVOUT,B-8393-OUT;proporder:7407-2258;pass:END;sub:END;*/

Shader "IceVFX/CircleDissolve" {
    Properties {
        _Pixelation ("Pixelation", Float ) = 25
        [MaterialToggle] _IsPixelated ("IsPixelated", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _Pixelation)
                UNITY_DEFINE_INSTANCED_PROP( fixed, _IsPixelated)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float _Pixelation_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Pixelation );
                float node_7757 = round(_Pixelation_var);
                float2 node_8393 = floor(i.uv0 * node_7757) / (node_7757 - 1);
                float2 _IsPixelated_var = lerp( i.uv0, node_8393, UNITY_ACCESS_INSTANCED_PROP( Props, _IsPixelated ) );
                float node_3141 = (1.0 - length((_IsPixelated_var*2.0+-1.0)));
                float node_5664 = step(node_3141,i.uv0.b);
                clip((ceil(node_3141)*node_5664) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = i.vertexColor.rgb;
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
