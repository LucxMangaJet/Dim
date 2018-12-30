
Shader "Particles/GrayScale" {
	Properties{
		_MainTex("Particle Texture", 2D) = "white" {}
		_Multiplyer("IntensityMuliplyer", Range(0,10)) = 1
	}

		Category{
			Tags { "Queue" = "Transparent" "PreviewType" = "Plane" }

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			//ColorMask RGB
			//Cull Off Lighting Off ZWrite Off


			SubShader {
					GrabPass
					{
					"_BackgroundTexture"
					}

				Pass {

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma multi_compile_particles

					#include "UnityCG.cginc"

					sampler2D _MainTex;
					sampler2D _BackgroundTexture;
					float _Multiplyer;

					struct appdata_t {
						float4 vertex : POSITION;
						fixed4 color : COLOR;
						float2 texcoord : TEXCOORD0;
						UNITY_VERTEX_INPUT_INSTANCE_ID
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						float4 grabPos : TEXCOORD1;
						fixed4 color : COLOR;
						float2 texcoord : TEXCOORD0;


						UNITY_VERTEX_OUTPUT_STEREO
					};

					float4 _MainTex_ST;

					v2f vert(appdata_t v)
					{
						v2f o;
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
						o.vertex = UnityObjectToClipPos(v.vertex);

						o.grabPos = ComputeGrabScreenPos(o.vertex);

						o.color = v.color;
						o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
						return o;
					}

					//UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

					fixed4 frag(v2f i) : SV_Target
					{
						half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);
						fixed4 col2 = i.color * tex2D(_MainTex, i.texcoord);
						float amount = saturate(col2.r* col2.a * _Multiplyer);

						fixed4 col = bgcolor;

						col.a = amount;
						col.rbg = (col.rbg*(1 - amount) + amount * (0.2126*col.r + 0.7152*col.b + 0.0722*col.g));
						return col;
					}
					ENDCG
				}
			}
		}
}
