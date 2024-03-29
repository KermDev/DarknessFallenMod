sampler2D sampleTexture;

struct VertexShaderOutput
{
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float time;

float2 imageSize;

float4 source;

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
    float4 output = tex2D(sampleTexture, input.TextureCoordinates);
    
    float localCoordsY = (input.TextureCoordinates.y * imageSize.y - source.y) / source.w;

    float str = 1 - localCoordsY;
    output.rgb -= str;
    
    output.r += str;
    output.g += str * 0.5f;
    
    return output * output.a;
}

technique Technique1
{
    pass FlamePass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}