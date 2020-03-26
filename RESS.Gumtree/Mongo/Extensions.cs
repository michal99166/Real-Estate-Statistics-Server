using System;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Mongo.Documents;

namespace RESS.Gumtree.Mongo
{
    public static class Extensions
    {
        public static GumtreeTopicDocument AsDocument(this GumtreeTopicDto dto)
        {
            return DtoToDocument(dto);
        }

        private static GumtreeTopicDocument DtoToDocument(GumtreeTopicDto dto)
        {
            var document = new GumtreeTopicDocument
            {
                Id = dto.Id,
                PropertyType = dto.PropertyType,
                Garage = dto.Garage,
                Price = dto.Price,
                TimeStamp = DateTime.Now,
                Title = dto.Title,
                Url = dto.Url,
                SizeM2 = dto.SizeM2,
                City = dto.City.Contains(dto.Province)
                    ? dto.City.Replace(dto.Province, "").Replace(" ", "").Replace(",", "")
                    : dto.City,
                Province = dto.Province,
                PricePerM2 = Math.Round(dto.Price / dto.SizeM2, 3),
                Description = dto.Description,
                CreatedDate = Convert.ToDateTime(dto.CreatedDate)
            };
            return document;
        }

        public static GumtreeTopicDocument AsRelatedDocument(this GumtreeTopicDto dto)
        {
            var document = DtoToDocument(dto);
            document.RelatedId = dto.Id;
            document.PriceChanged = true;
            return document;
        }

        public static GumtreeTopicDocument AsUpdateDocument(this GumtreeTopicDto dto)
        {
            var document = DtoToDocument(dto);
            document.Id = dto.Id;
            return document;
        }

        public static GumtreeTopicDto AsDto(this GumtreeTopicDocument document)
            => new GumtreeTopicDto
            {
                Title = document.Title,
                Garage = document.Garage,
                Url = document.Url,
                CreatedDate = document.CreatedDate.ToString(),
                PropertyType = document.PropertyType,
                Price = document.Price,
                Province = document.Province,
                City = document.City,
                Id = document.Id,
                SizeM2 = document.SizeM2,
                PricePerM2 = document.PricePerM2,
                Description = document.Description
            };
    }
}