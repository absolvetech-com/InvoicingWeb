using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Entities.Models;
using InvoicesAppAPI.Helpers;
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

        public async Task<List<CountryStateListViewModel>> GetCountryStateList()
        {
            if (db != null)
            {
                return await (from c in db.Countries
                              where (c.IsDeleted == false || c.IsDeleted == null)
                              select new CountryStateListViewModel
                              {
                                  CountryId = c.CountryId,
                                  Name = c.Name,
                                  ShortName = c.ShortName,
                                  Timezone = c.Timezone,
                                  CountryCode = c.CountryCode,
                                  States = (from state in db.States
                                            where state.CountryId == c.CountryId
                                            && (state.IsDeleted == false || state.IsDeleted == null)
                                            select new StateViewModel
                                            {
                                                StateId = state.StateId,
                                                Name = state.Name,
                                                CountryId = state.CountryId
                                            }).ToList()
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
                    objCustomer.BussinessName = model.BussinessName;
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

        public async Task<CustomerViewModel> GetCustomerById(long? Id)
        {
            if (db != null)
            {
                return await (from c in db.Customers
                              join cntry in db.Countries
                              on c.CountryId equals cntry.CountryId into ctryGroup
                              from cntry in ctryGroup.DefaultIfEmpty()
                              join s in db.States
                              on c.StateId equals s.StateId into sGroup
                              from s in sGroup.DefaultIfEmpty()
                              where c.CustomerId == Id
                              select new CustomerViewModel
                              {
                                  CustomerId=c.CustomerId,
                                  FirstName=c.FirstName,
                                  LastName=c.LastName,
                                  BussinessName=c.BussinessName,
                                  Phone=c.Phone,
                                  Fax=c.Fax,
                                  Mobile=c.Mobile,
                                  Address1=c.Address1,
                                  Address2 = c.Address2,
                                  BillingAddress=c.BillingAddress,
                                  MailingAddress=c.MailingAddress,
                                  CountryId = Convert.ToInt64(c.CountryId),
                                  CountryName = cntry.Name,
                                  StateId = Convert.ToInt64(c.StateId),
                                  StateName = s.Name,
                                  City = c.City,
                                  Postalcode = c.Postalcode,
                                  PersonalEmail = c.PersonalEmail,
                                  BussinessEmail = c.BussinessEmail,
                                  Gender = c.Gender,
                                  Dob = (c.Dob != null)? c.Dob.Value.ToString("dd/MM/yyyy"):"", 
                                  Gstin = c.Gstin,
                                  AccountNumber = c.AccountNumber.ToString(),
                                  PosoNumber = c.PosoNumber,
                                  Website = c.Website,
                                  IsActive=Convert.ToBoolean(c.IsActive),
                                  UserId=c.UserId
            }).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<bool> DeleteCustomer(CustomerViewModel model)
        {
            if (db != null)
            {
                Customers objCustomer = new Customers();
                objCustomer = db.Customers.Where(x => x.CustomerId == model.CustomerId).FirstOrDefault();
                objCustomer.IsDeleted = true;
                objCustomer.DeletedBy = model.UserId;
                objCustomer.DeletedDate = DateTime.Now;
                db.Customers.Update(objCustomer);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public ResponseModel<CustomerListViewModel> GetCustomerList(FilterationViewModel model)
        {
            if (db != null)
            {
                var customers = (from c in db.Customers
                                 join cntry in db.Countries
                                 on c.CountryId equals cntry.CountryId into ctryGroup
                                 from cntry in ctryGroup.DefaultIfEmpty()
                                 join s in db.States
                                 on c.StateId equals s.StateId into sGroup
                                 from s in sGroup.DefaultIfEmpty()
                                 where c.UserId == model.UserId && (c.IsDeleted == false || c.IsDeleted == null)
                                       select new CustomerListViewModel
                                       {
                                           CustomerId = c.CustomerId,
                                           FirstName = c.FirstName,
                                           LastName = c.LastName,
                                           BussinessName = c.BussinessName,
                                           Phone = c.Phone,
                                           Fax = c.Fax,
                                           Mobile = c.Mobile, 
                                           CountryName = cntry.Name, 
                                           StateName = s.Name,
                                           City = c.City,
                                           Postalcode = c.Postalcode,
                                           PersonalEmail = c.PersonalEmail,
                                           BussinessEmail = c.BussinessEmail,
                                           Gender = c.Gender,
                                           Dob = (c.Dob != null) ? c.Dob.Value.ToString("dd/MM/yyyy") : "",
                                           Gstin = c.Gstin,
                                           AccountNumber = c.AccountNumber.ToString(),
                                           PosoNumber = c.PosoNumber,
                                           Website = c.Website,
                                           IsActive = Convert.ToBoolean(c.IsActive) 
                                       }).AsQueryable();


                // searching
                if (!string.IsNullOrWhiteSpace(model.Search))
                {
                    var search = model.Search.ToLower();
                    customers = customers.Where(x =>
                                                x.FirstName.ToLower().Contains(search) ||
                                                x.LastName.ToLower().Contains(search) ||
                                                x.Phone.ToLower().Contains(search) ||
                                                x.Fax.ToLower().Contains(search) ||
                                                x.Mobile.ToLower().Contains(search)
                                                );
                }

                // sorting
                customers = customers.OrderBy(m=> model.SortBy + (model.Reverse ? "descending" : ""));

                // paging
                var customersPaging = customers.Skip((model.Page - 1) * model.ItemsPerPage).Take(model.ItemsPerPage); 
                ResponseModel<CustomerListViewModel> objcust = new ResponseModel<CustomerListViewModel>();
                objcust.Count = customers.Count();
                objcust.Data = customersPaging.ToList();
                return objcust;
            }
            return null;
        }

        //item

        public async Task<long> AddItem(Items model)
        {
            if (db != null)
            {
                await db.Items.AddAsync(model);
                await db.SaveChangesAsync();
                return model.ItemId;
            }
            return 0;
        }

        public async Task<long> UpdateItem(ItemViewModel model)
        {
            if (db != null)
            { 
                Items obj = new Items();
                obj = db.Items.Where(x => x.ItemId == model.ItemId).FirstOrDefault();
                obj.Name = model.Name;
                obj.Description = model.Description;
                obj.Quantity = model.Quantity;
                obj.Price = model.Price;
                obj.Tax = model.Tax; 
                obj.UpdatedBy = model.UserId;
                obj.UpdatedDate = DateTime.Now;
                db.Items.Update(obj);
                await db.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public async Task<ItemViewModel> GetItemById(long? Id)
        {
            if (db != null)
            {
                return await (from i in db.Items 
                              where i.ItemId == Id
                              select new ItemViewModel
                              {
                                  ItemId = i.ItemId,
                                  Name = i.Name,
                                  Description = i.Description,
                                  Quantity = i.Quantity,
                                  Price = i.Price,
                                  Tax = i.Tax, 
                                  UserId = i.UserId
                              }).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<bool> DeleteItem(ItemViewModel model)
        {
            if (db != null)
            {
                Items obj = new Items();
                obj = db.Items.Where(x => x.ItemId == model.ItemId).FirstOrDefault();
                obj.IsDeleted = true;
                obj.DeletedBy = model.UserId;
                obj.DeletedDate = DateTime.Now;
                db.Items.Update(obj);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
