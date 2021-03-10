
namespace AspNetCoreWebApiLab.ApiClient.DTOs
{
    public class PatchOperation
    {
        public string Op { get; set; }
        public string From { get; set; }
        public string Path { get; set; }
        public dynamic Value { get; set; }
    }
}
