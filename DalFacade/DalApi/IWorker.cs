﻿namespace DalApi;
using DO;
public interface IWorker
{
    int Create(Worker item); //Creates new entity object in DAL
    Worker? Read(int id); //Reads entity object by its ID 
    List<Worker> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(ref Worker item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
