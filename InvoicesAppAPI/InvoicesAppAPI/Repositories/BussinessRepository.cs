using InvoicesAppAPI.Entities;
using InvoicesAppAPI.Models;
using InvoicesAppAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Repositories
{
    public class BussinessRepository:IBussinessService
    {
        ApplicationDbContext db;
        public BussinessRepository(ApplicationDbContext _db)
        {
            db = _db;
        } 

        public async Task<bool> Create(BussinessDetail _bussinessDetail)
        {
            if (db != null)
            {
                await db.BussinessDetails.AddAsync(_bussinessDetail);
                await db.SaveChangesAsync(); 
                return true; 
            } 
            return false;
        }

        public async Task<BussinessDetailViewModel> GetBussinessDetailsById(string Id)
        {
            if (db != null)
            {
                return await (from b in db.BussinessDetails
                              join c in db.Countries
                              on b.CountryId equals c.CountryId into cGroup 
                              from c in cGroup.DefaultIfEmpty()
                              join s in db.States
                              on b.StateId equals s.StateId into sGroup
                              from s in sGroup.DefaultIfEmpty()
                              join cr in db.Currencies
                              on b.CurrencyId equals cr.CurrencyId into crGroup
                              from cr in crGroup.DefaultIfEmpty()
                              where b.IdentityId == Id
                              select new BussinessDetailViewModel
                              {
                                  Id= b.Id,
                                  IdentityId=b.IdentityId,
                                  UniqueBussinessId=b.UniqueBussinessId,
                                  BussinessName=b.BussinessName,
                                  BussinessLogo=b.BussinessLogo,
                                  BussinessCoverPhoto=b.BussinessCoverPhoto,
                                  AccountNumber=b.AccountNumber,
                                  CurrencyId= Convert.ToInt32(b.CurrencyId),
                                  CurrencyName = cr.Name,
                                  CurrencySymbol = cr.Symbol,
                                  CurrencyCode = cr.Code,
                                  CIN =b.CIN,
                                  GSTIN=b.GSTIN,
                                  BussinessSize=b.BussinessSize,
                                  BussinessClass=b.BussinessClass,
                                  Founded= b.Founded.Value.ToString("dd/MM/yyyy"),//Convert.ToDateTime(b.Founded),
                                  Fax =b.Fax,
                                  WebAddress=b.WebAddress,
                                  BussinessEmail=b.BussinessEmail,
                                  BussinessPhone=b.BussinessPhone,
                                  Address1=b.Address1,
                                  Address2=b.Address2,
                                  CountryId= Convert.ToInt32(b.CountryId), 
                                  CountryName=c.Name, 
                                  StateId= Convert.ToInt32(b.StateId),
                                  StateName=s.Name,
                                  City=b.City,
                                  Postalcode=b.Postalcode, 
                                  Signature=b.Signature,
                                  CreatedBy=b.CreatedBy,
                                  CreatedDate=Convert.ToDateTime(b.CreatedDate)
                              }).FirstOrDefaultAsync();
            } 
            return null;
        }

        public async Task<bool> UpdateBussinessProfile(BussinessDetailViewModel _model)
        { 
            if (db != null)
            { 
                var bussiness = await db.BussinessDetails.FirstOrDefaultAsync(x => x.IdentityId == _model.IdentityId);
                if (bussiness != null)
                {
                    //update the bussiness
                    if (!string.IsNullOrWhiteSpace(_model.BussinessName))
                    {
                        bussiness.BussinessName = _model.BussinessName;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.BussinessLogo))
                    {
                        bussiness.BussinessLogo = _model.BussinessLogo;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.BussinessCoverPhoto))
                    {
                        bussiness.BussinessCoverPhoto = _model.BussinessCoverPhoto;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.AccountNumber))
                    {
                        bussiness.AccountNumber = _model.AccountNumber;
                    } 
                    if (_model.CurrencyId != 0)
                    {
                        bussiness.CurrencyId = _model.CurrencyId;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.CIN))
                    {
                        bussiness.CIN = _model.CIN;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.GSTIN))
                    {
                        bussiness.GSTIN = _model.GSTIN;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.BussinessSize))
                    {
                        bussiness.BussinessSize = _model.BussinessSize;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.BussinessClass))
                    {
                        bussiness.BussinessClass = _model.BussinessClass;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.Founded))
                    {
                        bussiness.Founded = Convert.ToDateTime(_model.Founded);
                    }
                    if (!string.IsNullOrWhiteSpace(_model.Fax))
                    {
                        bussiness.Fax = _model.Fax;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.WebAddress))
                    {
                        bussiness.WebAddress = _model.WebAddress;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.BussinessEmail))
                    {
                        bussiness.BussinessEmail = _model.BussinessEmail;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.BussinessPhone))
                    {
                        bussiness.BussinessPhone = _model.BussinessPhone;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.Address1))
                    {
                        bussiness.Address1 = _model.Address1;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.Address2))
                    {
                        bussiness.Address2 = _model.Address2;
                    }
                    if (_model.CountryId!= 0)
                    {
                        bussiness.CountryId = _model.CountryId;
                    }
                    if (_model.StateId != 0)
                    {
                        bussiness.StateId = _model.StateId;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.City))
                    {
                        bussiness.City = _model.City;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.Postalcode))
                    {
                        bussiness.Postalcode = _model.Postalcode;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.Signature))
                    {
                        bussiness.Signature = _model.Signature;
                    }
                    bussiness.UpdatedBy = _model.IdentityId;
                    bussiness.UpdatedDate = DateTime.Now;
                    db.BussinessDetails.Update(bussiness);
                    //Commit the transaction  
                    await db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
