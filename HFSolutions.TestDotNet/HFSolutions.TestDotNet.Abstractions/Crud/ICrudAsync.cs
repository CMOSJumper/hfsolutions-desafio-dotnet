namespace HFSolutions.TestDotNet.Abstractions.Crud
{
    public interface ICrudAsync<TModelId, TModel> : ICreateAsync<TModel>, IReadAsync<TModelId, TModel>, IUpdateAsync<TModelId, TModel>, IDeleteAsync<TModelId>
        where TModel : class
    {
    }
}
