using System;

namespace Backend.DataLayer.Models
{
    public class Project : BaseModel
    {

        public string Name { get; set; }

        public string GitRepository { get; set; }
    }
}