// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using static MicroLocalStorage.FileSystem;
using static MicroLocalStorage.FileSystem.Storage;



var ms = new Storage();

ms.Add(new StorageElement("one", "oneeeeeeeeee", "string"));
ms.Add(new StorageElement("two", "twoooooooooo", "string"));
ms.Add(new StorageElement("three", "threeeeeeeee", "string"));
ms.Add(new StorageElement("four", "123.32", "double"));
ms.Save();


// load to memory
ms.LoadToMemory();


// show elements from memory
foreach (var ele in ms.InMemoryStorage.ToList())
{
    Debug.WriteLine($"Key:{ele.Key,10}       Value:{ele.Value.Value,20}      Type:{ele.Value.Type,20}");
}


// remove element 4
ms.Remove("four");
ms.Save();


ms.Update(new StorageElement("one", "123.23", "double"));
ms.Save();

Debug.WriteLine("");

// show elements
foreach (var ele in ms.InMemoryStorage.ToList())
{
    Debug.WriteLine($"Key:{ele.Key,10}      Value:{ele.Value.Value,20}      Type:{ele.Value.Type,20}");
}