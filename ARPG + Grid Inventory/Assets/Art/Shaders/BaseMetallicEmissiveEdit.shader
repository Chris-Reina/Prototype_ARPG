// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DoaT Studios/BaseMetallicEmissiveEdit"
{
	Properties
	{
		_Tint("Tint", Color) = (1,1,1,1)
		_BaseColor("BaseColor", 2D) = "white" {}
		_Metallic_Smoothness("Metallic_Smoothness", 2D) = "white" {}
		_SmoothnessIntensity("Smoothness Intensity", Range( 0 , 1)) = 1
		[Normal]_Normal("Normal", 2D) = "bump" {}
		_NormalIntensity("Normal Intensity", Float) = 1
		_Height("Height", 2D) = "white" {}
		_HeightScale("Height Scale", Range( 0 , 0.05)) = 0
		_Occlusion("Occlusion", 2D) = "white" {}
		_Emission("Emission", 2D) = "white" {}
		[HDR]_EmissionTint3("Emission Tint 3", Color) = (1,1,1,1)
		[HDR]_EmissionTint2("Emission Tint 2", Color) = (1,1,1,1)
		[HDR]_EmissionTint1("Emission Tint 1", Color) = (1,1,1,1)
		_TileandOffset("Tile and Offset", Vector) = (1,1,0,0)
		_OcclusionStrength("Occlusion Strength", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 5.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
			float3 worldNormal;
			float3 worldPos;
		};

		uniform float _NormalIntensity;
		uniform sampler2D _Normal;
		uniform float4 _TileandOffset;
		uniform sampler2D _Height;
		uniform float _HeightScale;
		uniform float4 _Height_ST;
		uniform float4 _Tint;
		uniform sampler2D _BaseColor;
		uniform sampler2D _Occlusion;
		uniform float _OcclusionStrength;
		uniform sampler2D _Emission;
		uniform float4 _EmissionTint1;
		uniform float4 _EmissionTint2;
		uniform float4 _EmissionTint3;
		uniform sampler2D _Metallic_Smoothness;
		uniform float _SmoothnessIntensity;


		inline float2 POM( sampler2D heightMap, float2 uvs, float2 dx, float2 dy, float3 normalWorld, float3 viewWorld, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv, int index )
		{
			float3 result = 0;
			int stepIndex = 0;
			int numSteps = ( int )lerp( (float)maxSamples, (float)minSamples, saturate( dot( normalWorld, viewWorld ) ) );
			float layerHeight = 1.0 / numSteps;
			float2 plane = parallax * ( viewDirTan.xy / viewDirTan.z );
			uvs += refPlane * plane;
			float2 deltaTex = -plane * layerHeight;
			float2 prevTexOffset = 0;
			float prevRayZ = 1.0f;
			float prevHeight = 0.0f;
			float2 currTexOffset = deltaTex;
			float currRayZ = 1.0f - layerHeight;
			float currHeight = 0.0f;
			float intersection = 0;
			float2 finalTexOffset = 0;
			while ( stepIndex < numSteps + 1 )
			{
				currHeight = tex2Dgrad( heightMap, uvs + currTexOffset, dx, dy ).r;
				if ( currHeight > currRayZ )
				{
					stepIndex = numSteps + 1;
				}
				else
				{
					stepIndex++;
					prevTexOffset = currTexOffset;
					prevRayZ = currRayZ;
					prevHeight = currHeight;
					currTexOffset += deltaTex;
					currRayZ -= layerHeight;
				}
			}
			int sectionSteps = 2;
			int sectionIndex = 0;
			float newZ = 0;
			float newHeight = 0;
			while ( sectionIndex < sectionSteps )
			{
				intersection = ( prevHeight - prevRayZ ) / ( prevHeight - currHeight + currRayZ - prevRayZ );
				finalTexOffset = prevTexOffset + intersection * deltaTex;
				newZ = prevRayZ - intersection * layerHeight;
				newHeight = tex2Dgrad( heightMap, uvs + finalTexOffset, dx, dy ).r;
				if ( newHeight > newZ )
				{
					currTexOffset = finalTexOffset;
					currHeight = newHeight;
					currRayZ = newZ;
					deltaTex = intersection * deltaTex;
					layerHeight = intersection * layerHeight;
				}
				else
				{
					prevTexOffset = finalTexOffset;
					prevHeight = newHeight;
					prevRayZ = newZ;
					deltaTex = ( 1 - intersection ) * deltaTex;
					layerHeight = ( 1 - intersection ) * layerHeight;
				}
				sectionIndex++;
			}
			return uvs + finalTexOffset;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult60 = (float2(_TileandOffset.x , _TileandOffset.y));
			float2 appendResult61 = (float2(_TileandOffset.z , _TileandOffset.w));
			float2 uv_TexCoord38 = i.uv_texcoord * appendResult60 + appendResult61;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float2 OffsetPOM37 = POM( _Height, uv_TexCoord38, ddx(uv_TexCoord38), ddy(uv_TexCoord38), ase_worldNormal, ase_worldViewDir, i.viewDir, 8, 8, _HeightScale, 0, _Height_ST.xy, float2(0,0), 0 );
			float2 TileUV62 = OffsetPOM37;
			float3 finalNormal27 = UnpackScaleNormal( tex2D( _Normal, TileUV62 ), _NormalIntensity );
			o.Normal = finalNormal27;
			float3 appendResult34 = (float3(_Tint.r , _Tint.g , _Tint.b));
			float4 tex2DNode2 = tex2D( _BaseColor, TileUV62 );
			float4 temp_cast_1 = (_OcclusionStrength).xxxx;
			float4 finalOcclusion18 = pow( tex2D( _Occlusion, TileUV62 ) , temp_cast_1 );
			float4 finalBaseColor8 = ( float4( ( appendResult34 * (tex2DNode2).rgb ) , 0.0 ) * finalOcclusion18 );
			o.Albedo = finalBaseColor8.rgb;
			float4 tex2DNode25 = tex2D( _Emission, TileUV62 );
			float4 temp_output_79_0 = ( tex2DNode25.r * _EmissionTint1 );
			float4 temp_output_77_0 = ( tex2DNode25.g * _EmissionTint2 );
			float4 temp_output_53_0 = ( tex2DNode25.b * _EmissionTint3 );
			float4 finalEmission26 = ( temp_output_79_0 + temp_output_77_0 + temp_output_53_0 );
			o.Emission = finalEmission26.rgb;
			float4 tex2DNode11 = tex2D( _Metallic_Smoothness, TileUV62 );
			float finalMetallic14 = tex2DNode11.r;
			o.Metallic = finalMetallic14;
			float finalSmoothness13 = ( tex2DNode11.a * _SmoothnessIntensity );
			o.Smoothness = finalSmoothness13;
			o.Occlusion = finalOcclusion18.r;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
254;126;1065;608;2164.098;467.775;1.596511;True;True
Node;AmplifyShaderEditor.CommentaryNode;64;-3571.795,-1357.953;Inherit;False;1133.725;819.0501;Comment;9;62;38;60;61;59;37;40;41;39;UV Tile and Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;59;-3521.795,-1291.953;Inherit;False;Property;_TileandOffset;Tile and Offset;14;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;61;-3290.795,-1203.953;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;60;-3288.795,-1307.953;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;-3119.283,-1277.629;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;39;-3239.685,-773.8215;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;41;-3319.875,-851.2126;Inherit;False;Property;_HeightScale;Height Scale;8;0;Create;True;0;0;False;0;0;0;0;0.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;40;-3274.685,-1035.822;Inherit;True;Property;_Height;Height;7;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;37;-2844.184,-1032.461;Inherit;False;0;8;False;-1;16;False;-1;2;0.02;0;False;1,1;False;0,0;Texture2D;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT2;0,0;False;6;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-2657.494,-1277.241;Inherit;False;TileUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;68;-2382.987,-1378.266;Inherit;False;1652.087;2018.086;Comment;37;25;67;26;53;54;51;52;9;27;8;14;13;47;73;24;12;48;11;75;3;74;46;34;18;6;65;5;70;2;63;17;71;66;78;76;86;79;Input/Output;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;-2260.876,-305.6529;Inherit;False;62;TileUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-1750.697,-246.1138;Inherit;False;Property;_OcclusionStrength;Occlusion Strength;15;0;Create;True;0;0;False;0;0;0.128;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-2053.559,-327.8419;Inherit;True;Property;_Occlusion;Occlusion;9;0;Create;True;0;0;False;0;-1;None;0f1b9c2b36d8b814fb6944c3ef0eb562;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;63;-2337.524,-1118.061;Inherit;False;62;TileUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;5;-1963.748,-1328.266;Inherit;False;Property;_Tint;Tint;0;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;70;-1464.697,-333.1138;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;-2243.975,-80.5499;Inherit;False;62;TileUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-2044.416,-1151.155;Inherit;True;Property;_BaseColor;BaseColor;1;0;Create;True;0;0;False;0;-1;None;3289b2bdc24c83e489fb4ed184c93545;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;34;-1728.47,-1301.093;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-1326.386,-339.2062;Inherit;False;finalOcclusion;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;6;-1735.642,-1151.631;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;-2272.271,-642.7727;Inherit;False;62;TileUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;25;-2055.725,-103.1958;Inherit;True;Property;_Emission;Emission;10;0;Create;True;0;0;False;0;-1;None;4ebd5434c9ff11844aa2e69e50c4801b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;-1734.281,445.2771;Inherit;False;Property;_EmissionTint3;Emission Tint 3;11;1;[HDR];Create;True;0;0;False;0;1,1,1,1;0,0.01695108,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;76;-1974.855,342.3251;Inherit;False;Property;_EmissionTint2;Emission Tint 2;12;1;[HDR];Create;True;0;0;False;0;1,1,1,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;78;-2166.27,137.015;Inherit;False;Property;_EmissionTint1;Emission Tint 1;13;1;[HDR];Create;True;0;0;False;0;1,1,1,1;3.027307,1.706399,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-2038.852,-469.2537;Inherit;False;Property;_SmoothnessIntensity;Smoothness Intensity;4;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;75;-2350.25,-954.5166;Inherit;False;62;TileUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1485.757,-1299.203;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1647.534,193.6035;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-1651.543,-115.523;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;-1476.847,-1201.421;Inherit;False;18;finalOcclusion;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;11;-2056.324,-667.8248;Inherit;True;Property;_Metallic_Smoothness;Metallic_Smoothness;3;0;Create;True;0;0;False;0;-1;None;0a671ca41ecf00b4f8375d5aa6d5d8ab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-1651.924,41.7707;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-2356.197,-832.8321;Inherit;False;Property;_NormalIntensity;Normal Intensity;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-1273.245,-169.2274;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;24;-2044.178,-954.8967;Inherit;True;Property;_Normal;Normal;5;1;[Normal];Create;True;0;0;False;0;-1;None;78ac2d32c312d84489b51caa55c487e5;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-1667.852,-534.2537;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;12;-1725.069,-640.1608;Inherit;False;True;True;True;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-1223.424,-1293.503;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-1741.057,-952.7352;Inherit;False;finalNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;8;-1021.616,-1302.386;Inherit;False;finalBaseColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-1016.766,-76.66017;Inherit;False;finalEmission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-1507.167,-538.1595;Inherit;False;finalSmoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-1495.268,-644.3599;Inherit;False;finalMetallic;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-421.5142,-47.69714;Inherit;False;27;finalNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-1299.144,-66.21724;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-424.7007,97.87091;Inherit;False;14;finalMetallic;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-432.5142,26.30286;Inherit;False;26;finalEmission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1184.296,-1079.227;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1478.968,-1092.205;Inherit;False;Property;_OpacityIntensity;Opacity Intensity;2;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;-439.5142,-119.6971;Inherit;False;8;finalBaseColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-452.7007,170.8709;Inherit;False;13;finalSmoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;33;-433.7007,243.8709;Inherit;False;18;finalOcclusion;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-1041.578,-1062.274;Inherit;False;finalOpacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;7;ASEMaterialInspector;0;0;Standard;DoaT Studios/BaseMetallicEmissiveEdit;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;61;0;59;3
WireConnection;61;1;59;4
WireConnection;60;0;59;1
WireConnection;60;1;59;2
WireConnection;38;0;60;0
WireConnection;38;1;61;0
WireConnection;37;0;38;0
WireConnection;37;1;40;0
WireConnection;37;2;41;0
WireConnection;37;3;39;0
WireConnection;62;0;37;0
WireConnection;17;1;66;0
WireConnection;70;0;17;0
WireConnection;70;1;71;0
WireConnection;2;1;63;0
WireConnection;34;0;5;1
WireConnection;34;1;5;2
WireConnection;34;2;5;3
WireConnection;18;0;70;0
WireConnection;6;0;2;0
WireConnection;25;1;67;0
WireConnection;3;0;34;0
WireConnection;3;1;6;0
WireConnection;53;0;25;3
WireConnection;53;1;54;0
WireConnection;79;0;25;1
WireConnection;79;1;78;0
WireConnection;11;1;65;0
WireConnection;77;0;25;2
WireConnection;77;1;76;0
WireConnection;87;0;79;0
WireConnection;87;1;77;0
WireConnection;87;2;53;0
WireConnection;24;1;75;0
WireConnection;24;5;46;0
WireConnection;47;0;11;4
WireConnection;47;1;48;0
WireConnection;12;0;11;1
WireConnection;73;0;3;0
WireConnection;73;1;74;0
WireConnection;27;0;24;0
WireConnection;8;0;73;0
WireConnection;26;0;87;0
WireConnection;13;0;47;0
WireConnection;14;0;12;0
WireConnection;86;0;79;0
WireConnection;86;1;77;0
WireConnection;86;2;53;0
WireConnection;51;0;52;0
WireConnection;51;1;2;4
WireConnection;9;0;51;0
WireConnection;0;0;28;0
WireConnection;0;1;29;0
WireConnection;0;2;30;0
WireConnection;0;3;31;0
WireConnection;0;4;32;0
WireConnection;0;5;33;0
ASEEND*/
//CHKSM=4CB91E002FA87862236CB17E4B97979F5601713B