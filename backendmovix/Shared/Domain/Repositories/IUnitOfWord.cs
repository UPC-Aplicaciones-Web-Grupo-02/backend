namespace backendmovix.Shared.Domain.Repositories;
/// <summary>
///     Unit of work interface for all repositories
/// </summary>
public interface IUnitOfWord
{
    /// <summary>
    ///     Save changes to the repository
    /// </summary>
    /// <returns></returns>
    Task CompleteAsync();
}