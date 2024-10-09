Shader "Custom/LitMask"
{
    Properties {}

    SubShader {
        Tags { 
            "RenderType" = "Opaque" 
        }

        Pass {
            ZWrite Off
        }
    }
}
