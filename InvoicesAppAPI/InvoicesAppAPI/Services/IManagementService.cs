using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Entities.Models;
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

        Task<List<CountryStateListViewModel>> GetCountryStateList();

        Task<long> AddCurrency(Currencies model);

        Task<long> AddCountry(Countries model);

        Task<long> AddState(States model);

        //customer
        Task<long> AddCustomer(Customers model);

        Task<long> UpdateCustomer(CustomerViewModel model);

        Task<CustomerViewModel> GetCustomerById(long? Id);

        Task<bool> DeleteCustomer(CustomerViewModel model);

        ResponseModel<CustomerListViewModel> GetCustomerList(FilterationListViewModel model, string UserId);

        //item
        Task<long> AddItem(Items model);

        Task<long> UpdateItem(ItemViewModel model);

        Task<ItemViewModel> GetItemById(long? Id);

        Task<bool> DeleteItem(ItemViewModel model);

        ResponseModel<ItemViewModel> GetItemList(FilterationListViewModel model, string UserId);
    }
}
