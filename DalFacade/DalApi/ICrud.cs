﻿namespace DalApi;

public interface ICrud<T> where T : class
{
    int Create(T item);
    T? Read(Func<T, bool> filter); // stage 2
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null); // stage 2
    void Update(T item);
    void Delete(int id);
    void DeleteAll();
}
