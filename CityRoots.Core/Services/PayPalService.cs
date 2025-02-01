using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class PayPalService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly bool _isLive;

    public PayPalService(IConfiguration configuration)
    {
        _clientId = configuration["PayPalOptions:ClientId"];
        _clientSecret = configuration["PayPalOptions:Secret key"];
        _isLive = configuration["PayPalOptions:Mode"] == "live";
    }

    public async Task<string> CreatePaymentLink(decimal amount, string sellerEmail)
    {
        PayPalEnvironment environment = _isLive
            ? new LiveEnvironment(_clientId, _clientSecret)
            : new SandboxEnvironment(_clientId, _clientSecret);

        var client = new PayPalHttpClient(environment);

        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(new OrderRequest()
        {
            CheckoutPaymentIntent = "CAPTURE",  
            PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown 
                    {
                        CurrencyCode = "USD",
                        Value = amount.ToString("F2")
                    },
                    Payee = new Payee 
                    {
                        Email = sellerEmail
                    }
                }
            },
            ApplicationContext = new ApplicationContext
            {
                ReturnUrl = "https://yourwebsite.com/success",
                CancelUrl = "https://yourwebsite.com/cancel"
            }
        });

        var response = await client.Execute(request);
        var result = response.Result<Order>();

        var approvalUrl = result.Links.FirstOrDefault(l => l.Rel == "approve")?.Href;
        return approvalUrl;
    }
}
