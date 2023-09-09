using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation.Application.Models
{
    public  class CreateTranslationJobResultDto
    {
        public string Id { get; set; }

        public CreateTranslationJobResultDto(string id)
        {
            Id = id;
        }
    }
}
