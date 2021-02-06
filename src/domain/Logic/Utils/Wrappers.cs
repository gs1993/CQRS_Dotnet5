using CSharpFunctionalExtensions;
using MediatR;

namespace Logic.Utils
{
    public interface IRequestWrapper<T> : IRequest<Result<T>>
    { 
    }

    public interface IHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, Result<TOut>> 
        where TIn : IRequestWrapper<TOut>
    { 
    }
}
