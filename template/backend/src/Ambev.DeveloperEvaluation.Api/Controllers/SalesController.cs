using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Dtos;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSales;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="command">The sale creation command</param>
        /// <returns>The ID of the created sale</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSaleCommand command)
        {
            var saleId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = saleId }, saleId);
        }

        /// <summary>
        /// Gets all sales
        /// </summary>
        /// <returns>List of sales</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SaleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetSalesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets a sale by ID
        /// </summary>
        /// <param name="id">The sale ID</param>
        /// <returns>The sale details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetSaleByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Updates the quantity of a sale item
        /// </summary>
        /// <param name="saleNumber">The sale number</param>
        /// <param name="productId">The product ID</param>
        /// <param name="command">The update quantity command</param>
        /// <returns>No content</returns>
        [HttpPut("{saleNumber}/items/{productId}/quantity")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItemQuantity(
            string saleNumber,
            Guid productId,
            [FromBody] UpdateSaleItemQuantityCommand command)
        {
            command.SaleNumber = saleNumber;
            command.ProductId = productId;
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Cancels a sale
        /// </summary>
        /// <param name="saleNumber">The sale number to cancel</param>
        /// <returns>No content</returns>
        [HttpPost("{saleNumber}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(string saleNumber)
        {
            var command = new CancelSaleCommand { SaleNumber = saleNumber };
            await _mediator.Send(command);
            return NoContent();
        }
    }
} 