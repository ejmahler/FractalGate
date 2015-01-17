Shader "Custom/FractalShader" {
    Properties {
        _Iterations ("Fractal Iterations", Int) = 20 
        
        _ColorTexture ("Color Texture", 2D) = "" {}

        _InsideColor ("Inner Fractal Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
	SubShader {
        Pass {
            Cull Off
        
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.0
            
            #include "UnityCG.cginc"
            
            int _Iterations;
            uniform float4x4 _Transformation;
            sampler2D _ColorTexture;
			fixed4 _InsideColor;

            struct fragmentInput{
                float4 position : SV_POSITION;
                float4 texcoord0 : TEXCOORD0;
            };

            fragmentInput vert(appdata_img input){
                fragmentInput o;
                o.position = mul (UNITY_MATRIX_MVP, input.vertex);
                o.texcoord0 = float4(input.texcoord, 0.0, 1.0);
                return o;
            }
            
            
            fixed4 frag(fragmentInput input) : COLOR {
                float2 c = mul(_Transformation, input.texcoord0).xy;
                float2 z = c;
                
                int i;
                for(i = 0; i < _Iterations; i++) {
                    float x = (z.x * z.x - z.y * z.y) + c.x;
                    float y = (2 * z.x * z.y) + c.y;
                    
                    z.x = x;
                    z.y = y;
                    
                    if(dot(z,z) > 25.0) break;
                }
                
                if(i == _Iterations) {
                    return _InsideColor;
                } else {
                    float smoothValue = log2(log(length(z)));
                    float smoothIterations = float(i) + 1 - smoothValue;
                
                    float2 uv = float2(smoothIterations / _Iterations, 0.0);
                    return tex2D(_ColorTexture, uv);
                }
            }

            ENDCG
        }
    }
    
    SubShader {    
        Pass {
            Cull Off
        
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma glsl
            
            #include "UnityCG.cginc"
            
            int _Iterations;
            uniform float4x4 _Transformation;
            sampler2D _ColorTexture;
			fixed4 _InsideColor;

            struct fragmentInput{
                float4 position : SV_POSITION;
                float4 texcoord0 : TEXCOORD0;
            };

            fragmentInput vert(appdata_img input){
                fragmentInput o;
                o.position = mul (UNITY_MATRIX_MVP, input.vertex);
                o.texcoord0 = float4(input.texcoord, 0.0, 1.0);
                return o;
            }
            
            
            fixed4 frag(fragmentInput input) : COLOR {
                float2 c = mul(_Transformation, input.texcoord0).xy;
                float2 z = c;
                
                int i;
                for(i = 0; i < _Iterations; i++) {
                    float x = (z.x * z.x - z.y * z.y) + c.x;
                    float y = (2 * z.x * z.y) + c.y;
                    
                    z.x = x;
                    z.y = y;
                    
                    if(dot(z,z) > 25.0) break;
                }
                
                if(i == _Iterations) {
                    return _InsideColor;
                } else {
                    float smoothValue = log2(log(length(z)));
                    float smoothIterations = float(i) + 1 - smoothValue;
                
                    float2 uv = float2(smoothIterations / _Iterations, 0.0);
                    return tex2D(_ColorTexture, uv);
                }
            }

            ENDCG
        }
    }
}
