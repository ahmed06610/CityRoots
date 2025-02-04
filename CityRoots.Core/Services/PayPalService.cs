using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Http;
using CityRoots.Core.Interfaces;
using System.Security.Claims;

public class PayPalService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly bool _isLive;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    public PayPalService(IConfiguration configuration,IHttpContextAccessor httpContextAccessor,IUnitOfWork unitOfWork)
    {
        _clientId = configuration["PayPalOptions:ClientId"];
        _clientSecret = configuration["PayPalOptions:Secret key"];
        _isLive = configuration["PayPalOptions:Mode"] == "live";
        _httpContextAccessor= httpContextAccessor;
        _unitOfWork=unitOfWork;
    }

    public async Task<string> CreatePaymentLink(decimal amount, string sellerEmail,int CycleId,string userId)
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
                ReturnUrl = "https://localhost:7109/api/paypal/success",
                CancelUrl = "https://localhost:7109/api/paypal/cancel"
            }
        });


        try
        {
            var response = await client.Execute(request);
            var result = response.Result<Order>();
            var approvalUrl = result.Links.FirstOrDefault(l => l.Rel == "approve")?.Href;

            // Save transaction as "PENDING"
            await SaveTransaction(result, "قيد الانتظار", CycleId, amount,userId);
            return approvalUrl;
        }
        catch (Exception ex)
        {
            // Save failed transaction
            await SaveTransaction(new Order { Id = "N/A" }, "FAILED", CycleId, amount,userId);
            throw new Exception("PayPal Payment Link creation failed: " + ex.Message);
        }


    }
    private async Task SaveTransaction(Order result,string status,int CycleId,decimal amount,string userId)
    {
        //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //if (userId is null)
        //{
        //    throw new Exception("User ID not found in token");

        //}
        var cycle = await _unitOfWork.Cycle.FindTWithIncludes<Cycle>(
                   CycleId, "CycleId",
                   x => x.LandParcel,
                   x => x.LandParcel.Farm,
                   x => x.LandParcel.Farm.Farmer,
                   x => x.LandParcel.Farm.Farmer.ApplicationUser
               ) ?? throw new Exception($"No cycle with ID {CycleId}");
        var transaction = new Payment
        {

            PaypalOrderId = result.Id,
            Statue = "PENDING",
            PayerId = userId,
            PayeeId = cycle.LandParcel.Farm.Farmer.ApplicationUserId,
            Amount = amount,
            CycleId = CycleId,
            PaymentMethod = "PayPal",
            Type = "استثمار",
            PaymentDate = DateTime.Now
        };
       await _unitOfWork.Payment.AddAsync( transaction );
        await _unitOfWork.CompleteAsync(); 


    }
   public async Task updateTransaction(string orderId,string status)
    {
        var payment=await _unitOfWork.Payment.FindTWithExpression<Payment>(x=>x.PaypalOrderId==orderId);
        if (payment is null)
            throw new Exception($"There is no payment with this PayPalOrderId {orderId}");
        payment.Statue=status;
        _unitOfWork.Payment.Update(payment);
        await _unitOfWork.CompleteAsync();
    }



}