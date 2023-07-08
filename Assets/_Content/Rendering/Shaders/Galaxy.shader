Shader "GMTK/Galaxy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    	_Color ("Color", Color) = (1,1,1,1)
    	_Contrast ("Contrast", float) = 1
    	_Speed ("Speed", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Contrast;
            float _Speed;

            float field(in float3 p) {
				float strength = 7. + .03 * log(1.e-6 + frac(sin((_Time.y * _Speed)) * 4373.11));
				float accum = 0.;
				float prev = 0.;
				float tw = 0.;
				for (int i = 0; i < 32; ++i) {
					float mag = dot(p, p);
					p = abs(p) / mag + float3(-.5, -.4, -1.5);
					float w = exp(-float(i) / 7.);
					accum += w * exp(-strength * pow(abs(mag - prev), 2.3));
					tw += w;
					prev = mag;
				}
				return max(0., 5. * accum / tw - .7);
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = 2. * i.uv.xy / _ScreenParams.xy - 1.;
	            float2 uvs = uv * _ScreenParams.xy / max(_ScreenParams.x, _ScreenParams.y);
	            float3 p = float3(uvs / 4., 0) + float3(1., -1.3, 0.);
	            //p += .2 * float3(sin((_Time.y * _Speed) / 16.), sin((_Time.y * _Speed) / 12.),  sin((_Time.y * _Speed) / 128.));
	            p += .02 * float3(sin((_Time.y * _Speed) / 16.), sin((_Time.y * _Speed) / 12.),  sin((_Time.y * _Speed) / 128.));
	            float t = field(p);
	            float v = (1. - exp((abs(uv.x) - 1.) * 6.)) * (1. - exp((abs(uv.y) - 1.) * 6.));
	            float4 fragColor = lerp(.4, 1., v) * float4(1.8 * t * t * t, 1.4 * t * t, t, 1.0);
            	fragColor.xyz = saturate(lerp(half3(0.5, 0.5, 0.5), fragColor, _Contrast));
                return fragColor * _Color;
            }
            ENDCG
        }
    }
}

