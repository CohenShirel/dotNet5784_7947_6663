

namespace DalApi;
using DO;

public interface ILink
{
    int Create(ILink item); //Creates new entity object in DAL
    ILink? Read(int id); //Reads entity object by its ID 
    List<ILink> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(ILink item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
