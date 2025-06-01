using System.ComponentModel.DataAnnotations;

namespace Shared.Extensions;

/// <summary>
///     Base class for entities with common properties
/// </summary>
public abstract class EntityBase<TKey>
{
    [Key] public TKey Id { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}
