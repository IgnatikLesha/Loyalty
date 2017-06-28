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
    public class GetCustomerProfileIds
    {
        public static string[] Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get Customer Profile Id sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new getCustomerProfileIdsRequest();

            // instantiate the controller that will call the service
            var controller = new getCustomerProfileIdsController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                return response.ids;
            }
            else
            {
                return null;
            }


        }
    }
}
