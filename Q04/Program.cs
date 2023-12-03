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
}
