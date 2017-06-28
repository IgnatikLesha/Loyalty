using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

namespace Loyalty_page.AuthorizeNET
{
    public class GetCustomerProfileAmount
    {
        public static decimal Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId)
        {
            

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new getTransactionListForCustomerRequest();
            request.customerProfileId = customerProfileId;

            // instantiate the controller that will call the service
            var controller = new getTransactionListForCustomerController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            decimal totalAmount = 0;

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactions == null)
                    return 0;
                
                
                foreach (var transaction in response.transactions)
                {
                    totalAmount += transaction.settleAmount;
                }
            }


            return totalAmount;
        }
    }
}
