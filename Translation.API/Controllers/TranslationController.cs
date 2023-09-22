using MediatR;
using Microsoft.AspNetCore.Mvc;
using Translation.Application.Commands;
using Translation.Application.Models;
using Translation.Application.Queries;

namespace Translation.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ReadTranslationQuery _readTranslationQuery;
        private readonly ILogger<TranslationController> _logger;

        public TranslationController(
            IMediator mediator,
            ReadTranslationQuery readTranslationQuery,
            ILogger<TranslationController> logger
            )
        {
            _mediator = mediator;
            _readTranslationQuery = readTranslationQuery;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTranslationJob([FromBody] CreateTranslationJobCommand translationJob)
        {
            try
            {
                var createTanslationResult = await _mediator.Send(translationJob);
                _logger.LogInformation($"TranslationController: Translation Job created with Id: {createTanslationResult.Id}");
                return Ok(createTanslationResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"TranslationController: Error creating Translation Job: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTranslation([FromQuery] string translationJobId)
        {
            try
            {
                var translatedText = await _readTranslationQuery.Execute(translationJobId);
                if (translatedText.Id == null)
                {
                    _logger.LogInformation($"TranslationController: Translation Job not found: {translationJobId}");
                    return NotFound();
                }
                _logger.LogInformation($"TranslationController: Translation Job found: {translationJobId}");
                return Ok(translatedText);
            }
            catch (Exception ex)
            {
                _logger.LogError($"TranslationController: Error getting Translation Job: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
