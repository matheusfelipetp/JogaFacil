﻿namespace JogaFacil.Domain.Repositories
{
    public interface IUnityOfWork
    {
        Task Commit();
    }
}