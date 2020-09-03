using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Console_Ecommerce_System.Contracts;
using Console_Ecommerce_System.Contracts.DALContracts;
using Console_Ecommerce_System.DataAccessLayer;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Helpers;

namespace Console_Ecommerce_System.BusinessLayer
{
    /// <summary>
    /// Contains data access layer methods for inserting, updating or deleting products
    /// </summary>
    public class ProductBL : BLBase<Product>, IProductBL, IDisposable
    {
        //fields
        ProductDALBase productDAL;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductBL()
        {
            this.productDAL = new ProductDAL();
        }

        /// <summary>
        /// Gets all products from the collection.
        /// </summary>
        /// <returns>Returns the list of all the products</returns>
        public async Task<List<Product>> GetAllProductsBL()
        {
            List<Product> productsList = null;

            try
            {
                await Task.Run(() =>
                {
                    productsList = productDAL.GetAllProductsDAL();

                });
            }

            catch (System.Exception)
            {
                throw;
            }

            return productsList;
        }

        /// <summary>
        /// Get product based on productID
        /// </summary>
        /// <param name="productID">Represents product ID to be searched.</param>
        /// <returns>Returns product object</returns>
        public async Task<Product> GetProductByProductIDBL(Guid productID)
        {
            Product matchingProduct = null;

            try
            {
                await Task.Run(() =>
                {
                    matchingProduct = productDAL.GetProductByProductIDDAL(productID);
                });
            }

            catch (System.Exception)
            {
                throw;
            }
            return matchingProduct;
        }

        /// <summary>
        /// Gets product list based on the given product category
        /// </summary>
        /// <param name="productCategory">Represents the product category whose products are to be fetched</param>
        /// <returns>The list of products of the given product category</returns>
        public async Task<List<Product>> GetProductsByProductCategoryBL(Category givenCategory)
        {
            List<Product> tempProductsList = null;

            try
            {
                await Task.Run(() =>
                {
                    tempProductsList = productDAL.GetProductsByProductCategoryDAL(givenCategory);

                });
            }

            catch (System.Exception)
            {
                throw;
            }

            return tempProductsList;

        }

        /// <summary>
        /// Gets the product list of the givenn product name
        /// </summary>
        /// <param name="productName">Represents the product name based on which products are to be fetched</param>
        /// <returns>The lsit of products with the given product name</returns>
        public async Task<List<Product>> GetProductsByProductNameBL(string productName)
        {
            List<Product> tempProductsList = null;

            try
            {
                await Task.Run(() =>
                {
                    tempProductsList = productDAL.GetProductsByProductNameDAL(productName);

                });
            }

            catch (System.Exception)
            {
                throw;
            }

            return tempProductsList;

        }

        /// <summary>
        /// Updates product description based on ProductID.
        /// </summary>
        /// <param name="updateProduct">Respresents product description only</param>
        /// <returns>Tells whether the description is updated</returns>
        public async Task<bool> UpdateProductDescriptionBL(Product updateProduct)
        {
            bool descriptionUpdated = false;

            try
            {
                if (await GetProductByProductIDBL(updateProduct.ProductID) != null)
                {
                    this.productDAL.UpdateProductDescriptionDAL(updateProduct);
                    descriptionUpdated = true;

                }

            }

            catch (System.Exception)
            {
                throw;
            }

            return descriptionUpdated;
        }

        /// <summary>
        /// Updates product price based on ProductID.
        /// </summary>
        /// <param name="updateProduct">Represents product price only</param>
        /// <returns>Tells whether the product price is updated</returns>
        public async Task<bool> UpdateProductPriceBL(Product updateProduct)
        {
            bool priceUpdated = false;

            try
            {
                if (await GetProductByProductIDBL(updateProduct.ProductID) != null)
                {
                    this.productDAL.UpdateProductPriceDAL(updateProduct);
                    priceUpdated = true;

                }

            }

            catch (System.Exception)
            {
                throw;
            }

            return priceUpdated;
        }

        /// <summary>
        /// Adds new product to Product collection.
        /// </summary>
        /// <param name="addProduct">Contains the product details of the code to be added</param>
        /// <returns>Tells whether the product is added</returns>
        public async Task<bool> AddProductBL(Product addProduct)
        {
            bool productAdded = false;
            try
            {
                if (await Validate(addProduct))
                {
                    await Task.Run(() =>
                    {
                        this.productDAL.AddProductDAL(addProduct);
                        productAdded = true;
                    });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return productAdded;
        }

        /// <summary>
        /// Deletes a product from the product collection
        /// </summary>
        /// <param name="deleteProductID">Represents the product ID of the product to be deleted </param>
        /// <returns>Tells whether the product is deleted </returns>
        public async Task<bool> DeleteProductBL(Guid deleteProductID)
        {
            bool productDeleted = false;

            try
            {
                await Task.Run(() =>
                {
                    productDeleted = productDAL.DeleteProductDAL(deleteProductID);
                });
            }

            catch (System.Exception)
            {
                throw;
            }

            return productDeleted;

        }

        /// <summary>
        /// Invokes Serialize method of DAL.
        /// </summary>
        public static void Serialize()
        {
            try
            {
                ProductDAL.Serialize();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///Invokes Deserialize method of DAL.
        /// </summary>
        public static void Deserialize()
        {
            try
            {
                ProductDAL.Deserialize();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Disposes DAL object(s).
        /// </summary>
        public void Dispose()
        {
            ((ProductDAL)productDAL).Dispose();
        }
    }
}
