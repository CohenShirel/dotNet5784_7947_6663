

namespace DalApi;
using DO;
public interface IAssignments
{
    int Create(IWorker item); //Creates new entity object in DAL
    IWorker? Read(int id); //Reads entity object by its ID 
    List<IWorker> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(IWorker item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
