using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml.Linq;

namespace MicroLocalStorage
{
    public class FileSystem
    {
        public class Storage
        {
            public Dictionary<string, StorageElement> InMemoryStorage = new Dictionary<string, StorageElement>();

            static Dictionary<string, Type> TypeDictionary { get; set; } = BuildDictionary();


            string TopHeader = "<?xml version='1.0' encoding='UTF-16' ?>\r\n<?xml-stylesheet type=\"text/xsl\" href=\"union.xsl\"?>";

            public Storage() { }

            public class StorageElement : IStorageElement
            {
                public string? Name { get; set; } = " ";
                public string? Value { get; set; } = " ";
                public string? Type { get; set; } = " ";

                public StorageElement()
                {
                }

                public StorageElement(string? Name, string? Value, string? type)
                {
                    this.Name = Name;
                    this.Value = Value;
                    this.Type = type;
                }
            }
            public interface IStorageElement
            {
                string? Name { get; set; }
                string? Value { get; set; }
                string? Type { get; set; }
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
                if (File.Exists(file))
                    return true;
                else
                    return false;
            }

            static List<string> IgnoreNodesList = new List<string>() { "<?xml", "<root>", "</root"};

            public void LoadToMemory()
            {
                var filename = GetFileName();

                if (!IsStorageActive(filename))
                    Create();

                var nodes = File.ReadAllLines(filename);

                if (nodes.Length < 5) throw new Exception("No data in storeage");


                bool IgnoredNode(string node) 
                {
                    if (IgnoreNodesList.Any(o => node.StartsWith(o)))
                        return true;
                    else
                        return false;
                }

                foreach (var node in nodes)
                {
                    if (!IgnoredNode(node)) 
                    {
                        var data = GetElement(node);
                        Add(new StorageElement(data.Name, data.Value, data.Type));
                    }
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
                        file.WriteLine($"<{element.Key} type='{element.Value.Type}' >{element.Value.Value}</{element.Key}>");
                    }
                    file.WriteLine("</root>");
                    file.Close();
                }
            }
            public IStorageElement Get(IStorageElement element) => FetchElement(element);
            public void Add(IStorageElement element) => AddElement(element);
            public void Update(IStorageElement element) => UpdateElement(element);
            public void Remove(string keyName) => RemoveElement(keyName);
            StorageElement FetchElement(IStorageElement element)
            {
                if (InMemoryStorage.TryGetValue(element.Name, out var Se))
                    return Se;
                else
                    throw new Exception("Element does not exist, please add to datastore and save");
            }
            bool FindElement(string keyName)
            {
                if (InMemoryStorage.TryGetValue(keyName, out var Se))
                    return true;
                else
                    return false;
            }
            void RemoveElement(string keyName)
            {
                if (FindElement(keyName))
                {
                    InMemoryStorage.Remove(keyName);
                }
            }
            void AddElement(IStorageElement element)
            {
                if (element.Name != "" && element.Name != "")
                {
                    if (!FindElement(element.Name))
                    {
                        InMemoryStorage.Add(element.Name, new StorageElement(element.Name, element.Value, element.Type));
                    }
                }
            }
            void UpdateElement(IStorageElement element)
            {
                if (FindElement(element.Name))
                {
                    InMemoryStorage[element.Name] = new StorageElement(element.Name, element.Value, element.Type);
                }
            }
            string GetFileName()
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var filename = string.Concat(dir, @"\Storage.ini");

                return filename;
            }
            IStorageElement GetElement(string element = "")
            {
                // learn element
                var FullElement = new Regex(@"(<[a-zA-Z].+>)([a-zA-Z0-9].+)(</[a-zA-Z]+>)").Match(element).Groups;

                // get element name
                var ElementName = FullElement[1].Value;

                // remove inner elementnames and brakets
                ElementName = ElementName.Replace("<", "").Replace(">", "");

                var ElementNameSplit = ElementName.Split(" ");

                // element name
                ElementName = ElementNameSplit.FirstOrDefault();
                // element inner type
                var ElementStringType = ElementNameSplit.Where(o => o.Contains("type")).FirstOrDefault();
                // element inner type 
                var ElementType = GetElementType(GetElementTypeString(ElementStringType));
                // element value of <element>
                var ElementValue = FullElement[2].Value;
                // return new storage element
                return new StorageElement(ElementName, ElementValue, ElementType);
            }

            static Dictionary<string, Type> BuildDictionary() 
            {
                Dictionary<string, Type> o = new Dictionary<string, Type>();

                o.Add("String", typeof(String));
                o.Add("string", typeof(string));
                o.Add("Boolean", typeof(Boolean));
                o.Add("bool", typeof(bool));
                o.Add("Byte", typeof(Byte));
                o.Add("byte", typeof(byte));
                o.Add("SByte", typeof(SByte));
                o.Add("sbyte", typeof(sbyte));
                o.Add("Char", typeof(Char));
                o.Add("char", typeof(char));
                o.Add("Decimal", typeof(Decimal));
                o.Add("decimal", typeof(decimal));
                o.Add("Double", typeof(Double));
                o.Add("double", typeof(double));
                o.Add("Single", typeof(Single));
                o.Add("single", typeof(Single));
                o.Add("Int16", typeof(Int16));
                o.Add("int", typeof(int));
                o.Add("UInt16", typeof(UInt16));
                o.Add("IntPtr", typeof(IntPtr));
                o.Add("UIntPtr", typeof(UIntPtr));
                o.Add("Int64", typeof(Int64));
                o.Add("UInt64", typeof(UInt64));
                o.Add("short", typeof(Int16));
                o.Add("ushort", typeof(UInt16));

                return TypeDictionary = o;
            }

            string GetElementType(string typename)
            {
                if (TypeDictionary.TryGetValue(typename, out var t))
                    return t.Name;
                else
                    throw new Exception("type not known to element type database");
            }

            string GetElementTypeString(string elementstring) 
            {
                var elementsplit = elementstring.Split('\'');

                // if well formed inner element
                if (elementsplit.Length == 3) 
                    return elementsplit[1];
                else
                    throw new Exception("");
            }

        }    
    }
}
