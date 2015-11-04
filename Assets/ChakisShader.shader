Shader "Custom/ChakisShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) ="white"{}
		_SpecMap("Specular Map",2D)= "white"{}
		_Glossiness("Gloss", range(1,4))= 2
		
		_Shading("Shading", range(0,1))= 0.5
		_Lighting("Lighting", range(0,1))= 0.5
		
		_Outline("Outline", range(0,1))=0.1
		_OutlineColor("Outline Color", Color)=(0,0,0,1)
		
		_ShadingSoftness("Shading Softness", range(0,1))=0
		_OutlineSoftness("Outline Softness", range(0,1))=0
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
		#pragma surface surf LonelyWorld
		#pragma target 3.0
		
		
		half4 _Color;
		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _SpecMap;
		half _Glossiness;
		
		half _Shading;
		half _Lighting;
		
		half _Outline;
		half4  _OutlineColor;
		
		half _ShadingSoftness;
		half _OutlineSoftness;
		
		struct MySurfaceOutput{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Specular;
			half Alpha;
			
			half3 GlossColor;
		};
		
		half halfDiffuseTerm(half3 a, half3 b)
        {
          	return dot(normalize(a), normalize(b)) * 0.5f + 0.5f;
        }
		
		
   		half diffuseTerm(half3 a, half3 b)
        {
          	return max(0, dot(normalize(a), normalize(b)));
          	
        }
        
        half softStep(half a, half b, half softness)
        {
        	return step(a,b) * clamp((b-a)/softness,0,1);
        }
          
        half4 LightingLonelyWorld (MySurfaceOutput o, half3 lightDir, half3 viewDir, half atten) 
        {
            half d = halfDiffuseTerm (o.Normal, lightDir);
            half shade = softStep(_Shading,d, _ShadingSoftness);
            half3 diffuseColor = _LightColor0 * o.Albedo * shade;
            
            half s = pow(diffuseTerm (o.Normal, lightDir + viewDir),_Glossiness);
            half light = softStep(_Lighting,s,_ShadingSoftness);
            half3 specularColor = _LightColor0 * o.GlossColor * light;
            
            half3 returnColor = (diffuseColor + specularColor) * atten * 2;
            return half4(returnColor, o.Alpha);
        }
          
		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		void surf (Input IN, inout MySurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			half4 c = tex2D (_MainTex, IN.uv_MainTex) ;
			half3 n = UnpackNormal (tex2D(_NormalMap,IN.uv_MainTex));
			o.Normal = n;
			
			half d = dot(normalize(IN.viewDir), o.Normal);
			
			half outline = softStep(_Outline,d,_OutlineSoftness);
			
			
			o.Albedo = (c.rgb * _Color * outline) + ((1-outline) * _OutlineColor);
			o.Alpha = c.a;
			
			
			half4 s = tex2D(_SpecMap, IN.uv_MainTex);
			o.GlossColor = s.rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
