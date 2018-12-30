﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Particles/RippleEffect"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_DispAmount("Displacement Amount", Range(0,2)) = 1
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			GrabPass
			{
			"_BackgroundTexture"
			}

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float4 grabPos : TEXCOORD1;
				};

				fixed4 _Color;

				

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				sampler2D _BackgroundTexture;
				float _AlphaSplitEnabled;
				float _DispAmount;




				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;

					float4 dir = normalize(IN.vertex);
					float4 newClipPos = UnityObjectToClipPos(IN.vertex -  dir * OUT.color.a*_DispAmount);

					OUT.grabPos = ComputeGrabScreenPos(newClipPos);

					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
					half4 bgColor = tex2Dproj(_BackgroundTexture, IN.grabPos);

					c.rgb = bgColor.rgb;
					c.rgb *= c.a;
					
					

					return c;
				}
			ENDCG
			}
		}
}