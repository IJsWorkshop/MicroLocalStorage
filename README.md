<h1>MicroLocalStorage</h1>

<p>Always wanted to keep some settings local but found it to much work and stress to use the built in tools.</p>
<p>This library has been made to keep local settings easy to keep and store.</p>

<p>Storage file is used to store data in a well formed xml</p>

```Xml
<?xml version='1.0' encoding='UTF-16' ?>
<?xml-stylesheet type="text/xsl" href="union.xsl"?>
<root>
<one>yooooooooooooooooooooo</one>
<two>twoooooooooo</two>
<three>threeeeeeeee</three>
<four>fourrrrrrrrrr</four>
</root>
```

<p>The data is stored as follows</p>

```Xml

<?xml version='1.0' encoding='UTF-16' ?>
<?xml-stylesheet type="text/xsl" href="union.xsl"?>
<root>
<one>yooooooooooooooooooooo</one>
<two>twoooooooooo</two>
<three>threeeeeeeee</three>
<four>fourrrrrrrrrr</four>
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
ms.Add(new StorageElement("one", "oneeeeeeeeee"));
ms.Add(new StorageElement("two", "twoooooooooo"));
ms.Add(new StorageElement("three", "threeeeeeeee"));
ms.Add(new StorageElement("four", "fourrrrrrrrrr"));
ms.Save();
```

<p>If you remove an item from memory dont forget to commit the changes to the storage file/p>

```C#
ms.Remove(new StorageElement("four", null));
ms.Save();
```

<p>If you want to update a value and commit changes</p>

```C#
ms.Update(new StorageElement("one", "yooooooooooooooooooooo"));
ms.Save();
```


