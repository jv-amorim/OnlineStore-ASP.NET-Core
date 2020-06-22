using System;
using System.Linq;
using PagarMe;
using System.Collections.Generic;
using OnlineStore.Models.Payment;
using OnlineStore.Models.ShoppingCart;
using OnlineStore.Libraries.Helpers.ConversionHelpers;
using OnlineStore.Libraries.Helpers.XmlHelpers;

namespace OnlineStore.Libraries.Services.Payment
{
    public class PagarMeManager
    {
        private readonly string xmlFilePath = "./Private/PrivateData.xml";
        private readonly string apiKeyXmlTag = "PagarMe_ApiKey";
        private readonly string encryptionKeyXmlTag = "PagarMe_EncryptionKey";

        public object GenerateBoleto(Models.Customer loggedInCustomer, decimal totalPrice)
        {   
            try
            {
                SetKeys();
                
                Transaction transaction = new Transaction()
                {
                    Amount = PriceConverter.ConvertDecimalPriceToPriceInCents(totalPrice),
                    PaymentMethod = PaymentMethod.Boleto,
                    Customer = CustomerFactory(loggedInCustomer)
                };

                transaction.Save();

                return new { BoletoUrl = transaction.BoletoUrl, ExpirationDate = transaction.BoletoExpirationDate };
            }
            catch (Exception e)
            {
                return new { Error = e.Message };
            }
        }

        // TODO - GenerateCreditCardPayment().
        public object GenerateCreditCardPayment(
            Models.Customer loggedInCustomer, 
            ShippingInformation shippingInfo, 
            Models.Address customerAddress, 
            List<CartItem> cartItems,
            CreditCard creditCart)
        {
            try
            {
                SetKeys();

                decimal totalPrice = CalculateTotalPrice(cartItems, shippingInfo);

                Transaction transaction = new Transaction()
                {
                    Amount = PriceConverter.ConvertDecimalPriceToPriceInCents(totalPrice),
                    Billing = BillingFactory(loggedInCustomer, customerAddress),
                    Card = CardFactory(creditCart),
                    Customer = CustomerFactory(loggedInCustomer),
                    Item = ItemArrayFactory(cartItems),
                    Shipping = ShippingFactory(loggedInCustomer, shippingInfo, customerAddress)
                };

                transaction.Save();

                return new { TransactionId = transaction.Id };
            }
            catch (Exception e)
            {
                return new { Error = e.Message };
            }
        }

        private decimal CalculateTotalPrice(List<CartItem> cartItems, ShippingInformation shippingInfo)
        {
            decimal totalPrice = 0;
            
            foreach (var cartItem in cartItems)
                totalPrice += cartItem.Amount * cartItem.Product.UnitPrice;
            totalPrice += (decimal)shippingInfo.Price;

            return totalPrice;
        }

        private void SetKeys()
        {
            PagarMeService.DefaultApiKey = XMLReader.GetDataFromXMLFile(xmlFilePath, apiKeyXmlTag);
            PagarMeService.DefaultEncryptionKey = XMLReader.GetDataFromXMLFile(xmlFilePath, encryptionKeyXmlTag);
        }

        private PagarMe.Address AddressFactory(Models.Address address)
        {
            return new PagarMe.Address()
            {
                Country = "br",
                State = address.State,
                City = address.City,
                Neighborhood = address.Neighborhood,
                Street = address.AddressLine,
                StreetNumber = address.Number,
                Complementary = address.Complement,
                Zipcode = address.Cep
            };
        }

        private PagarMe.Billing BillingFactory(Models.Customer customer, Models.Address address)
        {
            return new PagarMe.Billing()
            {
                Name = customer.Name,
                Address = AddressFactory(address)
            };
        }

        private PagarMe.Card CardFactory(CreditCard creditCart)
        {
            Card card = new Card()
            {
                Number = creditCart.Number,
                HolderName = creditCart.HolderName,
                ExpirationDate = creditCart.ExpirationDate,
                Cvv = creditCart.Cvv
            };

            card.Save();
            
            return new PagarMe.Card()
            {
                Id = card.Id
            };
        }

        private PagarMe.Customer CustomerFactory(Models.Customer customer)
        {
            return new PagarMe.Customer
            {
                ExternalId = customer.Id.ToString(),
                Name = customer.Name,
                Type = CustomerType.Individual,
                Country = "br",
                Email = customer.Email,
                Documents = new[]
                {
                    new Document
                    {
                        Type = DocumentType.Cpf,
                        Number = customer.CPF
                    },
                },
                PhoneNumbers = new string[]
                {
                    "+55" + customer.Phone
                },
                Birthday = customer.DateOfBirth.ToString("yyyy-MM-dd")
            };
        }

        private PagarMe.Item[] ItemArrayFactory(List<CartItem> cartItems)
        {
            Item[] itemArray = new Item[cartItems.Count];

            for (int i = 0; i < cartItems.Count; i++)
            {
                CartItem currentCartItem = cartItems[i];

                itemArray[i] = new Item()
                {
                    Id = currentCartItem.Id.ToString(),
                    Title = currentCartItem.Product.Name,
                    Quantity = currentCartItem.Amount,
                    Tangible = true,
                    UnitPrice = PriceConverter.ConvertDecimalPriceToPriceInCents(currentCartItem.Product.UnitPrice)
                };
            }
            
            return itemArray;
        }

        private PagarMe.Shipping ShippingFactory(Models.Customer customer, ShippingInformation shippingInfo, Models.Address address)
        {
            string deliveryDate = 
                DateTime.Now
                .AddDays(shippingInfo.EstimatedTimeOfArrivalInDays)
                .ToString("yyyy-MM-dd");

            return new PagarMe.Shipping 
            { 
                Name = customer.Name,
                Fee = PriceConverter.ConvertDoublePriceToPriceInCents(shippingInfo.Price),
                DeliveryDate = deliveryDate,
                Expedited = false,
                Address = AddressFactory(address)
            };
        }
    }
}