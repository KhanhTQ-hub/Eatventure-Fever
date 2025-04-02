using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.unimob.serialize.core;
using UnityEngine;

public class TestReflection : MonoBehaviour
{
    private void Start()
    {
        //var reflectionInfo = new ReflectionInformation();
        //var type = reflectionInfo.GetType();

        //var firstReflection = (ReflectionInformation)Activator.CreateInstance(type, new object[] { 10 });  //  Một tham số
        //var secondReflection = (ReflectionInformation)Activator.CreateInstance(type, new object[] { 10, "Name" });  //  Hai tham số

        //type.Dump();

        //foreach (var item in type.GetConstructors())
        //{
        //    item.Dump();
        //}

        //foreach (var item in type.CustomAttributes)
        //{
        //    item.Dump();
        //}

        //foreach (var item in type.GetMethods())
        //{
        //    item.Dump();
        //}

        //foreach (var item in type.GetProperties())
        //{
        //    item.Dump();
        //}

        //foreach (var item in type.GetFields())
        //{
        //    item.Dump();
        //}


        var type = typeof(IValidate);

        var needValids = AppDomain.CurrentDomain.GetAssemblies()
                                        .SelectMany(s => s.GetTypes())
                                        .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
        foreach (var item in needValids)
        {
            item.Dump();
        }
    }
}

[Custom]
public class ReflectionInformation
{
    [Custom]
    private int _id;

    private string _name;

    [Custom]
    public ReflectionInformation()
    {

    }

    public ReflectionInformation(int id)
    {
        Id = id;
    }

    public ReflectionInformation(int id, string name)
    {
        Id = id;
        Name = name;
    }

    [Custom]
    public int Id { get; set; }
    public string Name { get; set; }

    [Custom]
    public void Write()
    {
        Console.WriteLine("Id: " + Id);
        Console.WriteLine("Name: " + Name);
    }

    public void Write(string name)
    {
        Console.WriteLine("Name: " + name);
    }
}

public class CustomAttribute : Attribute
{
    public string Name { get; set; }

    public void Write()
    {
        Console.WriteLine("Hello CustomAttribute.");
    }
}

public interface IValidate
{
    bool IsOk(string text);
}

public class TextNotEmpty : IValidate
{
    public bool IsOk(string text)
    {
        return !string.IsNullOrEmpty(text);
    }
}

public class TextAtLeast8Chars : IValidate
{
    public bool IsOk(string text)
    {
        return text.Length >= 8;
    }
}