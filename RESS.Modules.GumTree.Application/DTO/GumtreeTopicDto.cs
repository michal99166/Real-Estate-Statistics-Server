using System;
using Microsoft.AspNetCore.Mvc;

namespace RESS.Modules.GumTree.Application.DTO
{
    public class GumtreeTopicDto
    {
        [FromRoute]
        public Guid Id { get; set; }
        public string CreatedDate { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public string PropertyType { get; set; }
        public string Garage { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public double SizeM2 { get; set; }
        public double PricePerM2 { get; set; }
        public string Description { get; set; }
    }
}