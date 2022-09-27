sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2;
sampler uImage3;
float3 uColor;
float uOpacity;
float3 uSecondaryColor;
float uTime;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uImageOffset;
float uIntensity;
float uProgress;
float2 uDirection;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;

// This is a shader. You are on your own with shaders. Compile shaders in an XNB project.

float dist(float a, float b, float c, float d){
	return sqrt((a - c) * (a - c) + (b - d) * (b - d));
}

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
	
	float WaveSize = 50 * uIntensity;
	
	float4 Color = 0;
	float f = sin(dist(uv.x + uTime/10, uv.y, 0.128, 0.128)*WaveSize)
                  + sin(dist(uv.x, uv.y, 0.64, 0.64)*WaveSize)
                  + sin(dist(uv.x, uv.y + uTime / 70, 0.192, 0.64)*WaveSize);
	uv.xy = uv.xy+((f/WaveSize));
	Color= tex2D( uImage0 , uv.xy);
	return Color; 
}

technique Technique1
{
    pass ExampleEffectScreen
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}