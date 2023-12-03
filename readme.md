# Notes
- Q03 - Q07 final codes code are available in solution. it will compile and run

# Question 01
## Original Code
```csharp
if (application != null)
{
    if (application.protected != null)
      {
    return application.protected.shieldLastRun;
      }
}
```
## Analysis
### Concern
- code readability
### Improvement
- leverage null condition operator ('?.') to handle null check.
- if any part of the chain is null, it returns null without the need for explicit if statements.
## Final Code
```csharp
return application?.protected?.shieldLastRun;
```

# Question 02
## Original Code
```csharp
public ApplicationInfo GetInfo()
{
    var application = new ApplicationInfo
    {
        Path = "C:/apps/",
        Name = "Shield.exe"
    };
    return application;
}
```
## Analysis
### Concern
- value for path and name is hardcoded
- return more than one value from class method
### Improvement
- depending on the use case using hardcoded value might be fine, but it is advised to make it more dynamic either by passing it as parameter or using function to get path value from a configuration (and set default value if no configuration available)
- to return more than one value from class method, we can use tuple return on this method or implementation of class object.
- using class object will be better for future maintainability. since if we need to extend the information in the future, we can easily add properties to `ApplicationInfo` class without changing method signature.
## Final Code
```csharp
public class ApplicationInfo
{
    public string Path { get; set; }
    public string Name { get; set; }
}

public class ApplicationService
{
    private readonly IConfiguration _configuration;

    public ApplicationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationInfo GetInfo()
    {
        var application = new ApplicationInfo
        {
            Path = _configuration["ApplicationPath"] ?? "C:/apps/",
            Name = "Shield.exe"
        };
        return application;
    }
}

```

# Question 03
## Original Code
```csharp
class Laptop
{
	public string Os{ get; set; } // can be modified
	public Laptop(string os)     
	{      
		Os= os;     
	} 
}
var laptop = new Laptop("macOs");
Console.WriteLine(Laptop.Os); // Laptop os: macOs 
```
## Analysis
### Concern
- modification to private member
### Improvement
- add private field to store os information
- either creating a method or using property  getter/setter to retrieve and assign value
- with method we have plenty of controls. but for something straightforward and doesn't need additional processing like this case, getter/setter is a good choice.
# Final Code
```csharp
var laptop = new Laptop("macOs");
Console.WriteLine("Laptop OS: " + laptop.Os); // Laptop os: macOs

laptop.Os = "Windows";
Console.WriteLine("Laptop OS: " + laptop.Os); // Laptop os: Windows

class Laptop
{
    private string _os; // Private field to store the OS information

    public Laptop(string os)
    {
        _os = os; // Assign the provided OS to the private field
    }

    public string Os
    {
        get { return _os; } // Return OS information value
        set { _os = value; } // Set OS information value
    }
}
```

# Question 04
## Original Code
```csharp
using System; 
using System.Collections.Generic; 

namespace MemoryLeakExample 
{ 
    class Program 
    { 
        static void Main(string[] args) 
        { 
            var myList = new List(); 
            while (true) 
            { 
                // populate list with 1000 integers  
                for (int i = 0; i < 1000; i++) 
                { 
                    myList.Add(new Product(Guid.NewGuid().ToString(), i)); 
                } 
                // do something with the list object  
                Console.WriteLine(myList.Count); 
            } 
        } 
    } 

    class Product 
    { 
        public Product(string sku, decimal price) 
        { 
            SKU = sku; 
            Price = price; 
        } 

        public string SKU { get; set; } 
        public decimal Price { get; set; } 
    } 
} 
```
## Analysis
### Concern
- Memory leak
- each loop will add the list, this will ultimately increase memory usage for list of product and crash the app at some point (not a matter of 'if' but 'when' based on available resource).
### Improvement
- clear or create new list on each loop. creating new list each loop would be preferable, because this will ensure the previous list is no longer referenced, and it becomes eligible for garbage collection.
## Final Code
```csharp
while (true)
{
    // create new list of product
    var myList = new List<Product>();

    for (int i = 0; i < 1000; i++)
    {
        myList.Add(new Product(Guid.NewGuid().ToString(), i));
    }
    // do something with the list object  
    Console.WriteLine(myList.Count);

    // Clear the list to release the references
    myList.Clear();
}

class Product
{
    public Product(string sku, decimal price)
    {
        SKU = sku;
        Price = price;
    }

    public string SKU { get; set; }
    public decimal Price { get; set; }
}```

