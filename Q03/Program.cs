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