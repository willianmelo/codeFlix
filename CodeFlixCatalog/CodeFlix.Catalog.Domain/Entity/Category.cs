using CodeFlix.Catalog.Domain.Exceptions;
using System.Net.Http.Headers;

namespace CodeFlix.Catalog.Domain.Entity
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Category(string name, string description, bool isActive = true)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsActive = isActive;
            CreatedAt = DateTime.Now;

            Validate();
        }

        public void Validate()
        {
            if (String.IsNullOrWhiteSpace(Name))
            {
                throw new EntityValidationException("Name should not be empty or null");
            }
            if(Description == null)
            {
                throw new EntityValidationException("Description should not be empty or null");
            }
        }
    }
}