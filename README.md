<h1>MicroLocalStorage</h1>

<p>Always wanted to keep some settings local but found it to much work and stress to use the built in tools.</p>
<p>This library has been made to keep local settings easy to keep and store.</p>

<p>Storage file is used to store data in a well formed xml file</p>

```Xml
<?xml version='1.0' encoding='UTF-16' ?>
<?xml-stylesheet type="text/xsl" href="union.xsl"?>
<root>
<one type='string'>yooooooooooooooooooooo</one>
<two type='int'>123</two>
<three type='string'>threeeeeeeee</three>
<four type='double'>123.01</four>
</root>
```

<p>The data is stored as follows</p>

```Xml

<?xml version='1.0' encoding='UTF-16' ?>
<?xml-stylesheet type="text/xsl" href="union.xsl"?>
<root>
<one type='string'>yooooooooooooooooooooo</one>
<two type='int'>123</two>
<three type='string'>threeeeeeeee</three>
<four type='double'>123.01</four>
</root>
```

<h3>How to Use</h3>

<p>To use just create a storage instance</p>

```C#
var ms = new Storage();
```

<p>To commit changes from memory store to filesystem just use save()</p>

```C#
ms.Save();
```

<p>Add new elements to Memory and save to filesystem</p>

```C#
ms.Add(new StorageElement("one", "123.23","double"));
ms.Add(new StorageElement("two", "twoooooooooo", "string"));
ms.Add(new StorageElement("three", "45", "int"));
ms.Add(new StorageElement("four", "fourrrrrrrrrr", "string"));
ms.Save();
```

<p>If you remove an item from memory dont forget to commit the changes to the storage file/p>

```C#
ms.Remove("four"));
ms.Save();
```

<p>If you want to update a value and commit changes</p>

```C#
ms.Update(new StorageElement("one", "yooooooooooooooooooooo", "string"));
ms.Save();
```


