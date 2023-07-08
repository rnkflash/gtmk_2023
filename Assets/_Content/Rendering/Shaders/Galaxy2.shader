Shader "GMTK/Galaxy2"
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

            float field2(in float3 p, float s) {

			    float strength = 4.3 + .03 * log(1.e-6 + frac(/*sin(iTime)*/1. * 4373.11));

			    float accum = s/4.;

			    float prev = 0.;

			    float tw = 0.;

			   for (int i = 0; i < 20; ++i) {

			        float mag = dot(p, p);

			        p = abs(p) / mag + float3(-.1, -.4, -1.5);

			        float w = exp(-float(i) / 7.);

			        accum += w * exp(-strength * pow(abs(mag - prev), 2.2));

			        tw += w;

			        prev = mag;

			    }

			   return ( 5.0 * accum / tw - 0.8);

			}

			float3 nrand3( float2 co ) {

			   float3 a = frac( cos( co.x*8.3e-3 + co.y )*float3(1.3e5, 4.7e5, 2.9e5) );

			    float3 b = frac( sin( co.x*0.3e-3 + co.y )*float3(8.1e5, 1.0e5, 0.1e5) );

			    float3 c = lerp(a, b, 0.5);

			    return c;
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

			    float freqs[4];

			    freqs[0]=1.2;

			    freqs[1]=-0.01;

			    freqs[2]=-0.3;

			    freqs[3]=0.01;

			    float v = (1.0 - exp((abs(uv.x) - 1.) * 1.)) * (1. - exp((abs(uv.y) - 1.0) * 1.0));

			    float3 p2 = float3(uvs / (2.+sin(_Time.y * 0.11) * 0.2 + 0.2 + sin(_Time.y * 0.15) * 0.3 + 0.4), 1.5) + float3(1.98, -1.3, -1.0);
			  //---------------------------------------------Range---mean----------------------------Range---mean-----------------------------Range---mean----
			    p2 += 0.4 * float3((sin(_Time.y / 50.0) * 0.1) + 1.0 , (sin(_Time.y / 100.0) * 0.3) - 0.2,  (sin(_Time.y * 0.25) * 0.07) + 0.875);

			    float t2 = field2(p2 ,freqs[3]);

			     float4 c2 = lerp(.7, 1., v) * float4(2.3 * t2 * t2  ,1.1 * t2 * t2 , t2 * freqs[0], t2);

			    float2 seed = p.xy * 30.;

			    seed = floor(seed * _ScreenParams.x);

			    float3 rnd = nrand3( seed );

			    float4 starcolor1 = float4(pow(rnd.y,80.0),pow(rnd.y,80.0),pow(rnd.y,80.0),pow(rnd.y,80.0));

			    float2 seed2 = p2.xy * 0.95;

			    seed2 = floor(seed2 * _ScreenParams.x);

			    float3 rnd2 = nrand3( seed2 );

			    float4 starcolor2 = float4(pow(rnd2.y,200.0),pow(rnd2.y,200.0),pow(rnd2.y,200.0),pow(rnd2.y,200.0));

			    //------------------------------------------------------------------------------------------------------------------col intensity------------col intensity--------col intensity
			    float4 fragColor = ((lerp(freqs[3],0.1,v) * lerp(freqs[1],-3.1,v) * float4(1.5*freqs[2],freqs[1],freqs[3],1.0) + ((c2 * 0.7)*0.9)) * 1.2) + ((starcolor1) * 1.3) + (starcolor2 * 0.7) ;


            	fragColor.xyz = saturate(lerp(half3(0.5, 0.5, 0.5), fragColor, _Contrast));
                return fragColor * _Color;
            }
            ENDCG
        }
    }
}

