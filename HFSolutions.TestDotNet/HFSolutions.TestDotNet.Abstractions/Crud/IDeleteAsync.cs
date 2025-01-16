namespace HFSolutions.TestDotNet.Abstractions.Crud
{
    public interface IDeleteAsync<TModelId>
    {
        Task<bool> DeleteAsync(TModelId modelId);
    }
}
