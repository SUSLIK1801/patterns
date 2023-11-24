using PZ;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
int choice;
List<Category> categories;
int categoryID;
string categoryName;
List<Supplier> suppliers;
int supplierID;
string supplierName;
string supplierCountry;
List<Product> products;
int productID;
string productName;
string productSupplier;
string productCategory;
decimal productPrice;
int productAvailable;

DAOFactory factory = new DAOFactory();

do
{
    Console.WriteLine("\n------------------------------------------" +
        "\n1 - Вивести всі категорії" +
        "\n2 - Знайти категорію за назвою" +
        "\n3 - Додати нову категорію" +
        "\n4 - Оновити категорію" +
        "\n5 - Видалити категорію" +
        "\n------------------------------------------" +
        "\n6 - Вивести всіх постачальників" +
        "\n7 - Знайти постачальника за назвою" +
        "\n8 - Додати нового постачальника" +
        "\n9 - Оновити дані постачальника" +
        "\n10 - Видалити постачальника" +
        "\n------------------------------------------" +
        "\n11 - Вивести увесь товар" +
        "\n12 - Знайти товар за назвою" +
        "\n13 - Додати новий товар" +
        "\n14 - Оновити дані про товар" +
        "\n15 - Видалити товар" +
        "\n------------------------------------------");
    Console.Write("\nВиберіть опцію (1 або 15): ");

    if (int.TryParse(Console.ReadLine(), out choice))
    {
        switch (choice)
        {
            case 1:
                categories = factory.CreateDAO<Category>().GetAll();
                Console.WriteLine("Список категорій:");
                foreach (Category category in categories)
                {
                    categoryID = category.GetId();
                    categoryName = category.GetName();
                    Console.WriteLine($"ID: {categoryID}, Назва: {categoryName}");
                    Console.WriteLine();
                }
                break;

            case 2:
                Console.Write("\nВведіть назву категорії для пошуку: ");
                string searchCategory = Console.ReadLine();
                categories = factory.CreateDAO<Category>().GetByName(searchCategory);
                if (categories.Count > 0)
                {
                    Console.WriteLine("\nЗнайдені категорії:");
                    foreach (Category category in categories)
                    {
                        categoryID = category.GetId();
                        categoryName = category.GetName();
                        Console.WriteLine($"ID: {categoryID}, Назва: {categoryName}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("\nКатегорії із введеною назвою не знайдено.");
                }
                break;
            
            case 3:
                Console.Write("\nВведіть назву категорії: ");
                categoryName = Console.ReadLine();
                Category newCategory = new Category.CategoryBuilder()
                    .SetName(categoryName)
                    .Build();
                factory.CreateDAO<Category>().Add(newCategory);
            break;

            case 4:
                Console.Write("\nВведіть ID категорії: ");
                categoryID = Convert.ToInt32(Console.ReadLine());
                Console.Write("\nВведіть нову назву категорії: ");
                categoryName = Console.ReadLine();
                Category UpdateCategory = new Category.CategoryBuilder()
                    .SetName(categoryName)
                    .Build();
                factory.CreateDAO<Category>().Update(categoryID, UpdateCategory);
            break;

            case 5:
                Console.Write("\nВведіть ID категорії: ");
                categoryID = Convert.ToInt32( Console.ReadLine());
                factory.CreateDAO<Category>().Delete(categoryID);
            break;

            case 6:
                suppliers = factory.CreateDAO<Supplier>().GetAll();
                Console.WriteLine("Список постачальників:");
                foreach (Supplier supplier in suppliers)
                {
                    supplierID = supplier.GetId();
                    supplierName = supplier.GetName();
                    supplierCountry = supplier.GetCountry();
                    Console.WriteLine($"ID: {supplierID}, Назва: {supplierName}, Країна: {supplierCountry}");
                    Console.WriteLine();
                }
                break;

            case 7:
                Console.Write("\nВведіть назву постачальника для пошуку: ");
                string searchSupplier = Console.ReadLine();
                suppliers = factory.CreateDAO<Supplier>().GetByName(searchSupplier);
                if (suppliers.Count > 0)
                {
                    Console.WriteLine("\nЗнайдені постачальники:");
                    foreach (Supplier supplier in suppliers)
                    {
                        supplierID = supplier.GetId();
                        supplierName = supplier.GetName();
                        supplierCountry = supplier.GetCountry();
                        Console.WriteLine($"ID: {supplierID}, Назва: {supplierName}, Країна: {supplierCountry}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("\nПостачальників із введеною назвою не знайдено.");
                }
                break;

            case 8:
                Console.Write("\nВведіть назву постачальника: ");
                supplierName = Console.ReadLine();
                Console.Write("\nВведіть країну постачальника: ");
                supplierCountry = Console.ReadLine();
                Supplier newSupplier = new Supplier.SupplierBuilder()
                    .SetName(supplierName)
                    .SetCountry(supplierCountry)
                    .Build();
                factory.CreateDAO<Supplier>().Add(newSupplier);
            break;

            case 9:
                Console.Write("\nВведіть ID постачальника: ");
                supplierID = Convert.ToInt32(Console.ReadLine());
                Console.Write("\nВведіть нову назву постачальника: ");
                supplierName = Console.ReadLine();
                Console.Write("\nВведіть нову країну постачальника: ");
                supplierCountry = Console.ReadLine();
                Supplier UpdateSupplier = new Supplier.SupplierBuilder()
                    .SetName(supplierName)
                    .SetCountry(supplierCountry)
                    .Build();
                factory.CreateDAO<Supplier>().Update(supplierID, UpdateSupplier);
            break;

            case 10:
                Console.Write("\nВведіть ID постачальника: ");
                supplierID = Convert.ToInt32(Console.ReadLine());
                factory.CreateDAO<Supplier>().Delete(supplierID);
            break;

            case 11:
                products = factory.CreateDAO<Product>().GetAll();
                Console.WriteLine("Список товарів:");
                foreach (Product product in products)
                {
                    productID = product.GetId();
                    productName = product.GetName();
                    productSupplier = product.GetProductSupplier();
                    productCategory = product.GetProductCategory();
                    productPrice = product.GetPrice();
                    productAvailable = product.GetAvailable();
                    Console.WriteLine($"ID: {productID}, Назва: {productName}, Постачальник: {productSupplier}, Категорія: {productCategory}, Ціна: {productPrice}, Кількість у наявності: {productAvailable}");
                    Console.WriteLine();
                }
            break;

            case 12:
                Console.Write("\nВведіть назву товару для пошуку: ");
                string searchProduct = Console.ReadLine();
                products = factory.CreateDAO<Product>().GetByName(searchProduct);
                if (products.Count > 0)
                {
                    Console.WriteLine("\nЗнайдений товар:");
                    foreach (Product product in products)
                    {
                        productID = product.GetId();
                        productName = product.GetName();
                        productSupplier = product.GetProductSupplier();
                        productCategory = product.GetProductCategory();
                        productPrice = product.GetPrice();
                        productAvailable = product.GetAvailable();
                        Console.WriteLine($"ID: {productID}, Назва: {productName}, Постачальник: {productSupplier}, Категорія: {productCategory}, Ціна: {productPrice}, Кількість у наявності: {productAvailable}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("\nТовар з такою назвою не знайдено.");
                }
                break;

            case 13:
                Console.Write("\nВведіть назву товару: ");
                productName = Console.ReadLine();
                Console.Write("\nВведіть назву постачальника (натисніть Enter, якщо не бажаєте вводити постачальника): ");
                productSupplier = Console.ReadLine();
                Console.Write("\nВведіть назву категорії (натисніть Enter, якщо не бажаєте вводити категорію): ");
                productCategory = Console.ReadLine();
                Console.Write("\nВведіть ціну (натисніть Enter, якщо не бажаєте вводити ціну): ");
                decimal.TryParse(Console.ReadLine(), out productPrice);
                Console.Write("\nВведіть кількість (натисніть Enter, якщо не бажаєте вводити кількість): ");
                int.TryParse(Console.ReadLine(), out productAvailable);
                Product newProduct = new Product.ProductBuilder()
                    .SetName(productName)
                    .SetSupplier(productSupplier)
                    .SetCategory(productCategory)
                    .SetPrice(productPrice)
                    .SetAvailable(productAvailable)
                    .Build();

                factory.CreateDAO<Product>().Add(newProduct);
                break;

            case 14:
                Console.Write("\nВведіть ID товару: ");
                productID = Convert.ToInt32(Console.ReadLine());
                Console.Write("\nВведіть назву постачальника (натисніть Enter, якщо не бажаєте вводити постачальника): ");
                productSupplier = Console.ReadLine();
                Console.Write("\nВведіть назву категорії (натисніть Enter, якщо не бажаєте вводити категорію): ");
                productCategory = Console.ReadLine();
                Console.Write("\nВведіть ціну (натисніть Enter, якщо не бажаєте вводити ціну): ");
                decimal.TryParse(Console.ReadLine(), out productPrice);
                Console.Write("\nВведіть кількість (натисніть Enter, якщо не бажаєте вводити кількість): ");
                int.TryParse(Console.ReadLine(), out productAvailable);
                Product UpdateProduct = new Product.ProductBuilder()
                    .SetSupplier(productSupplier)
                    .SetCategory(productCategory)
                    .SetPrice(productPrice)
                    .SetAvailable(productAvailable)
                    .Build();
                factory.CreateDAO<Product>().Update(productID, UpdateProduct);
            break;

            case 15:
                Console.Write("\nВведіть ID товару: ");
                productID = Convert.ToInt32(Console.ReadLine());
                factory.CreateDAO<Product>().Delete(productID);
            break;

            default:
                Console.WriteLine("Невірний вибір опції.");
                break;

        }
    }
} while (choice != 0);