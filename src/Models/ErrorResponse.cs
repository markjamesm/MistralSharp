using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace MistralSharp.Models
{
    public class ErrorResponse
    {
        /// <summary>
        /// Gets or Sets Error
        /// </summary>
        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }
}