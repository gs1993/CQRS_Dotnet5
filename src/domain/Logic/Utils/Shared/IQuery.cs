using System.Threading.Tasks;

namespace Logic.Utils.Shared
{
    public interface IQuery<TResult>
    {
    }

    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}