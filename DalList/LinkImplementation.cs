namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class LinkImplementation:ILink
{
    public int Create(Link link)
    {
        //for entities with normal id (not auto id)
        if (Read(link.IdLink) is not null)
            throw new Exception($"Link with ID={link.IdLink} already exists");
        //for entities with auto id
        int id = DataSource.Config.IdAssignments;
        Link copy = link with { IdLink = id };
        DataSource.Links.Add(copy);
        return id;

       // DataSource.Links.Add(link);
       // return link.IdLink;


       
    }
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Link with ID={id} not exists");
        Link link = Read(id);
        DataSource.Links.Remove(link);
    }
    public Link? Read(int id)
    {
        if (DataSource.Links.Find(IdL => IdL.IdLink == id) != null)
            return DataSource.Links.Find(IdL => IdL.IdLink == id);
        return null;
    }

    public List<Link> ReadAll()
    {
        return new List <Link> (DataSource.Links.ToList());
    }
    public void Update(ref Link item)
    {
        Delete(item.IdLink);
        Create(item);
    }
}