# Question 05
## Original Code
```csharp
using System; 
namespace MemoryLeakExample 
{ 
    class Program 
    { 
        static void Main(string[] args) 
        { 
            var publisher = new EventPublisher(); 

            while (true) 
            { 
                var subscriber = new EventSubscriber(publisher); 
                // do something with the publisher and subscriber objects  
            } 
        } 

        class EventPublisher 
        { 
            public event EventHandler MyEvent; 

            public void RaiseEvent() 
            { 
                MyEvent?.Invoke(this, EventArgs.Empty); 
            } 
        } 

    class EventSubscriber 
       { 
            public EventSubscriber(EventPublisher publisher) 
            { 
                publisher.MyEvent += OnMyEvent; 
            } 

            private void OnMyEvent(object sender, EventArgs e) 
            { 
                Console.WriteLine("MyEvent raised"); 
            } 
        } 
    } 
} 
```
## Analysis
### Concern
- memory leak
- creating new `EventSubscriber` inside infinite loop without unsubscribing from the event will lead to memory leak. this is because each subscriber hold reference to the publisher, so GC won't be able to collect these object. 
### Improvement
- implement `IDisposable` on `EventSubscriber` class, and we use `Dispose` method to unsubscribe from the event. 
- `using` statement in `main` method ensures that `EventSubscriber` is disposed of properly when it goes out of scope.
# Final Code
```csharp
var publisher = new EventPublisher();

while (true)
{
    using (var subscriber = new EventSubscriber(publisher))
    {
        // do something with the publisher and subscriber objects  
    }
}

class EventPublisher
{
    public event EventHandler MyEvent;

    public void RaiseEvent()
    {
        MyEvent?.Invoke(this, EventArgs.Empty);
    }
}

class EventSubscriber : IDisposable
{
    private readonly EventPublisher _publisher;

    public EventSubscriber(EventPublisher publisher)
    {
        _publisher = publisher;
        _publisher.MyEvent += OnMyEvent;
    }

    private void OnMyEvent(object sender, EventArgs e)
    {
        Console.WriteLine("MyEvent raised");
    }

    public void Dispose()
    {
        // Unsubscribe from the event when the subscriber is disposed
        _publisher.MyEvent -= OnMyEvent;
    }
}
```
# Question 06
## Original Code
```csharp
using System; 
using System.Collections.Generic; 
namespace MemoryLeakExample 
{ 
    class Program 
    { 
        static void Main(string[] args) 
        { 
            var rootNode = new TreeNode(); 
            while (true) 
            { 
                // create a new subtree of 10000 nodes  
                var newNode = new TreeNode(); 
                for (int i = 0; i < 10000; i++) 
                { 
                    var childNode = new TreeNode(); 
                    newNode.AddChild(childNode); 
                } 
                rootNode.AddChild(newNode); 
            } 
        } 
    } 

    class TreeNode 
    { 
        private readonly List<TreeNode> _children = new List<TreeNode>(); 
      public void AddChild(TreeNode child) 
      { 
            _children.Add(child); 
        } 
    } 
} 
```
## Analysis
### Concern
- potential issue with this code that it keep adding nodes to tree structure in an infinite loop, and never removed. this can lead to a memory leak over time because the memory consumed by the nodes is not released, and the garbage collector might not be able to collect these objects.
- if the goal is to continuously add nodes to the tree, it's crucial to ensure that we're not holding onto references unnecessarily and that the memory usage is manageable. in the current code, the tree keeps growing indefinitely
### Improvement
- consider periodically clearing or pruning parts of the tree, or selectively removing nodes when they are no longer needed.
- add `loopCount` to track loop position and add condition and check to prune child node from root. we can adjust the frequency of this check with our specific requirement and resource constraints. this approach can prevent the tree to grow indefinitely and allow GC to collect unreferenced nodes.
- replace `List` with `LinkedList`, this will decrease memory usage further an increase performance during add/removal of child node.
## Final Code
```csharp
var rootNode = new TreeNode();
int loopCount = 0;
while (true)
{
    //periodically prune tree after 100 loop / iteration
    if (loopCount % 100 == 0)
    {
        rootNode.ClearChildren();
    }

    // create a new subtree of 10000 nodes  
    var newNode = new TreeNode();
    for (int i = 0; i < 10000; i++)
    {
        var childNode = new TreeNode();
        newNode.AddChild(childNode);
    }
    rootNode.AddChild(newNode);
}

class TreeNode
{
    private readonly LinkedList<TreeNode> _children = new LinkedList<TreeNode>();
    public void AddChild(TreeNode child)
    {
        _children.AddLast(child);
    }

    // Clear the children of this node
    public void ClearChildren()
    {
        _children.Clear();
    }
}
```
# Question 07
## Original Code
```csharp
using System; 
using System.Collections.Generic;  
class Cache  
{  
  private static Dictionary<int, object> _cache = new Dictionary<int,
object>(); 

  public static void Add(int key, object value) 
  { 
    _cache.Add(key, value); 
  } 

  public static object Get(int key) 
  { 
    return _cache[key]; 
  } 
} 

class Program 
{ 
  static void Main(string[] args) 
  { 
    for (int i = 0; i < 1000000; i++) 
    { 
      Cache.Add(i, new object()); 
    } 

    Console.WriteLine("Cache populated"); 

    Console.ReadLine(); 
  } 
} 
```
## Analysis
### Concern
- the use of `Dictionary<int, object>` for caching a large number of objects can lead to memory-related problems. the issue lies in the fact that we're adding a million objects to the cache without any mechanism for eviction or limiting the size of the cache. this can lead to excessive memory consumption, especially in long-running applications.
- cache class implement caching using dictionary, but in `Add` method didn't implement any handling for duplicate key. `_cache.Add(key, value); ` will be problematic and throw an exception when tried to call `Add` with existing key.  this is because `Add` method of `Dictionary` doesn't allow duplicate key.
### Improvement
- to address first issue, we implement some form of cache management, such as limiting the size of the cache or using a cache eviction strategy.
- to address second issue, we modify `Add` method to check whether key already exist before attempting to add it. depending on the use case, we might also add handle for the situation where the key already exists, such as updating the existing value or simply skipping the addition.
## Final Code
```csharp
for (int i = 0; i < 1000000; i++)
{
    Cache.Add(i, new object());
}

Console.WriteLine("Cache populated");

Console.ReadLine();


class Cache
{
    private static Dictionary<int, object> _cache = new Dictionary<int, object>();
    private static int _maxCacheSize = 1000; // Set desired maximum cache size


    public static void Add(int key, object value)
    {
        if (!_cache.ContainsKey(key))
        {
            if (_cache.Count >= _maxCacheSize)
            {
                // Perform cache eviction (e.g., remove the oldest items)
                // This is a simple example; might want to implement a more sophisticated strategy.
                foreach (var oldestKey in GetOldestKeys())
                {
                    Console.WriteLine(oldestKey.ToString());
                    _cache.Remove(oldestKey);
                }
            }

            _cache.Add(key, value);
        }
        // Optionally, we can also handle the case where the key already exists (update or skip)
    }

    public static object Get(int key)
    {
        return _cache[key];
    }

    private static IEnumerable<int> GetOldestKeys()
    {
        // Implement cache eviction strategy here
        // This example returns the keys of the oldest items in the cache.
        return _cache.Keys.Take(_cache.Count - _maxCacheSize);
    }
}
```