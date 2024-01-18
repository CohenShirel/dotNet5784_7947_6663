namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml;
using System.Xml.Linq;



internal class WorkerImplementation : IWorker
{
    readonly string s_workers_xml = "workers";

    static Worker getWorker(XmlElement wrk)
    {
        return new Worker()
        {
            IdWorker = int.TryParse((string?)wrk.Element("idWorker"), out var idWorker) ? idWorker : throw new FormatException("can't convert id"),
            Name = (string)wrk.Element("Name") ?? "",
            Email=(string)wrk.Element("Email") ?? "",


            //        int IdWorker,
            //Level Experience,
            //int HourSalary,
            //string? Name = null,
            //string? Email = null
        };
    }
    public int Create(Worker item)
    {
        XElement? wrkRoot = XMLTools.LoadListFromXMLElement(s_workers_xml);
        XElement? wrkElem = wrkRoot.Elements().FirstOrDefault(wrk => (int?)wrk.Element("IdWorker") == item.IdWorker);
        if(wrkElem != null)
            throw new DalAlreadyExistsException($"Worker with ID={item.IdWorker} already exists");
        wrkRoot.Add(item);
        XMLTools.SaveListToXMLElement(wrkRoot, s_workers_xml);//save the changes in the file
        return item.IdWorker;
    }

    public void Delete(int id)
    {
        XElement? wrkRoot = XMLTools.LoadListFromXMLElement(s_workers_xml);
        XElement? elemToDelete = wrkRoot.Elements().FirstOrDefault(wrk => (int?)wrk.Element("idWorker") == id);
        if (elemToDelete != null) 
        {
            elemToDelete.Remove();//delete
            XMLTools.SaveListToXMLElement(wrkRoot, s_workers_xml);//save the changes in the file
        }
    }

    public Worker? Read(int id)
    {
       XElement? wrkRoot=XMLTools.LoadListFromXMLElement(s_workers_xml);
       XElement? wrkElem = wrkRoot.Elements().FirstOrDefault(wrk=>(int?)wrk.Element("IdWorker") ==id);
        if(wrkElem!=null)
        {
            return getWorker(wrkElem);
        }
        else
            return null;




            throw new DalDoesNotExistException($"Worker with ID={id} not exists");

       // return DataSource.Workers.FirstOrDefault(filter);

        //else
        //   return null; 
    }

    public IEnumerable<Worker?> ReadAll(Func<Worker, bool>? filter = null)
    {
        //if (filter != null)
        //{
        //    return from item in DataSource.Workers
        //           where filter(item)
        //           select item;
        //}
        //return from item in DataSource.Workers
        //       select item;
        return DataSource.Workers.FirstOrDefault(filter);
    }

    public void Update(ref Worker item)
    {
        Delete(item.IdWorker);
        Create(item);
    }
}
