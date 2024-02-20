

using DalApi;
using DO;

namespace Dal;

internal class AssignmentsImplementation : IAssignments
{
    readonly string s_assignmentss_xml = "assignmentss";
    public int Create(Assignment ass)
    {
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        Assignment newAssignments = ass with { IdAssignment = Config.IdPAssignments };
        assignmentss.Add(newAssignments);
        XMLTools.SaveListToXMLSerializer<DO.Assignment>(assignmentss, s_assignmentss_xml);
        return newAssignments.IdAssignment;
    }

    public void Delete(int id)
    {
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        if (assignmentss.RemoveAll(item => item.IdAssignment == id) == 0)
        {
            throw new DalDoesNotExistException($"Assignment with id {id} does not exist");
        }
        //if (Read(a => a.IdAssignment == id) is null)
        //    throw new DalDoesNotExistException($"Assignment with ID={id} not exists");
        //assignmentss.Remove(Read(a => a.IdAssignment == id)!);
        XMLTools.SaveListToXMLSerializer<DO.Assignment>(assignmentss, s_assignmentss_xml);
    }

    public void DeleteAll()
    {
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        assignmentss.RemoveAll(item => true);
        XMLTools.SaveListToXMLSerializer<DO.Assignment>(assignmentss, s_assignmentss_xml);
    }
    public Assignment? Read(Func<Assignment, bool> filter)
    {
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        return assignmentss.FirstOrDefault(filter);
    }
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
    {
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);

        if (filter != null)
        {
            return from item in assignmentss
                   where filter(item)
                   select item;
        }
        return from item in assignmentss
               select item;
    }

    public void Update(Assignment ass)
    {
        //Delete(ass.IdAssignment);
        //Create(ass);
        List<DO.Assignment> assignmentss = XMLTools.LoadListFromXMLSerializer<DO.Assignment>(s_assignmentss_xml);
        if (assignmentss.RemoveAll(item => item.IdAssignment == ass.IdAssignment) == 0)
        {
            throw new DalDoesNotExistException($"Assignment with id {ass.IdAssignment} does not exist");
        }
        assignmentss.Add(ass);
        XMLTools.SaveListToXMLSerializer<DO.Assignment>(assignmentss, s_assignmentss_xml);

    }
}
