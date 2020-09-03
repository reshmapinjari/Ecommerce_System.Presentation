using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;
using Console_Ecommerce_System.BusinessLayer;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Exceptions;
using Console_Ecommerce_System.Contracts;
using Console_Ecommerce_System.Presentation;
using System.Linq;

namespace Console_Ecommerce_System.Presentation
{
    public static class CustomerPresentation
    {
        /// <summary>
        /// Menu for Customer
        /// </summary>
        /// <returns></returns>
        public static async Task<int> CustomerUserMenu()
        {
            int choice = -2;
            using (ICustomerBL CustomerBL = new CustomerBL())
            {
                do
                {

                    //Menu
                    WriteLine("\n1. Initiate order");
                    WriteLine("2. Cancel Customer  Order");
                    WriteLine("3. Return OnlineOrder");
                    WriteLine("4. Update Account");
                    WriteLine("5. Change Password");
                    WriteLine("6. Manage Address");
                    WriteLine("7. Delete Account");
                    WriteLine("0. Logout");
                    WriteLine("-1. Exit");
                    Write("Choice: ");

                    //Accept and check choice
                    bool isValidChoice = int.TryParse(ReadLine(), out choice);
                    if (isValidChoice)
                    {
                        switch (choice)
                        {
                            case 1: await InitiateOrder(); break;
                            case 2: await CancelCustomerOrder(); break;
                            case 3: await ReturnOnlineOrder(); break;
                            case 4: await UpdateCustomerAccount(); break;
                            case 5: await ChangeCustomerPassword(); break;
                            case 6: await ManageAddress(); break;
                            case 7: await DeleteCustomerAccount(); break;
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
        /// Initiate Ordeer
        /// </summary>
        /// <returns></returns>
        public static async Task InitiateOrder()
        {
            try
            {
                using (IProductBL product = new ProductBL())
                {
                    await CustomerProductPresentation.CustomerProductMenu();
                }
            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Return  Order
        /// </summary>
        /// <returns></returns>
        public static async Task ReturnOnlineOrder()
        {
            try
            {
                using (IProductBL product = new ProductBL())
                {
                    await OnlineReturnPresentation.OnlineReturnMenu();
                }
            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }

        /// <summary>

        /// <summary>
        /// Manage Address
        /// </summary>
        /// <returns></returns>
        public static async Task ManageAddress()
        {
            try
            {
                int internalvalue = await AddressPresentation.AddressPresentationMenu();
            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Updates Customer
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateCustomerAccount()
        {
            try
            {
                using (ICustomerBL CustomerBL = new CustomerBL())
                {
                    //Read Sl.No
                    Customer Customer = await CustomerBL.GetCustomerByEmailBL(CommonData.CurrentUser.Email);
                    Write("Name: ");
                    Customer.CustomerName = ReadLine();
                    Write("Email: ");
                    Customer.Email = ReadLine();

                    //Invoke UpdateCustomerBL method to update
                    bool isUpdated = await CustomerBL.UpdateCustomerBL(Customer);
                    if (isUpdated)
                    {
                        WriteLine(" Account Updated");
                    }

                }
            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Delete Customer Account.
        /// </summary>
        /// <returns></returns>
        public static async Task DeleteCustomerAccount()
        {
            try
            {
                using (ICustomerBL CustomerBL = new CustomerBL())
                {

                    Write("Are you sure? (Y/N): ");
                    Write("Current Password: ");
                    string currentPassword = ReadLine();
                    string confirmation = ReadLine();
                    Customer Customer = await CustomerBL.GetCustomerByEmailAndPasswordBL(CommonData.CurrentUser.Email, currentPassword);
                    if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
                    {
                        //Invoke DeleteCustomerBL method to delete
                        bool isDeleted = await CustomerBL.DeleteCustomerBL(Customer.CustomerID);
                        if (isDeleted)
                        {
                            WriteLine("Customer Account Deleted");
                        }
                    }
                }

            }




            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);

            }
        }

        /// <summary>
        /// Updates Customer's Password.
        /// </summary>
        /// <returns></returns>
        public static async Task ChangeCustomerPassword()
        {
            try
            {
                using (ICustomerBL CustomerBL = new CustomerBL())
                {
                    //Read Current Password
                    Write("Current Password: ");
                    string currentPassword = ReadLine();

                    Customer existingCustomer = await CustomerBL.GetCustomerByEmailAndPasswordBL(CommonData.CurrentUser.Email, currentPassword);

                    if (existingCustomer != null)
                    {
                        //Read inputs
                        Write("New Password: ");
                        string newPassword = ReadLine();
                        Write("Confirm Password: ");
                        string confirmPassword = ReadLine();

                        if (newPassword.Equals(confirmPassword))
                        {
                            existingCustomer.Password = newPassword;

                            //Invoke UpdateCustomerBL method to update
                            bool isUpdated = await CustomerBL.UpdateCustomerPasswordBL(existingCustomer);
                            if (isUpdated)
                            {
                                WriteLine("Customer Password Updated");
                            }
                        }
                        else
                        {
                            WriteLine($"New Password and Confirm Password doesn't match");
                        }
                    }
                    else
                    {
                        WriteLine($"Current Password doesn't match.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }

        //public static async Task CancelCustomerOrder()
        //{
        //    List<OrderDetail> matchingOrder = new List<OrderDetail>();//list maintains the order details of the order which user wishes to cancel
        //    try
        //    {
        //        using (ICustomerBL CustomerBL = new CustomerBL())
        //        {
        //            //gives the current Customer
        //            Customer Customer = await CustomerBL.GetCustomerByEmailBL(CommonData.CurrentUser.Email);
        //            using (IOrderBL orderDAL = new OrderBL())
        //            {
        //                //list of orders ordered by the Customer
        //                List<Order> CustomerOrderList = await orderDAL.GetOrdersByCustomerIDBL(Customer.CustomerID);
        //                Console.WriteLine("Enter the order number which you want to cancel");
        //                int orderToBeCancelled = int.Parse(Console.ReadLine());//user input of order which he has to cancel 
        //                foreach (Order order in CustomerOrderList)
        //                {
        //                    using (IOrderDetailBL orderDetailBL = new OrderDetailBL())
        //                    {
        //                        //getting the order details of required order to be cancelled
        //                        List<OrderDetail> CustomerOrderDetails = await orderDetailBL.GetOrderDetailsByOrderIDBL(order.OrderId);
        //                        matchingOrder = CustomerOrderDetails.FindAll(
        //                                   (item) => { return item.OrderSerial == orderToBeCancelled; }
        //                               );
        //                        break;
        //                    }
        //                }

        //                if (matchingOrder.Count != 0)
        //                {
        //                    OrderDetailBL orderDetailBL = new OrderDetailBL();
        //                    //cancel order if order not delivered
        //                    if (!( await orderDetailBL.UpdateOrderDeliveredStatusBL(matchingOrder[0].OrderId)))
        //                    {
        //                        int serial = 0;
        //                        Console.WriteLine("Products in the order are ");
        //                        foreach (OrderDetail orderDetail in matchingOrder)
        //                        {
        //                            //displaying order details with the products ordered
        //                            serial++;
        //                            Console.WriteLine("#\tProductID \t ProductQuantityOrdered");
        //                            Console.WriteLine($"{ serial}\t{ orderDetail.ProductID}\t{ orderDetail.ProductQuantityOrdered}");
        //                        }
        //                        Console.WriteLine("Enter The Product to be Cancelled");
        //                        int ProductToBeCancelled = int.Parse(Console.ReadLine());
        //                        Console.WriteLine("Enter The Product Quantity to be Cancelled");
        //                        int quantityToBeCancelled = int.Parse(Console.ReadLine());
        //                        if (matchingOrder[ProductToBeCancelled - 1].ProductQuantityOrdered >= quantityToBeCancelled)
        //                        {
        //                            //updating order quantity and revenue
        //                            matchingOrder[ProductToBeCancelled - 1].ProductQuantityOrdered -= quantityToBeCancelled;
        //                            matchingOrder[ProductToBeCancelled - 1].TotalAmount -= matchingOrder[ProductToBeCancelled - 1].ProductPrice * quantityToBeCancelled;
        //                            OrderDetailBL orderDetailBL1 = new OrderDetailBL();
        //                            await orderDetailBL1.UpdateOrderDetailsBL(matchingOrder[ProductToBeCancelled - 1]);

        //                            Console.WriteLine("Product Cancelled Succesfully");

        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine("PRODUCT QUANTITY EXCEEDED");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("Order Can't be cancelled as it is delivered");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}



        public static async Task CancelCustomerOrder()
        {
            IProductBL productBL = new ProductBL();
            List<OrderDetail> matchingOrder = new List<OrderDetail>();//list maintains the order details of the order which user wishes to cancel
            Console.WriteLine("Enter the order number which you want to cancel");
            double orderToBeCancelled = double.Parse(Console.ReadLine());//user input of order which he has to cancel 
            Console.WriteLine("Entered Number is: " + orderToBeCancelled);
            try
            {
                using (IOrderBL orderBL = new OrderBL())
                {
                    try
                    {

                        Order order = await orderBL.GetOrderByOrderNumberBL(orderToBeCancelled);
                        using (IOrderDetailBL orderDetailBL = new OrderDetailBL())
                        {
                            matchingOrder = await orderDetailBL.GetOrderDetailsByOrderIDBL(order.OrderId);
                            int a = matchingOrder.Count();
                            Console.WriteLine("No. of Products ordered: " + a);
                        }
                    }
                    catch (System.Exception)
                    {
                        WriteLine("Invalid OrderId");
                    }

                }


                if (matchingOrder.Count != 0)
                {
                    OrderDetailBL orderDetailBL = new OrderDetailBL();
                    //cancel order if order not delivered
                    if ((await orderDetailBL.UpdateOrderDeliveredStatusBL(matchingOrder[0].OrderId)))
                    {
                        int serial = 0;

                        Console.WriteLine("Products in the order are ");
                        Console.WriteLine("# \t\t\tProductID \t\t\t ProductQuantityOrdered \t\t UnitProductPrice");
                        foreach (OrderDetail orderDetail in matchingOrder)
                        {
                            //displaying order details with the products ordered
                            Product product = await productBL.GetProductByProductIDBL(orderDetail.ProductID);
                            serial++;
                            //Console.WriteLine(product.ProductName);
                            Console.WriteLine($"{ serial}\t{ orderDetail.ProductID}\t\t{ orderDetail.ProductQuantityOrdered}\t\t{orderDetail.ProductPrice}");
                        }
                        Console.WriteLine("Enter The Product to be Cancelled");
                        int ProductToBeCancelled = int.Parse(Console.ReadLine());
                        if (ProductToBeCancelled <= matchingOrder.Count && ProductToBeCancelled > 0)
                        {
                            Console.WriteLine("Enter The Product Quantity to be Cancelled");
                            int quantityToBeCancelled = int.Parse(Console.ReadLine());
                            if (matchingOrder[ProductToBeCancelled - 1].ProductQuantityOrdered >= quantityToBeCancelled)
                            {
                                //updating order quantity and revenue
                                matchingOrder[ProductToBeCancelled - 1].ProductQuantityOrdered -= quantityToBeCancelled;
                                matchingOrder[ProductToBeCancelled - 1].TotalAmount -= matchingOrder[ProductToBeCancelled - 1].ProductPrice * quantityToBeCancelled;
                                Console.WriteLine("Total Refund Amount: " + (matchingOrder[ProductToBeCancelled - 1].ProductPrice * quantityToBeCancelled));
                                OrderDetailBL orderDetailBL1 = new OrderDetailBL();
                                await orderDetailBL1.UpdateOrderDetailsBL(matchingOrder[ProductToBeCancelled - 1]);
                                Console.WriteLine("Product Cancelled Succesfully");
                            }
                            else
                            {
                                Console.WriteLine("PRODUCT QUANTITY EXCEEDED");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong Input");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Order Can't be cancelled as it is delivered");
                    }
                }
                
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }

}


