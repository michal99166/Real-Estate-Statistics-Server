using System;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Mongo.Documents;

namespace RESS.Gumtree.Mongo
{
    public static class Extensions
    {
        public static GumtreeTopicDocument AsDocument(this GumtreeTopicDto dto)
            => new GumtreeTopicDocument
            {
                Id = dto.Id,
                CreatedDate = Convert.ToDateTime(dto.CreatedDate),
                PropertyType = Enum.TryParse<PropertyType>(dto.PropertyType, out var property) ? property : PropertyType.Unknown,
                Garage = dto.Garage,
                Price = double.TryParse(dto.Price, out var price) ? price : double.NaN,
                TimeStamp = DateTime.Now,
                Title = dto.Title,
                Url = dto.Url,
                SizeM2 = double.TryParse(dto.Price, out var sizeM2) ? sizeM2 : double.NaN,
            };

        public static GumtreeTopicDto AsDto(this GumtreeTopicDocument document)
            => new GumtreeTopicDto
            {
                Title = document.Title,
                Garage = document.Garage,
                Url = document.Url,
                CreatedDate = document.CreatedDate.ToString(),
                PropertyType = document.PropertyType.ToString(),
                Price = document.Price.ToString(),
                
            };
    }
}