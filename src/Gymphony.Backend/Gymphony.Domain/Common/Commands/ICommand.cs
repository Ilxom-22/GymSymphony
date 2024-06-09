using MediatR;

namespace Gymphony.Domain.Common.Commands;

public interface ICommand<out TResult> : ICommand, IRequest<TResult>
{
    
}

public interface ICommand
{
    
}