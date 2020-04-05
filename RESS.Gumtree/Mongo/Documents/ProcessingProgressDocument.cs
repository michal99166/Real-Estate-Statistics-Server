using System;

namespace RESS.Gumtree.Mongo.Documents
{
    public class ProcessingProgressDocument
    {
        public Guid Id { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool IsFinished { get; set; }
        public string Url { get; set; }
        public int PriceInterval { get; set; }
        public int PriceFrom { get; set; }
        public int PriceTo { get; set; }
    }
}