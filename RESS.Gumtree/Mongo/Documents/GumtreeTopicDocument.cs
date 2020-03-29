using System;
using System.Threading;
using Convey.Types;

namespace RESS.Gumtree.Mongo.Documents
{
    public class GumtreeTopicDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public double SizeM2 { get; set; }
        public string PropertyType { get; set; }
        public string Garage { get; set; }
        public double PricePerM2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Description { get; set; }
        public Guid RelatedId { get; set; }
        public bool PriceChanged { get; set; }
    }
}