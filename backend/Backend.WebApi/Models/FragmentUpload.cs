using System.Collections.Generic;
using Backend.DataLayer.Models;
using Microsoft.AspNetCore.Http;

namespace Backend.WebApi.Models
{
    public class FragmentUpload
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public FragmentType Type { get; set; } 
        public IFormFile BinaryData {get; set; }
    }
}