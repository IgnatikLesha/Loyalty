using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

namespace Loyalty_page.AuthorizeNET
{
    public class GetCustomerProfile
    {
        public static Customer[] Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId)
        {

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new getCustomerProfileRequest();
            request.customerProfileId = customerProfileId;

            // instantiate the controller that will call the service
            var controller = new getCustomerProfileController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {

                var profiles = response.profile.shipToList;

                Customer[] customers = new Customer[profiles.Length];

                for (int i = 0; i < profiles.Length; i++)
                {
                    customers[i] = new Customer();
                    customers[i].LastName = profiles[i].lastName; ;
                    customers[i].FirstName = profiles[i].firstName;
                    customers[i].Email = profiles[i].email;
                    customers[i].ProfileId = customerProfileId;
                }

                return customers;
            }
            else
            {
                return null;
            }
        }
    }
}