namespace HFSolutions.TestDotNet.Abstractions.Crud
{
    public interface ICreateAsync<TModel> where TModel : class
    {
        Task<TModel> CreateAsync(TModel model);
    }
}
