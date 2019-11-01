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
                                  BaseCurrencySymbol=b.BaseCurrencySymbol,
                                  BaseCurrencyName=b.BaseCurrencyName,
                                  CIN=b.CIN,
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
                    bussiness.BussinessName = _model.BussinessName;
                    if (!string.IsNullOrWhiteSpace(_model.BussinessLogo))
                    {
                        bussiness.BussinessLogo = _model.BussinessLogo;
                    } 
                    if (!string.IsNullOrWhiteSpace(_model.BussinessCoverPhoto))
                    {
                        bussiness.BussinessCoverPhoto = _model.BussinessCoverPhoto;
                    } 
                    bussiness.AccountNumber = _model.AccountNumber;
                    if (!string.IsNullOrWhiteSpace(_model.BaseCurrencyName))
                    {
                        bussiness.BaseCurrencyName = _model.BaseCurrencyName;
                    }
                    if (!string.IsNullOrWhiteSpace(_model.BaseCurrencySymbol))
                    {
                        bussiness.BaseCurrencySymbol = _model.BaseCurrencySymbol;
                    } 
                    bussiness.CIN = _model.CIN;
                    bussiness.GSTIN = _model.GSTIN;
                    bussiness.BussinessSize = _model.BussinessSize;
                    bussiness.BussinessClass = _model.BussinessClass;
                    bussiness.Founded = Convert.ToDateTime(_model.Founded);
                    bussiness.Fax = _model.Fax;
                    bussiness.WebAddress = _model.WebAddress;
                    if (!string.IsNullOrWhiteSpace(_model.BussinessEmail))
                    {
                        bussiness.BussinessEmail = _model.BussinessEmail;
                    } 
                    if (!string.IsNullOrWhiteSpace(_model.BussinessPhone))
                    {
                        bussiness.BussinessPhone = _model.BussinessPhone;
                    } 
                    bussiness.Address1 = _model.Address1;
                    bussiness.Address2 = _model.Address2;
                    bussiness.CountryId = _model.CountryId;
                    bussiness.StateId = _model.StateId;
                    bussiness.City = _model.City;
                    bussiness.Postalcode = _model.Postalcode;
                    bussiness.Signature = _model.Signature;
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
