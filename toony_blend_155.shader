Shader "Toon/Blended" {

Properties
{
	_WireCutoff ("Wireframe Cutoff", Float) = .01
	_WireColor ("Wireframe Color", Color) = (0.5,0.5,0.5,0.5)
	_Color ("Toon Tint Color (A = Blend)", Color) = (.5,.5,.5,1)
	_MainTex ("Main Toon Texture (A = Wireframe)", 2D) = ""
	_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" {Texgen CubeNormal}
}

SubShader
{
	Tags {Queue = Transparent}

	Pass 
	{
		AlphaTest Greater [_WireCutoff]
		Cull Off
		
		SetTexture[_MainTex]
		{
			ConstantColor[_WireColor] 
			Combine constant, texture
		}
	}
	
	Pass {ColorMask 0}

	Pass 
	{
		Blend SrcAlpha OneMinusSrcAlpha
				
		SetTexture [_ToonShade]
		{
			ConstantColor [_Color]
			Combine texture * constant, constant
		}
	}
}

}