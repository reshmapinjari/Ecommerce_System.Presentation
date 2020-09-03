using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;
using Console_Ecommerce_System.BusinessLayer;
using Console_Ecommerce_System.Helpers;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Exceptions;
using Console_Ecommerce_System.Contracts;
using Console_Ecommerce_System.Presentation;

namespace Console_Ecommerce_System.Presentation
{
    class CustomerProductPresentation
    {
        /// <summary>
        /// Menu for Product
        /// </summary>
        /// <returns></returns>

        public static List<CartProduct> cart = new List<CartProduct>();
        public static async Task<int> CustomerProductMenu()
        {

            int choice = -2;
            using (IProductBL productBL = new ProductBL())
            {
                do
                {
                    //Get and display list of Product.
                    CustomerBL CustomerBL = new CustomerBL();
                    Customer Customer = await CustomerBL.GetCustomerByEmailBL(CommonData.CurrentUser.Email);
                    List<Product> products = await productBL.GetAllProductsBL();
                    WriteLine("\n***************Products***********\n");
                    WriteLine("Product:");
                    if (products != null && products?.Count > 0)
                    {
                        WriteLine("#\tProductName\tPrice\t\tDescription\t");
                        int serial = 0;
                        foreach (var product in products)
                        {
                            serial++;
                            WriteLine($"{serial}\t{product.ProductName}\t{product.ProductPrice}\t{product.ProductColor}\t{product.ProductSize}\t{product.ProductMaterial}\t{product.category}");
                        }
                    }

                    //Menu

                    WriteLine("\n1.Add Product to cart");
                    WriteLine("2. Remove Product from Cart");
                    WriteLine("3. Confirm Order");
                    WriteLine("4.Get Product Details");
                    WriteLine("0. Back");
                    WriteLine("-1. Exit");
                    Write("Choice: ");

                    //Accept and check choice
                    bool isValidChoice = int.TryParse(ReadLine(), out choice);
                    if (isValidChoice)
                    {
                        switch (choice)
                        {
                            case 1: await AddtoCart(); break;
                            case 2: await RemoveFromCart(); break;
                            // case 3: await DeleteCart(); break;
                            case 3: await ConfirmOrder(); break;
                            case 4: await GetProduct(); break;
                            case 0: break;
                            case -1: break;
                            default: WriteLine("Invalid Choice"); break;
                        }
                    }
                    else
                    {
                        choice = -2;
                    }
                } while (choice != 0 && choice != -1);
            }
            return choice;
        }
        /// <summary>
        /// Add to Cart
        /// </summary>
        /// <returns></returns>
        public static async Task AddtoCart()
        {
            try
            {

                IProductBL productBL = new ProductBL();

                //Read Sl.No
                CartProduct cartProduct = new CartProduct();
                Write("Product #: ");
                bool isNumberValid = int.TryParse(ReadLine(), out int serial);
                if (isNumberValid)
                {
                    serial--;
                    List<Product> products = await productBL.GetAllProductsBL();
                    if (serial <= products.Count - 1)
                    {
                        //Read inputs
                        Product product = products[serial];

                        cartProduct.ProductID = product.ProductID;
                        cartProduct.ProductPrice = product.ProductPrice;
                        WriteLine("Enter Quantity Want to order");
                        cartProduct.ProductQuantityOrdered = int.Parse(ReadLine());
                        cart.Add(cartProduct);

                    }
                    else
                    {
                        WriteLine($"Invalid Product Serial #.\nPlease enter a number between 1 to {products.Count}");
                    }
                }
                else
                {
                    WriteLine($"Invalid number.");
                }

            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Remove From Cart
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task RemoveFromCart()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {


                WriteLine("#\tProduct Price\tProduct Quantity \t Total Amount");
                int serial = 0;
                foreach (var cartProduct in cart)
                {
                    serial++;
                    WriteLine($"{serial}\t{cartProduct.ProductPrice}\t{cartProduct.ProductQuantityOrdered}\t{cartProduct.TotalAmount}");
                }

                Write("Product #: ");
                bool isNumberValid = int.TryParse(ReadLine(), out int serial1);
                if (isNumberValid)
                {
                    serial1--;

                    if (serial1 <= cart.Count - 1)
                    {
                        cart.RemoveAt(serial1);

                    }
                }

            }
            catch (System.Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Confirm Order
        /// </summary>
        /// <returns></returns>
        public static async Task ConfirmOrder()
        {
            try
            {
                IProductBL productBL = new ProductBL();
                Guid tempOrderID;
                using (IOrderDetailBL orderDetailBL = new OrderDetailBL())
                {
                    CustomerBL CustomerBL = new CustomerBL();
                    Customer Customer = await CustomerBL.GetCustomerByEmailBL(CommonData.CurrentUser.Email);
                    AddressBL addressBL = new AddressBL();
                    List<Address> addresses = await addressBL.GetAddressByCustomerIDBL(Customer.CustomerID);
                    double totalamount = 0;
                    int quantity = 0;
                    Guid orderID = Guid.NewGuid();
                    tempOrderID = orderID;
                    foreach (var cartProduct in cart)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        WriteLine("#\tAddressLine 1\tLandMark\tCity");
                        int serial = 0;
                        foreach (var address in addresses)
                        {
                            serial++;
                            WriteLine($"{serial}\t{address.AddressLine1}\t{address.Landmark}\t{address.City}");
                        }

                        Write("Address #: ");
                        bool isNumberValid = int.TryParse(ReadLine(), out int serial1);
                        if (isNumberValid)
                        {
                            serial1--;

                            if (serial1 <= addresses.Count - 1)
                            {
                                //Read inputs
                                Address address = addresses[serial1];
                                orderDetail.AddressId = address.AddressID;
                            }
                        }
                        orderDetail.OrderId = orderID;
                        orderDetail.ProductID = cartProduct.ProductID;
                        orderDetail.ProductPrice = cartProduct.ProductPrice;
                        orderDetail.ProductQuantityOrdered = cartProduct.ProductQuantityOrdered;
                        orderDetail.TotalAmount = (cartProduct.ProductPrice * cartProduct.ProductQuantityOrdered);
                        totalamount += orderDetail.TotalAmount;
                        quantity += orderDetail.ProductQuantityOrdered;
                        bool isAdded;
                        Guid newguid;
                        (isAdded,newguid)= await orderDetailBL.AddOrderDetailsBL(orderDetail);


                    }

                    using (IOrderBL orderBL = new OrderBL())
                    {
                        Order order = new Order();
                        order.OrderId = orderID;
                        order.CustomerID = Customer.CustomerID;
                        order.TotalQuantity = quantity;
                        order.OrderAmount = totalamount;
                        bool isAdded;
                        Guid newguid;
                        (isAdded,newguid)= await orderBL.AddOrderBL(order);
                        if (isAdded)
                        {
                            WriteLine("Order  Added");
                        }


                    }

                }
                IOrderBL orderBL1 = new OrderBL();

                Order order1 = await orderBL1.GetOrderByOrderIDBL(tempOrderID);
                WriteLine($"Your Order No. {order1.OrderNumber}\t{order1.TotalQuantity}\t{order1.OrderAmount}\t{order1.DateOfOrder}");
                WriteLine("Order Details");
                IOrderDetailBL orderDetailBL1 = new OrderDetailBL();
                List<OrderDetail> orderDetailslist = await orderDetailBL1.GetOrderDetailsByOrderIDBL(tempOrderID);
                foreach (var item in orderDetailslist)
                {
                    Product product = await productBL.GetProductByProductIDBL(item.ProductID);
                    WriteLine($"{product.ProductName}\t{item.ProductPrice}\t{item.ProductQuantityOrdered}\t{item.TotalAmount}");
                }
                cart.Clear();

            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Gets product based on ID/Name/Category
        /// </summary>
        /// <returns></returns>
        public static async Task GetProduct()
        {
            try
            {
                using (IProductBL productBL = new ProductBL())
                {
                    int choice;
                    WriteLine("Enter:\n1.Get all products\n2.Get product by product name.\n3.Get product by product category");
                    choice = int.Parse(ReadLine());
                    switch (choice)
                    {
                        case 1:

                            List<Product> allProductList = new List<Product>();
                            allProductList = await productBL.GetAllProductsBL();
                            break;
                        case 2:
                            Write("Product name: ");
                            string productName = ReadLine();
                            List<Product> productNameList = new List<Product>();
                            productNameList = await productBL.GetProductsByProductNameBL(productName);
                            break;

                        case 3:
                            WriteLine("Choose among the following categories:");
                            WriteLine("1.Camping Equipment");
                            WriteLine("2.Outdoor Equipment");
                            WriteLine("3.Personal Accessories");
                            WriteLine("4.Mountaineering Equipment");
                            WriteLine("5.Golf Equipment");
                            bool isCategory = int.TryParse(ReadLine(), out int choice1); Category givenCategory;
                            if (isCategory)
                            {
                                if (choice1 == 1)
                                {
                                    givenCategory = Category.CampingEquipment;

                                }
                                else if (choice1 == 2)
                                {
                                    givenCategory = Category.OutdoorEquipment;

                                }
                                else if (choice1 == 3)
                                {
                                    givenCategory = Category.PersonalAccessories;

                                }
                                else if (choice1 == 4)
                                {
                                    givenCategory = Category.MountaineeringEquipment;

                                }
                                else if (choice1 == 5)
                                {
                                    givenCategory = Category.GolfEquipment;

                                }
                                else
                                {
                                    givenCategory = Category.CampingEquipment;
                                }

                            }
                            else
                            {
                                givenCategory = Category.CampingEquipment;
                            }
                            List<Product> categoryProductList = new List<Product>();
                            categoryProductList = await productBL.GetProductsByProductCategoryBL(givenCategory);
                        
                            break;

                        default: WriteLine("Invalid Choice"); break;
                    }



                }
            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }


    }
}
