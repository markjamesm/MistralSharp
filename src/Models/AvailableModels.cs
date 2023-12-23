using System.Collections.Generic;

namespace MistralSharp.Models
{
    public class AvailableModels
    {
        public string Object { get; set; }

        public IEnumerable<ModelData> Data { get; set; }
    }

    public class ModelData
    {
        public string Id { get; set; }

        public string Object { get; set; }
    
        public int Created { get; set; }

        public string OwnedBy { get; set; }
    
        public object Root { get; set; }
    
        public object Parent { get; set; }
    
        public IEnumerable<Permission> Permission { get; set; }
    }

    public class Permission
    {
        public string Id { get; set; }

        public string Object { get; set; }
    
        public int Created { get; set; }

        public bool AllowCreateEngine { get; set; }

        public bool AllowSampling { get; set; }

        public bool AllowLogprobs { get; set; }
    
        public bool AllowSearchIndices { get; set; }
    
        public bool AllowView { get; set; }
    
        public bool AllowFineTuning { get; set; }
    
        public string Organization { get; set; }
    
        public object Group { get; set; }
    
        public bool IsBlocking { get; set; }
    }
}