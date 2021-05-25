
namespace AspNetCoreWebApiLab.Api.Models.V3
{
    public class Endpoint
    {
        public string HRef { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }

        public Endpoint(string href, string rel, string method)
        {
            HRef = href;
            Rel = rel;
            Method = method;
        }
    }
}
