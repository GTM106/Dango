Shader "Unlit/BasicBlur"
{
	Properties{
		[HideInInspector] _MainTex("Texture", 2D) = "white" {}
		_BlurLevel("Blur Level", int) = 1
	}

		SubShader
		{
			Cull Off        // �J�����O�͕s�v
			ZTest Always    // ZTest�͏�ɒʂ�
			ZWrite Off      // ZWrite�͕s�v

			Tags { "RenderType" = "Opaque" }

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _BlurLevel; //�u���[�̋��x
				float2 _Resolution; //C#����n���Ă���f�B�X�v���C�T�C�Y

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float offsetU = _BlurLevel / _Resolution.x; //U�����̃I�t�Z�b�g���v�Z
					float offsetV = _BlurLevel / _Resolution.y; //V�����̃I�t�Z�b�g���v�Z

					float4 color = tex2D(_MainTex, i.uv);

					color += tex2D(_MainTex, i.uv + float2(offsetU, 0.0f)); //�E�̃e�N�Z���̃J���[���T���v�����O

					color += tex2D(_MainTex, i.uv + float2(-offsetU, 0.0f)); //���̃e�N�Z���̃J���[���T���v�����O

					color += tex2D(_MainTex, i.uv + float2(0.0f, offsetV)); //���̃e�N�Z���̃J���[���T���v�����O

					color += tex2D(_MainTex, i.uv + float2(0.0f, -offsetV)); //��̃e�N�Z���̃J���[���T���v�����O

					color += tex2D(_MainTex, i.uv + float2(offsetU, offsetV)); //�E���̃e�N�Z���̃J���[���T���v�����O

					color += tex2D(_MainTex, i.uv + float2(offsetU, -offsetV)); //�E��̃e�N�Z���̃J���[���T���v�����O

					color += tex2D(_MainTex, i.uv + float2(-offsetU, offsetV)); //�����̃e�N�Z���̃J���[���T���v�����O

					color += tex2D(_MainTex, i.uv + float2(-offsetU, -offsetV)); //����̃e�N�Z���̃J���[���T���v�����O

					color /= 9.0f; //8�e�N�Z�����̐F�����Z����Ă���̂ŁA9�ŏ��Z�����ω�

					return color;
				}
				ENDCG
			}
		}
}