using Microsoft.AspNetCore.Mvc;

namespace RS1_2024_25.API.Helper.Api;

//https://github.com/ardalis/ApiEndpoints/blob/main/src/Ardalis.ApiEndpoints/FluentGenerics/EndpointBaseAsync.cs
public static class MyEndpointBaseAsync
{
    public static class WithRequest<TRequest>
    {
        public abstract class WithResult<TResponse> : MyEndpointBase
        {
            public abstract Task<TResponse> HandleAsync(
                TRequest request,
                CancellationToken cancellationToken = default
            );
        }

        public abstract class WithoutResult : MyEndpointBase
        {
            public abstract Task HandleAsync(
                TRequest request,
                CancellationToken cancellationToken = default
            );
        }

        public abstract class WithActionResult<TResponse> : MyEndpointBase
        {
            public abstract Task<ActionResult<TResponse>> HandleAsync(
                TRequest request,
                CancellationToken cancellationToken = default
            );
        }
        public abstract class WithActionResult : MyEndpointBase
        {
            public abstract Task<ActionResult> HandleAsync(
                TRequest request,
                CancellationToken cancellationToken = default
            );
        }
        //public abstract class WithAsyncEnumerableResult<T> : MyEndpointBase
        //{
        //    public abstract IAsyncEnumerable<T> HandleAsync(
        //      TRequest request,
        //      CancellationToken cancellationToken = default
        //    );
        //}
    }

    public static class WithoutRequest
    {
        public abstract class WithResult<TResponse> : MyEndpointBase
        {
            public abstract Task<TResponse> HandleAsync(
                CancellationToken cancellationToken = default
            );
        }

        public abstract class WithoutResult : MyEndpointBase
        {
            public abstract Task HandleAsync(
                CancellationToken cancellationToken = default
            );
        }

        public abstract class WithActionResult<TResponse> : MyEndpointBase
        {
            public abstract Task<ActionResult<TResponse>> HandleAsync(
                CancellationToken cancellationToken = default
            );
        }

        public abstract class WithActionResult : MyEndpointBase
        {
            public abstract Task<ActionResult> HandleAsync(
                CancellationToken cancellationToken = default
            );
        }

        //public abstract class WithAsyncEnumerableResult<T> : MyEndpointBase
        //{
        //    public abstract IAsyncEnumerable<T> HandleAsync(
        //      CancellationToken cancellationToken = default
        //    );
        //}
    }
}