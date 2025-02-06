using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a product in the system.
    /// This entity follows domain-driven design principles and includes business rules validation.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product Id.
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the product count.
        /// </summary>
        public decimal Quantity { get; set; }
    }
}