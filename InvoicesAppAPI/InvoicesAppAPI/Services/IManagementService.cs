using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Services
{
    public interface IManagementService
    {
        Task<List<CurrencyViewModel>> GetCurrencyList();

        Task<CurrencyViewModel> GetCurrencyById(long? Id);

        Task<List<CountryViewModel>> GetCountryList();

        Task<CountryViewModel> GetCountryById(long? Id);

        Task<StateViewModel> GetStateById(long? Id);

        Task<List<StateViewModel>> GetStateByCountry(long? Id);

        Task<long> AddCurrency(Currencies model);

        Task<long> AddCountry(Countries model);

        Task<long> AddState(States model);
    }
}
