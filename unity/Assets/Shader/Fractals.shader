Shader "Fractal" {
	SubShader {
    Pass {

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		#include "UnityCG.cginc"

		float speed=1f;

		struct v2f {
		    float4 pos : SV_POSITION;
		    float2  uv : TEXCOORD0;
		};

		v2f vert (appdata_base v)
		{
		    v2f o;
		    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		    o.uv = v.texcoord;
		    return o;
		}

		half4 frag (v2f i) : COLOR
		{
			float2 p = i.uv;

			float time= _Time*4;
		    float speed = 0.25;
			float3 color = float3(1.0f,0.5f,0.25f);
			float2 loc = float2(
				cos(time/4.0f*speed)/1.9f-cos(time/2.0f*speed)/3.8f,
				sin(time/4.0f*speed)/1.9f-sin(time/2.0f*speed)/3.8f
			);
			float depth;

			for(int i = 0; i < 40; i+=1){
				p = float2( p.x* p.x-p.y * p.y, 
						   2.0f * p.x * p.y) + loc;
				depth = float(i);
				if((p.x*p.x+p.y*p.y) >= 4.0) break;
			}

		    return half4(clamp(color*depth*0.05f, 0.0f, 1.0f), 1.0f );

		}
		ENDCG

	    }
	}
	Fallback "VertexLit"
}