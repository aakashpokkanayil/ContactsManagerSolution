using ContactsManager.Core.DTO;
using ContactsManager.Core.Enum;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.UI.Filters.ActionFilters;
using ContactsManager.UI.Filters.AuthFilter;
using ContactsManager.UI.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [Route("persons")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-CustomController-Key", "Custom-Value" ,3},Order =3)]
    [ResponseHeaderActionFilter("X-CustomController-Key", "Custom-Value", 3)]
    public class PersonController : Controller
    {
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsGetterService _personGetterService;
        private readonly IPersonsSorterService _personSorterService;

        public PersonController(
            ICountriesService countriesService,
            ILogger<PersonController> logger,
            IPersonsAdderService personsAdderService,
            IPersonsGetterService personGetterService,
            IPersonsSorterService personSorterService)
        {
           
            _countriesService = countriesService;
            _logger = logger;
            _personsAdderService = personsAdderService;
            _personGetterService = personGetterService;
            _personSorterService = personSorterService;
        }

        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter))]
        //[TypeFilter(typeof(ResponseHeaderActionFilter),Arguments =new object[] { "X-CustomAction-Key","Custom-Value",2}
        //,Order =2)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [ResponseHeaderActionFilter("X-CustomAction-Key", "Custom-Value", 2)]
        public async Task<IActionResult> Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponseDto.PersonName), SortOderOption sortOrder = SortOderOption.ASC)
        {
            _logger.LogInformation("PersonController:Index Action");
            _logger.LogDebug($"searchBy:{searchBy}, searchString:{searchString}, sortBy:{sortBy}, sortOrder:{sortOrder}");

            
            List<PersonResponseDto>? personResponse = await _personGetterService.GetPersonByFilter(searchBy, searchString);
            //ViewBag.CurrentSearchBy = searchBy;
            //ViewBag.CurrentSearchString = searchString;
            if (personResponse != null)
            personResponse = _personSorterService.GetSortedPersons(personResponse, sortBy, sortOrder);
            //ViewBag.CurrentSortBy = sortBy;
            //ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(personResponse);
        }


        [Route("[action]")]
        [HttpGet]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Create()
        {
           ViewBag.Countries= await _countriesService.GetAllCountries();
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(ModelStateValidationActionFilter))]
        [TypeFilter(typeof(TokenAuthFilter))]
        public async Task<IActionResult> Create(PersonAddRequestDto personAddRequest)
        {
            
            await _personsAdderService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Person");

        }
    }
}
