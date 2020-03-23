using System;
using Microsoft.AspNetCore.Mvc;

namespace RESS.Gumtree.DTO
{
    public class GumtreeTopicDto
    {
        [FromRoute]
        public Guid Id { get; set; }
        public string CreatedDate { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Url { get; set; }
        public string PropertyType { get; set; }
        public string Garage { get; set; }
    }
}