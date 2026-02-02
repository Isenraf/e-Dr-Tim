
// DEPRECATED FILE
// This file has been replaced by IA/IAService.cs and appsettings.json configuration.
// Please use the injected IAIService instead.

using System;
using System.Threading.Tasks;

class Gemini
{
    [Obsolete("Use IAIService.AskAsync instead.")]
    public async Task<string> AskGeminiAsync(string prompt)
    {
         throw new NotImplementedException("This class is deprecated. Use IAIService.");
    }
}
