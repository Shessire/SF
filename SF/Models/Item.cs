using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SF.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string? GPSC { get; set; }
        public string Name { get; set; }
        public string BaseUOMId { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }

    }
}
