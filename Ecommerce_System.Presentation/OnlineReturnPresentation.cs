using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;
using Console_Ecommerce_System.BusinessLayer;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Exceptions;
using Console_Ecommerce_System.Contracts;

namespace Console_Ecommerce_System.Presentation
{
    public static class OnlineReturnPresentation
    {
        /// <summary>
        /// Menu for OnlineReturn
        /// </summary>
        /// <returns></returns>
        public static async Task<int> OnlineReturnMenu()
        {
            int choice = -2;

            do
            {
                //Menu
                WriteLine("\n***************Online Return Menu ***********");
                WriteLine("1. View Online Returns");
                WriteLine("2.  Add Online Return");
                WriteLine("3. Update Online Return");
                WriteLine("4. Delete Online Return");
                WriteLine("-----------------------");

                WriteLine("0. Logout");
                WriteLine("-1. Exit");
                Write("Choice: ");


                //Accept and check choice
                bool isValidChoice = int.TryParse(ReadLine(), out choice);
                if (isValidChoice)
                {
                    switch (choice)
                    {
                        case 1: await ViewOnlineReturns(); break;
                        case 2: await AddOnlineReturn(); break;
                        case 3: await UpdateOnlineReturn(); break;
                        case 4: await DeleteOnlineReturn(); break;
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
            return choice;
        }




        /// <summary>
        /// Displays list of OnlineReturns.
        /// </summary>
        /// <returns></returns>
        public static async Task ViewOnlineReturns()
        {
            try
            {
                using (IOnlineReturnBL onlineReturnBL = new OnlineReturnBL())
                {
                    //Get and display list of OnlineReturn.
                    List<OnlineReturn> onlineReturns = await onlineReturnBL.GetAllOnlineReturnsBL();
                    WriteLine("OnlineReturns:");
                    if (onlineReturns != null && onlineReturns?.Count > 0)
                    {
                        WriteLine("#\tPurpose\tQuantityOfReturn\\tCreated\tModified");
                        int serial = 0;
                        foreach (var onlineReturn in onlineReturns)
                        {
                            serial++;
                            WriteLine($"{serial}\t{onlineReturn.Purpose}\t{onlineReturn.QuantityOfReturn}\t{onlineReturn.CreationDateTime}\t{onlineReturn.LastModifiedDateTime}");
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


        public static async Task AddOnlineReturn()
        {
            OnlineReturn onlineReturn = new OnlineReturn();
            List<OrderDetail> matchingOrder = new List<OrderDetail>();
            Console.WriteLine("Enter the order number which you want to return");
            double orderToBeReturned = double.Parse(Console.ReadLine());//user input of order which he has to return
            Console.WriteLine("Entered Number is: " + orderToBeReturned);
            try
            {
                using (IOrderBL orderBL = new OrderBL())
                {
                    Order order = await orderBL.GetOrderByOrderNumberBL(orderToBeReturned);
                    onlineReturn.OrderID = order.OrderId;
                    using (IOrderDetailBL orderDetailBL = new OrderDetailBL())
                    {
                        matchingOrder = await orderDetailBL.GetOrderDetailsByOrderIDBL(order.OrderId);

                    }

                }

                if (matchingOrder.Count != 0)
                {

                    int serial = 0;
                    Console.WriteLine("Products in the order are ");
                    foreach (var orderDetail in matchingOrder)
                    {
                        serial++;
                        Console.WriteLine("#\tProductID \t ProductQuantityOrdered  \tProductPrice     \t TotalAmount ");
                        Console.WriteLine($"{ serial}\t{ orderDetail.ProductID}\t{ orderDetail.ProductQuantityOrdered}\t{orderDetail.ProductPrice}\t{orderDetail.TotalAmount}");
                    }

                    Console.WriteLine("Enter The Product Number to be Returned");
                    int ProductToBeReturned = int.Parse(Console.ReadLine());
                    if (ProductToBeReturned <= matchingOrder.Count && ProductToBeReturned > 0)
                    {
                        Console.WriteLine("Enter The Product Serial No. to be Returned");
                        Write("Product  #: ");
                        bool isNumberValid = int.TryParse(ReadLine(), out int serial1);
                        if (isNumberValid)
                        {
                            serial1--;

                            if (serial1 <= matchingOrder.Count - 1)
                            {
                                //Read inputs
                                OrderDetail orderDetail = matchingOrder[serial1];
                                onlineReturn.ProductID = orderDetail.ProductID;
                            }
                        }
                        Console.WriteLine("Enter The Quantity to be Returned");
                        int QuantityOfReturn = int.Parse(Console.ReadLine());
                        IOrderDetail orderDetail1 = new OrderDetail();
                        onlineReturn.QuantityOfReturn = QuantityOfReturn;

                        foreach (var orderDetail in matchingOrder)
                        {
                            orderDetail1 = matchingOrder.Find(
                           (item) => { return item.ProductQuantityOrdered == orderDetail.ProductQuantityOrdered; });
                        }

                        if (QuantityOfReturn <= orderDetail1.ProductQuantityOrdered)
                        {

                            WriteLine("Purpose of Return:\n1.  UnsatiSfactoryProduct\n2. WrongProductShipped\n3.  WrongProductOrdered\n4. DefectiveProduct  ");
                            bool isPurposeValid = int.TryParse(ReadLine(), out int purpose);

                            if (isPurposeValid)
                            {

                                if (purpose == 1)
                                {
                                    onlineReturn.Purpose = Helpers.PurposeOfReturn.UnsatiSfactoryProduct;
                                }
                                else if (purpose == 2)
                                {
                                    onlineReturn.Purpose = Helpers.PurposeOfReturn.WrongProductShipped;
                                }
                                else if (purpose == 3)
                                {
                                    onlineReturn.Purpose = Helpers.PurposeOfReturn.WrongProductOrdered;
                                }
                                else if (purpose == 4)
                                {
                                    onlineReturn.Purpose = Helpers.PurposeOfReturn.DefectiveProduct;
                                }
                                else
                                {
                                    WriteLine("Invalid Option Selected ");
                                }

                                Write("Are you sure? (Y/N): ");
                                string confirmation = ReadLine();
                                if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
                                {
                                    using (IOnlineReturnBL onlineReturnBL = new OnlineReturnBL())
                                    {
                                        bool confirmed = await onlineReturnBL.AddOnlineReturnBL(onlineReturn);

                                        if (confirmed)
                                        {
                                            //updating order quantity and revenue
                                            matchingOrder[ProductToBeReturned - 1].ProductQuantityOrdered -= QuantityOfReturn;
                                            matchingOrder[ProductToBeReturned - 1].TotalAmount -= matchingOrder[ProductToBeReturned - 1].ProductPrice *QuantityOfReturn;
                                            Console.WriteLine("Total Return Amount: " + (matchingOrder[ProductToBeReturned - 1].ProductPrice * QuantityOfReturn));
                                            


                                            WriteLine("OnlineReturn Confirmed");
                                            WriteLine("OnlineReturnID is" + "  " + onlineReturn.OnlineReturnID);

                                        }
                                        else
                                        {
                                            WriteLine("Data Not Serialized");
                                        }
                                    }
                                }
                                else
                                {
                                    WriteLine(" Not Confirmed");
                                }

                            }
                            else
                            {
                                WriteLine(" Purpose of return not valid");
                            }


                        }
                        else
                        {
                            WriteLine("Invalid QuantityOfReturn");
                        }



                    }
                    else
                    {
                        WriteLine("Wrong Input");
                    }
                }
                else
                {
                    Console.WriteLine("OrderID not Found");
                }



            }
            catch (System.Exception)
            {

                throw;
            }


        }




        /// <summary>
        /// Updates OnlineReturn.
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateOnlineReturn()
        {
            try
            {
                using (IOnlineReturnBL onlineReturnsBL = new OnlineReturnBL())
                {
                    //Read Sl.No
                    Write("OnlineReturn #: ");
                    bool isNumberValid = int.TryParse(ReadLine(), out int serial);
                    if (isNumberValid)
                    {
                        serial--;
                        List<OnlineReturn> onlineReturns = await onlineReturnsBL.GetAllOnlineReturnsBL();
                        if (serial <= onlineReturns.Count - 1)
                        {
                            // Read inputs
                            OnlineReturn onlineReturn = onlineReturns[serial];
                            WriteLine("Purpose of Return:\n1.  UnsatiSfactoryProduct\n2. WrongProductShipped\n3.  WrongProductOrdered\n4. DefectiveProduct  ");
                            bool isPurposeValid = int.TryParse(ReadLine(), out int purpose);
                            Write("QuantityOfReturn: ");
                            onlineReturn.QuantityOfReturn = Convert.ToInt32(ReadLine());

                            //  Write("ProductPrice: ");
                            //onlineReturn.ProductPrice = Convert.ToInt32(ReadLine());
                            //Write("ReturnAmount: ");
                            //onlineReturn.ReturnAmount = Convert.ToInt32(ReadLine());




                            //Invoke UpdateOnlineReturnBL method to update
                            bool isUpdated = await onlineReturnsBL.UpdateOnlineReturnBL(onlineReturn);
                            if (isUpdated)
                            {
                                WriteLine("OnlineReturn Updated");
                            }
                        }
                        else
                        {
                            WriteLine($"Invalid OnlineReturn #.\nPlease enter a number between 1 to {onlineReturns.Count}");
                        }
                    }
                    else
                    {
                        WriteLine($"Invalid number.");
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
        /// Delete OnlineReturn.
        /// </summary>
        /// <returns></returns>
        public static async Task DeleteOnlineReturn()
        {
            try
            {
                using (IOnlineReturnBL onlineReturnBL = new OnlineReturnBL())
                {
                    //Read Sl.No
                    Write("OnlineReturn #: ");
                    bool isNumberValid = int.TryParse(ReadLine(), out int serial);
                    if (isNumberValid)
                    {
                        serial--;
                        List<OnlineReturn> onlineReturns = await onlineReturnBL.GetAllOnlineReturnsBL();
                        if (serial <= onlineReturns.Count - 1)
                        {
                            //Confirmation
                            OnlineReturn onlineReturn = onlineReturns[serial];
                            Write("Are you sure? (Y/N): ");
                            string confirmation = ReadLine();

                            if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
                            {
                                //Invoke DeleteOnlineReturnBL method to delete
                                bool isDeleted = await onlineReturnBL.DeleteOnlineReturnBL(onlineReturn.OnlineReturnID);
                                if (isDeleted)
                                {
                                    WriteLine("OnlineReturn Deleted");
                                }
                            }
                        }
                        else
                        {
                            WriteLine($"Invalid OnlineReturn #.\nPlease enter a number between 1 to {onlineReturns.Count}");
                        }
                    }
                    else
                    {
                        WriteLine($"Invalid number.");
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