// Made with Amplify Shader Editor v1.9.1.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Cainos/Customizable Pixel Character/Hair"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_SkinShadeTex("Skin Shade Tex", 2D) = "white" {}
		_RampTex("Ramp Tex", 2D) = "white" {}
		_HairValueTex("Hair Value Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite On
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform sampler2D _HairValueTex;
			uniform float4 _HairValueTex_ST;
			uniform sampler2D _RampTex;
			uniform sampler2D _SkinShadeTex;
			uniform float4 _SkinShadeTex_ST;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_HairValueTex = IN.texcoord.xy * _HairValueTex_ST.xy + _HairValueTex_ST.zw;
				float4 tex2DNode7 = tex2D( _HairValueTex, uv_HairValueTex );
				clip( tex2DNode7.a - 0.01);
				float4 ShadowColor32 = tex2DNode7;
				float grayscale11 = Luminance(tex2DNode7.rgb);
				#ifdef UNITY_COLORSPACE_GAMMA
				float staticSwitch49 = grayscale11;
				#else
				float staticSwitch49 = pow( grayscale11 , 0.32 );
				#endif
				float Greyscale13 = staticSwitch49;
				float2 appendResult16 = (float2(Greyscale13 , Greyscale13));
				float4 color19 = IsGammaSpace() ? float4(0.68,0.52,0.4,1) : float4(0.4200333,0.233022,0.1328683,1);
				float2 uv_SkinShadeTex = IN.texcoord.xy * _SkinShadeTex_ST.xy + _SkinShadeTex_ST.zw;
				float luminance65 = Luminance(tex2D( _SkinShadeTex, uv_SkinShadeTex ).rgb);
				float3 temp_cast_2 = (luminance65).xxx;
				half3 linearToGamma68 = temp_cast_2;
				linearToGamma68 = half3( LinearToGammaSpaceExact(linearToGamma68.r), LinearToGammaSpaceExact(linearToGamma68.g), LinearToGammaSpaceExact(linearToGamma68.b) );
				float3 temp_cast_3 = (luminance65).xxx;
				#ifdef UNITY_COLORSPACE_GAMMA
				float3 staticSwitch67 = temp_cast_3;
				#else
				float3 staticSwitch67 = linearToGamma68;
				#endif
				float4 lerpResult24 = lerp( tex2D( _RampTex, appendResult16 ) , color19 , float4( staticSwitch67 , 0.0 ));
				float4 HairColor22 = lerpResult24;
				float IsShadow31 = ( ( ( tex2DNode7.r - tex2DNode7.g ) < 0.01 ? 1.0 : 0.0 ) * ( ( tex2DNode7.r - tex2DNode7.b ) < 0.01 ? 1.0 : 0.0 ) );
				float4 lerpResult33 = lerp( ShadowColor32 , HairColor22 , IsShadow31);
				
				fixed4 c = lerpResult33;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19102
Node;AmplifyShaderEditor.SamplerNode;15;-1448.809,543.7746;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;26;-1305.107,-56.85229;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-1302.829,90.80352;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-882.5092,28.98635;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-729.8909,23.474;Inherit;False;IsShadow;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClipNode;8;-1313.934,-276.2669;Inherit;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0.01;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-943.7798,-343.4242;Inherit;False;ShadowColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;33;-258.6688,230.3995;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;-843.0235,768.4109;Inherit;False;HairColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;-465.8622,343.4253;Inherit;False;31;IsShadow;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;24;-1043.458,773.3892;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-1920.117,668.2694;Inherit;False;13;Greyscale;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-1677.227,655.5414;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;19;-1298.99,787.0502;Inherit;False;Constant;_SkinShadeColor;Skin Shade Color;3;0;Create;True;0;0;0;False;0;False;0.68,0.52,0.4,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;11;-940.365,-214.2864;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;67;-1397.147,1057.513;Inherit;False;Property;_LinearColorSpace1;Linear Color Space;3;0;Create;True;0;0;0;False;0;False;0;1;1;False;UNITY_COLORSPACE_GAMMA;Toggle;2;Key0;Key1;Fetch;False;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;25;-2102.057,1044.506;Inherit;True;Property;_TextureSample2;Texture Sample 2;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LuminanceNode;65;-1807.391,1055.245;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;49;-498.7641,-235.7103;Inherit;False;Property;_LinearColorSpace;Linear Color Space;3;0;Create;True;0;0;0;False;0;False;0;1;1;False;UNITY_COLORSPACE_GAMMA;Toggle;2;Key0;Key1;Fetch;False;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LinearToGammaNode;68;-1590.49,1027.808;Inherit;False;1;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;70;-697.1916,-270.7003;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0.32;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;28;-1096.836,-56.51617;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0.01;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;29;-1101.395,101.9487;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0.01;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;18;-2400.302,1038.701;Inherit;True;Property;_SkinShadeTex;Skin Shade Tex;0;0;Create;True;0;0;0;False;0;False;1d1c6d7c787373a4f97be7b727b770e2;1d1c6d7c787373a4f97be7b727b770e2;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;9;-1834.182,452.1387;Inherit;True;Property;_RampTex;Ramp Tex;1;0;Create;True;0;0;0;False;0;False;0c92460b57c27fb4f91184df01e542e8;0c92460b57c27fb4f91184df01e542e8;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;23;-463.7112,254.6777;Inherit;False;22;HairColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;34;-477.6458,166.0914;Inherit;False;32;ShadowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-213.1568,-235.7793;Inherit;False;Greyscale;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;73;-38.13931,232.2776;Float;False;True;-1;2;ASEMaterialInspector;0;10;Cainos/Customizable Pixel Character/Hair;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;True;True;1;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.SamplerNode;7;-1716.354,-280.5356;Inherit;True;Property;_HairValueTex;Hair Value Texture;2;0;Create;False;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;15;0;9;0
WireConnection;15;1;16;0
WireConnection;26;0;7;1
WireConnection;26;1;7;2
WireConnection;27;0;7;1
WireConnection;27;1;7;3
WireConnection;30;0;28;0
WireConnection;30;1;29;0
WireConnection;31;0;30;0
WireConnection;8;0;7;0
WireConnection;8;1;7;4
WireConnection;32;0;8;0
WireConnection;33;0;34;0
WireConnection;33;1;23;0
WireConnection;33;2;35;0
WireConnection;22;0;24;0
WireConnection;24;0;15;0
WireConnection;24;1;19;0
WireConnection;24;2;67;0
WireConnection;16;0;17;0
WireConnection;16;1;17;0
WireConnection;11;0;8;0
WireConnection;67;1;68;0
WireConnection;67;0;65;0
WireConnection;25;0;18;0
WireConnection;65;0;25;0
WireConnection;49;1;70;0
WireConnection;49;0;11;0
WireConnection;68;0;65;0
WireConnection;70;0;11;0
WireConnection;28;0;26;0
WireConnection;29;0;27;0
WireConnection;13;0;49;0
WireConnection;73;0;33;0
ASEEND*/
//CHKSM=4C37185CDF43C0D8BE17B2131B023882FC7EEB0D