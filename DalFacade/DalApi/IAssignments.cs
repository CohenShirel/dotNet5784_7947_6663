

namespace DalApi;
using DO;
public interface IAssignments
{
    int Create(IAssignments item); //Creates new entity object in DAL
    IAssignments? Read(int id); //Reads entity object by its ID 
    List<IAssignments> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(IAssignments item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
