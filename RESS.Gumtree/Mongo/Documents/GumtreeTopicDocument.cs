﻿using System;
using System.Threading;
using Convey.Types;

namespace RESS.Gumtree.Mongo.Documents
{
    public class GumtreeTopicDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public double SizeM2 { get; set; }
        public PropertyType PropertyType { get; set; }
        public string Garage { get; set; }
        public double PricePerM2 => Math.Round(Price / SizeM2, 3);
    }
}