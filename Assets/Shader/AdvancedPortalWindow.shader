Shader "Custom/AdvancedPortalWindow"
{
	SubShader
	{
		ZWrite off
		ColorMask 0
		cull off

	Stencil{
			Ref 1
			Pass replace
	}
		Pass
		{

		}
	}

}
