﻿using AutoMapper;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enum;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContactsManager.ServiceTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsAdderService? _personsAdderService;
        private readonly IPersonsSorterService? _personsSorterService;
        private readonly IPersonsGetterService? _personGetterService;
        private readonly IPersonsUpdaterService? _personUpdaterService;
        private readonly IPersonsDeleterService? _personDeleterService;
        private readonly Mock<IMapper>? _mockMapper;
        private PersonAddRequestDto? _personAddRequestDto = null;
        private PersonUpdateRequestDto? _personUpdateRequestDto = null;
        private readonly IPersonRepository _personRepository;

        private readonly Mock<IPersonRepository> _mockPersonRepository;

        private readonly Mock<ILogger<PersonsAdderServices>> _mockLogger;
        private readonly ILogger<PersonsAdderServices> _logger;

        public PersonsServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockPersonRepository= new Mock<IPersonRepository>();
            _mockLogger= new Mock<ILogger<PersonsAdderServices>>();   
            _personRepository= _mockPersonRepository.Object;
            _logger=_mockLogger.Object;

            _personGetterService = new PersonsGetterServices(_mockMapper.Object, _personRepository, _logger);
            _personsAdderService = new PersonsAdderServices(_mockMapper.Object, _personRepository, _logger);
            _personsSorterService = new PersonsSorterServices(_mockMapper.Object, _personRepository, _logger);
            _personUpdaterService = new PersonsUpdaterServices(_mockMapper.Object, _personRepository, _logger);
            _personDeleterService = new PersonsDeleterServices(_mockMapper.Object, _personRepository, _logger);

            Initializeobjects();
            MockPersonMapper();
            MockCountryMapper();
        }
        #region PrivateMethods
        private void  MockPersonMapper()
        {
           _mockMapper?.Setup(m => m.Map<Person>(It.IsAny<PersonAddRequestDto>()))
                .Returns<PersonAddRequestDto>((req) => new Person
                {
                    PersonName = req.PersonName,
                    Email = req.Email,
                    Address = req.Address,
                    CountryId = req.CountryId,
                    Gender = req.Gender.ToString(),
                    Dob = req.Dob,
                    ReceiveNewsLetters = req.ReceiveNewsLetters
                });
            _mockMapper?.Setup(m => m.Map<PersonResponseDto>(It.IsAny<Person>()))
                .Returns<Person>((p) => new PersonResponseDto
                {
                    PersonId = p.PersonId,
                    PersonName = p.PersonName,
                    Email = p.Email,
                    Dob = p.Dob,
                    Address = p.Address,
                    CountryId = p.CountryId,
                    Gender = p.Gender,
                    ReceiveNewsLetters = p.ReceiveNewsLetters
                });
            _mockMapper?.Setup(m => m.Map<List<PersonResponseDto>>(It.IsAny<List<Person>>()))
                .Returns<List<Person>>((pList) => pList.Select((pRes) => new PersonResponseDto
                {
                    PersonId = pRes.PersonId,
                    PersonName = pRes.PersonName,
                    Email = pRes.Email,
                    Dob = pRes.Dob,
                    Address = pRes.Address,
                    CountryId = pRes.CountryId,
                    Gender = pRes.Gender,
                    ReceiveNewsLetters = pRes.ReceiveNewsLetters
                }).ToList());

        }

        private void  MockCountryMapper()
        {
            _mockMapper?.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns<CountryAddRequestDto>((cReq) => new Country()
                {
                    CountryName = cReq.CountryName,
                });
            _mockMapper?.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
               .Returns<Country>((c) => new CountryResponseDto()
               {
                   CountryId = c.CountryId,
                   CountryName = c.CountryName
               });
        }

       

        private void  Initializeobjects()
        {
           
            _personAddRequestDto = new PersonAddRequestDto()
            {
                PersonName = "Aakash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = Guid.NewGuid(),
                Gender = Gender.Male,
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            
            _personUpdateRequestDto = new PersonUpdateRequestDto()
            {
                PersonId = Guid.Empty,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = Guid.NewGuid(),
                Gender = Gender.Male,
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
        }
        #endregion

        #region AddPerson

        [Fact]
        public async Task  AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequestDto? personAddRequestDto = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
              if(_personsAdderService!=null) await _personsAdderService.AddPerson(personAddRequestDto);
            });


        }

        [Fact]
        public async Task  AddPerson_PersonNameNull()
        {
            //Arrange
            PersonAddRequestDto personAddRequestDto = new()
            {
                PersonName = null,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                if (_personsAdderService != null) await _personsAdderService.AddPerson(personAddRequestDto);
            });
        }



        [Fact]
        public async Task  AddPerson_WithProperDetails_ToBeSucessfull()
        {
            //Arrange
            _mockPersonRepository.Setup(x => x.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync( new Person { 
                    PersonId = Guid.NewGuid(),
                    PersonName =_personAddRequestDto?.PersonName,
                    CountryId= _personAddRequestDto?.CountryId,
                    Email = _personAddRequestDto?.Email,
                    Dob = _personAddRequestDto?.Dob,
                    Gender=_personAddRequestDto?.Gender.ToString(),
                    Address = _personAddRequestDto?.Address,
                    ReceiveNewsLetters= _personAddRequestDto?.ReceiveNewsLetters,
                    Country=null
                });
            PersonResponseDto? actualPersonResponse = null;

            //Act
            if (_personsAdderService != null)
            {
                actualPersonResponse = await _personsAdderService.AddPerson(_personAddRequestDto);
            }


            //Assert
            Assert.True(actualPersonResponse?.PersonId != Guid.Empty);

        }
        #endregion

        #region GetAllPerson
        [Fact]
        public async Task  GetAllPerson_NullList()
        {
            //Arrange
            _mockPersonRepository.Setup(x => x.GetAllPersons())
                 .ReturnsAsync(new List<Person>());
            List<PersonResponseDto>? personResponseDto = null;
            //Act
            if (_personGetterService != null) 
                personResponseDto = await _personGetterService.GetAllPerson();
            //Assert
            if (personResponseDto != null) 
                Assert.Empty(personResponseDto);
        }
        [Fact]
        public async Task  GetAllPerson_WithProperdetails()
        {
            //Arrange
            _mockPersonRepository.Setup(x => x.GetAllPersons())
                 .ReturnsAsync(new List<Person>()
                 { 
                     new(){ PersonId=Guid.NewGuid() },
                     new(){ PersonId=Guid.NewGuid() },
                 });

            List<PersonResponseDto>? personResponseDto = null;
            //Act
            if (_personGetterService != null)
                personResponseDto = await _personGetterService.GetAllPerson();
            //Assert
                Assert.NotNull(personResponseDto);
        }
        #endregion

        #region GetPersonById
        [Fact]
        public async Task  GetPersonById_NullPersonId()
        {
            // Arrange
            Guid? personID = null;
            PersonResponseDto? personResponseDto = null;
            // Act
            if (_personGetterService != null)
                personResponseDto = await _personGetterService.GetPersonById(personID);
            // Assert
            Assert.Null(personResponseDto);

        }
        [Fact]
        public async Task  GetPersonById_WithProperId()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            _mockPersonRepository.Setup(x => x.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(new Person()
                {
                    PersonId = guid,
                    PersonName = "Kokash",
                    Email = "Aakash@mail.com",
                    Address = "Aakash Address",
                    CountryId = Guid.NewGuid(),
                    Gender = "Male",
                    Dob = DateTime.Parse("1996-5-13"),
                    ReceiveNewsLetters = true
                });
            PersonResponseDto? actualPersonResponse=null;

             // Act
             if (_personGetterService != null)
                actualPersonResponse = await _personGetterService.GetPersonById(guid);
            // Assert
            Assert.NotNull(actualPersonResponse);
            Assert.True(actualPersonResponse.PersonId == guid);

        }

        #endregion

        #region GetPersonFiltered
        [Fact]
        public async Task  GetFilteredPerson_EmptySearchTest()
        {
            //Arrange
            List<Person> persons = new()
                {
                     new Person(){ PersonId=Guid.NewGuid()},
                     new Person(){PersonId=Guid.NewGuid() },
                };
            List<PersonResponseDto> personsResponse = new()
            {
                new PersonResponseDto() { PersonId = persons[0].PersonId },
                new PersonResponseDto() { PersonId = persons[1].PersonId }
            };

            _mockPersonRepository.Setup(x => x.GetAllPersons())
                .ReturnsAsync(persons);
            List<PersonResponseDto>? actualPersonResponseList = null;


            //Act
            if (_personGetterService != null)    
                actualPersonResponseList= await _personGetterService.GetPersonByFilter(nameof(Person.PersonName),"");

            // Assert
            Assert.NotNull(actualPersonResponseList);
            foreach (var person in personsResponse)
            {
                Assert.Contains(person, actualPersonResponseList);
            }

        }

        [Fact]
        public async Task  GetFilteredPerson_SearchByPersonName()
        {
            //Arrange

            List<Person> persons = new()
                {
                     new Person(){ PersonId=Guid.NewGuid(),PersonName="Vishnu"},
                     new Person(){PersonId=Guid.NewGuid(),PersonName="Deva"},
                };
            List<PersonResponseDto> personsResponse = new()
            {
                new PersonResponseDto() { PersonId = persons[0].PersonId, PersonName = persons[0].PersonName },
                new PersonResponseDto() { PersonId = persons[1].PersonId, PersonName = persons[1].PersonName }
            };

            _mockPersonRepository.Setup(x => x.GetAllPersons())
                .ReturnsAsync(persons);
            List<PersonResponseDto>? actualPersonResponseList = null;

            //Act
            if (_personGetterService!=null) 
             actualPersonResponseList =
               await _personGetterService.GetPersonByFilter(nameof(Person.PersonName), "v");

            // Assert

            foreach (var person in personsResponse)
            {
                if (person.PersonName != null && actualPersonResponseList!=null)
                {
                    if (person.PersonName.Contains('V', StringComparison.OrdinalIgnoreCase))
                        Assert.Contains(person, actualPersonResponseList);
                }
            }




        }
        #endregion

        #region GetSortedPerson
        [Fact]
        public void  GetSortedPerson_ByPersonName_Desc()
        {
            //Arrange

            List<Person> persons = new()
                {
                     new Person(){ PersonId=Guid.NewGuid(),PersonName="Vishnu"},
                     new Person(){PersonId=Guid.NewGuid(),PersonName="Deva"},
                };
            List<PersonResponseDto> personsResponse = new()
            {
                new PersonResponseDto() { PersonId = persons[0].PersonId, PersonName = persons[0].PersonName },
                new PersonResponseDto() { PersonId = persons[1].PersonId, PersonName = persons[1].PersonName }
            };


            List<PersonResponseDto> expectedPersonResponseList=
                personsResponse.OrderByDescending(person => person.PersonName).ToList();
            List<PersonResponseDto>? actualPersonResponseList=null;

            //Act
            if (_personsSorterService != null)
            {
                actualPersonResponseList =
                     _personsSorterService.GetSortedPersons(personsResponse, nameof(Person.PersonName), SortOderOption.DESC);
            }

            // Assert

            for (int i = 0; i < expectedPersonResponseList.Count; i++)
            {
                if (actualPersonResponseList != null)
                {
                    Assert.Equal(expectedPersonResponseList[i], actualPersonResponseList[i]);
                }
            }




        }

        [Fact]
        public void  GetSortedPerson_ByPersonName_Asc()
        {
            //Arrange

            List<Person> persons = new()
                {
                     new Person(){ PersonId=Guid.NewGuid(),PersonName="Vishnu"},
                     new Person(){PersonId=Guid.NewGuid(),PersonName="Deva"},
                };
            List<PersonResponseDto> personsResponse = new()
            {
                new PersonResponseDto() { PersonId = persons[0].PersonId, PersonName = persons[0].PersonName },
                new PersonResponseDto() { PersonId = persons[1].PersonId, PersonName = persons[1].PersonName }
            };
            List<PersonResponseDto> expectedPersonResponseList =
                personsResponse.OrderBy(person => person.PersonName).ToList();
            List<PersonResponseDto>? actualPersonResponseList = null;

            //Act
            if (_personsSorterService != null)
            {
                actualPersonResponseList =
                _personsSorterService.GetSortedPersons(personsResponse, nameof(Person.PersonName), SortOderOption.ASC);
            }

            // Assert

            for (int i = 0; i < expectedPersonResponseList.Count; i++)
            {

                if (actualPersonResponseList != null)
                {
                    Assert.Equal(expectedPersonResponseList[i], actualPersonResponseList[i]);
                }
            }




        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task  UpdatePerson_NullPerson()
        {
            // Arrange
            PersonUpdateRequestDto? personUpdateRequestDto = null;

            
            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async() => {
                // Act
                if (_personUpdaterService != null) 
                    await _personUpdaterService.UpdatePerson(personUpdateRequestDto);
            });

        }
        [Fact]
        public async Task  UpdatePerson_InvalidPersonId()
        {
            // Arrange
            PersonUpdateRequestDto personUpdateRequestDto = new() {PersonId=Guid.NewGuid() };
            _mockPersonRepository.Setup(m=>m.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Person);


            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                // Act
                if(_personUpdaterService!=null)
                    await _personUpdaterService.UpdatePerson(personUpdateRequestDto);
            });

        }
       
        [Fact]
        public async Task  UpdatePerson_WithProperDetails()
        {
            // Arrange
            Guid personId = Guid.NewGuid();
            PersonUpdateRequestDto personUpdateRequestDto = new()
            {
                PersonId = personId,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = null,
                Gender = Gender.Male,
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            Person person = new() {
                PersonId = personId,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = null,
                Gender = "Male",
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            PersonResponseDto personResponseDto = new()
            {
                PersonId = personId,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = null,
                Gender = "Male",
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            _mockPersonRepository.Setup(m => m.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            _mockPersonRepository.Setup(m => m.UpdatePerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            PersonResponseDto? actualPersonResponse = null;

            // Act
            if(_personUpdaterService!=null)
                actualPersonResponse = await _personUpdaterService.UpdatePerson(_personUpdateRequestDto);

            // Assert
            Assert.Equal(personResponseDto, actualPersonResponse);

        }
        #endregion


        [Fact]
        public async Task  DeletePerson_WithoutProperDetails()
        {
            //Arrange
             Guid personId= Guid.NewGuid();
            _mockPersonRepository.Setup(m => m.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Person);
            bool isdeleted=false;
            //Act
            if (_personDeleterService != null)
                 isdeleted = await _personDeleterService.DeletePerson(personId);

            //Assert
            Assert.False(isdeleted);
        }

        [Fact]
        public async Task  DeletePerson_WithProperDetails()
        {
            //Arrange
            Guid personId = Guid.NewGuid();
            Person person = new()
            {
                PersonId = personId,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = null,
                Gender = "Male",
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            _mockPersonRepository.Setup(m => m.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            _mockPersonRepository.Setup(m => m.DeletePersonById(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            bool isdeleted = false;
            //Act
            if (_personDeleterService != null)
                isdeleted = await _personDeleterService.DeletePerson(personId);

            //Assert
            Assert.True(isdeleted); 
        }
    }
}
