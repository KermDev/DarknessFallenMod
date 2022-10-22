sampler2D sampleTexture;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float time;

float2 imageSize;

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
    float4 output = tex2D(sampleTexture, input.TextureCoordinates) * input.Color;
    
    float2 pixelSize = 0.05 / float2((float) imageSize.x, (float) imageSize.y);
    
    float4 left = tex2D(sampleTexture, input.TextureCoordinates + (pixelSize.x, 0));
    float4 right = tex2D(sampleTexture, input.TextureCoordinates - (pixelSize.x, 0));
    float4 top = tex2D(sampleTexture, input.TextureCoordinates + (0, pixelSize.y));
    float4 bottom = tex2D(sampleTexture, input.TextureCoordinates - (0, pixelSize.y));
    
    if ((left.a == 0 || right.a == 0 || top.a == 0 || bottom.a == 0) && output.a == 1)
    {
        output.rgb = 0;
        output.a = 1;
    }

    return output;
}

technique Technique1
{
    pass FlamePass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}