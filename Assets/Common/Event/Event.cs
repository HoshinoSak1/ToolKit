using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData 
{
    public EventName Name;
    public List<Delegate> handles;

    public EventData(EventName name)
    {
        this.Name = name;
        this.handles = new List<Delegate>(4);
    }

    public void Add(Delegate d)
    {
        this.handles.Add(d);
    }

    public void Remove(Delegate d)
    {
        for (int i = 0; i < this.handles.Count; i++)
        {
            if (this.handles[i] == d)
            {
                this.handles.RemoveAt(i);
            }
        }
        
    }
    public void RemoveAll()
    {
        handles.Clear();
    }
    public void Fire(params object[] parameters)
    {
        try
        {
            foreach(Delegate handle in handles)
            {

                handle.DynamicInvoke(parameters);
            }
        }
        catch (Exception ex)
        {
            if (null != ex.InnerException)
            {
                ex = ex.InnerException;
            }
        }
    }

    public void Destroy()
    {

    }
}
