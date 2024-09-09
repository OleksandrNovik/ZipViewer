namespace ZipViewer.Models.Contracts;

/// <summary>
/// Contract that checks if item with specified name is contained
/// </summary>
public interface IEntriesContainer
{
    /// <summary>
    /// Checks if item is inside container
    /// </summary>
    /// <param name="name"> Name of item to check </param>
    /// <returns> True if item is inside of container, False if there is no such item </returns>
    public bool Contains(string name);
}
