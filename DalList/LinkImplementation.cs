

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class LinkImplementation : ILink
{
    public int Create(ILink link)
    {
        //for entities with normal id (not auto id)
        if (Read(link.IdLink) is not null)
            throw new Exception($"Link with ID={link.IdLink} already exists");
        DataSource.Links.Add(link);
        return link.IdLink;
    }
    //checkkkkkkkkkkkkkkkkkkkkkkkkkkk
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new Exception($"Link with ID={id} not exists");
        Link link = Read(id);
        DataSource.Links.Remove(link);
        return link.IdWorker;
    }

    public ILink? Read(int id)
    {
        return (DataSource.Links.Find(IdL->IdL.IdLink == id) = !null)

    }

    public List<ILink> ReadAll()
    {
        return new List<Link>(DataSource.Links.ToList);
    }
    //checkkkkkkkkkkkkkkkkkkk
    public void Update(ref Link item)
    {
        if (Read(item.IdLink) is null)
            throw new Exception($"Link with ID={item.IdLink} not exists");
        Link item = Read(item.IdLink);
        //worker.Experience = w.Experience;
        //worker.HourSalary = w.HourSalary;
        //worker.Name = w.Name;
        //worker.Email = w.Email;
    }
}
