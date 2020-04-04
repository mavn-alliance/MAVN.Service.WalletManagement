using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using MAVN.Service.WalletManagement.Client;
using MAVN.Service.WalletManagement.Client.Enums;
using MAVN.Service.WalletManagement.Client.Models.Requests;
using MAVN.Service.WalletManagement.Client.Models.Responses;
using MAVN.Service.WalletManagement.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MAVN.Service.WalletManagement.Controllers
{
    [Route("api/wallets")]
    public class WalletsController : Controller, IWalletManagementApi
    {
        private readonly ITransferService _transferService;
        private readonly IWalletManagementService _walletManagementService;
        private readonly IMapper _mapper;

        public WalletsController(
            ITransferService transferService,
            IPaymentTransfersService paymentTransfersService,
            IWalletManagementService walletManagementService,
            IMapper mapper)
        {
            _transferService = transferService;
            _walletManagementService = walletManagementService;
            _mapper = mapper;
        }

        /// <summary>
        /// Transfers Balance from One customer to Another
        /// </summary>
        /// <returns><see cref="TransferBalanceResponse"/></returns>
        [HttpPost("transfer-balance")]
        [SwaggerOperation("TransferBalance")]
        [ProducesResponseType(typeof(TransferBalanceResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<TransferBalanceResponse> TransferBalanceAsync([FromBody] TransferBalanceRequestModel request)
        {
            var result = await _transferService.TransferBalanceAsync(
                request.OperationId,
                request.SenderCustomerId,
                request.ReceiverCustomerId,
                request.Amount);

            return _mapper.Map<TransferBalanceResponse>(result);
        }

        /// <summary>
        /// Blocks Customer's Wallet
        /// </summary>
        /// <param name="request" cref="CustomerWalletBlockRequest">Contains information about the request</param>
        /// <returns><see cref="CustomerWalletBlockResponse"/></returns>
        [HttpPost("block")]
        [ProducesResponseType(typeof(CustomerWalletBlockResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<CustomerWalletBlockResponse> CustomerWalletBlockAsync([FromBody] CustomerWalletBlockRequest request)
        {
            var result = await _walletManagementService.BlockCustomerWalletAsync(request.CustomerId);
            
            return new CustomerWalletBlockResponse
            {
                Error = _mapper.Map<CustomerWalletBlockError>(result)
            };
        }
        
        /// <summary>
        /// Unblocks Customer's Wallet
        /// </summary>
        /// <param name="request" cref="CustomerWalletUnblockRequest">Contains information about the request</param>
        /// <returns><see cref="CustomerWalletUnblockResponse"/></returns>
        [HttpPost("unblock")]
        [ProducesResponseType(typeof(CustomerWalletUnblockResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<CustomerWalletUnblockResponse> CustomerWalletUnblockAsync([FromBody] CustomerWalletUnblockRequest request)
        {
            var result = await _walletManagementService.UnblockCustomerWalletAsync(request.CustomerId);
            
            return new CustomerWalletUnblockResponse
            {
                Error = _mapper.Map<CustomerWalletUnblockError>(result)
            };
        }
        
        /// <summary>
        /// Gets block status of the Customer's Wallet
        /// </summary>
        /// <param name="customerId">Id of the Customer</param>
        /// <returns><see cref="CustomerWalletBlockStatusResponse"/></returns>
        [HttpGet("blockStatus/{customerId}")]
        [ProducesResponseType(typeof(CustomerWalletBlockStatusResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<CustomerWalletBlockStatusResponse> GetCustomerWalletBlockStateAsync(string customerId)
        {
            var result = await _walletManagementService.IsCustomerWalletBlockedAsync(customerId);
            
            return new CustomerWalletBlockStatusResponse
            {
                Error = result.HasValue ? CustomerWalletBlockStatusError.None : CustomerWalletBlockStatusError.CustomerNotFound,
                Status = result.HasValue ? result.Value ? CustomerWalletActivityStatus.Blocked : CustomerWalletActivityStatus.Active : default(CustomerWalletActivityStatus?)
            };
        }
    }
}
