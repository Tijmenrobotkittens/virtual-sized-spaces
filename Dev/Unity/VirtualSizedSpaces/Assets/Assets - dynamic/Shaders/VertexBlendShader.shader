// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VertexBlendShader"
{
	Properties
	{
		_RedAlbedo("Red Albedo", 2D) = "white" {}
		_RedNormal("Red Normal", 2D) = "white" {}
		_BlueAlbedo("Blue Albedo", 2D) = "white" {}
		_BlueNormal("Blue Normal", 2D) = "white" {}
		_GreenAlbedo("Green Albedo", 2D) = "white" {}
		_GreenNormal("Green Normal", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _RedNormal;
		uniform float4 _RedNormal_ST;
		uniform sampler2D _BlueNormal;
		uniform float4 _BlueNormal_ST;
		uniform sampler2D _GreenNormal;
		uniform float4 _GreenNormal_ST;
		uniform sampler2D _RedAlbedo;
		uniform float4 _RedAlbedo_ST;
		uniform sampler2D _BlueAlbedo;
		uniform float4 _BlueAlbedo_ST;
		uniform sampler2D _GreenAlbedo;
		uniform float4 _GreenAlbedo_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_RedNormal = i.uv_texcoord * _RedNormal_ST.xy + _RedNormal_ST.zw;
			float2 uv_BlueNormal = i.uv_texcoord * _BlueNormal_ST.xy + _BlueNormal_ST.zw;
			float3 lerpResult19 = lerp( UnpackNormal( tex2D( _RedNormal, uv_RedNormal ) ) , UnpackNormal( tex2D( _BlueNormal, uv_BlueNormal ) ) , i.vertexColor.g);
			float2 uv_GreenNormal = i.uv_texcoord * _GreenNormal_ST.xy + _GreenNormal_ST.zw;
			float3 lerpResult20 = lerp( lerpResult19 , UnpackNormal( tex2D( _GreenNormal, uv_GreenNormal ) ) , i.vertexColor.b);
			o.Normal = lerpResult20;
			float2 uv_RedAlbedo = i.uv_texcoord * _RedAlbedo_ST.xy + _RedAlbedo_ST.zw;
			float2 uv_BlueAlbedo = i.uv_texcoord * _BlueAlbedo_ST.xy + _BlueAlbedo_ST.zw;
			float4 lerpResult16 = lerp( tex2D( _RedAlbedo, uv_RedAlbedo ) , tex2D( _BlueAlbedo, uv_BlueAlbedo ) , i.vertexColor.g);
			float2 uv_GreenAlbedo = i.uv_texcoord * _GreenAlbedo_ST.xy + _GreenAlbedo_ST.zw;
			float4 lerpResult18 = lerp( lerpResult16 , tex2D( _GreenAlbedo, uv_GreenAlbedo ) , i.vertexColor.b);
			o.Albedo = lerpResult18.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
3224;1;1252;1021;868.9387;352.9987;1.627771;False;False
Node;AmplifyShaderEditor.TexturePropertyNode;2;-709.9296,-265.8899;Float;True;Property;_RedAlbedo;Red Albedo;0;0;Create;True;0;0;False;0;788f239dea958b24182495904e1aa6e1;9bc21295b2ef45940a6e1cd00e311d32;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-651.382,-66.42394;Float;True;Property;_BlueAlbedo;Blue Albedo;2;0;Create;True;0;0;False;0;8fa584af9385ed744a563167e58eb296;9bc21295b2ef45940a6e1cd00e311d32;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;5;-640.3524,679.323;Float;True;Property;_BlueNormal;Blue Normal;3;0;Create;True;0;0;False;0;6b84c454b73952f4381dbff355a0d5aa;9bc21295b2ef45940a6e1cd00e311d32;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;3;-703.5594,487.0307;Float;True;Property;_RedNormal;Red Normal;1;0;Create;True;0;0;False;0;50396218ed6411645a1a77a6e66ad97b;9bc21295b2ef45940a6e1cd00e311d32;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;6;-654.7122,124.7203;Float;True;Property;_GreenAlbedo;Green Albedo;4;0;Create;True;0;0;False;0;f08d4d83bdb090944986c038503ee72d;9bc21295b2ef45940a6e1cd00e311d32;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;7;-647.3909,864.8689;Float;True;Property;_GreenNormal;Green Normal;5;0;Create;True;0;0;False;0;a31be3641b1f7f846a4926eb64fa66f6;9bc21295b2ef45940a6e1cd00e311d32;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;13;-355.0656,679.293;Float;True;Property;_TextureSample3;Texture Sample 3;13;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-375.5104,486.0361;Float;True;Property;_TextureSample2;Texture Sample 2;13;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-369.4977,-65.55215;Float;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-373.2536,-264.3723;Float;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;11;-253.9499,315.7121;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;8.429749,617.1526;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;15;-355.0654,864.7264;Float;True;Property;_TextureSample5;Texture Sample 5;13;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;-3.547729,-127.1729;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;14;-365.7889,125.4439;Float;True;Property;_TextureSample4;Texture Sample 4;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;18;206.4523,16.82715;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;20;211.9011,771.7908;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;652.3999,7.6;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;VertexBlendShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;5;0
WireConnection;10;0;3;0
WireConnection;12;0;4;0
WireConnection;8;0;2;0
WireConnection;19;0;10;0
WireConnection;19;1;13;0
WireConnection;19;2;11;2
WireConnection;15;0;7;0
WireConnection;16;0;8;0
WireConnection;16;1;12;0
WireConnection;16;2;11;2
WireConnection;14;0;6;0
WireConnection;18;0;16;0
WireConnection;18;1;14;0
WireConnection;18;2;11;3
WireConnection;20;0;19;0
WireConnection;20;1;15;0
WireConnection;20;2;11;3
WireConnection;0;0;18;0
WireConnection;0;1;20;0
ASEEND*/
//CHKSM=D99F8E2955B9BFF909ECD6C677ED7A745D7F3529