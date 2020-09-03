using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Ecommerce_System.Contracts;
using Console_Ecommerce_System.DataAccessLayer;
using Console_Ecommerce_System.Entities;
using Console_Ecommerce_System.Exceptions;
using Console_Ecommerce_System.Contracts.DALContracts;

namespace Console_Ecommerce_System.BusinessLayer
{
    /// <summary>
    /// Contains data access layer methods for inserting, updating, deleting CartProduct from CartProducts collection.
    /// </summary>
    public class CartProductBL : BLBase<CartProduct>, ICartProductBL, IDisposable
    {
        //fields
        CartProductDALBase cartProductDAL;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CartProductBL()
        {
            this.cartProductDAL = new CartProductDAL();
        }

        /// <summary>
        /// Validations on data before adding or updating.
        /// </summary>
        /// <param name="entityObject">Represents object to be validated.</param>
        /// <returns>Returns a boolean value, that indicates whether the data is valid or not.</returns>
        protected async override Task<bool> Validate(CartProduct entityObject)
        {
            //Create string builder
            StringBuilder sb = new StringBuilder();
            bool valid = await base.Validate(entityObject);

            //Cart ID is Unique
            var existingObject = await GetCartProductByCartIDBL(entityObject.CartId);
            if (existingObject != null && existingObject?.ProductID != entityObject.ProductID)
            {
                valid = false;
                sb.Append(Environment.NewLine + $"CartID {entityObject.CartId} already exists");
            }

            if (valid == false)
                throw new System.Exception(sb.ToString());
            return valid;
        }

        /// <summary>
        /// Adds new systemUser to SystemUsers collection.
        /// </summary>
        /// <param name="newCartProduct">Contains the systemUser details to be added.</param>
        /// <returns>Determinates whether the new systemUser is added.</returns>
        public async Task<bool> AddCartProductBL(CartProduct newCartProduct)
        {
            bool cartProductAdded = false;
            try
            {
                if (await Validate(newCartProduct))
                {
                    await Task.Run(() =>
                    {
                        this.cartProductDAL.AddCartProductDAL(newCartProduct);
                        cartProductAdded = true;
                        Serialize();
                    });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return cartProductAdded;
        }


        /// <summary>
        /// Gets systemUser based on Email and Password.
        /// </summary>
        /// <param name="cartId">Represents Customer's CardID.</param>
        /// <returns>Returns SystemUser object.</returns>
        public async Task<CartProduct> GetCartProductByCartIDBL(Guid cartId)
        {
            CartProduct matchingCartProduct = null;
            try
            {
                await Task.Run(() =>
                {
                    matchingCartProduct = cartProductDAL.GetCartProductByCartIDDAL(cartId);
                });
            }
            catch (System.Exception)
            {
                throw;
            }
            return matchingCartProduct;
        }

        /// <summary>
        /// Updates CartProduct based on CartID.
        /// </summary>
        /// <param name="updateCartProduct">Represents CartProduct details like SystemUserID</param>
        /// <returns>Determinates whether the existing CartProduct is updated.</returns>
        public async Task<bool> UpdateCartProductBL(CartProduct updateCartProduct)
        {
            bool cartProductUpdated = false;
            try
            {
                if ((await Validate(updateCartProduct)) && (await GetCartProductByCartIDBL(updateCartProduct.CartId)) != null)
                {
                    this.cartProductDAL.UpdateCartProductDAL(updateCartProduct);
                    cartProductUpdated = true;
                    Serialize();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return cartProductUpdated;
        }

        /// <summary>
        /// Deletes CartProduct based on CartID.
        /// </summary>
        /// <param name="deleteCartID">Represents CartID to delete.</param>
        /// <returns>Determinates whether the existing CartProduct is updated.</returns>
        public async Task<bool> DeleteCartProductBL(Guid deleteCartID)
        {
            bool cartProductDeleted = false;
            try
            {
                await Task.Run(() =>
                {
                    cartProductDeleted = cartProductDAL.DeleteCartProductDAL(deleteCartID);
                    Serialize();
                });
            }
            catch (System.Exception)
            {
                throw;
            }
            return cartProductDeleted;
        }

        /// <summary>
        /// Disposes DAL object(s).
        /// </summary>
        public void Dispose()
        {
            ((CartProductDAL)cartProductDAL).Dispose();
        }

        /// <summary>
        /// Invokes Serialize method of DAL.
        /// </summary>
        public void Serialize()
        {
            try
            {
                CartProductDAL.Serialize();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///Invokes Deserialize method of DAL.
        /// </summary>
        public void Deserialize()
        {
            try
            {
                CartProductDAL.Deserialize();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

    }
}
