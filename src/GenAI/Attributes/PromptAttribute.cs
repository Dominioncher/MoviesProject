using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAI.Attributes
{
    public class PromptAttribute : Attribute
    {
        public string Description { get; private set; }

        public PromptAttribute(string description)
        {
            Description = description;
        }
    }
}
