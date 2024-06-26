using CodeFlix.Catalog.Domain.Exceptions;
using CodeFlix.Catalog.Domain.SeedWork;

namespace CodeFlix.Catalog.Domain.Entity;

public class Category : AggreateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public void Activate()
    { 
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }

    public void Update(string name, string description = null)
    {
        Name = name;
        Description = description ?? Description;

        Validate();
    }

    private void Validate()
    {
        if (String.IsNullOrWhiteSpace(Name))
        {
            throw new EntityValidationException("Name should not be empty or null");
        }
        if(Name.Length < 3)
        {
            throw new EntityValidationException("Name should has at least 3 characters");
        }
        if (Name.Length > 255)
        {
            throw new EntityValidationException("Name should has less then 255 characters");
        }
        if (Description == null)
        {
            throw new EntityValidationException("Description should not be empty or null");
        }
        if (Description.Length > 10000)
        {
            throw new EntityValidationException("Description should has less then 10.000 characters");
        }
    }
}