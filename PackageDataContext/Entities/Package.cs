namespace PackageDataContext.Entities;

public class Package
{
    public int Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string SenderName { get; set; }
    public string ReceiverName { get; set; }
    public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public int Weight { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Package package &&
               Id == package.Id &&
               SenderName == package.SenderName &&
               ReceiverName == package.ReceiverName &&
               Description == package.Description &&
               Weight == package.Weight;
    }

    public override int GetHashCode()
    {
        int result = Id.GetHashCode();
        result = 31 * result + (SenderName?.GetHashCode() ?? 0);
        result = 31 * result + (ReceiverName?.GetHashCode() ?? 0);
        result = 31 * result + (Description?.GetHashCode() ?? 0);
        result = 31 * result + Weight.GetHashCode();

        return result;
    }
}
