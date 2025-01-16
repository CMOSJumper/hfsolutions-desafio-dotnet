namespace HFSolutions.TestDotNet.Abstractions.Crud
{
    public interface IUpdateAsync<TModelId, TModel> where TModel : class
    {
        Task<TModel> UpdateAsync(TModelId modelId, TModel model);
    }
}
