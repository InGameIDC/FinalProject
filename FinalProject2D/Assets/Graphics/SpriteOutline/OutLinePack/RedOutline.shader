Shader "Unlit/RedOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Colour", Color) = (1, 1, 1 ,1)
    }
    Tags { "Queue" = "Transparent" }
    SubShader
    {
        Cull off
        Blend One OneMinusSrcAlpha

        Pass{
        CGPROGRAM

        #pragma vertext vertextFunc
        #pragma fragment fragmentFunc
        #include "UnityCG.cginc"

        sampler2D _MainTex;

        struct v2f {
        float4 pos : SV_POSITION;
        half2 uv : TEXCOORD0;
	    };

        v2f vertextFunc(appdata_base v){
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.textcoord;
            return o;
		}

        fixed4 _Color;
        float4 _MainTex_TexelSize;

        fixed4 fragmentFunc(v2f i) : COLOR{
            half4 c = text2D(_MainTexm, i.uv);
            c.rgb *= c.a;
            half4 outlineC = _Color;
            outlineC.a *= ceil(c.a);
            outlineC.rgb *= outlineC.a;

            fixed upAlpha = text2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
            fixed downAlpha = text2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
            fixed rightAlpha = text2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;
            fixed leftAlpha = text2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;

            return lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));
	    }
        
        ENDCD
		}
    }
}
