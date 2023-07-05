using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MicroLocalStorage
{
    public class FileSystem
    {
        public class Storage
        {
            public Dictionary<string?, StorageElement> InMemoryStorage = new Dictionary<string?, StorageElement>();

            string TopHeader = "<?xml version='1.0' encoding='UTF-16' ?>\r\n<?xml-stylesheet type=\"text/xsl\" href=\"union.xsl\"?>";

            public Storage() { }

            public class StorageElement : IStorageElement
            {
                public string? Name { get; set; }
                public string? Value { get; set; }

                public StorageElement()
                {
                }

                public StorageElement(string? Name, string? Value)
                {
                    this.Name = Name;
                    this.Value = Value;
                }
            }
            public interface IStorageElement
            {
                string? Name { get; set; }
                string? Value { get; set; }
            }
            public void Create()
            {
                string FullHeader = "<?xml version='1.0' encoding='UTF-16' ?>\r\n<?xml-stylesheet type=\"text/xsl\" href=\"union.xsl\"?>\r\n<root>\r\n</root>";

                // create storage file if not exist
                var filename = GetFileName();

                if (!File.Exists(filename))
                    File.WriteAllText(filename, FullHeader);
                else
                    File.WriteAllText(filename, FullHeader);
            }
            bool IsStorageActive(string file) 
            {
                if(File.Exists(file))
                return true;
                else
                return false;
            }
            public void LoadToMemory()
            {
                var filename = GetFileName();

                if (!IsStorageActive(filename))
                    Create();

                var nodes = File.ReadAllLines(filename);

                if (nodes.Length < 5) Debug.WriteLine("No data in storeage");

                foreach (var node in nodes) 
                {
                    var data = GetElement(node);
                    
                    Add(new StorageElement(data.Name ,data.Value));
                }
            }
            public void Save()
            {
                var filename = GetFileName();

                System.IO.FileMode fm = FileMode.Create;

                if (!IsStorageActive(filename))
                    fm = FileMode.Create;
                else
                    fm = FileMode.Truncate;

                using (var fs = new FileStream(filename, fm))
                using (var file = new StreamWriter(fs))
                {
                    file.WriteLine(TopHeader);
                    file.WriteLine("<root>");
                    foreach (var element in InMemoryStorage)
                    {
                        file.WriteLine($"<{element.Key}>{element.Value.Value}</{element.Key}>");
                    }
                    file.WriteLine("</root>");
                    file.Close();
                }
            }
            public void Add(IStorageElement element) => AddElement(element);
            public void Update(IStorageElement element) => UpdateElement(element);
            public void Remove(IStorageElement element) => RemoveElement(element);
            bool FindElement(IStorageElement element) 
            {
                if (InMemoryStorage.TryGetValue(element.Name, out var Se))
                    return true;
                else
                    return false;
            }
            void RemoveElement(IStorageElement element) 
            {
                if (FindElement(element))
                {
                    InMemoryStorage.Remove(element.Name);
                }
            }
            void AddElement(IStorageElement element) 
            {
                if (element.Name != "" && element.Name != "")
                {
                    if (!FindElement(element))
                    {
                        InMemoryStorage.Add(element.Name, new StorageElement(element.Name, element.Value));
                    }
                }
            }
            void UpdateElement(IStorageElement element) 
            {
                if (FindElement(element))
                {
                    InMemoryStorage[element.Name] = new StorageElement(element.Name, element.Value);
                }
            }
            string GetFileName() 
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var filename = string.Concat(dir, @"\Storage.ini");

                return filename;
            }
            IStorageElement GetElement(string? element) 
            {
                // learn element
                var FullElement = new Regex(@"(<[a-zA-Z]+>)([a-zA-Z0-9]+)((</[a-zA-Z]+>))").Match(element).Groups;

                // get element name
                var ElementName = FullElement[1].Value;

                // remove brackets
                ElementName = ElementName.Replace("<", "").Replace(">", "");

                // element value of <element>
                var ElementValue = FullElement[2].Value;

                return new StorageElement(ElementName,ElementValue);
            }
        }
    }
}
