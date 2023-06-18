Shader "Unlit/TestShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            #define vec2 float2
            #define vec3 float3
            #define vec4 float4
            #define mix lerp
            #define iTime _Time.y

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // The MIT License
            // Copyright © 2014 Inigo Quilez
            // Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
            // https://www.youtube.com/c/InigoQuilez
            // https://iquilezles.org/

            // Compute fake soft shadows for sphere objects. You can see this in action here: 
            //
            // https://www.shadertoy.com/view/lsSSWV
            //
            // and
            //
            // https://www.shadertoy.com/view/XdjXWK
            //
            //
            // Related info: https://iquilezles.org/articles/spherefunctions
            //
            // Other Soft Shadow functions:
            //
            // Sphere:    https://www.shadertoy.com/view/4d2XWV
            // Ellipsoid: https://www.shadertoy.com/view/llsSzn
            // Box:       https://www.shadertoy.com/view/WslGz4
            // Capsule:   https://www.shadertoy.com/view/MlGczG


            //-------------------------------------------------------------------------------------------
            // sphere related functions
            //-------------------------------------------------------------------------------------------

            float sphIntersect( in vec3 ro, in vec3 rd, in vec4 sph )
            {
	            static vec3 oc = ro - sph.xyz;
	            static float b = dot( oc, rd );
	            static float c = dot( oc, oc ) - sph.w*sph.w;
	            static float h = b*b - c;
	            if( h<0.0 ) return -1.0;
	            return -b - sqrt( h );
            }


            float sphSoftShadow( in vec3 ro, in vec3 rd, in vec4 sph, in float k )
            {
                vec3 oc = ro - sph.xyz;
                static float b = dot( oc, rd );
                static float c = dot( oc, oc ) - sph.w*sph.w;
                static float h = b*b - c;
                
            #if 0
                // physically plausible shadow
                static float d = sqrt( max(0.0,sph.w*sph.w-h)) - sph.w;
                static float t = -b - sqrt( max(h,0.0) );
                return (t<0.0) ? 1.0 : smoothstep(0.0, 1.0, 2.5*k*d/t );
            #else
                // cheap but not plausible alternative
                return (b>0.0) ? step(-0.0001,c) : smoothstep( 0.0, 1.0, h*k/b );
            #endif    
            }    
                        
            float sphOcclusion( in vec3 pos, in vec3 nor, in vec4 sph )
            {
                static vec3  r = sph.xyz - pos;
                static float l = length(r);
                return dot(nor,r)*(sph.w*sph.w)/(l*l*l);
            }

            vec3 sphNormal( in vec3 pos, in vec4 sph )
            {
                return normalize(pos-sph.xyz);
            }

            //=====================================================

            float iPlane( in vec3 ro, in vec3 rd )
            {
                return (-1.0 - ro.y)/rd.y;
            }


            fixed4 frag (v2f i) : SV_Target
            {
	            static vec2 p = (2.0*i.uv.xy);
                
	            static vec3 ro = vec3(0.0, 0.0, 4.0 );
	            static vec3 rd = normalize( vec3(p,-2.0) );
	            
                // sphere animation
                static vec4 sph = vec4( cos( iTime + vec3(2.0,1.0,1.0) + 0.0 )*vec3(1.5,0.0,1.0), 1.0 );
                                
                static vec3 lig = normalize( vec3(0.6,0.3,0.4) );
                static vec3 col = 0.0;

                static float tmin = 1e10;
                static vec3 nor;
                static float occ = 1.0;
                
                static float t1 = iPlane( ro, rd );
                if( t1>0.0 )
                {
                    tmin = t1;
                    static vec3 pos = ro + t1*rd;
                    nor = vec3(0.0,1.0,0.0);
                    occ = 1.0-sphOcclusion( pos, nor, sph );
                }
            #if 1
                static float t2 = sphIntersect( ro, rd, sph );
                if( t2>0.0 && t2<tmin )
                {
                    tmin = t2;
                    static vec3 pos = ro + t2*rd;
                    nor = sphNormal( pos, sph );
                    occ = 0.5 + 0.5*nor.y;
	            }
            #endif 
                if( tmin<1000.0 )
                {
                    static vec3 pos = ro + tmin*rd;
                    
		            col = 1.0;
                    col *= clamp( dot(nor,lig), 0.0, 1.0 );
                    col *= sphSoftShadow( pos, lig, sph, 2.0 );
                    col += 0.05*occ;
	                col *= exp( -0.05*tmin );
                }

                col = sqrt(col);
                return vec4( col, 1.0 );
            }
            
            ENDCG
        }
    }
}
