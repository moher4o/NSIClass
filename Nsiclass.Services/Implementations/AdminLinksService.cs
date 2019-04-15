using AutoMapper.QueryableExtensions;
using Nsiclass.Data;
using Nsiclass.Data.Models;
using Nsiclass.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nsiclass.Services.Implementations
{
    public class AdminLinksService : IAdminLinksService
    {
        private readonly ClassDbContext db;

        public AdminLinksService(ClassDbContext db)
        {
            this.db = db;
        }

        public List<AdminLinksServiceModel> GetAllLinks()
        {
            var links = this.db.Links.AsQueryable();

            var result = links.AsQueryable()
                    .ProjectTo<AdminLinksServiceModel>()
                    .ToList();

            return result;
        }

        public AdminLinksServiceModel GetLinkById(int linkId)
        {
            return this.db.Links
                .Where(t => t.Id == linkId)
                .ProjectTo<AdminLinksServiceModel>()
                .FirstOrDefault();

        }

        public async Task<bool> CreateLink(string name, string adress)
        {
            try
            {
                var newLink = new PartnersLinks()
                {
                    Name = name,
                    Link = adress
                };
                await this.db.Links.AddAsync(newLink);
                await this.db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
         }

        public async Task<bool> EditLink(AdminLinksServiceModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Link))
            {
                return false;
            }
            try
            { 
                var editableLink = this.db.Links.FirstOrDefault(l => l.Id == model.Id);
                if (editableLink == null)
                {
                    return false;
                }

                editableLink.Name = model.Name;
                editableLink.Link = model.Link;

                await this.db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteLink(int linkId)
        {
            var currentLink = this.db.Links.FirstOrDefault(l => l.Id == linkId);
            if (currentLink == null)
            {
                return false;
            }

            currentLink.isDeleted = true;
            await this.db.SaveChangesAsync();

            return true;

        }

        public async Task<bool> RestoreLink(int linkId)
        {
            var currentLink = this.db.Links.FirstOrDefault(l => l.Id == linkId);
            if (currentLink == null)
            {
                return false;
            }

            currentLink.isDeleted = false;
            await this.db.SaveChangesAsync();

            return true;
        }
    }
}

