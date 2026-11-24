
namespace DownhillApp.Service
{
    public interface IGeminiAiService
    {
        Task<string> GetResponse(string promt);
    }
}