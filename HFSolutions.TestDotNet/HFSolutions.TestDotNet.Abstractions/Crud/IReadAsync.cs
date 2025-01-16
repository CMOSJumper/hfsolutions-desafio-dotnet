using System.Linq.Expressions;

namespace HFSolutions.TestDotNet.Abstractions.Crud
{
    public interface IReadAsync<TModelId, TModel> where TModel : class
    {
        Task<TModel> ReadAsync(TModelId modelId);
        Task<IEnumerable<TModel>> ReadAllAsync(Expression<Func<TModel, bool>>? predicate = default);
    }
}
