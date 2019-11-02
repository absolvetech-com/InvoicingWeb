using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Repositories
{
    public class ManagementRepository: IManagementService
    {
        ApplicationDbContext db;
        public ManagementRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<List<CurrencyViewModel>> GetCurrencyList()
        {
            if (db != null)
            {
                return await (from c in db.Currencies
                              where (c.IsDeleted == false || c.IsDeleted == null)
                              select new CurrencyViewModel
                              {
                                  CurrencyId = c.CurrencyId,
                                  Name = c.Name,
                                  Symbol = c.Symbol,
                                  Code = c.Code 
                              }).ToListAsync();
            }
            return null;
        }

        public async Task<CurrencyViewModel> GetCurrencyById(long? Id)
        {
            if (db != null)
            {
                return await (from c in db.Currencies 
                              where c.CurrencyId == Id
                              select new CurrencyViewModel
                              {
                                  CurrencyId = c.CurrencyId,
                                  Name = c.Name,
                                  Symbol = c.Symbol,
                                  Code = c.Code
                              }).FirstOrDefaultAsync();
            } 
            return null;
        }

        public async Task<List<CountryViewModel>> GetCountryList()
        {
            if (db != null)
            {
                return await (from c in db.Countries
                              where (c.IsDeleted == false || c.IsDeleted == null)
                              select new CountryViewModel
                              {
                                  CountryId = c.CountryId,
                                  Name = c.Name,
                                  ShortName = c.ShortName,
                                  Timezone = c.Timezone,
                                  CountryCode = c.CountryCode
                              }).ToListAsync();
            }
            return null;
        }

        public async Task<CountryViewModel> GetCountryById(long? Id)
        {
            if (db != null)
            {
                return await (from c in db.Countries
                              where c.CountryId == Id
                              select new CountryViewModel
                              {
                                  CountryId = c.CountryId,
                                  Name = c.Name,
                                  ShortName = c.ShortName,
                                  Timezone = c.Timezone,
                                  CountryCode = c.CountryCode
                              }).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<StateViewModel> GetStateById(long? Id)
        {
            if (db != null)
            {
                return await (from c in db.States
                              where c.StateId == Id
                              select new StateViewModel
                              {
                                  StateId = c.StateId,
                                  Name = c.Name
                              }).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<List<StateViewModel>> GetStateByCountry(long? Id)
        {
            if (db != null)
            {
                return await (from c in db.States
                              where c.CountryId == Id && (c.IsDeleted == false || c.IsDeleted == null) 
                              select new StateViewModel
                              {
                                  StateId = c.StateId,
                                  Name = c.Name
                              }).ToListAsync();
            }
            return null;
        }

        public async Task<long> AddCurrency(Currencies model)
        {
            if (db != null)
            {
                bool checkNameExists = db.Currencies.Any(x => x.Name.ToLower() == model.Name.ToLower());
                if (!checkNameExists)
                {
                    await db.Currencies.AddAsync(model);
                    await db.SaveChangesAsync();
                    return model.CurrencyId;
                } 
                else
                    return -1;
            } 
            return 0;
        }

        public async Task<long> AddCountry(Countries model)
        {
            if (db != null)
            {
                bool checkNameExists = db.Countries.Any(x => x.Name.ToLower() == model.Name.ToLower());
                if (!checkNameExists)
                {
                    await db.Countries.AddAsync(model);
                    await db.SaveChangesAsync();
                    return model.CountryId;
                }
                else
                    return -1;
            }
            return 0;
        }

        public async Task<long> AddState(States model)
        {
            if (db != null)
            {
                bool checkNameExists = db.States.Any(x => x.Name.ToLower() == model.Name.ToLower() && x.CountryId == model.CountryId);
                if (!checkNameExists)
                {
                    await db.States.AddAsync(model);
                    await db.SaveChangesAsync();
                    return model.StateId;
                }
                else
                    return -1;
            }
            return 0;
        }

        public async Task<long> AddCustomer(Customers model)
        {
            if (db != null)
            {
                bool checkNameExists = db.Customers.Any(x => x.PersonalEmail.ToLower() == model.PersonalEmail.ToLower());
                if (!checkNameExists)
                {
                    await db.Customers.AddAsync(model);
                    await db.SaveChangesAsync();
                    return model.CustomerId;
                }
                else
                    return -1;
            }
            return 0;
        }

        public async Task<long> UpdateCustomer(CustomerViewModel model)
        {
            if (db != null)
            {
                bool checkNameExists = db.Customers.Any(x => x.PersonalEmail.ToLower() == model.PersonalEmail.ToLower() && x.CustomerId!=model.CustomerId);
                if (!checkNameExists)
                {
                    Customers objCustomer = new Customers();
                    objCustomer = db.Customers.Where(x => x.CustomerId == model.CustomerId).FirstOrDefault();
                    objCustomer.FirstName = model.FirstName;
                    objCustomer.LastName = model.LastName;
                    objCustomer.Phone = model.Phone;
                    objCustomer.Fax = model.Fax;
                    objCustomer.Mobile = model.Mobile;
                    objCustomer.Address1 = model.Address1;
                    objCustomer.Address2 = model.Address2;
                    objCustomer.BillingAddress = model.BillingAddress;
                    objCustomer.MailingAddress = model.MailingAddress;
                    objCustomer.CountryId = model.CountryId;
                    objCustomer.StateId = model.StateId;
                    objCustomer.City = model.City;
                    objCustomer.Postalcode = model.Postalcode;
                    objCustomer.PersonalEmail = model.PersonalEmail;
                    objCustomer.BussinessEmail = model.BussinessEmail;
                    objCustomer.Gender = model.Gender;
                    objCustomer.Dob = (!string.IsNullOrWhiteSpace(model.Dob)) ? DateTime.ParseExact(model.Dob, "dd/MM/yyyy", null) : (DateTime?)null;
                    objCustomer.Gstin = model.Gstin;
                    objCustomer.AccountNumber = Convert.ToInt64(model.AccountNumber);
                    objCustomer.PosoNumber = model.PosoNumber;
                    objCustomer.Website = model.Website;
                    objCustomer.UpdatedBy = model.UserId;
                    objCustomer.UpdatedDate = DateTime.Now;
                    db.Customers.Update(objCustomer);
                    await db.SaveChangesAsync();
                    return 1;
                }
                else
                    return -1;
            }
            return 0;
        }
    }
}
