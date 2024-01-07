
namespace DalApi;
using DO;
public interface IAssignments
{
    int Create(Assignments item); //Creates new entity object in DAL
    Assignments? Read(int id); //Reads entity object by its ID 
    public int ReadName(string name);
    List<Assignments> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(ref Assignments item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
