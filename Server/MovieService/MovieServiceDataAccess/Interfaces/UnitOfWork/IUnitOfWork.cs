﻿using MovieServiceDataAccess.Interfaces.Repositories;

namespace MovieServiceDataAccess.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movies { get; }
        IGenreRepository Genres { get; }
        IPersonRepository People { get; }
        IWatchlistRepository Watchlists{ get; }
        IReviewRepository Reviews{ get; }
        Task SaveAsync();
    }
}
