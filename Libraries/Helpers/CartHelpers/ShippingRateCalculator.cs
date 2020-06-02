using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Models.ShoppingCart;
using ServiceReference;

namespace OnlineStore.Libraries.Helpers.CartHelpers
{
    /// <summary> Shipping rate calculator that consumes a SOAP web service of the Correios (brazilian company). </summary>
    public class ShippingRateCalculator
    {
        private CalcPrecoPrazoWSSoap service;

        public ShippingRateCalculator(CalcPrecoPrazoWSSoap service) => this.service = service;

        /// <summary> Returns the shipping rate and the ETA (Estimated Time of Arrival).  </summary>
        public async Task<List<ShippingInformation>> CalculateShippingRateAndETA(string originCEP, string destinationCEP, string freightType, List<ShippingPackage> packages)
        {
            List<ShippingInformation> results = new List<ShippingInformation>();
            foreach (var package in packages)
            {
                var result = await CalculateShippingRateAndETA(originCEP, destinationCEP, freightType, package);
                results.Add(result);
            }

            bool doesTheListHaveNullValues = results.Select(s => s == null).Count() > 0;
            if (doesTheListHaveNullValues)
                return null;

            List<ShippingInformation> shippingInfos = 
                results
                .GroupBy(s => s.FreightType)
                .Select(group => new ShippingInformation()
                {
                    FreightType = group.First().FreightType,
                    EstimatedTimeOfArrivalInDays = group.Max(s => s.EstimatedTimeOfArrivalInDays),
                    Price = group.Sum(s => s.Price)
                })
                .ToList();

            return shippingInfos;
        }

        /// <summary> Returns the shipping rate and the ETA (Estimated Time of Arrival).  </summary>
        private async Task<ShippingInformation> CalculateShippingRateAndETA(string originCEP, string destinationCEP, string freightType, ShippingPackage package)
        {
            var result = await service.CalcPrecoPrazoAsync(
                "", "", 
                freightType, 
                originCEP, destinationCEP, 
                package.Weight.ToString(), 1, 
                package.Length, package.Height, package.Width, package.Diameter, 
                "N", 0, "N"
            );

            if (result.Servicos[0].Erro != "0")
                return null;

            ShippingInformation shippingInfo = new ShippingInformation()
            {
                FreightType = freightType,
                EstimatedTimeOfArrivalInDays = int.Parse(result.Servicos[0].PrazoEntrega)
            };

            try 
            {
                shippingInfo.Price = 
                double.Parse(
                    result.Servicos[0].Valor
                    .Replace(".", "")
                    .Replace(",", ".")
                );
            }
            catch (Exception)
            {
                return null;
            }

            return shippingInfo;
        }
    }
}