﻿namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
