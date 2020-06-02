using System;
using System.Collections.Generic;
using OnlineStore.Models.ShoppingCart;

namespace OnlineStore.Libraries.Services.Shipping
{
    /// <summary> Class that creates shipping packages that are used in shipping rate calculation, following the Correios rules and specifications. 
    /// Note: The algorithm is for study purposes only. There are better algorithms for real cases (where you want to save on shipping rates by 
    /// creating a package with several products). </summary>
    public class ShippingPackageFactory
    {
        public List<ShippingPackage> CreateShippingPackages(List<CartItem> cartItems)
        {
            var shippingPackages = new List<ShippingPackage>();
            
            ShippingPackage package = new ShippingPackage();
            foreach (var cartItem in cartItems)
            {
                for (int i = 0; i < cartItem.Amount; i++)
                {
                    if (ValidateCartItemAdditionToThePackage(package, cartItem))
                    {
                        shippingPackages.Add(package);
                        package = new ShippingPackage();
                    }

                    package.Weight += cartItem.Product.Weight;
                    package.Height += (int)Math.Ceiling(package.Height + cartItem.Product.Height);
                    package.Width += GetCorrectPackageWidth(package, cartItem);
                    package.Length += GetCorrectPackageLength(package, cartItem);
                    package.Diameter = Math.Max(Math.Max(package.Width, package.Height), package.Length);
                }
            }
            shippingPackages.Add(package);

            return shippingPackages;
        }

        private bool ValidateCartItemAdditionToThePackage(ShippingPackage package, CartItem cartItem)
        {
            double weight = package.Weight + cartItem.Product.Weight;
            int height = (int)Math.Ceiling(package.Height + cartItem.Product.Height);
            int width = GetCorrectPackageWidth(package, cartItem);
            int length = GetCorrectPackageLength(package, cartItem);
        
            if (weight > 30 || width + height + length > 200)
                return true;

            return false;
        }

        private int GetCorrectPackageWidth(ShippingPackage package, CartItem cartItem)
        {
            double correctWidth = package.Width > cartItem.Product.Width ? package.Width : cartItem.Product.Width;
            return (int)Math.Ceiling(correctWidth);
        }

        private int GetCorrectPackageLength(ShippingPackage package, CartItem cartItem)
        {
            double correctLength = package.Length > cartItem.Product.Length ? package.Length : cartItem.Product.Length;
            return (int)Math.Ceiling(correctLength);
        }
    }
}